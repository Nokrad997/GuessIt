using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using NetTopologySuite.Geometries;

public class PolygonConverter : JsonConverter<Geometry>
{
    public override Polygon Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected StartObject token");
        }

        double[][][] coordinates = null;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                string propertyName = reader.GetString();

                if (propertyName == "coordinates")
                {
                    reader.Read();
                    coordinates = JsonSerializer.Deserialize<double[][][]>(ref reader, options);
                }
            }
            else if (reader.TokenType == JsonTokenType.EndObject)
            {
                break;
            }
        }

        if (coordinates == null || coordinates.Length == 0)
        {
            throw new JsonException("Invalid Polygon format. 'coordinates' must be an array of linear rings.");
        }

        var linearRings = new LinearRing[coordinates.Length];

        for (int i = 0; i < coordinates.Length; i++)
        {
            var points = new Coordinate[coordinates[i].Length];
            for (int j = 0; j < coordinates[i].Length; j++)
            {
                points[j] = new Coordinate(coordinates[i][j][0], coordinates[i][j][1]);
            }
            linearRings[i] = new LinearRing(points);
        }

        return new Polygon(linearRings[0], linearRings.Length > 1 ? linearRings[1..] : null);
    }

    public override void Write(Utf8JsonWriter writer, Geometry value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("type", value is MultiPolygon ? "MultiPolygon" : "Polygon");

        writer.WritePropertyName("coordinates");
        writer.WriteStartArray();

        if (value is Polygon polygon)
        {
            WriteLinearRing(writer, polygon.ExteriorRing);
            foreach (var interiorRing in polygon.InteriorRings)
            {
                WriteLinearRing(writer, interiorRing);
            }
        }
        else if (value is MultiPolygon multiPolygon)
        {
            foreach (var polygon2 in multiPolygon.Geometries.OfType<Polygon>())
            {
                writer.WriteStartArray();
                WriteLinearRing(writer, polygon2.ExteriorRing);
                foreach (var interiorRing in polygon2.InteriorRings)
                {
                    WriteLinearRing(writer, interiorRing);
                }
                writer.WriteEndArray();
            }
        }

        writer.WriteEndArray();
        writer.WriteEndObject();
    }

    private void WriteLinearRing(Utf8JsonWriter writer, LineString ring)
    {
        writer.WriteStartArray();
        foreach (var coord in ring.Coordinates)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(coord.X);  // Longitude
            writer.WriteNumberValue(coord.Y);  // Latitude
            writer.WriteEndArray();
        }
        writer.WriteEndArray();
    }
}
