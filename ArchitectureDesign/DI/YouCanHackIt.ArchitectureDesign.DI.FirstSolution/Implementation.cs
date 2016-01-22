namespace YouCanHackIt.ArchitectureDesign.DI.FirstSolution
{
    using System;

    public class Implementation
    {
        public void BuyProducts()
        {
            var importedCD = new Product("imported CD", 10.99m, false, true);
            var perfume = new Product("perfume", 19.99m, false, false);
            var headachePills = new Product("headache pills", 4.65m, true, false);
            var importedChocolates = new Product("imported chocolates", 16.45m, true, true);

            ShoppingCart shoppingCart = new ShoppingCart();

            shoppingCart.Buy(importedCD);
            shoppingCart.Buy(perfume);
            shoppingCart.Buy(headachePills);
            shoppingCart.Buy(importedChocolates);

            shoppingCart.Checkout();
        }
    }

    public class Product
    {
        public Product(string name, decimal price, bool isTaxFree, bool isImport)
        {
            this.Name = name;
            this.Price = price;
            this.IsTaxFree = isTaxFree;
            this.IsImport = isImport;
        }

        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool IsTaxFree { get; set; }
        public bool IsImport { get; set; }
    }

    public class ShoppingCart
    {
        public decimal TotalPrices { get; set; }
        public decimal TotalTaxes { get; set; }

        public void Buy(Product product)
        {
            decimal taxes = 0m;

            if (!product.IsTaxFree)
            {
                taxes += product.Price * 0.09m;
            }

            if (product.IsImport)
            {
                taxes += product.Price * 0.04m;
            }

            taxes = decimal.Round(taxes, 2, MidpointRounding.AwayFromZero);

            this.TotalPrices += product.Price + taxes;
            this.TotalTaxes += taxes;

            Console.WriteLine(product.Name);
            Console.WriteLine(product.Price + taxes);
        }

        public void Checkout()
        {
            Console.WriteLine("Total Taxes:");
            Console.WriteLine(this.TotalTaxes);
            Console.WriteLine("Total Prices:");
            Console.WriteLine(this.TotalPrices);
        }
    }
}
