using LogicGates.DefaultGates;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LogicGates
{
    class Program
    {
        private static List<IGate> gates;
        static void Main(string[] args)
        {
            /* AndGate a1 = new AndGate(2);
             NotGate n1a1 = new NotGate();
             NotGate n2a1 = new NotGate();

             n1a1.ConnectToInput(0, a1, 0);
             n1a1.ConnectToInput(0, a1, 1);

             AndGate a2 = new AndGate(2);
             NotGate n1a2 = new NotGate();

             n1a2.ConnectToInput(0, a2, 0);
             a1.ConnectToInput(0, a2, 1);*/

            //gates = new List<IGate>() { a1, n1a1, n2a1, a2, n1a2 };

            NotGate n1 = new NotGate(1);
            NotGate n2 = new NotGate(1);
            NotGate n3 = new NotGate(1);

            n1.ConnectToInput(0, n2, 0);
            n2.ConnectToInput(0, n3, 0);
            n3.ConnectToInput(0, n1, 0);

            gates = new List<IGate>() { n1, n2, n3 };

            print();
            Gate.Update(n1, print);
            //Gate.Update(a1, print);
        }


        private static void print()
        {
            foreach (IGate gate in gates)
                Console.WriteLine(gate);
            Console.WriteLine();
            Console.ReadKey();
        }
    }

}
