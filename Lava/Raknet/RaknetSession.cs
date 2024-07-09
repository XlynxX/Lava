using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Net;
using System;
using System.IO.Compression;
using System.Text;
using Lava.Raknet.Protocol;
using Lava.Raknet.Packets;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Agreement;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Lava.Raknet.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.X509;
using System.Diagnostics;
using Lava.Raknet.Utils;
using Jose;
using System.Collections;
using Org.BouncyCastle.Crypto;
using Newtonsoft.Json.Linq;
//using Lava.Raknet.Utils;

namespace Lava.Raknet
{
    public class RaknetSession
    {
        public IPEndPoint PeerEndPoint { get; private set; }
        public byte[] SharedSecret { get; private set; }
        public Func<byte[], byte[]> Decrypt { get; private set; }
        public Func<byte[], byte[]> Encrypt { get; private set; }

        private static readonly Dictionary<int, List<(Type, Delegate)>> Listeners = new Dictionary<int, List<(Type, Delegate)>>();

        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        public Thread SenderThread;
        public byte rak_version;
        public Timer PingTimer;
        public bool Connected;
        public RecvQ Recvq;
        public SendQ Sendq;

        private readonly RaknetListener socket;
        private bool thrownUnkownPackets;
        public int MaxRepingCount = 6;
        private int repingCount;
        private ulong guid;

        private CryptoContext cryptoContext;

        public delegate void SessionDisconnectedDelegate(RaknetSession session);
        public SessionDisconnectedDelegate SessionDisconnected = delegate { };

        public delegate void PacketReceiveBytesDelegate(IPEndPoint address, byte[] bytes);
        public PacketReceiveBytesDelegate SessionReceiveRaw = delegate { };
        private bool useEncryption;
        private int count;

        public RaknetSession(RaknetListener socket, IPEndPoint Address, ulong guid, byte rakVersion, RecvQ recvQ, SendQ sendQ, bool thrownUnkownPackets = false)
        {
            this.socket = socket;
            PeerEndPoint = Address;
            this.guid = guid;
            Connected = true;
            this.thrownUnkownPackets = thrownUnkownPackets;

            rak_version = rakVersion;

            Sendq = sendQ;
            Recvq = recvQ;

            StartPing();
            SenderThread = StartSender();

        }

        //public void Subscribe<T>(Action<T> action) where T : Packet {
        //    Type packetType = typeof(T);

        //    //Ensure the packet has a registered packet id.
        //    RegisterPacketID attribute = 
        //        packetType.GetCustomAttribute<RegisterPacketID>() ?? throw new Exception(packetType.FullName + " must have the [RegisterPacketID(int)] attribute.");

        //    bool hasBufferConstructor = false;
        //    foreach(ConstructorInfo constructor in packetType.GetConstructors())
        //    {
        //        ParameterInfo[] parameters = constructor.GetParameters();

        //        if (parameters.Length != 1) continue;
        //        if (parameters[0].ParameterType == typeof(byte[])) continue;

        //        hasBufferConstructor = true;
        //    }

        //    if (!hasBufferConstructor) throw new Exception(packetType.FullName + " must have a constructor that takes only a byte[].");

        //    int packetId = attribute.ID;

        //    bool exists = Listeners.TryGetValue(packetId, out var listeners);
        //    if(!exists)
        //    {
        //        listeners = new List<(Type, Delegate)>();
        //        Listeners.Add(packetId, listeners);
        //    }

        //    listeners.Add((packetType, action));
        //}
        private static long CurTimestampMillis()
        {
            DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan timeSinceEpoch = DateTime.UtcNow - unixEpoch;
            long milliseconds = (long)timeSinceEpoch.TotalMilliseconds;
            return milliseconds;
        }

