using System;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace Capstone_Project
{
    public class MainClass
    {

        /// <summary>
        /// Asingment Capstone_Project NMC CIT 110
        /// By steel99xl
        /// GtiHub : https://github.com/steel99xl
        /// </summary>
        static public TcpClient client = null;
       static  public NetworkStream stream = null;
        static public bool running = true;
       /// static public bool threadBalancer = true; // this is used to balance only the thred for sending messages // Now unused
        public static void Main(string[] args)
        {
            //Console.Write("Message to send :");
            //userInput = Console.ReadLine();
            MainClass project = new MainClass();
            string IP = "steel99xl.ddns.net";
            Int32 PORT = 80;
            Connect(IP, PORT);

            ThreadStart RVTH = new ThreadStart(Recive);
            Thread reciveThread = new Thread(RVTH);


            ThreadStart SDTH = new ThreadStart(Messanger);
            Thread sendThread = new Thread(SDTH);

            //reciveThread.Start();
            // sendThread.Start();

            Console.Write("Do you want this instane to send or recive messages? : ");
            string userInput = Console.ReadLine();
            if (userInput.ToUpper() == "SEND" || userInput.ToUpper() == "SENDER")
            {
                running = false;
                Recive();
                sendThread.Start();
            }
            else
            {
                running = false;
                Recive();
                Server_Sender("Client Message Reciver");
                running = true;
                reciveThread.Start();
             }

        }

        static void Messanger()
        {
            while (!running)
            {
                string message;
                Console.Write("Message to send :");
                message = Console.ReadLine();
                if (message == "{quit}")
                {
                    Server_Sender(message);
                    Kill_Connection();
                }
                else
                {
                    Server_Sender(message);
                }
            }
            

        }

        static void Recive()
        {
            do
            {
                Byte[] data = new Byte[256];

                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine(responseData);

                Messanger();
            } while (running);
        }

        static void Kill_Connection()
        {
            stream.Close();
            client.Close();
            Environment.Exit(1);
        }

        static void Server_Sender(string message)
        {
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

            stream.Write(data, 0, data.Length);


        }

        static void Connect(String server, Int32 port)
        {
            try
            {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer 
                // connected to the same address as specified by the server, port
                // combination.
                client = new TcpClient(server, port);
                stream = client.GetStream();

                // Close everything.
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }
    }
}
