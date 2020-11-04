using System;
using System.Collections.Generic;
using System.Text;

namespace Båthamnen
{
    class Sailboat : Boat
    {
        public int length { get; set; }

        public Sailboat(string identification, int weight, int maxSpeed, int length, int daysInPort = 4, int size = 2) : base(identification, weight, maxSpeed, daysInPort, size)
        {
            this.length = length;
        }

        public override string ToString()
        {
            return base.ToString() + "\t\t" + "Båtlängd: " +length;
        }
        public override int UniqueProp()
        {
            return length;
        }
    }
}
