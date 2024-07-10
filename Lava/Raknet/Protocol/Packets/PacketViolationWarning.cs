using Lava.Raknet.Protocol;

namespace Lava.Raknet.Packets
{

    [RegisterPacketID((int)GamePacketID.PACKET_VIOLATION_WARNING_PACKET)]
    public class PacketViolationWarning : GamePacket
    {
        public int violation_type;
        public int violation_severity;
        public int violating_packet_id;
        public string violaton_context;

        public PacketViolationWarning(byte[] buffer) : base(buffer)
        {
            Deserialize();
        }
        public PacketViolationWarning() : base(new byte[] { })
        {
            this.violation_type = 0; 
            this.violation_severity = 0;
            this.violating_packet_id = 0;
            this.violaton_context = string.Empty;

            Buffer = Serialize();
        }

        public override byte[] Serialize()
        {
            RaknetWriter stream = new RaknetWriter();
            //stream.WriteU8(GamePacketID.PLAY_STATUS_PACKET);
            //stream.WriteI32(status, Endian.Big);
            //stream.Write([0x45, 0x10, 0x1c]);

            //      0xFF 0x05 0x02 0x00 0x00 0x00 0x00
            return [0x02, 0x00, 0x00, 0x00, 0x00];

            //return stream.GetRawPayload();
        }

        public override void Deserialize()
        {
            if (Buffer == null) throw new System.Exception("Buffer is not present idk");

            RaknetReader stream = new RaknetReader(Buffer);
            stream.ReadU8();
            violation_type = stream.ReadVarInt();
            violation_severity = stream.ReadVarInt();
            violating_packet_id = stream.ReadVarInt();
            violaton_context = stream.ReadVString();
        }
    }
}