using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food
{
    public class Food
    {
        // The coordinates representing where to place the Food object
        private int xCoord, yCoord;


        /// <summary>
        /// Constructor for a Food object.
        /// </summary>
        public Food(int x, int y)
        {
            xCoord = x;
            yCoord = y;
        }
    }
}
