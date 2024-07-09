using Lava.Raknet.Protocol;

namespace Lava.Raknet.Packets
{

    [RegisterPacketID((int) GamePacketID.NETWORK_SETTINGS_PACKET)]
    public class NetworkSettings : GamePacket
    {
        public short threshold;
        public short method;
        public bool throttle_enabled;
        public byte throttle_threshold;
        public float throttle_scalar;

        public NetworkSettings(byte[] buffer) : base(buffer) { }
        public NetworkSettings(short threshold, short method, bool throttle_enabled, byte throttle_threshold, float throttle_scalar) : base(new byte[] { })
        {
            this.threshold = threshold;
            this.method = method;
            this.throttle_enabled = throttle_enabled;
            this.throttle_threshold = throttle_threshold;
            this.throttle_scalar = throttle_scalar;

            Buffer = Serialize();
        }

        public override byte[] Serialize()
        {
            RaknetWriter stream = new RaknetWriter();
            stream.WriteU8(GamePacketID.NETWORK_SETTINGS_PACKET);
            stream.WriteU8(0x01);
            stream.WriteI16(threshold, Endian.Big);
            stream.WriteI16(method, Endian.Big);
            stream.WriteBool(throttle_enabled);
            stream.WriteU8(throttle_threshold);
            stream.WriteF32(throttle_scalar, Endian.Big);
            
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

            threshold = stream.ReadI16(Endian.Big);
            method = stream.ReadI16(Endian.Big);
            throttle_enabled = stream.ReadBool();
            throttle_threshold = stream.ReadU8();
            throttle_scalar = stream.ReadF32(Endian.Big);
        }
    }
}