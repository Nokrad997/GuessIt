using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using NetTopologySuite.Geometries;

public class MultiPolygonConverter : JsonConverter<MultiPolygon>
{
    public override MultiPolygon Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected StartObject token");
        }

        double[][][][] coordinates = null;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                string propertyName = reader.GetString();

                if (propertyName == "coordinates")
                {
                    reader.Read();
                    coordinates = JsonSerializer.Deserialize<double[][][][]>(ref reader, options);
                }
            }
            else if (reader.TokenType == JsonTokenType.EndObject)
            {
                break;
            }
        }

        if (coordinates == null || coordinates.Length == 0)
        {
            throw new JsonException("Invalid MultiPolygon format. 'coordinates' must be an array of polygons.");
        }

        var polygons = new Polygon[coordinates.Length];

        for (int i = 0; i < coordinates.Length; i++)
        {
            var linearRings = new LinearRing[coordinates[i].Length];

            for (int j = 0; j < coordinates[i].Length; j++)
            {
                var points = new Coordinate[coordinates[i][j].Length];
                for (int k = 0; k < coordinates[i][j].Length; k++)
                {
                    points[k] = new Coordinate(coordinates[i][j][k][0], coordinates[i][j][k][1]);
                }
                linearRings[j] = new LinearRing(points);
            }

            polygons[i] = new Polygon(linearRings[0], linearRings.Length > 1 ? linearRings[1..] : null);
        }

        return new MultiPolygon(polygons);
    }

    public override void Write(Utf8JsonWriter writer, MultiPolygon value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("type", "MultiPolygon");
        writer.WritePropertyName("coordinates");
        writer.WriteStartArray();

        // Write each polygon
        foreach (var polygon in value.Geometries)
        {
            writer.WriteStartArray();
            WritePolygon(writer, (Polygon)polygon);
            writer.WriteEndArray();
        }

        writer.WriteEndArray();
        writer.WriteEndObject();
    }

    private void WritePolygon(Utf8JsonWriter writer, Polygon polygon)
    {
        // Write the exterior ring
        WriteLinearRing(writer, polygon.ExteriorRing);

        // Write each interior ring (holes)
        foreach (var interiorRing in polygon.InteriorRings)
        {
            WriteLinearRing(writer, interiorRing);
        }
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
