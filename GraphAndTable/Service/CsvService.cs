using GraphAndTable.Models;
using System.Formats.Asn1;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;

namespace GraphAndTable.Service
{
    public class CsvService
    {
        public List<DataModel> ReadCsv(string filePath)
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));
            var records = csv.GetRecords<DataModel>().ToList();

            // Assign order dynamically
            for (int i = 0; i < records.Count; i++)
            {
                records[i].Id = i + 1;  // 🔹 Assign incremental order (1,2,3...)
            }
            return records;
        }
    }
}
