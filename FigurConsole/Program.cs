using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Figuren;

namespace FigurConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Kreis k1 = new Kreis(0, 0, 7.5);

            Console.WriteLine(k1);

            Console.Write("\n");
            //PrintEckpunkte(k1.GetEckpunkte());

            Console.Write("\nPress any key to continue...");
            Console.ReadKey();
        }

        static void PrintEckpunkte(Punkt[] punkte)
        {
            for(int idx = 0; idx < punkte.Length; idx++)
            {
                Console.WriteLine("Eckpunkt {0}: {1}, {2}", idx + 1, punkte[idx].x, punkte[idx].y);
            }
        }
    }
}
