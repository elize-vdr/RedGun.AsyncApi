// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using System.Globalization;
using System.Linq;
using System.Text;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Models;

namespace RedGun.AsyncApi.Readers.ParseNodes
{
    internal static class AsyncApiAnyConverter
    {
        /// <summary>
        /// Converts the <see cref="AsyncApiString"/>s in the given <see cref="IAsyncApiAny"/>
        /// into the appropriate <see cref="IAsyncApiPrimitive"/> type based on the given <see cref="AsyncApiSchema"/>.
        /// For those strings that the schema does not specify the type for, convert them into
        /// the most specific type based on the value.
        /// </summary>
        public static IAsyncApiAny GetSpecificAsyncApiAny(IAsyncApiAny asyncApiAny, AsyncApiSchema schema = null)
        {
            if (asyncApiAny is AsyncApiArray asyncApiArray)
            {
                var newArray = new AsyncApiArray();
                foreach (var element in asyncApiArray)
                {
                    newArray.Add(GetSpecificAsyncApiAny(element, schema?.Items));
                }

                return newArray;
            }

            if (asyncApiAny is AsyncApiObject asyncApiObject)
            {
                var newObject = new AsyncApiObject();

                foreach (var key in asyncApiObject.Keys.ToList())
                {
                    if (schema?.Properties != null && schema.Properties.TryGetValue(key, out var property))
                    {
                        newObject[key] = GetSpecificAsyncApiAny(asyncApiObject[key], property);
                    }
                    else
                    {
                        newObject[key] = GetSpecificAsyncApiAny(asyncApiObject[key], schema?.AdditionalProperties);
                    }
                }

                return newObject;
            }

            if (!(asyncApiAny is AsyncApiString))
            {
                return asyncApiAny;
            }

            var value = ((AsyncApiString)asyncApiAny).Value;
            var type = schema?.Type;
            var format = schema?.Format;

            if (((AsyncApiString)asyncApiAny).IsExplicit())
            {
                // More narrow type detection for explicit strings, only check types that are passed as strings
                if (schema == null)
                {
                    if (DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTimeValue))
                    {
                        return new AsyncApiDateTime(dateTimeValue);
                    }
                }
                else if (type == "string")
                {
                    if (format == "byte")
                    {
                        try
                        {
                            return new AsyncApiByte(Convert.FromBase64String(value));
                        }
                        catch (FormatException)
                        { }
                    }

                    if (format == "binary")
                    {
                        try
                        {
                            return new AsyncApiBinary(Encoding.UTF8.GetBytes(value));
                        }
                        catch (EncoderFallbackException)
                        { }
                    }

                    if (format == "date")
                    {
                        if (DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateValue))
                        {
                            return new AsyncApiDate(dateValue.Date);
                        }
                    }

                    if (format == "date-time")
                    {
                        if (DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTimeValue))
                        {
                            return new AsyncApiDateTime(dateTimeValue);
                        }
                    }

                    if (format == "password")
                    {
                        return new AsyncApiPassword(value);
                    }
                }

                return asyncApiAny;
            }

            if (value == null || value == "null")
            {
                return new AsyncApiNull();
            }

            if (schema?.Type == null)
            {
                if (value == "true")
                {
                    return new AsyncApiBoolean(true);
                }

                if (value == "false")
                {
                    return new AsyncApiBoolean(false);
                }

                if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var intValue))
                {
                    return new AsyncApiInteger(intValue);
                }

                if (long.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var longValue))
                {
                    return new AsyncApiLong(longValue);
                }

                if (double.TryParse(value, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var doubleValue))
                {
                    return new AsyncApiDouble(doubleValue);
                }

                if (DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTimeValue))
                {
                    return new AsyncApiDateTime(dateTimeValue);
                }
            }
            else
            {
                if (type == "integer" && format == "int32")
                {
                    if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var intValue))
                    {
                        return new AsyncApiInteger(intValue);
                    }
                }

                if (type == "integer" && format == "int64")
                {
                    if (long.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var longValue))
                    {
                        return new AsyncApiLong(longValue);
                    }
                }

                if (type == "integer")
                {
                    if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var intValue))
                    {
                        return new AsyncApiInteger(intValue);
                    }
                }

                if (type == "number" && format == "float")
                {
                    if (float.TryParse(value, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var floatValue))
                    {
                        return new AsyncApiFloat(floatValue);
                    }
                }

                if (type == "number" && format == "double")
                {
                    if (double.TryParse(value, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var doubleValue))
                    {
                        return new AsyncApiDouble(doubleValue);
                    }
                }

                if (type == "number")
                {
                    if (double.TryParse(value, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var doubleValue))
                    {
                        return new AsyncApiDouble(doubleValue);
                    }
                }

                if (type == "string" && format == "byte")
                {
                    try
                    {
                        return new AsyncApiByte(Convert.FromBase64String(value));
                    }
                    catch (FormatException)
                    { }
                }

                // binary
                if (type == "string" && format == "binary")
                {
                    try
                    {
                        return new AsyncApiBinary(Encoding.UTF8.GetBytes(value));
                    }
                    catch (EncoderFallbackException)
                    { }
                }

                if (type == "string" && format == "date")
                {
                    if (DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateValue))
                    {
                        return new AsyncApiDate(dateValue.Date);
                    }
                }

                if (type == "string" && format == "date-time")
                {
                    if (DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTimeValue))
                    {
                        return new AsyncApiDateTime(dateTimeValue);
                    }
                }

                if (type == "string" && format == "password")
                {
                    return new AsyncApiPassword(value);
                }

                if (type == "string")
                {
                    return asyncApiAny;
                }

                if (type == "boolean")
                {
                    if (bool.TryParse(value, out var booleanValue))
                    {
                        return new AsyncApiBoolean(booleanValue);
                    }
                }
            }

            // If data conflicts with the given type, return a string.
            // This converter is used in the parser, so it does not perform any validations, 
            // but the validator can be used to validate whether the data and given type conflicts.
            return asyncApiAny;
        }
    }
}
