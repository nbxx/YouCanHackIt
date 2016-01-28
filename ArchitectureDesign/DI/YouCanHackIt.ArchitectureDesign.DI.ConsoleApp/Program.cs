using System;
//using YouCanHackIt.ArchitectureDesign.DI.FirstSolution;
//using YouCanHackIt.ArchitectureDesign.DI.SecondSolution;
using YouCanHackIt.ArchitectureDesign.DI.ThirdSolution;

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
