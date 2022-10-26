using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework_4_fin
{
    internal class Interval
    {
        public int up;
        public int down;

        public Interval(int down, int up)
        {
            this.up = up;
            this.down = down;
        }

        public override string ToString()
        {
            return down.ToString() + " - " + up.ToString();
        }
    }
}
