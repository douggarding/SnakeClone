using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetworkController;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using Snake;

namespace ClientView
{
    public partial class SnakeGame : Form
    {
        private SocketState theServer;
        private World world;

        public SnakeGame()
        {
            InitializeComponent();
        }

        private void FirstContact(SocketState ss)
        {
            Console.Out.WriteLine("First Contact");
            ss.callbackFunction = RecieveStartup;
            //Sends the server the players name
            Networking.Send(ss, NameTextBox.Text+'\n');
        }

        private void RecieveStartup(SocketState ss)
        {
            Console.Out.WriteLine("Recieve Startup");
            //Recieves the world parameters in the string builder seperated by \n characters. The first number is the players ID #.
            //the second and third numbers ar the width and height of the world
            String[] worldParameters = ss.sb.ToString().Split('\n');

            //Initializes the world
            world = new World(int.Parse(worldParameters[1]), int.Parse(worldParameters[2]));
            gamePanel.SetWorld(world);

            //Sets the gamePanel and window to the correct size
            this.Invoke(new MethodInvoker(
            () => gamePanel.Size = new Size(world.width * World.pixelsPerCell, world.height * World.pixelsPerCell)
            ));

            this.Invoke(new MethodInvoker(
            () => this.Size = new Size(world.width * World.pixelsPerCell + 50, world.height * World.pixelsPerCell +100 )
            ));

            ss.callbackFunction = RecieveWorld;
            Networking.GetData(ss);

            // TODO: We would also need to update this form's size to expand or shrink to fit the panel
            // this.Size = (large enough to hold all buttons, panels, etc)
        }

        private static void RecieveWorld(SocketState ss)
        {
            //Console.Out.WriteLine(ss.sb);
            Networking.GetData(ss);
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            theServer = Networking.ConnectToServer(FirstContact, HostTextBox.Text);
        }
    }
}
