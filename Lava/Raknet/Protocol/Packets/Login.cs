using Lava.Raknet.Protocol;
using System.Text;

namespace Lava.Raknet.Packets
{

    [RegisterPacketID((int)GamePacketID.LOGIN_PACKET)]
    public class Login : GamePacket
    {
        public int protocol_version;
        public string chain_data;
        public string skin_data;

        public Login(byte[] buffer) : base(buffer) { }
        public Login(int protocol_version, string chain_data, string skin_data) : base(new byte[] { })
        {
            this.protocol_version = protocol_version;
            this.chain_data = chain_data;
            this.skin_data = skin_data;

            Buffer = Serialize();
        }

        public override byte[] Serialize()
        {
            //RaknetWriter stream = new RaknetWriter();
            //stream.WriteU8(PacketID.Login);
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

            protocol_version = stream.ReadI32(Endian.Big);
            var chain_size = stream.ReadVarInt();
            var chain = stream.Read(chain_size);

            //Console.WriteLine(Buffer.Length);
            //Console.WriteLine(chain.Length);
            //Console.WriteLine(Buffer.Length - 5);

            RaknetReader chain_stream = new RaknetReader(chain);
            var size = chain_stream.ReadI32(Endian.Little);
            //Console.WriteLine(size);
            chain_data = Encoding.UTF8.GetString(chain_stream.Read(size));
            var skinsize = chain_stream.ReadI32(Endian.Little);
            skin_data = Encoding.UTF8.GetString(chain_stream.Read(skinsize));
            //Console.WriteLine($"{size}, {skinsize}, {chain.Length}");
        }
    }
}