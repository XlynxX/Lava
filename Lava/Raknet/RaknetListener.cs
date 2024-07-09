using SharpRakNet.Network;
using SharpRakNet.Protocol.Raknet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lava.Raknet
{
    public class RaknetListener
    {
        //public PacketReceivedDelegate PacketReceived = delegate { };
        public static readonly byte RAKNET_PROTOCOL_VERSION = 0xA;
        public static readonly ushort RAKNET_CLIENT_MTU = 1400;
        public static readonly int RECEIVE_TIMEOUT = 60000;

        private static readonly Dictionary<int, List<(Type, Delegate)>> Listeners = new Dictionary<int, List<(Type, Delegate)>>();
        private Dictionary<IPEndPoint, RaknetSession> Sessions = new Dictionary<IPEndPoint, RaknetSession>();
        public UdpClient socket;
        private bool _closed = true;
        private byte rak_version = 0xB;
        private ulong guid;
        public RaknetListener(IPEndPoint ip)
        {
            guid = (ulong)new Random().NextDouble() * ulong.MaxValue;
            socket = new UdpClient(ip);
            _closed = false;
            StartReceiving();
        }
        private async void StartReceiving()
        {
            while (!_closed)
            {
                try
                {
                    UdpReceiveResult result = await socket.ReceiveAsync();
                    OnPacketReceived(result.RemoteEndPoint, result.Buffer);
                    //PacketReceived?.Invoke(result.RemoteEndPoint, result.Buffer);
                }
                catch (ObjectDisposedException)
                {
                    // Socket has been closed, exit the loop
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    Console.WriteLine($"StackTrace: {ex.StackTrace}");
                }
            }
        }

        private void OnPacketReceived(IPEndPoint address, byte[] data)
        {
            switch ((PacketID)data[0])
            {
                case PacketID.UnconnectedPing1:
                    HandleUnconnectedPing1(address, data);
                    break;
                case PacketID.OpenConnectionRequest1:
                    HandleOpenConnectionRequest1(address, data);
                    break;
                case PacketID.OpenConnectionRequest2:
                    HandleOpenConnectionRequest2(address, data);
                    break;
                //case PacketID.Game:
                //    OnGamePacketReceived(address, data);
                //    break;
                default:
                    {
                        //if (data[0] >= 0x80 && data[0] <= 0x8D)
                        //{
                        //    throw new NotImplementedException($"Unhandled Raknet Frame Set Packet");
                        //}

                        //throw new NotImplementedException($"Unhandled PacketID 0x{data[0]:X2} ({(PacketID)data[0]})");
                        if (Sessions.TryGetValue(address, out var session))
                        {
                            HandleIncomingPacket(address, data);
                            session.HandleFrameSet(address, data);
                        }
                        break;
                    }
            }
        }

        private void HandleIncomingPacket(IPEndPoint address, byte[] buffer)
        {
            byte packetID = buffer[0];

            bool exists = Listeners.TryGetValue(packetID, out List<(Type, Delegate)> value);
            if (!exists) return;

            foreach ((Type, Delegate) registration in value)
            {
                Delegate callback = registration.Item2;
                Type packetType = registration.Item1;

                MethodInfo method = packetType.GetMethod("Deserialize");
                if (method == null) return;

                object packet = Activator.CreateInstance(packetType, new object[] { buffer });
                method.Invoke(packet, new object[] { });

                callback.DynamicInvoke(address, packet);
            }
        }

        private void HandleUnconnectedPing1(IPEndPoint peer_addr, byte[] data)
        {
            PacketUnconnectedPing packet = Packet.ReadPacketPing(data);
            //string message = "--- " + peer_addr.Address + " ---";

            //Console.WriteLine(message);

            //Console.WriteLine("Ping Time: " + packet.time);
            //Console.WriteLine("Ping Magic: " + packet.magic);
            //Console.WriteLine("Ping GUID: " + packet.guid);

            //Console.WriteLine(string.Concat(Enumerable.Repeat("-", message.Length)));

            PacketUnconnectedPong unconnectedPongPacket = new PacketUnconnectedPong
            {
                time = 0,
                guid = guid,
                magic = true,
                motd = "MCPE;§6§eLava Server;685;1.21.1;900;999;13253860892328930865;§l§eLava - A cutting-edge Minecraft Bedrock core designed for unparalleled performance, rock-solid stability, and advanced features.;Survival;1;19132;19133;"
            };
            byte[] unconnectedPongBuff = Packet.WritePacketPong(unconnectedPongPacket);
            this.Send(peer_addr, unconnectedPongBuff);
        }

        private void HandleOpenConnectionRequest1(IPEndPoint peer_addr, byte[] data)
        {
            OpenConnectionReply1 reply1Packet = new OpenConnectionReply1
            {
                magic = true,
                guid = guid,
                use_encryption = 0x00,
                mtu_size = RAKNET_CLIENT_MTU,
            };
            byte[] reply1Buf = Packet.WritePacketConnectionOpenReply1(reply1Packet);
            this.Send(peer_addr, reply1Buf);
        }

        private void HandleOpenConnectionRequest2(IPEndPoint peer_addr, byte[] data)
        {
            OpenConnectionRequest2 req = Packet.ReadPacketConnectionOpenRequest2(data);
            OpenConnectionReply2 reply2Packet = new OpenConnectionReply2
            {
                magic = true,
                guid = guid,
                address = peer_addr,
                mtu = req.mtu,
                encryption_enabled = 0x00,
            };
            byte[] reply2Buf = Packet.WritePacketConnectionOpenReply2(reply2Packet);

            var session = new RaknetSession(this, peer_addr, guid, rak_version, new RecvQ(), new SendQ(req.mtu));
            lock (Sessions)
            {
                Sessions.Add(peer_addr, session);
                this.Send(peer_addr, reply2Buf);
                this.OnSessionConnected(session);
            }
        }

        private void OnSessionConnected(RaknetSession session)
        {
            session.SessionReceiveRaw += OnPacketReceived;
            session.SessionDisconnected += RemoveSession;
        }

        void RemoveSession(RaknetSession session)
        {
            session.SessionReceiveRaw -= OnPacketReceived;
            IPEndPoint peerAddr = session.PeerEndPoint;

            lock (Sessions)
                Sessions.Remove(peerAddr);
        }

        public void Send(IPEndPoint peer_addr, byte[] bytes)
        {
            socket.Send(bytes, bytes.Length, peer_addr);
        }

        public void Stop()
        {
            _closed = true;
            socket?.Close();
        }
    }
}
