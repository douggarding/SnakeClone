using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Food;

namespace Snake
{
    public class Snake
    {
        /// <summary>
        /// Constructor for a Snake.
        /// </summary>
        public Snake()
        {

        }

        
        /// <summary>
        /// If the snake collides with another snake, it should dissapear and turn into food.
        /// </summary>
        /// <param name="secondSnake"></param>
        /// <returns></returns>
        public bool CollidesWithSnake(Snake secondSnake)
        {
            return false;
        }


        /// <summary>
        /// If the Snake collides with food, the food should dissapear and the Snake should
        /// grow by one unit.
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public bool CollidesWithFood(Food.Food food)
        {
            return false;
        }


        /// <summary>
        /// If the Snake collides with a wall, it should dissapear and turn into food.
        /// grow by one unit.
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public bool CollidesWithWall()
        {
            return false;
        }
    }
}
