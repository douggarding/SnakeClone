using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Snake
{

    /// <summary>
    /// This is a helper class for drawing a world
    /// We can place one of these panels in our GUI, alongside other controls like buttons
    /// Anything drawn within this panel will use a local coordinate system
    /// </summary>
    public class DrawingPanel : Panel
    {
        /// We need a reference to the world, so we can draw the objects in it
        private World world;

        public DrawingPanel()
        {
            // Setting this property to true prevents flickering
            this.DoubleBuffered = true;
        }

        /// <summary>
        /// Pass in a reference to the world, so we can draw the objects in it
        /// </summary>
        /// <param name="_world"></param>
        public void SetWorld(World _world)
        {
            world = _world;
        }

        /// <summary>
        /// Override the behavior when the panel is redrawn
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            // If we don't have a reference to the world yet, nothing to draw.
            if (world == null)
                return;

            using (System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(Color.Black))
            {
                // Constructor for Rectangle(x-coordinate, y-coordinate, width, height) should be in relation
                // to the upper left-hand corner of the panel. I'm not sure why coordinates 0,0 does not 
                // produce the same coordinates when painted between the top and left walls.

                // Turn on anti-aliasing for smooth round edges
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // Draw top wall
                Rectangle topWall = new Rectangle(0, 0, Size.Width * World.pixelsPerCell, World.pixelsPerCell);
                e.Graphics.FillRectangle(drawBrush, topWall);

                // Draw right wall
                Rectangle rightWall = new Rectangle((world.width - 1) * World.pixelsPerCell, 0, World.pixelsPerCell, world.height * World.pixelsPerCell);
                e.Graphics.FillRectangle(drawBrush, rightWall);

                // Draw bottom wall
                Rectangle bottomWall = new Rectangle(0, (world.height - 1) * World.pixelsPerCell, world.width * World.pixelsPerCell, World.pixelsPerCell);
                e.Graphics.FillRectangle(drawBrush, bottomWall);

                // Draw left wall
                Rectangle leftWall = new Rectangle(0, 0, World.pixelsPerCell, Size.Height * World.pixelsPerCell);
                e.Graphics.FillRectangle(drawBrush, leftWall);
            }

            // Draw the "world" (just one dot) within this panel by using the PaintEventArgs
            world.Draw(e);

        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);

        }
    }
}

