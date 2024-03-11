using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.DirectoryServices;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;

namespace PT_lab_4
{
    class CarBindingList : BindingList<Car>
    {
        private PropertyDescriptor sortProperty;
        private ListSortDirection sortDirection;
        private bool isSorted = false;
        private ArrayList indicesList;

        public CarBindingList(List<Car> list)
        {
            foreach (var car in list)
            {
                Add(car);
            }
        }

        protected override bool SupportsSortingCore => true;
        protected override bool SupportsSearchingCore => true;
        protected override ListSortDirection SortDirectionCore => sortDirection;
        protected override PropertyDescriptor SortPropertyCore => sortProperty;
        protected override bool IsSortedCore => isSorted;


        public void Sort(string property, ListSortDirection direction)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(Car));
            PropertyDescriptor prop = properties.Find(property, true);
            ApplySortCore(prop, direction);
        }

        protected override void ApplySortCore(PropertyDescriptor property, ListSortDirection direction)
        {
            List<Car> list = (List<Car>)Items;
            
            if (property.PropertyType.GetInterface("IComparable") == null)
            {
                throw new NotSupportedException("IComparable not included");
            }

            list.Sort((x, y) =>
            {
                object xValue = property.GetValue(x);
                object yValue = property.GetValue(y);
                return ((IComparable)xValue).CompareTo(yValue);
            });

            if (direction == ListSortDirection.Descending)
            {
                list.Reverse();
            }

            isSorted = true;
            sortProperty = property;
            sortDirection = direction;
            OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        private int FindCore(PropertyDescriptor property, object key, bool isEngine)
        {
            indicesList = new ArrayList();
            int found = 0;

            for (int i = 0; i < Count; i++)
            {
                if (isEngine)
                {
                    if (property.GetValue(Items[i].motor).Equals(key))
                    {
                        found++;
                        indicesList.Add(i);
                    }
                }
                else
                {
                    if (property.GetValue(Items[i]).Equals(key))
                    {
                        found++;
                        indicesList.Add(i);
                    }
                }
            }
            return found;
        }

        public List<Car> Find(string property, string value)
        {
            List<Car> matchingCars = new List<Car>();
            PropertyDescriptorCollection properties;
            bool isEngine = false;
            object key;

            if (property == "motor model")
            {
                property = "model";
                isEngine = true;
                key = value;
            }
            else if (property == "displacement")
            {
                isEngine = true;
                key = Double.Parse(value);
            }
            else if(property == "horsepower")
            {
                property = "horsePower";
                isEngine = true;
                key = Int32.Parse(value);
            }
            else if(property == "year")
            {
                key = Int32.Parse(value);
            }
            else
            {
                key = value;
            }

            if (isEngine)
            {
                properties = TypeDescriptor.GetProperties(typeof(Engine));
            }
            else
            {
                properties = TypeDescriptor.GetProperties(typeof(Car));
            }
            PropertyDescriptor prop = properties.Find(property, true);

            
            if (FindCore(prop, key, isEngine) > 0)
            {
                foreach (var index in indicesList)
                {
                    int element = Convert.ToInt32(index);

                    matchingCars.Add(Items[element]);
                }
                return matchingCars;
            }
            else return null;
        }


    }
}