        public void HandleFrameSet(IPEndPoint peer_addr, byte[] data)
        {
            if (!Connected)
            {
                foreach (FrameSetPacket f in Recvq.Flush(peer_addr))
                {
                    HandleFrame(peer_addr, f);
                }
            }

            RaknetReader stream = new RaknetReader(data);
            byte headerFlags = stream.ReadU8();

            switch ((PacketID)headerFlags)
            {
                case PacketID.Nack:
                    {
                        //Console.WriteLine("Nack");
                        lock (Sendq)
                        {
                            Nack nack = Packet.ReadPacketNack(data);
                            for (int i = 0; i < nack.record_count; i++)
                            {
                                if (nack.sequences[i].Start == nack.sequences[i].End)
                                {
                                    Sendq.Nack(nack.sequences[i].Start, CurTimestampMillis());
                                }
                                else
                                {
                                    for (uint j = nack.sequences[i].Start; j <= nack.sequences[i].End; j++)
                                    {
                                        Sendq.Nack(j, CurTimestampMillis());
                                    }
                                }
                            }
                        }

                        break;
                    }
                case PacketID.Ack:
                    {
                        //Console.WriteLine("Ack");
                        lock (Sendq)
                        {
                            Ack ack = Packet.ReadPacketAck(data);
                            for (int i = 0; i < ack.record_count; i++)
                            {
                                if (ack.sequences[i].Start == ack.sequences[i].End)
                                {
                                    Sendq.Ack(ack.sequences[i].Start, CurTimestampMillis());
                                }
                                else
                                {
                                    for (uint j = ack.sequences[i].Start; j <= ack.sequences[i].End; j++)
                                    {
                                        Sendq.Ack(j, CurTimestampMillis());
                                    }
                                }
                            }
                        }

                        break;
                    }
                default:
                    {
                        if ((PacketID)data[0] >= PacketID.FrameSetPacketBegin 
                            && (PacketID)data[0] <= PacketID.FrameSetPacketEnd)
                        {
                            var frames = new FrameVec(data);
                            lock (Recvq)
                            {
                                foreach (var frame in frames.frames)
                                {
                                    Recvq.Insert(frame);
                                    foreach (FrameSetPacket f in Recvq.Flush(peer_addr))
                                    {
                                        HandleFrame(peer_addr, f);
                                    }
                                }

                            }
                            var acks = Recvq.GetAck();
                            if (acks.Count != 0)
                            {
                                Ack packet = new Ack
                                {
                                    record_count = (ushort)acks.Count,
                                    sequences = acks,
                                };
                                byte[] buf = Packet.WritePacketAck(packet);
                                socket.Send(peer_addr, buf);
                            }
                        }
                        break;
                    }
            }
        }

        public void HandleFrame(IPEndPoint address, FrameSetPacket frame)
        {
            PacketID packetID = (PacketID)frame.data[0];

            switch (packetID)
            {
                case PacketID.ConnectedPing:
                    HandleConnectPing(frame.data);
                    break;
                case PacketID.ConnectedPong:
                    repingCount = 0;
                    break;
                case PacketID.ConnectionRequest:
                    HandleConnectionRequest(address, frame.data);
                    break;
                case PacketID.ConnectionRequestAccepted:
                    HandleConnectionRequestAccepted(frame.data);
                    break;
                //case PacketID.NewIncomingConnection:
                //    HandleNewIncomingConnection(frame.data);
                //    break;
                case PacketID.Disconnect:
                    HandleDisconnectionNotification();
                    break;
                case PacketID.Game:
                    HandleGamePacketRaw(frame.data);
                    break;
                default:
                    SessionReceiveRaw(address, frame.data);
                    HandleIncomingPacket(frame.data);
                    break;
            }
        }

