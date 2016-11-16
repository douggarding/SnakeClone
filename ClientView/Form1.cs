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
    public partial class Form1 : Form
    {
        private SocketState theServer;
        private World world;

        public Form1()
        {
            InitializeComponent();
        }

        private void FirstContact(SocketState ss)
        {
            Console.Out.WriteLine("First Contact");
            ss.callbackFunction = RecieveStartup;
            Networking.Send(ss, NameTextBox.Text+'\n');
        }

        private void RecieveStartup(SocketState ss)
        {
            Console.Out.WriteLine("Recieve Startup");
            String[] world = ss.sb.ToString().Split('\n');
            this.world = new World(int.Parse(world[1]), int.Parse(world[2]));
            ss.callbackFunction = RecieveWorld;
            Networking.GetData(ss);

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
