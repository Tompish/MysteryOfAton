using System;
using System.Collections.Generic;
using System.Text;
using Lidgren.Network;
using AtonShared;
using SharedLib;

namespace MysteryOfAtonServer
{
    class Server
    {
        private NetServer _netServer;
        private int count = 0;
        
        public Server()
        {
            var config = new NetPeerConfiguration("aCode")
            { Port = 14242 };
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            _netServer = new NetServer(config);
            
        }

        public void InitiateServer()
        {
            Console.WriteLine("Initiating");
            _netServer.Start();
        }

        public void checkMessages()
        {
            NetIncomingMessage message;
            while ((message = _netServer.ReadMessage()) != null)
            {
                switch (message.MessageType)
                {
                    case NetIncomingMessageType.ConnectionApproval:
                        {
                            Console.WriteLine("New connection...");
                            var outMsg = _netServer.CreateMessage();
                            
                            var msg = message.ReadByte();

                            //Login credentials = new Login();

                            //message.ReadAllProperties(credentials);

                            if (msg == (byte)PacketType.Login)
                            {
                                Console.WriteLine("Connection accepted...");
                                var login = new Login();

                                message.ReadAllProperties(login);
                                Console.WriteLine("User: " + login.userName + ", pw: " + login.password);
                                if(login.password == "hejsan" && login.userName == "Sandra")
                                {
                                    Console.WriteLine("Correct credentials!");
                                    message.SenderConnection.Approve();
                                }
                                else
                                {
                                    Console.WriteLine("Login denied...");
                                    message.SenderConnection.Deny();
                                }
                            }
                            else
                            {
                                Console.WriteLine("Needs to be login");
                                message.SenderConnection.Deny();
                            }
                            break;
                        }
                    case NetIncomingMessageType.DiscoveryRequest:
                        {
                            var msg = message.ReadString();
                            Console.WriteLine(msg);
                            break;
                        }
                    case NetIncomingMessageType.Data:
                        {
                            var packetType = message.ReadByte();

                            if(packetType == (byte)PacketType.Login)
                            {
                            }
                            break;
                        }
                    default:
                        {
                            var msg = message.ReadByte();
 
                            Console.WriteLine(message.MessageType+" : "+ (NetConnectionStatus)msg);
                            break;
                        }
                }
                count++;
            }
        }
        public void SendMessages()
        {

                var msg = _netServer.CreateMessage(count.ToString());
                
                _netServer.SendToAll(msg, NetDeliveryMethod.ReliableOrdered);
                count++;
            
        }

        public void StopServer()
        {
            _netServer.Shutdown("Bye");
        }
    }
}
