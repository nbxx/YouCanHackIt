namespace YouCanHackIt.ArchitectureDesign.DI.ThirdSolution
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

            #region Manually Dependency Injection.

            // Builds all calculators.
            List<ICalculator> calculators = new List<ICalculator>
            {
                new DefaultTaxCalculator(),
                new FoodTaxCalculator(),
                new BookTaxCalculator(),
                new MedicalTaxCalculator(),
                new DomesticTaxCalculator(),
                new ImportTaxCalculator()
            };

            // Builds ICalculatorResolver instance and injects all calculators.
            ICalculatorResolver calculatorResolver = new CalculatorResolver(calculators);

            // Builds ITaxService instance and injects ICalculatorResolver.
            ITaxService taxService = new TaxService(calculatorResolver);

            // Builds Cashier instance and injects ITaxService.
            Cashier Cashier = new Cashier(taxService);

            #endregion

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
        private readonly ITaxService _taxService;

        public Cashier(ITaxService taxService)
        {
            this._taxService = taxService;
        }

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
            decimal salesTax = this._taxService.CalculateTax(orderItem.Item.SalesTaxType.ToString(), netPrices);
            decimal dutyTax = this._taxService.CalculateTax(orderItem.Item.DutyTaxType.ToString(), netPrices);
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
        string Name { get; }
        decimal Calculate(decimal value);
    }

    public class DefaultTaxCalculator : ICalculator
    {
        public DefaultTaxCalculator()
        {
            this.Name = "Default";
        }

        public string Name { get; private set; }

        public decimal Calculate(decimal value)
        {
            return value * 0.09m;
        }
    }

    public class FoodTaxCalculator : ICalculator
    {
        public FoodTaxCalculator()
        {
            this.Name = "Food";
        }

        public string Name { get; private set; }

        public decimal Calculate(decimal value)
        {
            return value * 0m;
        }
    }

    public class BookTaxCalculator : ICalculator
    {
        public BookTaxCalculator()
        {
            this.Name = "Book";
        }

        public string Name { get; private set; }

        public decimal Calculate(decimal value)
        {
            return value * 0m;
        }
    }

    public class MedicalTaxCalculator : ICalculator
    {
        public MedicalTaxCalculator()
        {
            this.Name = "Medical";
        }

        public string Name { get; private set; }

        public decimal Calculate(decimal value)
        {
            return value * 0m;
        }
    }

    public class DomesticTaxCalculator : ICalculator
    {
        public DomesticTaxCalculator()
        {
            this.Name = "Domestic";
        }

        public string Name { get; private set; }

        public decimal Calculate(decimal value)
        {
            return value * 0m;
        }
    }

    public class ImportTaxCalculator : ICalculator
    {
        public ImportTaxCalculator()
        {
            this.Name = "Import";
        }

        public string Name { get; private set; }

        public decimal Calculate(decimal value)
        {
            return value * 0.04m;
        }
    }

    public interface ICalculatorResolver
    {
        ICalculator Resolve(string name);
    }

    public class CalculatorResolver : ICalculatorResolver
    {
        private readonly IEnumerable<ICalculator> _calculators;

        public CalculatorResolver(IEnumerable<ICalculator> calculators)
        {
            this._calculators = calculators;
        }

        public ICalculator Resolve(string name)
        {
            ICalculator calculator = this._calculators.FirstOrDefault(calc => calc.Name == name);

            if (calculator == null)
            {
                throw new ArgumentException("Calculator was not found", name);
            }

            return calculator;
        }
    }

    public interface ITaxService
    {
        decimal CalculateTax(string taxType, decimal value);
    }

    public class TaxService : ITaxService
    {
        private readonly ICalculatorResolver _calculatorResolver;

        public TaxService(ICalculatorResolver calculatorResolver)
        {
            this._calculatorResolver = calculatorResolver;
        }

        public decimal CalculateTax(string taxType, decimal value)
        {
            ICalculator calculator = this._calculatorResolver.Resolve(taxType);
            return calculator.Calculate(value);
        }
    }
}
