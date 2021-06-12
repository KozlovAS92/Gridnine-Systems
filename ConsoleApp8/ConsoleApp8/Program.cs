using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gridnine.FlightCodingTest;

namespace ConsoleApp8
{
    class Program
    {
        static void Main(string[] args)
        {
            WorkClass Work = new WorkClass();
            Work.ShowFlights(Work.GetFlights());
            Work.ShowTestFlights1(Work.GetFlights());
            Work.ShowTestFlights2(Work.GetFlights());
            Work.ShowTestFlights3(Work.GetFlights());
            Console.ReadKey();
        }
    }
}
