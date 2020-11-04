using System;
using System.Collections.Generic;
using System.Text;

namespace Båthamnen
{
    class Rowboat : Boat
    {
        public int maxPassengers { get; set; }

        public Rowboat (string identification, int weight, int maxSpeed, int maxPassengers, int daysInPort = 1, int size = 1) : base(identification, weight, maxSpeed, daysInPort, size)
        {
            this.maxPassengers = maxPassengers;
        }

        public override string ToString()
        {
            return base.ToString() + "\t\t" + "Max pers: " + maxPassengers;
        }
        public override int UniqueProp()
        {
            return maxPassengers;
        }
    }
}
