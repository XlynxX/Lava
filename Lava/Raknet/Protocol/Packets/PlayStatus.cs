using Lava.Raknet.Protocol;

namespace Lava.Raknet.Packets
{

    [RegisterPacketID((int)GamePacketID.PLAY_STATUS_PACKET)]
    public class PlayStatus : GamePacket
    {
        public int status;

        public PlayStatus(byte[] buffer) : base(buffer) { }
        public PlayStatus(int status) : base(new byte[] { })
        {
            this.status = status;

            Buffer = Serialize();
        }

        public override byte[] Serialize()
        {
            RaknetWriter stream = new RaknetWriter();
            stream.WriteU8(GamePacketID.PLAY_STATUS_PACKET);
            stream.WriteI32(status, Endian.Big);
            //stream.Write([0x45, 0x10, 0x1c]);

            return stream.GetRawPayload();
        }

        public override void Deserialize()
        {
            if (Buffer == null) throw new System.Exception("Buffer is not present idk");

            RaknetReader stream = new RaknetReader(Buffer);
            stream.ReadU8();
            status = stream.ReadI32(Endian.Big);
        }
    }
}