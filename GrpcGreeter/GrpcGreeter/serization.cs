using System.Text.Json.Serialization;
using System.Text.Json;

namespace GrpcGreeter.serizaliza
{
    public class NullToEmptyStringConverter : JsonConverter<string>
    {
        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return string.Empty;
            }

            return reader.GetString();
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value ?? string.Empty);
        }
    }
}
