namespace YouCanHackIt.ArchitectureDesign.DI.Samples
{
    public class SetterInjection
    {
        public void Run()
        {
            ICalculator calculator1 = new DefaultTaxCalculator();
            ICalculator calculator2 = new FoodTaxCalculator();
            ICalculator calculator3 = new BookTaxCalculator();

            TaxClient taxClient = new TaxClient();

            taxClient.Calculator = calculator1;
            var tax1 = taxClient.GetTax(10m); //tax1=0.9

            taxClient.Calculator = calculator2;
            var tax2 = taxClient.GetTax(10m); //tax2=0

            taxClient.Calculator = calculator3;
            var tax3 = taxClient.GetTax(10m); //tax3=0.1
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
                return value * 0.01m;
            }
        }

        public class TaxClient
        {
            public ICalculator Calculator { get; set; }

            public decimal GetTax(decimal price)
            {
                return this.Calculator.Calculate(price);
            }
        }
    }
}
