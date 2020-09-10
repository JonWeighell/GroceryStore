using FruitSystem.Data.Classes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FruitSystem.Data.Repositories
{
    public interface IProductRepository
    {
        IQueryable<Product> AllProducts { get; }
        Product GetById(Guid id);
        void AddProducts(IEnumerable<Product> products);
        void Add(Product product);
        void Update(Product product);
        void DeleteAllProducts();
        void SaveChanges();
    }

    public class ProductRepository : IProductRepository
    {
        private readonly GroceryStoreDbContext _dbContext;

        public ProductRepository(GroceryStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Product> AllProducts => 
            _dbContext.Products.OrderByDescending(p => p.UpdatedDated);

        public Product GetById(Guid id) =>
            _dbContext.Products.SingleOrDefault(p => p.Id == id);

        public void AddProducts(IEnumerable<Product> products)
        {
            foreach (Product product in products)
                Add(product);
        }

        public void Add(Product product)
        {
            _dbContext.Products.Add(product);
        }

        public void Update(Product product)
        {
            _dbContext.Entry(product).State = EntityState.Modified;
        }

        public void DeleteAllProducts()
        {
            foreach (Product product in AllProducts.ToList())
                Delete(product);
        }

        public void Delete(Product product)
        {
            _dbContext.Entry(product).State = EntityState.Deleted;
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}