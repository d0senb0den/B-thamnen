using System;
using System.Collections.Generic;
using System.Text;

namespace Båthamnen
{
    class Boat
    {
        public string identification { get; set; }
        public int weight { get; set; }
        public int maxSpeed { get; set; }
        public int daysInPort { get; set; }
        public int size { get; set; }


        public Boat (string identification, int weight, int maxSpeed, int daysInPort, int size)
        {
            this.identification = identification;
            this.weight = weight;
            this.maxSpeed = maxSpeed;
            this.daysInPort = daysInPort;
            this.size = size;
        }

        public override string ToString()
        {
            return this.identification + "\t\t" + this.weight + "\t\t" + (int)(this.maxSpeed * 1.852);
        }
        public virtual int UniqueProp()
        {
            return 0;
        }
    }
}
