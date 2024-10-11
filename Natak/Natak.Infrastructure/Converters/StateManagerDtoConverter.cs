using System.Text.Json;
using System.Text.Json.Serialization;
using Natak.Infrastructure.DTOs;

namespace Natak.Infrastructure.Converters;

public sealed class StateManagerDtoConverter : JsonConverter<StateManagerDto>
{
    private const string TypeDiscriminatorName = "TypeDiscriminator";
    private const string TypeValuePropertyName = "TypeValue";
    
    private enum TypeDiscriminator
    {
        SetupStateManagerDto,
        GameStateManagerDto
    }
    
    public override bool CanConvert(Type type)
    {
        return typeof(StateManagerDto).IsAssignableFrom(type);
    }
    
    public override StateManagerDto? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }

        if (!reader.Read()
            || reader.TokenType != JsonTokenType.PropertyName
            || reader.GetString() != TypeDiscriminatorName)
        {
            throw new JsonException();
        }
        
        if (!reader.Read() || reader.TokenType != JsonTokenType.Number)
        {
            throw new JsonException();
        }
        
        StateManagerDto stateManager;
        var stateManagerType = (TypeDiscriminator)reader.GetInt32();
        switch (stateManagerType)
        {
            case TypeDiscriminator.SetupStateManagerDto:
                if (!reader.Read() || reader.GetString() != TypeValuePropertyName)
                {
                    throw new JsonException();
                }
                if (!reader.Read() || reader.TokenType != JsonTokenType.StartObject)
                {
                    throw new JsonException();
                }
                stateManager = (SetupStateManagerDto)JsonSerializer.Deserialize(ref reader, typeof(SetupStateManagerDto))!;
                break;
            case TypeDiscriminator.GameStateManagerDto:
                if (!reader.Read() || reader.GetString() != TypeValuePropertyName)
                {
                    throw new JsonException();
                }
                if (!reader.Read() || reader.TokenType != JsonTokenType.StartObject)
                {
                    throw new JsonException();
                }
                stateManager = (GameStateManagerDto)JsonSerializer.Deserialize(ref reader, typeof(GameStateManagerDto))!;
                break;
            default:
                throw new NotSupportedException();
        }

        if (!reader.Read() || reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException();
        }

        return stateManager;
    }

    public override void Write(Utf8JsonWriter writer, StateManagerDto value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        switch (value)
        {
            case SetupStateManagerDto setupStateManagerDto:
                writer.WriteNumber(TypeDiscriminatorName, (int)TypeDiscriminator.SetupStateManagerDto);
                writer.WritePropertyName(TypeValuePropertyName);
                JsonSerializer.Serialize(writer, setupStateManagerDto);
                break;
            case GameStateManagerDto gameStateManagerDto:
                writer.WriteNumber(TypeDiscriminatorName, (int)TypeDiscriminator.GameStateManagerDto);
                writer.WritePropertyName(TypeValuePropertyName);
                JsonSerializer.Serialize(writer, gameStateManagerDto);
                break;
            default:
                throw new NotSupportedException();
        }

        writer.WriteEndObject();
    }
}