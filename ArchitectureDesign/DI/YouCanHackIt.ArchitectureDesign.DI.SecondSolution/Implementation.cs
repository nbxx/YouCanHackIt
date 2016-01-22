namespace YouCanHackIt.ArchitectureDesign.DI.SecondSolution
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Implementation
    {
        public void BuyProducts()
        {
            var importedCD = new Product("imported CD", 10.99m, SalesTaxType.Default, DutyTaxType.Import);
            var perfume = new Product("perfume", 19.99m, SalesTaxType.Default, DutyTaxType.Domestic);
            var headachePills = new Product("headache pills", 4.65m, SalesTaxType.Medical, DutyTaxType.Domestic);
            var importedChocolates = new Product("imported chocolates", 16.45m, SalesTaxType.Food, DutyTaxType.Import);

            ShoppingCart shoppingCart = new ShoppingCart();

            shoppingCart.Buy(importedCD);
            shoppingCart.Buy(perfume);
            shoppingCart.Buy(headachePills);
            shoppingCart.Buy(importedChocolates);

            Cashier Cashier = new Cashier();

            Cashier.Checkout(shoppingCart);
        }
    }

    public enum SalesTaxType
    {
        Default,
        Food,
        Book,
        Medical,
    }

    public enum DutyTaxType
    {
        Domestic,
        Import,
    }

    public class Product
    {
        public Product(string name, decimal price, SalesTaxType salesTaxType, DutyTaxType dutyTaxType)
        {
            this.Name = name;
            this.Price = price;
            this.SalesTaxType = salesTaxType;
            this.DutyTaxType = dutyTaxType;
        }

        public string Name { get; set; }
        public decimal Price { get; set; }
        public SalesTaxType SalesTaxType { get; set; }
        public DutyTaxType DutyTaxType { get; set; }
    }

    public class OrderItem
    {
        public Product Item { get; set; }
        public int Quantity { get; set; }
        public decimal TotalTaxes { get; set; }
        public decimal TotalPrices { get; set; }
    }

    public class Order
    {
        public Order()
        {
            this.Items = new List<OrderItem>();
        }

        public List<OrderItem> Items { get; set; }
        public decimal TotalTaxes { get; set; }
        public decimal TotalPrices { get; set; }
    }

    public class ShoppingCart
    {
        public ShoppingCart()
        {
            this.Items = new List<Product>();
        }

        public List<Product> Items { get; set; }

        public void Buy(Product item)
        {
            this.Items.Add(item);
        }

        public void Remove(Product item)
        {
            var itemToRemove = this.Items.LastOrDefault(i => i.Name.Equals(item.Name));

            if (itemToRemove != null)
            {
                this.Items.Remove(itemToRemove);
            }
        }
    }

    public class Cashier
    {
        private ITaxService _salesTaxService = new SalesTaxService();
        private ITaxService _dutyTaxService = new DutyTaxService();

        public void Checkout(ShoppingCart shoppingCart)
        {
            var order = this.BuildOrder(shoppingCart.Items);
            this.Print(order);
        }

        public Order BuildOrder(IList<Product> products)
        {
            var result = new Order();
            var productGroup = products.GroupBy(product => product.Name);

            foreach (var item in productGroup)
            {
                var orderItem = new OrderItem { Item = item.First(), Quantity = item.Count() };
                this.ProcessPrice(orderItem);
                result.Items.Add(orderItem);
            }

            result.TotalTaxes = result.Items.Sum(item => item.TotalTaxes);
            result.TotalPrices = result.Items.Sum(item => item.TotalPrices);
            return result;
        }

        private void ProcessPrice(OrderItem orderItem)
        {
            decimal netPrices = orderItem.Item.Price * orderItem.Quantity;
            decimal salesTax = this._salesTaxService.CalculateTax(orderItem.Item.SalesTaxType.ToString(), netPrices);
            decimal dutyTax = this._dutyTaxService.CalculateTax(orderItem.Item.DutyTaxType.ToString(), netPrices);
            decimal totalTaxes = decimal.Round(salesTax + dutyTax, 2, MidpointRounding.AwayFromZero);
            decimal totalPrice = netPrices + totalTaxes;
            orderItem.TotalTaxes = totalTaxes;
            orderItem.TotalPrices = totalPrice;
        }

        private void Print(Order order)
        {
            Console.WriteLine();

            foreach (var item in order.Items)
            {
                Console.WriteLine("{0} {1} : {2:F2}", item.Quantity, item.Item.Name, item.TotalPrices);
            }

            Console.WriteLine("Sales Taxes: {0:F2}", order.TotalTaxes);
            Console.WriteLine("Total: {0:F2}", order.TotalPrices);
        }
    }

    public interface ICalculator
    {
        decimal Calculate(decimal value);
    }

    public class DefaultTaxCalculator : ICalculator
    {
        public decimal Calculate(decimal value)
        {
            return value * 0.09m;
        }
    }

    public class FoodTaxCalculator : ICalculator
    {
        public decimal Calculate(decimal value)
        {
            return value * 0m;
        }
    }

    public class BookTaxCalculator : ICalculator
    {
        public decimal Calculate(decimal value)
        {
            return value * 0m;
        }
    }

    public class MedicalTaxCalculator : ICalculator
    {
        public decimal Calculate(decimal value)
        {
            return value * 0m;
        }
    }

    public class DomesticTaxCalculator : ICalculator
    {
        public decimal Calculate(decimal value)
        {
            return value * 0m;
        }
    }

    public class ImportTaxCalculator : ICalculator
    {
        public decimal Calculate(decimal value)
        {
            return value * 0.04m;
        }
    }

    public interface ITaxService
    {
        decimal CalculateTax(string taxType, decimal value);
    }

    public class SalesTaxService : ITaxService
    {
        public decimal CalculateTax(string taxType, decimal value)
        {
            ICalculator calculator = null;

            switch (taxType)
            {
                case "Food":
                    calculator = new FoodTaxCalculator();
                    break;
                case "Book":
                    calculator = new BookTaxCalculator();
                    break;
                case "Medical":
                    calculator = new MedicalTaxCalculator();
                    break;
                case "Default":
                default:
                    calculator = new DefaultTaxCalculator();
                    break;
            }

            return calculator.Calculate(value);
        }
    }

    public class DutyTaxService : ITaxService
    {
        public decimal CalculateTax(string taxType, decimal value)
        {
            ICalculator calculator = null;

            switch (taxType)
            {
                case "Domestic":
                    calculator = new DomesticTaxCalculator();
                    break;
                case "Import":
                    calculator = new ImportTaxCalculator();
                    break;
                default:
                    calculator = new DomesticTaxCalculator();
                    break;
            }

            return calculator.Calculate(value);
        }
    }
}
