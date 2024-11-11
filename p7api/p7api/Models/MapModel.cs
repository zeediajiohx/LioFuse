
using Newtonsoft.Json;
using System.Collections.Generic;
namespace p7api.Models.Map;
public class Mapping
{
	[JsonProperty("well_definition")]
	public Dictionary<int, FieldMapping> WellDefinition { get; set; }

	[JsonProperty("well_details")]
	public Dictionary<int, FieldMapping> WellDetails { get; set; }

    [JsonProperty("structure")]
    public Dictionary<int, FieldMapping> Structure  { get; set; }

    //[JsonProperty("structure_details")]
    //public Dictionary<int, FieldMapping> StructureDetails { get; set; }
}

public class FieldMapping
{
	[JsonProperty("MSD_source")]
	public List<MsdSource> MSD_source { get; set; }

	[JsonProperty("default_value")]
	public string Default_value { get; set; }

	[JsonProperty("method")]
	public string Method { get; set; }

	[JsonProperty("nullable")]
	public bool Nullable { get; set; }

	[JsonProperty("description")]
	public string Description { get; set; }

	[JsonProperty("value")]
	public string Value { get; set; }

	[JsonProperty("need_review")]
	public bool Need_review { get; set; }
}

public class MsdSource
{
	[JsonProperty("model")]
	public string Model { get; set; }

	[JsonProperty("key")]
	public string Key { get; set; }
}