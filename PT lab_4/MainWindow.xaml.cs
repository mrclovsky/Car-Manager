using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;

namespace PT_lab_4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CarBindingList myCarsBindingList;
        private BindingSource carBindingSource;
        private Dictionary<string, bool> sorting = new Dictionary<string, bool>();

        private List<Car> myCars = new List<Car>(){
            new Car("E250", new Engine(1.8, 204, "CGI"), 2009),
            new Car("E350", new Engine(3.5, 292, "CGI"), 2009),
            new Car("A6", new Engine(2.5, 187, "FSI"), 2012),
            new Car("A6", new Engine(2.8, 220, "FSI"), 2012),
            new Car("A6", new Engine(3.0, 295, "TFSI"), 2012),
            new Car("A6", new Engine(2.0, 175, "TDI"), 2011),
            new Car("A6", new Engine(3.0, 309, "TDI"), 2011),
            new Car("S6", new Engine(4.0, 414, "TFSI"), 2012),
            new Car("S8", new Engine(4.0, 513, "TFSI"), 2012)
        };

        public MainWindow()
        {
            myCarsBindingList = new CarBindingList(myCars);
            carBindingSource = new BindingSource();

            InitializeComponent();
            InitiateComboBox();
            InitiateSortingDictionary();
            UpdateGrid();

            DataOperations.LinqQueries(myCars);
            DataOperations.PerformOperations(myCars);
            DataOperations.sortAndFind(myCars, myCarsBindingList);
        }

        private void Sort(object sender, RoutedEventArgs arg)
        {
            var columnHeader = sender;
            string columnName = columnHeader.ToString().Split(' ').Last().ToLower();
            if (columnName != "model" && columnName != "motor" && columnName != "year")
            {
                return;
            }

            bool isAscending = sorting[columnName];
            if (isAscending == true)
            {
                myCarsBindingList.Sort(columnName, ListSortDirection.Descending);
            }
            else
            {
                myCarsBindingList.Sort(columnName, ListSortDirection.Ascending);
            }
            sorting[columnName] = !isAscending;
            UpdateGrid();
        }

        private void SearchButton(object sender, RoutedEventArgs arg)
        {
            AddNewItems();
            List<Car> matchingCars;
            string value = SearchTextBox.Text;
            string property = ComboBox.SelectedItem.ToString().ToLower();
            if (value == "")
            {
                return;
            }
            matchingCars = myCarsBindingList.Find(property, value);
            myCarsBindingList = new CarBindingList(matchingCars);
            UpdateGrid();
        }

        private void DeleteButton(object sender, RoutedEventArgs arg)
        {
            try
            {
                Car car = (Car)dataGridView1.SelectedItem;
                myCarsBindingList.Remove(car);
                myCars.Remove(car);
                UpdateGrid();
            }
            catch (InvalidCastException exception)
            {
                return;
            }
        }

        private void ReloadButton(object sender, RoutedEventArgs arg)
        {
            myCarsBindingList = new CarBindingList(myCars);
            UpdateGrid();
        }

        private void AddNewItems()
        {
            foreach (var car in myCarsBindingList)
            {
                if (!myCars.Contains(car))
                {
                    myCars.Add(car);
                }
            }
        }

        private void InitiateSortingDictionary()
        {
            sorting.Add("model", false);
            sorting.Add("motor", false);
            sorting.Add("year", false);
        }

        private void InitiateComboBox()
        {
            BindingList<string> list = new BindingList<string>();
            list.Add("Model");
            list.Add("Year");
            list.Add("Displacement");
            list.Add("Motor model");
            list.Add("Horsepower");
            ComboBox.ItemsSource = list;
            ComboBox.SelectedItem = list[0];
        }

        private void UpdateGrid()
        {
            carBindingSource.DataSource = myCarsBindingList;
            dataGridView1.ItemsSource = carBindingSource;
        }
    }
}
