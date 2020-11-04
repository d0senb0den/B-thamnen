using System;
using System.Collections.Generic;
using System.Text;

namespace Båthamnen
{
    class Cargoship : Boat
    {
        public int containers { get; set; }
        public Cargoship(string identification, int weight, int maxSpeed, int containers, int daysInPort = 6, int size = 4) : base(identification, weight, maxSpeed, daysInPort, size)
        {
            this.containers = containers;
        }

        public override string ToString()
        {
            return base.ToString() + "\t\t" + "Containers: " + containers;
        }
        public override int UniqueProp()
        {
            return containers;
        }
    }
}
