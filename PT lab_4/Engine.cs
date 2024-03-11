using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PT_lab_4
{
    public class Engine : IComparable
    {
        public double displacement { get; set; }
        public int horsePower { get; set; }
        public string model { get; set; }

        public Engine() { }
        public Engine(double displacement, int horsePower, string model)
        {
            this.displacement = displacement;
            this.horsePower = horsePower;
            this.model = model;
        }

        public override string ToString()
        {
            return $"{displacement} {model} {horsePower}hp";
        }

        public int CompareTo(object obj)
        {
            Engine other = obj as Engine;

            if (other == null)
                throw new ArgumentException("Object is not an Engine");
            if (this.horsePower != other.horsePower)
                return this.horsePower.CompareTo(other.horsePower);
            if (this.model != other.model)
                return this.model.CompareTo(other.model);
            return this.displacement.CompareTo(other.displacement);
        }
    }
}
