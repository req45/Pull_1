using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pull_11;



namespace TestApp
{
    internal class Programm
    {
        static void Main(string[] args)
        {
            Pulling pulling = new Pulling(100, 25, 5, 1, 1.1, 0.51, 0.7, 0.77, 0.55, 0.7, 0.9, 500, 330);
            pulling.CalculationPulling();
            Console.WriteLine("Horosh chel");
            Console.WriteLine(pulling._amountPulling);
            Console.ReadKey();
        }
    }
}