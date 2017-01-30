using System.IO;
using Newtonsoft.Json;

namespace Yargon.JsonRpc
{
    /// <summary>
    /// A JSON RPC service, which handles requests and returns responses.
    /// </summary>
    public interface IJsonRpcService
    {
        /// <summary>
        /// Gets the JSON serializer used.
        /// </summary>
        /// <value>The JSON serializer.</value>
        JsonSerializer Serializer { get; }

        /// <summary>
        /// Processes JSON RPC requests.
        /// </summary>
        /// <param name="json">The JSON requests to handle.</param>
        /// <returns>The JSON responses; or an empty string when there is no response.</returns>
        string Process(string json);

        /// <summary>
        /// Processes JSON RPC requests.
        /// </summary>
        /// <param name="reader">The text reader from which to read the JSON requests.</param>
        /// <param name="writer">The text writer to which to write the JSON responses.</param>
        void Process(TextReader reader, TextWriter writer);

        /// <summary>
        /// Processes JSON RPC requests.
        /// </summary>
        /// <param name="reader">The JSON reader from which to read the JSON requests.</param>
        /// <param name="writer">The JSON writer to which to write the JSON responses.</param>
        void Process(JsonReader reader, JsonWriter writer);
    }
}