using Lava.Raknet.Protocol;

namespace Lava.Raknet.Packets
{

    [RegisterPacketID((int)GamePacketID.CLIENT_CACHE_STATUS_PACKET)]
    public class ClientCacheStatus : GamePacket
    {
        public bool enabled;

        public ClientCacheStatus(byte[] buffer) : base(buffer)
        {
            Deserialize();
        }
        public ClientCacheStatus(bool enabled) : base(new byte[] { })
        {
            this.enabled = enabled;

            Buffer = Serialize();
        }

        public override byte[] Serialize()
        {
            RaknetWriter stream = new RaknetWriter();
            stream.WriteU8(GamePacketID.CLIENT_CACHE_STATUS_PACKET);
            stream.WriteBool(enabled);

            return stream.GetRawPayload();
        }

        public override void Deserialize()
        {
            if (Buffer == null) throw new System.Exception("Buffer is not present idk");

            RaknetReader stream = new RaknetReader(Buffer);
            stream.ReadU8();
            enabled = stream.ReadBool();
        }
    }
}