        private void HandleGamePacketRaw(byte[] rawData)
        {
            //Console.WriteLine($"(RAW) 0x{rawData[0]:X2} 0x{rawData[1]:X2} 0x{rawData[2]:X2} 0x{rawData[3]:X2} ({rawData.Length} bytes)");

            if (useEncryption)
            {
                byte[] decrypted = CryptoUtils.Decrypt(rawData.Skip(1).ToArray(), cryptoContext);

                rawData = new byte[decrypted.Length + 1];
                rawData[0] = 0xfe;
                Array.Copy(decrypted, 0, rawData, 1, decrypted.Length);
            }

            //Console.WriteLine($"0x{rawData[0]:X2} 0x{rawData[1]:X2} 0x{rawData[2]:X2} 0x{rawData[3]:X2} ({rawData.Length} bytes)");

            RaknetReader stream = new RaknetReader(rawData);
            stream.ReadU8();
            if (Sendq.is_compression_ready) stream.ReadU8();
            int packet_size = stream.ReadVarInt();
            byte[] data = stream.Read(packet_size);

            GamePacketID packetID = (GamePacketID)data[0];
            Console.WriteLine(packetID);

            switch (packetID)
            {
                case GamePacketID.REQUEST_NETWORK_SETTINGS_PACKET:
                    HandleRequestNetworkSettings(data);
                    break;
                case GamePacketID.LOGIN_PACKET:
                    HandleLogin(data);
                    break;
                case GamePacketID.CLIENT_TO_SERVER_HANDSHAKE_PACKET:
                    Console.WriteLine("Encryption is not implemented!");
                    break;
                case GamePacketID.RESOURCE_PACK_CLIENT_RESPONSE_PACKET:
                    HandleResourcePacksClientResponse();
                    break;
                default:
                    Console.WriteLine($"Unhandled Game Packet ({packetID} 0x{data[0]:X2})");
                    break;
            }
        }

        private void HandleLogin(byte[] data)
        {
            Login loginPacket = new Login(data);
            loginPacket.Deserialize();

            PlayStatus playStatus = new PlayStatus(0);
            ResourcePacksInfo resourcePacksInfo = new ResourcePacksInfo();
            lock (Sendq)
            {
                Sendq.Insert(Reliability.ReliableOrdered, playStatus);
                Sendq.Insert(Reliability.ReliableOrdered, resourcePacksInfo);
            }

            // IF ENCRYPTION IS ENABLED
            //StartEncryption(loginPacket.chain_data);
        }
        private void HandleResourcePacksClientResponse()
        {
            if (count == 0)
            {
                this.count++;
                ResourcePackStack resourcePacksStack = new ResourcePackStack();
                lock (Sendq)
                {
                    Sendq.Insert(Reliability.ReliableOrdered, resourcePacksStack);
                }

                return;
            }

            StartGame startGame = new StartGame();
            lock (Sendq) Sendq.Insert(Reliability.ReliableOrdered, startGame);
        }
        public void HandleMcpeServerToClientHandshake(string token)
        {
            dynamic jwt = token;
            //string jwt = tokenData;

            if (string.IsNullOrEmpty(jwt))
            {
                throw new Exception("Server did not return a valid JWT, cannot start encryption");
            }

            // No verification here, not needed

            string[] jwtParts = jwt.Split('.');
            string headerPart = jwtParts[0];
            string payloadPart = jwtParts[1];

            byte[] header = Base64UrlDecode(headerPart);
            byte[] payload = Base64UrlDecode(payloadPart);

            dynamic head = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(header));
            dynamic body = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(payload));

            string x5u = head.x5u.ToString();

            //if (Log.IsDebugEnabled) Log.Debug($"JWT payload:\n{JWT.Payload(token)}");

            var remotePublicKey = (ECPublicKeyParameters) PublicKeyFactory.CreateKey(Base64UrlDecode(x5u));

            var signParam = new ECParameters
            {
                Curve = ECCurve.NamedCurves.nistP384,
                Q =
                {
                    X = remotePublicKey.Q.AffineXCoord.GetEncoded(),
                    Y = remotePublicKey.Q.AffineYCoord.GetEncoded()
                },
            };
            signParam.Validate();

            var signKey = ECDsa.Create(signParam);

