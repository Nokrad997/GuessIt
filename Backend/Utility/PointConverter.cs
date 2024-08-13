using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using NetTopologySuite.Geometries;

public class PointConverter : JsonConverter<Point>
{
    public override Point Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected StartObject token");
        }

        double[] coordinates = null;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                string propertyName = reader.GetString();

                if (propertyName == "coordinates")
                {
                    reader.Read();
                    coordinates = JsonSerializer.Deserialize<double[]>(ref reader, options);
                }
            }
            else if (reader.TokenType == JsonTokenType.EndObject)
            {
                break;
            }
        }

        if (coordinates == null || coordinates.Length != 2)
        {
            throw new JsonException("Invalid Point format. 'coordinates' must be an array with two elements [longitude, latitude].");
        }

        return new Point(coordinates[0], coordinates[1]);
    }

    public override void Write(Utf8JsonWriter writer, Point value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("type", "Point");
        writer.WritePropertyName("coordinates");
        writer.WriteStartArray();
        writer.WriteNumberValue(value.X);
        writer.WriteNumberValue(value.Y);
        writer.WriteEndArray();
        writer.WriteEndObject();
    }
}