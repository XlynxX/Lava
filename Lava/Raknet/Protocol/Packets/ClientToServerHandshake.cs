using Lava.Raknet.Protocol;
using System.Text;

namespace Lava.Raknet.Packets
{

    [RegisterPacketID((int)GamePacketID.CLIENT_TO_SERVER_HANDSHAKE_PACKET)]
    public class ClientToServerHandshake : GamePacket
    {

        public ClientToServerHandshake(byte[] buffer) : base(buffer) { }
        public ClientToServerHandshake() : base(new byte[] { })
        {

            Buffer = Serialize();
        }

        public override byte[] Serialize()
        {
            RaknetWriter stream = new RaknetWriter();
            stream.WriteU8(GamePacketID.CLIENT_TO_SERVER_HANDSHAKE_PACKET);
            stream.WriteU8((byte)0x00);

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

            //RaknetReader stream = new RaknetReader(Buffer);
            //stream.ReadU8();
            //stream.ReadU8(); // idk

            //threshold = stream.ReadI16(Endian.Big);
            //method = stream.ReadI16(Endian.Big);
            //throttle_enabled = stream.ReadBool();
            //throttle_threshold = stream.ReadU8();
            //throttle_scalar = stream.ReadF32(Endian.Big);
        }
    }
}