using Lava.Raknet.Protocol;

namespace Lava.Raknet.Packets
{

    [RegisterPacketID((int) GamePacketID.REQUEST_NETWORK_SETTINGS_PACKET)]
    public class RequestNetworkSettings : GamePacket
    {
        public int protocol_version;

        public RequestNetworkSettings(byte[] buffer) : base(buffer) { }
        public RequestNetworkSettings(int protocol_version) : base(new byte[] { })
        {
            this.protocol_version = protocol_version;

            Buffer = Serialize();
        }

        public override byte[] Serialize()
        {
            //RaknetWriter stream = new RaknetWriter();
            //stream.WriteU8(PacketID.RequestNetworkSettings);
            //stream.WriteI64(time, Endian.Big);
            //stream.WriteMagic();
            //stream.WriteU64(guid, Endian.Big);

            //return stream.GetRawPayload();
            return [];
        }

        public override void Deserialize()
        {
            if (Buffer == null) throw new System.Exception("Buffer is not present idk");

            RaknetReader stream = new RaknetReader(Buffer);
            stream.ReadU8();
            stream.ReadU8();

            protocol_version = stream.ReadI32(Endian.Big);
        }
    }
}