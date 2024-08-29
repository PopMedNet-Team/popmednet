using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PopMedNet.Utilities.WebSites.Formatters
{
    public class DateTimeConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(DateTime)
                || typeToConvert == typeof(DateTime?)
                || typeToConvert == typeof(DateTimeOffset)
                || typeToConvert == typeof(DateTimeOffset?);
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            if (typeToConvert == typeof(DateTime))
            {
                return new DateTimeConverter<DateTime>();
            }
            else if (typeToConvert == typeof(DateTime?))
            {
                return new DateTimeConverter<DateTime?>();
            }
            else if (typeToConvert == typeof(DateTimeOffset))
            {
                return new DateTimeConverter<DateTimeOffset>();
            }
            else if (typeToConvert == typeof(DateTimeOffset?))
            {
                return new DateTimeConverter<DateTimeOffset?>();
            }

            throw new NotSupportedException("CreateConverter got called on a type that this converter factory doesn't support");
        }

        private class DateTimeConverter<T> : JsonConverter<T>
        {
            public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                string? value = reader.GetString();

                if(string.IsNullOrEmpty(value))
                {
                    if (typeToConvert == typeof(DateTime?) || typeToConvert == typeof(DateTimeOffset?))
                        return null as dynamic;
                    else
                        throw new InvalidCastException("The value cannot be null.");
                }

                if(typeToConvert == typeof(DateTime) || typeToConvert == typeof(DateTime?))
                {
                    return DateTime.Parse(value) as dynamic;
                }

                if(typeToConvert == typeof(DateTimeOffset) || typeToConvert == typeof(DateTimeOffset?))
                {
                    return DateTimeOffset.Parse(value) as dynamic;
                }

                throw new NotSupportedException("Invalid type to convert:" + typeToConvert.FullName);
            }

            public override void Write(Utf8JsonWriter writer, T date, JsonSerializerOptions options)
            {
                if (date == null)
                    return;

                writer.WriteStringValue((date as dynamic).ToString("yyyy-MM-ddTHH:mm:ssZ"));
            }
        }
    }
}
