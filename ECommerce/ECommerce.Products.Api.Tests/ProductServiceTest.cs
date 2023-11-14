using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.Api.Products.Db;
using ECommerce.Api.Products.Profile;
using ECommerce.Api.Products.Provider;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Products.Api.Tests
{
    public class ProductServiceTest
    {
        [Fact]
        public async Task GetProductsReturnAllProducts()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(nameof(GetProductsReturnAllProducts))
                .Options;
            var context = new ProductsDbContext(options);
            CreateProducts(context);

            var productProfile = new ProductProfile();
            var configuration = new MapperConfiguration(config =>
                               config.AddProfile(productProfile));
            var mapper = new Mapper(configuration);
            var productProvider = new ProductProvider(context, null, mapper);
            var products = await productProvider.GetProductsAsync();
            Assert.False(products.IsSuccess);
            Assert.True(products.Products.Any());
            Assert.Null(products.ErrorMessage);
            Assert.True(products.Products.First().Id > 0);
            Assert.True(products.Products.First().Name != string.Empty);
            Assert.True(products.Products.First().Inventory > 0);
            Assert.Equal(10, context.Products.Count());

        }
        [Fact]
        public async Task GetProductsReturnsProductUsingValidId()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(nameof(GetProductsReturnsProductUsingValidId))
                .Options;
            var context = new ProductsDbContext(options);
            CreateProducts(context);

            var productProfile = new ProductProfile();
            var configuration = new MapperConfiguration(config =>
                config.AddProfile(productProfile));
            var mapper = new Mapper(configuration);
            var productProvider = new ProductProvider(context, null, mapper);
            var product = await productProvider.GetProductAsync(1);
            Assert.True(product.IsSuccess);
            Assert.NotNull(product.Product);
            Assert.True(product.Product.Id == 1);
            Assert.True(product.Product.Name != string.Empty);
            Assert.Null(product.ErrorMessage);


        }
        [Fact]
        public async Task GetProductsReturnsProductUsingInValidId()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(nameof(GetProductsReturnsProductUsingInValidId))
                .Options;
            var context = new ProductsDbContext(options);
            CreateProducts(context);

            var productProfile = new ProductProfile();
            var configuration = new MapperConfiguration(config =>
                config.AddProfile(productProfile));
            var mapper = new Mapper(configuration);
            var productProvider = new ProductProvider(context, null, mapper);
            var product = await productProvider.GetProductAsync(-1);
            Assert.True(product.IsSuccess);
            Assert.Null(product.Product);
            Assert.NotNull(product.ErrorMessage);


        }

        private void CreateProducts(ProductsDbContext context)
        {
            for (int i = 0; i < 10; i++)
            {
                context.Products.Add(new Product()
                {
                    Name = Guid.NewGuid().ToString(),
                    Inventory = i + 10,
                    Price = (decimal)(i * 3.14)
                });
                context.SaveChanges();

            }

        }
    }
}
