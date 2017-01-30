using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Yargon.JsonRpc
{
    /// <summary>
    /// The base class for implementations of the <see cref="IJsonRpcService"/> interface.
    /// </summary>
    public abstract class JsonRpcServiceBase : IJsonRpcService
    {
        /// <summary>
        /// Gets the JSON serializer used.
        /// </summary>
        /// <value>The JSON serializer.</value>
        public JsonSerializer Serializer { get; }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonRpcServiceBase"/> class.
        /// </summary>
        /// <param name="serializer">The JSON serializert to use.</param>
        protected JsonRpcServiceBase(JsonSerializer serializer)
        {
            #region Contract
            if (serializer == null)
                throw new ArgumentNullException(nameof(serializer));
            #endregion

            this.Serializer = serializer;
            this.Serializer.Converters.Add(new JsonBatch<JsonRequest>.Converter());
            this.Serializer.Converters.Add(new JsonBatch<JsonResponse>.Converter());
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="JsonRpcServiceBase"/> class.
        /// </summary>
        protected JsonRpcServiceBase()
            : this(JsonSerializer.Create())
        {
            // Nothing to do.
        }
        #endregion

        /// <inheritdoc />
        public string Process(string jsonRequests)
        {
            #region Contract
            if (jsonRequests == null)
                throw new ArgumentNullException(nameof(jsonRequests));
            #endregion

            using (var reader = new StringReader(jsonRequests))
            using (var writer = new StringWriter())
            {
                Process(reader, writer);
                return writer.ToString();
            }
        }

        /// <inheritdoc />
        public void Process(TextReader reader, TextWriter writer)
        {
            #region Contract
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            #endregion

            using (var jsonReader = new JsonTextReader(reader))
            using (var jsonWriter = new JsonTextWriter(writer))
            {
                Process(jsonReader, jsonWriter);
            }
        }

        /// <inheritdoc />
        public void Process(JsonReader reader, JsonWriter writer)
        {
            #region Contract
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            #endregion

            JsonBatch<JsonRequest> requests;
            try
            {
                requests = ReadRequests(reader);
                if (requests == null)
                {
                    this.Serializer.Serialize(writer, InvalidRequest());
                    return;
                }
            }
            catch (JsonReaderException ex)
            {
                this.Serializer.Serialize(writer, ParseError(ex));
                return;
            }

            var responses = new List<JsonResponse>();

            foreach (var request in requests.Messages)
            {
                if (request == null)
                {
                    responses.Add(InvalidRequest());
                    continue;
                }

                try
                {
                    var response = Handle(request);
                    if (response != null && !request.IsNotification)
                        responses.Add(response);
                }
                catch (Exception ex)
                {
                    // Any exception that occurred during handling of the request is caught and causes an JsonError response,
                    // iff the request wasn't a notification.
                    if (!request.IsNotification)
                        responses.Add(CreateJsonError(request.Id, ex));
                }
            }

            this.Serializer.Serialize(writer, new JsonBatch<JsonResponse>(responses, requests.IsSingleton));
        }

        /// <summary>
        /// Reads the requests.
        /// </summary>
        /// <param name="reader">The JSON reader.</param>
        /// <returns>The read requests, each of which may be <see langword="null"/> when it could not be read;
        /// or <see langword="null"/> when everything is invalid.</returns>
        /// <exception cref="JsonReaderException">
        /// The requests could not be parsed.
        /// </exception>
        [CanBeNull]
        private JsonBatch<JsonRequest> ReadRequests(JsonReader reader)
        {
            #region Contract
            Debug.Assert(reader != null);
            #endregion

            bool isBatch;
            var requests = new List<JsonRequest>();

            reader.Read();

            if (reader.TokenType == JsonToken.StartArray)
            {
                // Batch
                isBatch = true;

                reader.Read();
                while (reader.TokenType != JsonToken.EndArray)
                {
                    try
                    {
                        requests.Add(this.Serializer.Deserialize<JsonRequest>(reader));
                    }
                    catch (JsonSerializationException)
                    {
                        requests.Add(null);
                    }
                    reader.Read();
                }
                reader.Read();

                if (requests.Count == 0)
                {
                    // An empty batch is not allowed.
                    return null;
                }
            }
            else if (reader.TokenType == JsonToken.StartObject)
            {
                // Request
                isBatch = false;
                try
                {
                    requests.Add(this.Serializer.Deserialize<JsonRequest>(reader));
                }
                catch (JsonSerializationException)
                {
                    requests.Add(null);
                }
            }
            else
            {
                // Invalid request.
                return null;
            }

            return new JsonBatch<JsonRequest>(requests, !isBatch);
        }

        /// <summary>
        /// Creates a <see cref="JsonError"/> from an exception.
        /// </summary>
        /// <param name="id">The message identifier string; or <see langword="null"/>.</param>
        /// <param name="exception">The exceptiont to capture.</param>
        /// <returns>The resulting <see cref="JsonError"/>.</returns>
        public JsonError CreateJsonError([CanBeNull] string id, Exception exception)
        {
            #region Contract
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));
            #endregion

            var jsonRpcException = exception as JsonRpcException;
            if (jsonRpcException == null)
            {
                // Return an 'Internal error' instead.
                string details = $"Unexpected exception {exception.GetType().FullName}: {exception.Message}";
                return CreateJsonError(id, new JsonRpcInternalException(null, details, exception));
            }
            
            return new JsonError(id, new JsonErrorObject(jsonRpcException.Code, jsonRpcException.Message, ToTokenOrNull(jsonRpcException.Data)));
        }

        /// <summary>
        /// Creates an 'Invalid request' JSON RPC error.
        /// </summary>
        /// <param name="id">The message identifier string; or <see langword="null"/>.</param>
        /// <returns>The resulting <see cref="JsonError"/>.</returns>
        public JsonError InvalidRequest([CanBeNull] string id = null)
        {
            return CreateJsonError(id, new InvalidRequestException());
        }

        /// <summary>
        /// Creates a 'Parse error' JSON RPC error.
        /// </summary>
        /// <param name="exception">The JSON reader exception; or <see langword="null"/>.</param>
        /// <returns>The resulting <see cref="JsonError"/>.</returns>
        public JsonError ParseError([CanBeNull] JsonReaderException exception = null)
        {
            return CreateJsonError(null, new ParseException(null, exception?.Message, exception));
        }

        /// <summary>
        /// Converts an object to a JSON token.
        /// </summary>
        /// <typeparam name="T">The type of object.</typeparam>
        /// <param name="obj">The object.</param>
        /// <returns>The resulting JSON token.</returns>
        protected JToken ToTokenOrNull<T>(T obj)
        {
            if (obj == null)
                return JValue.CreateNull();
            return JToken.FromObject(obj, this.Serializer);
        }

        /// <summary>
        /// Handles a JSON request.
        /// </summary>
        /// <param name="request">The JSON request.</param>
        /// <returns>The JSON response; or <see langword="null"/> to return no response.</returns>
        [CanBeNull]
        protected abstract JsonResponse Handle(JsonRequest request);
    }
}
