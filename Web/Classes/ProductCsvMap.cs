using CsvHelper.Configuration;
using FruitSystem.Data.Classes;

namespace FruitSystem.Web.Classes
{
    public class ProductCsvMap : ClassMap<Product>
    {
        public ProductCsvMap()
        {
            Map(x => x.Name).Name("fruit");
            Map(x => x.Price).Name("price");
            Map(x => x.Stock).Name("quantity_in_stock");
            Map(x => x.UpdatedDated).Name("updated_date");
        }
    }
}