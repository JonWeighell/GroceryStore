using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Linq;

namespace FruitSystem.Web.Classes
{
    public interface ICsvHelper<T, TCsvMap>
    {
        bool CsvIsValid(IFormFile file);
        IEnumerable<T> ReadFromCsv(IFormFile file);
    }

    public class CsvHelper<T, TCsvMap> : ICsvHelper<T, TCsvMap> where TCsvMap : ClassMap
    {
        public bool CsvIsValid(IFormFile file) =>
            file != null &&
            file.FileName.ToLower().EndsWith(".csv");

        public IEnumerable<T> ReadFromCsv(IFormFile file)
        {
            IEnumerable<T> records;

            using (StreamReader streamReader = new StreamReader(file.OpenReadStream(), Encoding.Default))
            {
                using (CsvReader csvReader = new CsvReader(streamReader, CultureInfo.CurrentCulture))
                {
                    csvReader.Configuration.RegisterClassMap<TCsvMap>();
                    records = csvReader.GetRecords<T>().ToList<T>();
                }
            }   

            return records;
        }
    }
}
