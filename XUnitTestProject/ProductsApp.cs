using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Xunit;

namespace ProductsApp.Tests
{

    public class ProductsAppShould
    {
        [Fact]
        public void ReturnAnArgumentNullExceptionWhenProductIsNotSpecifiedOrNull()
        {
            //Arrange
            Products sut = new Products();

            //Act
            Action act = () => sut.AddNew(null);

            //Assert
            var exception = Assert.Throws<ArgumentNullException>(act);
        }

        [Fact]
        public void AddsNewProductToOurListOfProducts()
        {
            //Arrange
            Products sut = new Products();
            Product apple = new Product() { Name = "Apple", IsSold = false };

            //Act
            sut.AddNew(apple);

            //Assert
            Assert.Contains("Apple", sut.Items.Select(f => f.Name));
        }

        [Fact]
        public void ReturnsValidationErrorWhenProductNameIsNotSpecifiedOrNull()
        {
            //Arrange
            Products sut = new Products();
            Product productNullName = new Product() { Name = null, IsSold = false };

            //Act
            Action act = () => sut.AddNew(productNullName);

            //Assert
            var exception = Assert.Throws<NameRequiredException>(act);
        }

        [Fact]
        public void NotReturnSoldProducts()
        {
            //Arrange
            Products sut = new Products();
            Product apple = new Product() { Name = "Apple", IsSold = false };

            //Act
            sut.AddNew(apple);
            sut.Sold(apple);

            //Assert
            Assert.DoesNotContain("Apple", sut.Items.Select(f => f.Name));
        }

    }

    internal class Products
    {
        private readonly List<Product> _products = new List<Product>();

        public IEnumerable<Product> Items => _products.Where(t => !t.IsSold);

        public void AddNew(Product product) 
        {
            product = product ??
                throw new ArgumentNullException();
            product.Validate();
            _products.Add(product);
        }

        public void Sold(Product product)
        {
            product.IsSold = true;
        }

    }

    internal class Product
    {
        public bool IsSold { get; set; }
        public string Name { get; set; }

        internal void Validate()
        {
            Name = Name ??
                throw new NameRequiredException();
        }

    }

    [Serializable]
    internal class NameRequiredException : Exception
    {
        public NameRequiredException() { /* ... */ }

        public NameRequiredException(string message) : base(message) { /* ... */ }

        public NameRequiredException(string message, Exception innerException) : base(message, innerException) { /* ... */ }

        protected NameRequiredException(SerializationInfo info, StreamingContext context) : base(info, context) { /* ... */ }
    }
}