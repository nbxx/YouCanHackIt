namespace YouCanHackIt.ArchitectureDesign.DI.Samples
{
    public class ConstructorInjection
    {
        public void Run()
        {
            ICalculator calculator1 = new DefaultTaxCalculator();
            ICalculator calculator2 = new FoodTaxCalculator();
            ICalculator calculator3 = new BookTaxCalculator();

            TaxClient taxClient1 = new TaxClient(calculator1);
            var tax1 = taxClient1.GetTax(10m); //tax1=0.9

            TaxClient taxClient2 = new TaxClient(calculator2);
            var tax2 = taxClient2.GetTax(10m); //tax2=0

            TaxClient taxClient3 = new TaxClient(calculator3);
            var tax3 = taxClient3.GetTax(10m); //tax3=0.1
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
            private ICalculator _calculator;

            public TaxClient(ICalculator calculator)
            {
                this._calculator = calculator;
            }

            public decimal GetTax(decimal price)
            {
                return this._calculator.Calculate(price);
            }
        }
    }
}
