using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yargon.JsonRpc
{
    /// <summary>
    /// Compares two <see cref="JsonError"/> instances, comparing only the error codes.
    /// </summary>
    public sealed class JsonResponseComparer : EqualityComparer<JsonResponse>
    {
        /// <inheritdoc />
        public override bool Equals(JsonResponse x, JsonResponse y)
        {
            JsonError xerr = x as JsonError;
            JsonError yerr = y as JsonError;
            if (xerr != null && yerr != null)
            {
                return Object.Equals(xerr.Id, yerr.Id)
                       && xerr.Error.Code == yerr.Error.Code;
            }
            else
            {
                return Object.Equals(x, y);
            }
        }

        /// <inheritdoc />
        public override int GetHashCode(JsonResponse obj)
        {
            JsonError err = obj as JsonError;
            if (err != null)
            {
                int hash = 17;
                unchecked
                {
                    hash = hash * 29 + err.Id?.GetHashCode() ?? 0;
                    hash = hash * 29 + err.Error.Code.GetHashCode();
                }
                return hash;
            }
            else
            {
                return obj?.GetHashCode() ?? 0;
            }
        }
    }
}
