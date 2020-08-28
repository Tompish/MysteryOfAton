using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using AtonShared;
using Lidgren.Network;
using SharedLib;

namespace MysteryOfAtonClient
{
    class Networking
    {
        private NetClient _clientNet;
        public bool isConnected { get; private set; }

        public bool initiateClientNetwork(string userName, string password)
        {
            var config = new NetPeerConfiguration("aCode");
            _clientNet = new NetClient(config);

            _clientNet.Start();
            var outMsg = _clientNet.CreateMessage();
            var credentials = new Login() { password = password, userName = userName };
            outMsg.Write((byte)PacketType.Login);
            outMsg.WriteAllProperties(credentials);
            _clientNet.Connect("localhost", 14242, outMsg);

            isConnected = EstablishInfo();
            return isConnected;
        }

        private bool EstablishInfo()
        {
            var time = DateTime.Now;
            NetIncomingMessage inc;

            while (true)
            {
                if (DateTime.Now.Subtract(time).Seconds > 5)
                {
                    _messageFromS = "Connection not found";
                    return false;
                }

                if ((inc = _clientNet.ReadMessage()) == null) continue;

                switch (inc.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        {
                            var data = inc.ReadByte();
                            if (data == (byte)3)
                            {
                                _messageFromS = "Found connection";
                                _clientNet.Recycle(inc);
                                return SendCredentials();
                            }
                            else { _messageFromS = data.ToString(); }

                            return false;
                        }
                    case NetIncomingMessageType.StatusChanged:
                        {
                            var status = inc.ReadByte();
                            if(status == (byte)NetConnectionStatus.InitiatedConnect)
                            {
                                _messageFromS = "Connected!";
                                return true;
                            }
                            return false;
                        }
                    default:
                        {
                            
                            _messageFromS = "Still wrong";
                            return false;
                        }
                }
            }
        }

        private bool SendCredentials()
        {
            var outMsg = _clientNet.CreateMessage();
            var credentials = new Login() { password = "hejsan", userName = "Sandra" };
            outMsg.Write((byte)PacketType.Login);
            outMsg.WriteAllProperties(credentials);

            _clientNet.SendMessage(outMsg, NetDeliveryMethod.ReliableOrdered);

            return true;

            
        }
        public void checkWithServer()
        {
            NetIncomingMessage message;
            while ((message = _clientNet.ReadMessage()) != null)
            {
                switch (message.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        // handle custom messages
                        var data = message.ReadString();
                        _messageFromS = data;
                        break;

                    case NetIncomingMessageType.DiscoveryResponse:
                        {
                            Console.WriteLine("Found server at: " + message.SenderEndPoint + " name: " + message.ReadString());
                                break;
                        }

                    case NetIncomingMessageType.StatusChanged:
                        // handle connection status messages
                        switch (message.ReadByte()){
                            case (byte)NetConnectionStatus.Connected:
                                {
                                    _messageFromS = "Finally connected!";
                                    break;
                                }
                            default:
                                {
                                    _messageFromS = "Some other connection.";
                                    break;
                                }
                        }
                        //_messageFromS = "status has changed to " + (NetConnectionStatus)message.ReadByte();
                        break;

                    case NetIncomingMessageType.DebugMessage:
                        // handle debug messages
                        // (only received when compiled in DEBUG mode)
                        Console.WriteLine(message.ReadString());
                        break;

                    /* .. */
                    default:
                        Console.WriteLine("unhandled message with type: "
                            + message.MessageType);
                        break;
                }
            }
        }
        private string _messageFromS;
        public string messageFromS { get { return _messageFromS; } }
    }
}
