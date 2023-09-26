// ReSharper disable UnusedMember.Global

using System.Text.Json;
using System.Text.Json.Serialization;

namespace HamedStack.SharedKernel;

/// <summary>
/// Provides a converter for <see cref="Enumeration"/> to handle custom serialization with System.Text.Json.
/// </summary>
public class EnumerationConverter : JsonConverter<Enumeration>
{
    /// <summary>
    /// Reads and converts the JSON to type <see cref="Enumeration"/>.
    /// </summary>
    public override Enumeration Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        var id = root.GetProperty("Id").GetInt32();
        var name = root.GetProperty("Name").GetString()!;
        var description = root.TryGetProperty("Description", out var descElement) ? descElement.GetString()! : string.Empty;
        var typeName = root.GetProperty("Type").GetString()!;

        var derivedType = Type.GetType(typeName);

        if (derivedType == null || !derivedType.IsSubclassOf(typeof(Enumeration)))
            throw new JsonException("Invalid or unknown Enumeration type.");

        return (Enumeration)Activator.CreateInstance(derivedType, id, name, description)!;
    }

    /// <summary>
    /// Writes the provided <see cref="Enumeration"/> value as JSON.
    /// </summary>
    public override void Write(Utf8JsonWriter writer, Enumeration value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("Id", value.Id);
        writer.WriteString("Name", value.Name);
        writer.WriteString("Description", value.Description);
        writer.WriteString("Type", value.GetType().AssemblyQualifiedName!);
        writer.WriteEndObject();
    }
}