            try
            {
                byte[] salt = Base64UrlDecode(body.salt.ToString());

                InitiateEncryption(Base64UrlDecode(x5u), salt);
            }
            catch (Exception e)
            {
                Console.WriteLine(token, e);
                throw;
            }
        }

        public void InitiateEncryption(byte[] serverKey, byte[] randomKeyToken)
        {
            try
            {
                ECPublicKeyParameters remotePublicKey = (ECPublicKeyParameters)
                    PublicKeyFactory.CreateKey(serverKey);

                //ECDiffieHellmanPublicKey publicKey = CryptoUtils.FromDerEncoded(serverKey);
                //Log.Debug("ServerKey (b64):\n" + serverKey);
                //Log.Debug($"Cert:\n{publicKey.ToXmlString()}");

                //Log.Debug($"RANDOM TOKEN (raw):\n\n{Encoding.UTF8.GetString(randomKeyToken)}");

                //if (randomKeyToken.Length != 0)
                //{
                //	Log.Error("Lenght of random bytes: " + randomKeyToken.Length);
                //}

                //var bedrockHandler = (BedrockClientMessageHandler)Session.CustomMessageHandler;

                var agreement = new ECDHBasicAgreement();

                var publicKeyBytes = Convert.FromBase64String("MHYwEAYHKoZIzj0CAQYFK4EEACIDYgAE7ae9gGfOzRhlAP/cX9/0NtcKX+nEmTi0kqIrTKBqFdssUPL34smuS4zqgt9SxdHzAx9xBUKLEqnhLNUdY6OQTnu7BLHtpbPUOXjjNuCdJ3kbrKxLw9eTW7w8ivEsOFcV");
                var publicKey = Encoding.UTF8.GetString(publicKeyBytes);

                //agreement.Init(publicKeyParameters);
                byte[] secret;
                using (var sha = SHA256.Create())
                {
                    secret = sha.ComputeHash(randomKeyToken.Concat(agreement.CalculateAgreement(remotePublicKey).ToByteArrayUnsigned()).ToArray());
                }

                //Log.Debug($"SECRET KEY (raw):\n{Encoding.UTF8.GetString(secret)}");

                // Create a decrytor to perform the stream transform.
                var encryptor = new StreamingSicBlockCipher(new SicBlockCipher(new AesEngine()));
                var decryptor = new StreamingSicBlockCipher(new SicBlockCipher(new AesEngine()));
                decryptor.Init(false, new ParametersWithIV(new KeyParameter(secret), secret.Take(12).Concat(new byte[] { 0, 0, 0, 2 }).ToArray()));
                encryptor.Init(true, new ParametersWithIV(new KeyParameter(secret), secret.Take(12).Concat(new byte[] { 0, 0, 0, 2 }).ToArray()));

                cryptoContext = new CryptoContext
                {
                    Decryptor = decryptor,
                    Encryptor = encryptor,
                    UseEncryption = true,
                    Key = secret
                };

                //Thread.Sleep(1250);
                //McpeClientToServerHandshake magic = new McpeClientToServerHandshake();
                //SendPacket(magic);
                
            }
            catch (Exception e)
            {
                Console.WriteLine("Initiate encryption", e);
            }
        }

        private byte[] Base64UrlDecode(string input)
        {
            string base64 = input.Replace('-', '+').Replace('_', '/');
            switch (input.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
        private void StartEncryption(string chainToken)
        {
            Console.WriteLine("[encrypt] Starting encryption");
            //Console.WriteLine(chainToken);

            // Assuming `token` is a JSON structure that contains the JWT and other data.
            dynamic tokenData = JsonConvert.DeserializeObject(chainToken);

            int chain_length = ((JArray)tokenData.chain).Count;
            for (int i = 0; i < chain_length; i++)
            {
                string jwt = tokenData?.chain[i];

                if (string.IsNullOrEmpty(jwt))
                {
                    throw new Exception("Server did not return a valid JWT, cannot start encryption");
                }

                // No verification here, not needed

                string[] jwtParts = jwt.Split('.');
                string headerPart = jwtParts[0];
                string payloadPart = jwtParts[1];

                byte[] header = Base64UrlDecode(headerPart);
                byte[] payload = Base64UrlDecode(payloadPart);

                dynamic head = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(header));
                dynamic body = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(payload));

                string x5uString = head.x5u.ToString();
                byte[] x5uBytes = Base64UrlDecode(x5uString);

                // Use bouncy to parse the DER key
                ECPublicKeyParameters remotePublicKey = (ECPublicKeyParameters)
                    PublicKeyFactory.CreateKey(x5uBytes);

                var b64RemotePublicKey = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(remotePublicKey).GetEncoded().EncodeBase64();
                Debug.Assert(x5uString == b64RemotePublicKey);
                Debug.Assert(remotePublicKey.PublicKeyParamSet.Id == "1.3.132.0.34");
                //Log.Debug($"{remotePublicKey.PublicKeyParamSet}");

                var generator = new ECKeyPairGenerator("ECDH");
                generator.Init(new ECKeyGenerationParameters(remotePublicKey.PublicKeyParamSet, SecureRandom.GetInstance("SHA256PRNG")));
                var keyPair = generator.GenerateKeyPair();

                ECPublicKeyParameters pubAsyKey = (ECPublicKeyParameters)keyPair.Public;
                ECPrivateKeyParameters privAsyKey = (ECPrivateKeyParameters)keyPair.Private;

                var secretPrepend = Encoding.UTF8.GetBytes("RANDOM SECRET");

                ECDHBasicAgreement agreement = new ECDHBasicAgreement();
                agreement.Init(keyPair.Private);
                byte[] secret;
                using (var sha = SHA256.Create())
                {
                    secret = sha.ComputeHash(secretPrepend.Concat(agreement.CalculateAgreement(remotePublicKey).ToByteArrayUnsigned()).ToArray());
                }

                Debug.Assert(secret.Length == 32);

                //if (Log.IsDebugEnabled) Log.Debug($"SECRET KEY (b64):\n{secret.EncodeBase64()}");

                var encryptor = new StreamingSicBlockCipher(new SicBlockCipher(new AesEngine()));
                var decryptor = new StreamingSicBlockCipher(new SicBlockCipher(new AesEngine()));
                decryptor.Init(false, new ParametersWithIV(new KeyParameter(secret), secret.Take(12).Concat(new byte[] { 0, 0, 0, 2 }).ToArray()));
                encryptor.Init(true, new ParametersWithIV(new KeyParameter(secret), secret.Take(12).Concat(new byte[] { 0, 0, 0, 2 }).ToArray()));

                //IBufferedCipher decryptor = CipherUtilities.GetCipher("AES/CFB8/NoPadding");
                //decryptor.Init(false, new ParametersWithIV(new KeyParameter(secret), secret.Take(16).ToArray()));

                //IBufferedCipher encryptor = CipherUtilities.GetCipher("AES/CFB8/NoPadding");
                //encryptor.Init(true, new ParametersWithIV(new KeyParameter(secret), secret.Take(16).ToArray()));

                cryptoContext = new CryptoContext();
                cryptoContext.Key = secret;
                cryptoContext.Decryptor = decryptor;
                cryptoContext.Encryptor = encryptor;

                var signParam = new ECParameters
                {
                    Curve = ECCurve.NamedCurves.nistP384,
                    Q =
                                {
                                    X = pubAsyKey.Q.AffineXCoord.GetEncoded(),
                                    Y = pubAsyKey.Q.AffineYCoord.GetEncoded()
                                }
                };
                signParam.D = CryptoUtils.FixDSize(privAsyKey.D.ToByteArrayUnsigned(), signParam.Q.X.Length);
                signParam.Validate();

                string signedToken = null;

                var signKey = ECDsa.Create(signParam);
                var b64PublicKey = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(pubAsyKey).GetEncoded().EncodeBase64();
                var handshakeJson = new HandshakeData
                {
                    salt = secretPrepend.EncodeBase64(),
                    signedToken = signedToken
                };
                string val = JWT.Encode(handshakeJson, signKey, JwsAlgorithm.ES384, new Dictionary<string, object> { { "x5u", b64PublicKey } });

                Console.WriteLine($"Headers:\n{string.Join(";", JWT.Headers(val))}");
                Console.WriteLine($"Return salt:\n{JWT.Payload(val)}");
                Console.WriteLine($"JWT:\n{val}");


                ServerToClientHandshake handshake = new ServerToClientHandshake(val);
                lock (Sendq)
                    Sendq.Insert(Reliability.ReliableOrdered, handshake);

                Console.WriteLine($"Encryption enabled for {"username"}");
                useEncryption = true;
            }
        }

        private Func<byte[], byte[]> CreateDecryptor(byte[] secretKeyBytes, byte[] iv)
        {
            // Example implementation using AES
            using (var aes = new AesCryptoServiceProvider())
            {
                aes.Key = secretKeyBytes;
                aes.IV = iv;

                // Assuming AES decryption with CBC mode
                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                return blob =>
                {
                    // Decrypt data here using `decryptor`
                    // Example:
                    // byte[] decryptedData = decryptor.TransformFinalBlock(blob, 0, blob.Length);
                    // return decryptedData;

                    return null; // Placeholder return
                };
            }
        }

        private Func<byte[], byte[]> CreateEncryptor(byte[] secretKeyBytes, byte[] iv)
        {
            // Example implementation using AES
            using (var aes = new AesCryptoServiceProvider())
            {
                aes.Key = secretKeyBytes;
                aes.IV = iv;

                // Assuming AES encryption with CBC mode
                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                return blob =>
                {
                    // Encrypt data here using `encryptor`
                    // Example:
                    // byte[] encryptedData = encryptor.TransformFinalBlock(blob, 0, blob.Length);
                    // return encryptedData;

                    return null; // Placeholder return
                };
            }
        }

        private void HandleRequestNetworkSettings(byte[] data)
        {
            //foreach (byte _byte in data)
            //{
            //    Console.Write($"0x{_byte:X2} ");
            //}
            //Console.WriteLine("-----");
            RequestNetworkSettings packet = new RequestNetworkSettings(data);
            packet.Deserialize();

            //Console.WriteLine("Request network");
            //Console.WriteLine(packet.protocol_version);
            //ConnectedPong pongPacket = new ConnectedPong
            //{
            //    client_timestamp = pingPacket.client_timestamp,
            //    server_timestamp = CurTimestampMillis(),
            //};

            //byte[] buf = Packet.WritePacketConnectedPong(pongPacket);
            //lock (Sendq)
            //    Sendq.Insert(Reliability.Unreliable, buf);

            NetworkSettings network_settings = new NetworkSettings(0, 0, false, (byte)0, 0);
            //{
            //    threshold = 0,
            //    method = 0,
            //    throttle_enabled = false,
            //    throttle_threshold = (byte) 0,
            //    throttle_scalar = 0,
            //};
            //byte[] buf = network_settings.Serialize();
            lock (Sendq)
                Sendq.Insert(Reliability.ReliableOrdered, network_settings);
            Sendq.is_compression_ready = true;
        }

        private void HandleNewIncomingConnection(byte[] data)
        {
            NewIncomingConnection packet = Packet.ReadPacketNewIncomingConnection(data);
            Console.WriteLine("New Incoming Connection");
            //Console.WriteLine(packet.server_address);
            //Console.WriteLine(packet.request_timestamp);
            //Console.WriteLine(packet.accepted_timestamp);
        }

        private void HandleIncomingPacket(byte[] buffer) {
            //byte packetID = buffer[0];

            ////foreach (byte _byte in buffer)
            ////{
            ////    Console.Write($"0x{_byte:X2} ");
            ////}
            ////Console.WriteLine("----------------");
            ////Console.WriteLine($"UNHANDLED PACKET ID 0x{buffer[1]:X2}");
            ////if (buffer[1] == 0x06)
            ////{
            ////    byte[] compressedBuff = new byte[buffer.Length - 1];
            ////    Array.Copy(buffer, 1, compressedBuff, 0, compressedBuff.Length);
            ////    var decompressedBuff = Decompress(compressedBuff);
            ////    Console.WriteLine($"decompressed PACKET ID 0x{decompressedBuff[0]:X2}");
            ////}

            //bool exists = Listeners.TryGetValue(packetID, out List<(Type, Delegate)> value);
            //if (!exists) return;

            //foreach((Type, Delegate) registration in value)
            //{
            //    Delegate callback = registration.Item2;
            //    Type packetType = registration.Item1;

            //    MethodInfo method = packetType.GetMethod("Deserialize");
            //    if (method == null) return;

            //    object packet = Activator.CreateInstance(packetType, new object[] { buffer });
            //    method.Invoke(packet, new object[] {});

            //    callback.DynamicInvoke(packet);
            //}
        }

        private void HandleConnectPing(byte[] data)
        {
            ConnectedPing pingPacket = Packet.ReadPacketConnectedPing(data);
            ConnectedPong pongPacket = new ConnectedPong
            {
                client_timestamp = pingPacket.client_timestamp,
                server_timestamp = CurTimestampMillis(),
            };

            byte[] buf = Packet.WritePacketConnectedPong(pongPacket);
            lock (Sendq)
                Sendq.Insert(Reliability.Unreliable, buf);
        }

        private void HandleConnectionRequestAccepted(byte[] data)
        {
            ConnectionRequestAccepted packet = Packet.ReadPacketConnectionRequestAccepted(data);
            NewIncomingConnection packet_reply = new NewIncomingConnection
            {
                server_address = (IPEndPoint)socket.socket.Client.LocalEndPoint,
                request_timestamp = packet.request_timestamp,
                accepted_timestamp = CurTimestampMillis(),
            };
            byte[] buf = Packet.WritePacketNewIncomingConnection(packet_reply);
            lock (Sendq)
                Sendq.Insert(Reliability.ReliableOrdered, buf);
        }

        private void HandleConnectionRequest(IPEndPoint peer_addr, byte[] data)
        {
            ConnectionRequest packet = Packet.ReadPacketConnectionRequest(data);
            ConnectionRequestAccepted packet_reply = new ConnectionRequestAccepted
            {
                client_address = peer_addr,
                system_index = 0,
                request_timestamp = packet.time,
                accepted_timestamp = CurTimestampMillis(),
            };
            byte[] buf = Packet.WritePacketConnectionRequestAccepted(packet_reply);
            lock (Sendq)
                Sendq.Insert(Reliability.ReliableOrdered, buf);
        }

        private void HandleDisconnectionNotification()
        {
            Connected = false;
            StopSender();
            PingTimer.Dispose();
            SessionDisconnected(this);
            GC.Collect();
        }

        public void HandleConnect()
        {
            ConnectionRequest requestPacket = new ConnectionRequest
            {
                guid = guid,
                time = CurTimestampMillis(),
                use_encryption = 0x00,
            };
            byte[] buf = Packet.WritePacketConnectionRequest(requestPacket);
            lock (Sendq)
                Sendq.Insert(Reliability.ReliableOrdered, buf);
        }

        public Thread StartSender()
        {
            Thread thread = new Thread(() => 
            {
                while (!_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    Thread.Sleep(100);
                    foreach (FrameSetPacket item in Sendq.Flush(CurTimestampMillis(), PeerEndPoint))
                    {
                        byte[] sdata = item.Serialize();
                        socket.Send(PeerEndPoint, sdata);
                    }
                }
            });
            thread.Start();
            return thread;
        }

        public void StopSender()
        {
            _cancellationTokenSource.Cancel();
        }

        public void StartPing()
        {
            PingTimer = new Timer(SendPing, null,
                new Random().Next(1000, 1500), new Random().Next(1000, 1500));
        }

        public void SendPing(object obj)
        {
            if (!Connected) return;
            ConnectedPing pingPacket = new ConnectedPing
            {
                client_timestamp = CurTimestampMillis(),
            };

            byte[] buffer = Packet.WritePacketConnectedPing(pingPacket);
            lock (Sendq)
                Sendq.Insert(Reliability.Unreliable, buffer);

            repingCount++;
            if (repingCount < MaxRepingCount) return;

            HandleDisconnectionNotification();
        }
    }
}
