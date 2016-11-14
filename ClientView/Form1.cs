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

namespace ClientView
{
    public partial class Form1 : Form
    {
        private SocketState theServer;
        public Form1()
        {
            InitializeComponent();
        }

        private delegate void ConnectDelegate(SocketState ss);
        private delegate void ReceiveDelegate(IAsyncResult ar);

        private static void ProcessMessage(SocketState ss)
        {
            string totalData = ss.sb.ToString();
            string[] parts = Regex.Split(totalData, @"(?<=[\n])");

            // Loop until we have processed all messages.
            // We may have received more than one.

            foreach (string p in parts)
            {
                // Ignore empty strings added by the regex splitter
                if (p.Length == 0)
                    continue;
                // The regex splitter will include the last string even if it doesn't end with a '\n',
                // So we need to ignore it if this happens. 
                if (p[p.Length - 1] != '\n')
                    break;

                // Display the message
                // "messages" is the big message text box in the form.
                // We must use a MethodInvoker, because only the thread that created the GUI can modify it.
                System.Diagnostics.Debug.WriteLine("appending \"" + p + "\"");
                // this.Invoke(new MethodInvoker(
                // () => messages.AppendText(p)));

                // Then remove it from the SocketState's growable buffer
                ss.sb.Remove(0, p.Length);
            }
        }

        private static void ReceiveCallback(IAsyncResult ar)
        {
            Console.Out.WriteLine("recieved");
        }

        ConnectDelegate connect = ProcessMessage;
        ReceiveDelegate recieve = ReceiveCallback;

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            theServer = Networking.ConnectToServer(connect, HostTextBox.Text);
        }

    }
}
