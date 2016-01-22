using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouCanHackIt.ArchitectureDesign.DI.SecondSolution;

namespace YouCanHackIt.ArchitectureDesign.DI.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Implementation impl = new Implementation();
            impl.BuyProducts();

            Console.ReadLine();
        }
    }
}
