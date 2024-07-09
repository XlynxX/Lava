using Lava.Raknet.Protocol;
using Org.BouncyCastle.Utilities;
using System.Reflection.PortableExecutable;
using System.Text;

namespace Lava.Raknet.Packets
{

    [RegisterPacketID((int)GamePacketID.SERVER_TO_CLIENT_HANDSHAKE_PACKET)]
    public class ServerToClientHandshake : GamePacket
    {
        public string token;

        public ServerToClientHandshake(byte[] buffer) : base(buffer) { }
        public ServerToClientHandshake(string token) : base(new byte[] { })
        {
            this.token = token;

            Buffer = Serialize();
        }

        public override byte[] Serialize()
        {
            RaknetWriter stream = new RaknetWriter();
            stream.WriteU8(GamePacketID.SERVER_TO_CLIENT_HANDSHAKE_PACKET);

            byte[] token_bytes = Encoding.UTF8.GetBytes(token);

            stream.WriteVarInt(token_bytes.Length);
            stream.Write(token_bytes);

            //stream.Write([
            //    0xfe,                         // PACKET HEADER GAME PACKET
            //    0x0c,                         // PACKET SIZE
            //    0x8f,                         // PACKET ID
            //    0x01,                         // IDK
            //    0x00, 0x02,                   // threshold
            //    0x00, 0x00,                   // method
            //    0x00,                         // throttle_enabled
            //    0x00,                         // throttle_threshold
            //    0x00, 0x00, 0x00, 0x00]);     // throttle_scalar

            return stream.GetRawPayload();
        }

        public override void Deserialize()
        {
            if (Buffer == null) throw new System.Exception("Buffer is not present idk");

            RaknetReader stream = new RaknetReader(Buffer);
            stream.ReadU8();
            stream.ReadU8(); // idk
            int size = stream.ReadVarInt();
            byte[] tokenBytes = stream.Read(size);

            token = Encoding.UTF8.GetString(tokenBytes);
        }
    }
}