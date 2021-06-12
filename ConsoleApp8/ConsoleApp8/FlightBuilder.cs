using System;
using System.Collections.Generic;
using System.Linq;

namespace Gridnine.FlightCodingTest
{
    public class FlightBuilder
    {
        private DateTime _threeDaysFromNow;

        public FlightBuilder()
        {
            _threeDaysFromNow = DateTime.Now.AddDays(3);
        }

        public IList<Flight> GetFlights()
        {
            return new List<Flight>
			           {
                           //A normal flight with two hour duration
			               CreateFlight(_threeDaysFromNow, _threeDaysFromNow.AddHours(2)),

                           //A normal multi segment flight
			               CreateFlight(_threeDaysFromNow, _threeDaysFromNow.AddHours(2), _threeDaysFromNow.AddHours(3), _threeDaysFromNow.AddHours(5)),
                           
                           //A flight departing in the past
                           CreateFlight(_threeDaysFromNow.AddDays(-6), _threeDaysFromNow),

                           //A flight that departs before it arrives
                           CreateFlight(_threeDaysFromNow, _threeDaysFromNow.AddHours(-6)),

                           //A flight with more than two hours ground time
                           CreateFlight(_threeDaysFromNow, _threeDaysFromNow.AddHours(2), _threeDaysFromNow.AddHours(5), _threeDaysFromNow.AddHours(6)),

                            //Another flight with more than two hours ground time
                           CreateFlight(_threeDaysFromNow, _threeDaysFromNow.AddHours(2), _threeDaysFromNow.AddHours(3), _threeDaysFromNow.AddHours(4), _threeDaysFromNow.AddHours(6), _threeDaysFromNow.AddHours(7))
			           };
        }

        private static Flight CreateFlight(params DateTime[] dates)
        {
            if (dates.Length % 2 != 0) throw new ArgumentException("You must pass an even number of dates,", "dates");

            var departureDates = dates.Where((date, index) => index % 2 == 0);
            var arrivalDates = dates.Where((date, index) => index % 2 == 1);

            var segments = departureDates.Zip(arrivalDates,
                                              (departureDate, arrivalDate) =>
                                              new Segment { DepartureDate = departureDate, ArrivalDate = arrivalDate }).ToList();

            return new Flight { Segments = segments };
        }
    }

    public class Flight
    {
        public IList<Segment> Segments { get; set; }
    }

    public class Segment
    {
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
    }

    public class WorkClass
    {
        private List<Flight> Flights { get; set; }
        public List<Flight> GetFlights() { return Flights; }
        public void SetFlights(List<Flight> TakeFlights) { Flights = TakeFlights; }
        private DateTime CurrentTime { get; set; }
        public DateTime GetCurrentTime() { return CurrentTime; }
        public void SetCurrentTime(DateTime TakeCurrentTime) { CurrentTime = TakeCurrentTime; }
        public WorkClass()
        {
            FlightBuilder WorkFlightBuilder = new FlightBuilder();
            SetFlights((List<Flight>)WorkFlightBuilder.GetFlights());
            SetCurrentTime(DateTime.Now);
        }
        public void ShowFlights(List<Flight> A)
        {
            Console.WriteLine("-----Showing Data-----");
            for(int i = 0; i < A.Count; i++)
            {
                Console.WriteLine($"Flight {i}");
                List<Segment> B = (List<Segment>)A[i].Segments;
                for(int j = 0; j < B.Count; j++)
                {
                    Console.WriteLine($"Segment {j}");
                    Console.WriteLine($"DepartureDate:{B[j].DepartureDate}");
                    Console.WriteLine($"ArrivalDate:{B[j].ArrivalDate}");
                }
            }
        }
        public void ShowTestFlights1(List<Flight> A)
        {
            Console.WriteLine("-----StartTest1-----");
            List<Flight> TestFlight = new List<Flight>();
            for (int i = 0; i < A.Count; i++)
            {
                bool Check = true;
                List<Segment> B = (List<Segment>)A[i].Segments;
                for (int j = 0; j < B.Count; j++)
                {
                    if (B[j].DepartureDate < CurrentTime) { Check = false; break; }
                }
                if (Check) { TestFlight.Add(A[i]); }
            }
            ShowFlights(TestFlight);
        }
        public void ShowTestFlights2(List<Flight> A)
        {
            Console.WriteLine("-----StartTest2-----");
            List<Flight> TestFlight = new List<Flight>();
            for (int i = 0; i < A.Count; i++)
            {
                bool Check = true;
                List<Segment> B = (List<Segment>)A[i].Segments;
                for (int j = 0; j < B.Count; j++)
                {
                    if (B[j].DepartureDate > B[j].ArrivalDate) { Check = false; break; }
                }
                if (Check) { TestFlight.Add(A[i]); }
            }
            ShowFlights(TestFlight);
        }
        public void ShowTestFlights3(List<Flight> A)
        {
            Console.WriteLine("-----StartTest3-----");
            TimeSpan TwoHours = new TimeSpan(2,0,0);
            List<Flight> TestFlight = new List<Flight>();
            for (int i = 0; i < A.Count; i++)
            {
                bool Check = true;
                List<Segment> B = (List<Segment>)A[i].Segments;
                if (B.Count > 1)
                {
                    for (int j = 0; j < B.Count - 1; j++)
                    {
                        TimeSpan timeSpan = B[j + 1].DepartureDate - B[j].ArrivalDate;
                        if(timeSpan > TwoHours) { Check = false; break; }
                    }
                    if (Check) { TestFlight.Add(A[i]); }
                }
            }
            ShowFlights(TestFlight);
        }
    }
}

