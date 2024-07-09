using Lava.Raknet.Protocol;
using System.IO;

namespace Lava.Raknet.Packets
{

    [RegisterPacketID((int)GamePacketID.RESOURCE_PACKS_INFO_PACKET)]
    public class ResourcePacksInfo : GamePacket
    {
        public bool mustAccept;
        public bool hasAddons;
        public bool hasScripts;
        public bool forceServerPacks;
        //public ResourcePackInfos behahaviorpackinfos;
        //public TexturePackInfos texturepacks;
        public uint cndUrls;
        public ResourcePacksInfo(byte[] buffer) : base(buffer) { }
        public ResourcePacksInfo() : base(new byte[] { })
        {
            this.mustAccept = false;
            this.hasAddons = false;
            this.hasScripts = false;
            this.cndUrls = 0;

            Buffer = Serialize();
        }

        public override byte[] Serialize()
        {
            RaknetWriter stream = new RaknetWriter();
            stream.WriteU8(GamePacketID.RESOURCE_PACKS_INFO_PACKET);
            stream.WriteBool(mustAccept);
            stream.WriteBool(false);
            stream.WriteBool(hasScripts);
            stream.WriteBool(forceServerPacks);
            stream.WriteShort(0); // .WriteI16((short)0, Endian.Little); //Write(behahaviorpackinfos);
            stream.WriteShort(0); // .WriteI16((short)0, Endian.Little); //Write(texturepacks);
            stream.WriteUVarInt(cndUrls);

            return stream.GetRawPayload();
        }

        public override void Deserialize()
        {
            if (Buffer == null) throw new System.Exception("Buffer is not present idk");

            RaknetReader stream = new RaknetReader(Buffer);
            stream.ReadU8();
            //status = stream.ReadI32(Endian.Big);
        }
    }
}