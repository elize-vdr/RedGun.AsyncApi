// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

namespace RedGun.AsyncApi.Any
{
    /// <summary>
    /// Primitive type.
    /// </summary>
    public enum PrimitiveType
    {
        /// <summary>
        /// Integer
        /// </summary>
        Integer,

        /// <summary>
        /// Long
        /// </summary>
        Long,

        /// <summary>
        /// Float
        /// </summary>
        Float,

        /// <summary>
        /// Double
        /// </summary>
        Double,

        /// <summary>
        /// String
        /// </summary>
        String,

        /// <summary>
        /// Byte
        /// </summary>
        Byte,

        /// <summary>
        /// Binary
        /// </summary>
        Binary,

        /// <summary>
        /// Boolean
        /// </summary>
        Boolean,

        /// <summary>
        /// Date
        /// </summary>
        Date,

        /// <summary>
        /// DateTime
        /// </summary>
        DateTime,

        /// <summary>
        /// Password
        /// </summary>
        Password
    }

    /// <summary>
    /// Base interface for the Primitive type.
    /// </summary>
    public interface IAsyncApiPrimitive : IAsyncApiAny
    {
        /// <summary>
        /// Primitive type.
        /// </summary>
        PrimitiveType PrimitiveType { get; }
    }
}
