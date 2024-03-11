using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace PT_lab_4
{
    class DataOperations
    {
        public static void LinqQueries(List<Car>myCars)
        {
            var expressionSyntax = from car in myCars
                                   where car.model == "A6"
                                   select new
                                   {
                                       EngineType = car.motor.model.ToString() == "TDI" ? "Diesel" : "Petrol",
                                       Hppl = car.motor.horsePower / car.motor.displacement,
                                   };

            var methodSyntax = expressionSyntax
                 .GroupBy(car => car.EngineType)
                 .Select(car => new {
                     EngineType = car.Key,
                     AverageHppl = car.Average(c => c.Hppl)
                 })
                 .OrderByDescending(car => car.AverageHppl);

            Console.WriteLine("Linq query");
            foreach (var car in methodSyntax)
            {
                Console.WriteLine("Engine Type: {0}  Average Hppl: {1}", car.EngineType, car.AverageHppl);
            }
            Console.WriteLine();
        }

        public static void PerformOperations(List<Car> myCars)
        {
            List<Car> myCarsCopy = new List<Car>(myCars);
            Func<Car, Car, int> arg1 = (car1, car2) => car2.motor.horsePower.CompareTo(car1.motor.horsePower);
            Predicate<Car> arg2 = car => car.motor.model == "TDI";
            Action<Car> arg3 = car => System.Windows.MessageBox.Show(car.ToString(), "Car");

            myCarsCopy.Sort(new Comparison<Car>(arg1));
            myCarsCopy.FindAll(arg2).ForEach(arg3);
        }

        public static void sortAndFind(List<Car> myCars, CarBindingList bindingList)
        {
            CarBindingList myCarsBindingList = bindingList;
            myCarsBindingList.Sort("motor", ListSortDirection.Descending);
            
            Console.WriteLine("Sorted by motor");
            foreach (var car in myCarsBindingList)
            {
                Console.WriteLine(car.ToString());
            }
            Console.WriteLine();
            Console.WriteLine("A6 model");
            List<Car> matchingCars;
            matchingCars = myCarsBindingList.Find("Model", "A6");
            foreach (var car in matchingCars)
            {
                Console.WriteLine(car.ToString());
            }
        }

    }
}
