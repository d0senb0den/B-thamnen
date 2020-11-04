using System;
using System.Collections.Generic;
using System.Text;

namespace Båthamnen
{
    class Motorboat : Boat
    {
        public int horsePower { get; set; }

        public Motorboat(string identification, int weight, int maxSpeed, int horsePower, int daysInPort = 3, int size = 1) : base(identification, weight, maxSpeed, daysInPort, size)
        {
            this.horsePower = horsePower;
        }

        public override string ToString()
        {
            return base.ToString() + "\t\t" + "Hästkrafter: " + horsePower;
        }
        public override int UniqueProp()
        {
            return horsePower;
        }
    }
}
