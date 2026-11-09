using System.Text.Json;
using System.Text.Json.Serialization;

namespace Brimborium.Angular;

[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.Unspecified,
    PropertyNameCaseInsensitive = true)]
[JsonSerializable(typeof(global::Brimborium.Angular.API.Project))]
public partial class BrimboriumAngularContractJsonSerializerContext : JsonSerializerContext { }
