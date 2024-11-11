using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using GrpcGreeterClient.Wells;
using Microsoft.AspNetCore.Mvc;
using p7api.Models.Map;
namespace p7api.Map;
public static class JsonToTextConverter
{
	//private static int definition_increaseCounter = 1;
	//private static int ref_struct_increaseCount = 1;
	private static readonly Dictionary<string, int> IncreaseCounters = new Dictionary<string, int>();
	private static ILogger _logger;
	public static void InitializeLogger(ILogger logger)
	{
		_logger = logger;
	}
	public static List<string> ConvertJsonToText(JsonElement jsonElement, Dictionary<int, FieldMapping> fieldMappings, string recordType, object WellDetail)
	{
		_logger?.LogInformation("convety:{types}",recordType);
		var records = new List<string>();


		// 处理指定的字段映射
		records.Add(CreateRecord(jsonElement, fieldMappings, recordType, WellDetail));

		return records;
	}

	private static string CreateRecord(JsonElement jsonElement, Dictionary<int, FieldMapping> fieldMappings, string recordType, object WellDetail)
	{
		if (fieldMappings == null)
		{
			throw new ArgumentNullException(nameof(fieldMappings), "Field mappings cannot be null.");
		}
		var fields = new List<string>();
		foreach (var fieldMapping in fieldMappings.OrderBy(fm => fm.Key))
		{
			if (fieldMapping.Value == null)
			{
				_logger?.LogWarning("Field mapping for key {FieldKey} is null.", fieldMapping.Key);
				continue;
			}
			var fieldValue = GetFieldValue(jsonElement, fieldMapping.Value,recordType,fieldMapping.Key, WellDetail);
			fields.Add(fieldValue);
			_logger?.LogInformation("Field {FieldKey}: {FieldValue}", fieldMapping.Key, fieldValue);
		}
		return string.Join(",", fields);
	}

	private static string GetFieldValue(JsonElement jsonElement, FieldMapping fieldMapping,string recordType, int fieldKey, object WellDetail)
	{
		//Console.WriteLine($"Getting field value for field mapping: {fieldMapping.Description.Substring(0,Math.Min(15,fieldMapping.Description.Length))}");
		//Console.WriteLine("^-^-^-^-^-^-^-^-^-^-^-^-^-^-^-^-^-^-^-^-^-^-^-^-^:");
		//foreach (var property in WellDetail.GetType().GetProperties())
		//{
		//	Console.WriteLine($"{property.Name}: { property.GetValue(WellDetail)}"   );
		//}
		if (fieldMapping.Description != null && fieldMapping.Description.Contains("Fixed fields"))
		{
			return fieldMapping.Value;

		}
		if (fieldMapping.MSD_source != null && fieldMapping.MSD_source.Any())
		{
			Console.WriteLine($"**********it'sto convert^^^^^^^^^^:{fieldMapping.MSD_source}" );
			foreach (var source in fieldMapping.MSD_source)
			{
				Console.WriteLine($"Processing MSD_source: Model={source.Model}, Key={source.Key}");

				if (!string.IsNullOrEmpty(source.Model) && !string.IsNullOrEmpty(source.Key))
				{
					var propertyValue = GetJsonElementByModel(jsonElement, source.Key,source.Model,WellDetail);

					
					Console.WriteLine($"JSONelement_Welldetail:{WellDetail}");
					if (propertyValue != null)
					{
						Console.WriteLine($"Found value for key {source.Key}, sour {source.Model}: {propertyValue}"  );
							return propertyValue.ToString();
						
					}
					else
					{
						Console.WriteLine($"Model element not found for model: {source.Key}"  );
					}
				//if (modelElement.ValueKind == JsonValueKind.Object && modelElement.TryGetProperty(ToPascalCase(source.Key), out var valueElement))
				//{
				//	var value = valueElement.GetString() ?? string.Empty;
				//	_logger?.LogInformation("Found value for model {Model}, key {Key}: {Value}", source.Model, source.Key, value);
				//	return value;
				//}
			}
			}
		}

		if (!string.IsNullOrEmpty(fieldMapping.Method))
		{
			if (fieldMapping.Method == "Direct Mapping")
			{
				return fieldMapping.Description.Substring(0, Math.Min(15, fieldMapping.Description.Length));
			}
			if (fieldMapping.Method == "Increase from 1")
			{
				var counterkey = $"{recordType}_{fieldKey}";
				if (!IncreaseCounters.ContainsKey(counterkey))
				{
					IncreaseCounters[counterkey] = 1;
				}
				var value = IncreaseCounters[counterkey].ToString();
				IncreaseCounters[counterkey]++;
				return value;

				//return (_increaseCounter++).ToString();
			}

		}
		
		if (!fieldMapping.Nullable)
		{
			return fieldMapping.Default_value ?? fieldMapping.Description.Substring(0, Math.Min(15, fieldMapping.Description.Length));
		}

		return fieldMapping.Description.Substring(0, Math.Min(15, fieldMapping.Description.Length)) ?? string.Empty;
	}

	private static object GetJsonElementByModel(JsonElement jsonElement, string model,string key,object WellDetail)
	{

		//if (jsonElement.ValueKind == JsonValueKind.Object && jsonElement.TryGetProperty(ToPascalCase(model), out var modelElement))
		//{
		//	return modelElement;
		//}

		//return default;

		var properties = model.Split('.');
		var currentInstance = WellDetail;

		foreach (var property in properties)
		{
			var propInfo = currentInstance.GetType().GetProperty(ToPascalCase(property));
			if (propInfo == null)
			{
				return null;
			}

			currentInstance = propInfo.GetValue(currentInstance);
			if (currentInstance == null)
			{
				return null;
			}
		}
		return currentInstance;
		//var property = WellDetail.GetType().GetProperty(ToPascalCase(model));
		//if (property != null)
		//{
		//	return property.GetValue(WellDetail);
		//}

		//return null;
	}

	private static string ToPascalCase(string str)
	{
		if (string.IsNullOrEmpty(str))
		{
			return str;
		}

		if (str.Length == 1)
		{
			return str.ToUpper();
		}

		return char.ToUpper(str[0]) + str.Substring(1).Replace("_", string.Empty);
	}
}



public static class ResourceHelper
{
	private static ILogger _logger;

	public static void InitializeLogger(ILogger logger)
	{
		_logger = logger;
	}
	public static string GetEmbeddedResource(string resourceName)
	{
		_logger?.LogInformation("Attempting to read embedded resource: {ResourceName}", resourceName);

		var assembly = Assembly.GetExecutingAssembly();
		using (var stream = assembly.GetManifestResourceStream(resourceName))
		{
			if (stream == null)
			{
				var errorMessage = $"Resource '{resourceName}' not found.";
				_logger?.LogError(errorMessage);
				throw new FileNotFoundException(errorMessage);
			}

			using (var reader = new StreamReader(stream))
			{
				var content = reader.ReadToEnd();
				_logger?.LogInformation("Successfully read embedded resource: {ResourceName}", resourceName);
				return content;
			}
		}
	}
}







 

