using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkController
{
    /// <summary>
    /// This class holds all the necessary state to handle a client connection
    /// Note that all of its fields are public because we are using it like a "struct"
    /// It is a simple collection of fields
    /// </summary>
    public class SocketState
    {
        public Socket theSocket;
        public int ID;
        public Delegate callbackFunction;

        // This is the buffer where we will receive message data from the client
        public byte[] messageBuffer = new byte[1024];

        // This is a larger (growable) buffer, in case a single receive does not contain the full message.
        public StringBuilder sb = new StringBuilder();

        public SocketState(Socket s, int id, Delegate cb)
        {
            theSocket = s;
            ID = id;
            callbackFunction = cb;
        }
    }

    public class Networking
    {
        public const int DEFAULT_PORT = 11000;
        private static SocketState theServer;

        public static SocketState ConnectToServer(Delegate callbackFunction, string hostName)
        {
            System.Diagnostics.Debug.WriteLine("connecting  to " + hostName);

            // Connect to a remote device.
            try
            {

                // Establish the remote endpoint for the socket.
                IPHostEntry ipHostInfo;
                IPAddress ipAddress = IPAddress.None;

                // Determine if the server address is a URL or an IP
                try
                {
                    ipHostInfo = Dns.GetHostEntry(hostName);
                    bool foundIPV4 = false;
                    foreach (IPAddress addr in ipHostInfo.AddressList)
                        if (addr.AddressFamily != AddressFamily.InterNetworkV6)
                        {
                            foundIPV4 = true;
                            ipAddress = addr;
                            break;
                        }
                    // Didn't find any IPV4 addresses
                    if (!foundIPV4)
                    {
                        System.Diagnostics.Debug.WriteLine("Invalid addres: " + hostName);
                        return null;
                    }
                }
                catch (Exception e1)
                {
                    // see if host name is actually an ipaddress, i.e., 155.99.123.456
                    System.Diagnostics.Debug.WriteLine("using IP");
                    ipAddress = IPAddress.Parse(hostName);
                }

                // Create a TCP/IP socket.
                Socket socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);

                theServer = new SocketState(socket, -1, callbackFunction);

                theServer.theSocket.BeginConnect(ipAddress, Networking.DEFAULT_PORT, ConnectedToServer, theServer);

                theServer.callbackFunction = callbackFunction;

                return theServer;

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Unable to connect to server. Error occured: " + e);
                return  null;
            }
            
        }

        public static void ConnectedToServer(IAsyncResult state_in_an_ar_object)
        {
            SocketState ss = (SocketState)state_in_an_ar_object.AsyncState;

            try
            {
                // Complete the connection.
                ss.theSocket.EndConnect(state_in_an_ar_object);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Unable to connect to server. Error occured: " + e);
                return;
            }

            // TODO: If we had a "EventProcessor" delagate stored in the state, we could call that,
            //       instead of hard-coding a method to call.
            //AwaitDataFromServer(ss);
        }

        // TODO: Move all networking code to this class.
        // Networking code should be completely general-purpose, and useable by any other application.
        // It should contain no references to a specific project.
    }

}
