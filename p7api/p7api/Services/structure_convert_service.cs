using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

namespace JsonToCsvConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            string staticFolderPath = "static";
            string liteFilePath = Path.Combine(staticFolderPath, "lite.json");
            string slotFilePath = Path.Combine(staticFolderPath, "slot.json");
            string detailFilePath = Path.Combine(staticFolderPath, "detail.json");
            string outputFilePath = Path.Combine(staticFolderPath, "structure_records.csv");

            var liteData = JArray.Parse(File.ReadAllText(liteFilePath));
            var slotData = JArray.Parse(File.ReadAllText(slotFilePath));
            var detailData = JArray.Parse(File.ReadAllText(detailFilePath));

            List<string> csvRecords = new List<string>();

            foreach (var detail in detailData)
            {
                string id = detail["id"].ToString();
                string name = detail["name"].ToString();
                string environmentDescription = detail["is_offshore"].ToObject<bool>() ? "Offshore" : "Onshore";
                string elevation = detail["elevation"]["name"].ToString();

                string description = "Structure Definition";
                int structureReferenceNumber = 1; // Assuming fixed value as per the instruction
                string srp = "SRP";
                int srpNumber = 1; // Assuming fixed value as per the instruction
                string structureReferencePointDescription = "Centre of slot A1"; // Example value

                string record = $"H7,1,1,0,\"{description}\",{structureReferenceNumber},\"{name}\",\"{srp}\",{srpNumber},\"{structureReferencePointDescription}\",1,\"{environmentDescription}\",\"{elevation}\",,";
                csvRecords.Add(record);
            }

            File.WriteAllLines(outputFilePath, csvRecords);
            Console.WriteLine("CSV records have been generated and saved to " + outputFilePath);
        }
    }
}
