using Lava.Raknet.Protocol;
using System.IO;

namespace Lava.Raknet.Packets
{

    [RegisterPacketID((int)GamePacketID.RESOURCE_PACK_STACK_PACKET)]
    public class ResourcePackStack : GamePacket
    {
        public bool mustAccept;
        //public ResourcePackIdVersions behaviorpackidversions;
        //public ResourcePackIdVersions resourcepackidversions;
        public string gameVersion;
        //public Experiments experiments;
        public bool experimentsPreviouslyToggled;
        public bool hasEditorPacks;
        public ResourcePackStack(byte[] buffer) : base(buffer) { }
        public ResourcePackStack() : base(new byte[] { })
        {
            this.mustAccept = false;
            this.gameVersion = null;
            this.experimentsPreviouslyToggled = false;
            this.hasEditorPacks = false;

            Buffer = Serialize();
        }

        public override byte[] Serialize()
        {
            RaknetWriter stream = new RaknetWriter();
            stream.WriteU8(GamePacketID.RESOURCE_PACK_STACK_PACKET);
            stream.WriteBool(mustAccept);
            stream.WriteUVarInt(0); // Write(behaviorpackidversions);
            stream.WriteUVarInt(0); // Write(resourcepackidversions);
            stream.WriteVarInt(0);  // stream.WriteString(gameVersion);
            stream.WriteI32(0, Endian.Little);
            stream.WriteBool(experimentsPreviouslyToggled);
            stream.WriteBool(hasEditorPacks);
            //stream.WriteBool(false);
            //stream.WriteBool(hasScripts);
            //stream.WriteBool(forceServerPacks);
            //stream.WriteI16((short)0, Endian.Big); //Write(behahaviorpackinfos);
            //stream.WriteI16((short)0, Endian.Big); //Write(texturepacks);
            //stream.WriteUVarInt(cndUrls);

            //Write(mustAccept);
            //Write(behaviorpackidversions);
            //Write(resourcepackidversions);
            //Write(gameVersion);
            //Write(experiments);
            //Write(experimentsPreviouslyToggled);
            //Write(hasEditorPacks);

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