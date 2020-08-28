using System;
using System.Net.NetworkInformation;
using Lidgren.Network;

namespace MysteryOfAtonServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new Server();

            server.InitiateServer();

            DateTime clock = DateTime.Now;
            while(1==1)
            {
                
                //System.Threading.Thread.Sleep(60);
                server.checkMessages();

                if (DateTime.Now.Subtract(clock).TotalMilliseconds > 16)
                {
                    server.SendMessages();
                    clock = DateTime.Now;
                }
            }


        }
    }
}
