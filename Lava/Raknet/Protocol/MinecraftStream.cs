using Lava.Raknet.Protocol.Types;
using System.Security.Cryptography;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Lava.Raknet.Protocol
{
    public class MinecraftStream
    {
        private MemoryStream bufStream;
        private BinaryReader reader;
        private List<byte> writeBuff = new List<byte>();

        public MinecraftStream(byte[] buf)
        {
            bufStream = new MemoryStream(buf);
            reader = new BinaryReader(bufStream);
        }

        public byte[] Read(int count)
        {
            byte[] data = reader.ReadBytes(count);
            if (data.Length != count)
            {
                throw new Exception("ReadPacketBufferError");
            }
            return data;
        }

        public void Write(byte[] v)
        {
            writeBuff.AddRange(v);
        }

        public byte ReadByte()
        {
            if (bufStream.Length - bufStream.Position < 1)
            {
                throw new Exception("ReadPacketBufferError");
            }
            return reader.ReadByte();
        }

        public void WriteByte(PacketID id)
        {
            WriteByte((byte)id);
        }

        public void WriteByte(GamePacketID id)
        {
            WriteByte((byte)id);
        }

        public void WriteByte(byte v)
        {
            writeBuff.Add(v);
        }

        public ulong Position
        {
            get { return (ulong)bufStream.Position; }
        }

        public bool ReadBoolean()
        {
            if (bufStream.Length - bufStream.Position < 1)
            {
                throw new Exception("ReadPacketBufferError");
            }
            return reader.ReadByte() != 0;
        }

        public void WriteBoolean(bool value)
        {
            writeBuff.Add((byte)(value ? 1 : 0));
        }

        public short ReadShort()
        {
            if (bufStream.Length - bufStream.Position < 2)
            {
                throw new Exception("ReadPacketBufferError");
            }
            return reader.ReadInt16();
        }

        public void WriteShort(short value)
        {
            writeBuff.AddRange(BitConverter.GetBytes(value));
        }

        public ushort ReadUnsignedShort()
        {
            if (bufStream.Length - bufStream.Position < 2)
            {
                throw new Exception("ReadPacketBufferError");
            }
            return reader.ReadUInt16();
        }

        public void WriteUnsignedShort(ushort value)
        {
            writeBuff.AddRange(BitConverter.GetBytes(value));
        }

        public int ReadInt()
        {
            if (bufStream.Length - bufStream.Position < 4)
            {
                throw new Exception("ReadPacketBufferError");
            }
            return reader.ReadInt32();
        }

        public void WriteInt(int value)
        {
            writeBuff.AddRange(BitConverter.GetBytes(value));
        }

        public uint ReadUnsignedInt()
        {
            if (bufStream.Length - bufStream.Position < 4)
            {
                throw new Exception("ReadPacketBufferError");
            }
            return reader.ReadUInt32();
        }

        public void WriteUnsignedInt(uint value)
        {
            writeBuff.AddRange(BitConverter.GetBytes(value));
        }

        public long ReadLong()
        {
            if (bufStream.Length - bufStream.Position < 8)
            {
                throw new Exception("ReadPacketBufferError");
            }
            return reader.ReadInt64();
        }

        public void WriteLong(long value)
        {
            writeBuff.AddRange(BitConverter.GetBytes(value));
        }

        public ulong ReadUnsignedLong()
        {
            if (bufStream.Length - bufStream.Position < 8)
            {
                throw new Exception("ReadPacketBufferError");
            }
            return reader.ReadUInt64();
        }

        public void WriteUnsignedLong(ulong value)
        {
            writeBuff.AddRange(BitConverter.GetBytes(value));
        }

        public float ReadFloat()
        {
            if (bufStream.Length - bufStream.Position < 4)
            {
                throw new Exception("ReadPacketBufferError");
            }
            return reader.ReadSingle();
        }

        public void WriteFloat(float value)
        {
            writeBuff.AddRange(BitConverter.GetBytes(value));
        }

        public double ReadDouble()
        {
            if (bufStream.Length - bufStream.Position < 8)
            {
                throw new Exception("ReadPacketBufferError");
            }
            return reader.ReadDouble();
        }

        public void WriteDouble(double value)
        {
            writeBuff.AddRange(BitConverter.GetBytes(value));
        }

        public int ReadVarInt()
        {
            int numRead = 0;
            int result = 0;
            byte read;
            do
            {
                if (bufStream.Length - bufStream.Position < 1)
                {
                    throw new Exception("ReadPacketBufferError");
                }
                read = reader.ReadByte();
                int value = (read & 0b01111111);
                result |= (value << (7 * numRead));

                numRead++;
                if (numRead > 5)
                {
                    throw new Exception("VarInt is too big");
                }
            } while ((read & 0b10000000) != 0);

            return result;
        }

        public void WriteVarInt(int value)
        {
            do
            {
                byte temp = (byte)(value & 0b01111111);
                value >>= 7;
                if (value != 0)
                {
                    temp |= 0b10000000;
                }
                writeBuff.Add(temp);
            } while (value != 0);
        }

        public long ReadUnsignedVarLong()
        {
            int numRead = 0;
            long result = 0;
            byte read;
            do
            {
                if (bufStream.Length - bufStream.Position < 1)
                {
                    throw new Exception("ReadPacketBufferError");
                }
                read = reader.ReadByte();
                long value = (read & 0b01111111);
                result |= (value << (7 * numRead));

                numRead++;
                if (numRead > 10)
                {
                    throw new Exception("VarLong is too big");
                }
            } while ((read & 0b10000000) != 0);

            return result;
        }

        public void WriteUnsignedVarLong(long value)
        {
            do
            {
                byte temp = (byte)(value & 0b01111111);
                value >>= 7;
                if (value != 0)
                {
                    temp |= 0b10000000;
                }
                writeBuff.Add(temp);
            } while (value != 0);
        }

        public string ReadString()
        {
            int length = ReadVarInt();
            if (bufStream.Length - bufStream.Position < length)
            {
                throw new Exception("ReadPacketBufferError");
            }
            byte[] bytes = reader.ReadBytes(length);
            return Encoding.UTF8.GetString(bytes);
        }

        public void WriteString(string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            WriteVarInt(bytes.Length);
            writeBuff.AddRange(bytes);
        }

        public Vector3 ReadVector3()
        {
            float x = ReadFloat();
            float y = ReadFloat();
            float z = ReadFloat();
            return new Vector3(x, y, z);
        }

        public void WriteVector3(Vector3 value)
        {
            WriteFloat(value.X);
            WriteFloat(value.Y);
            WriteFloat(value.Z);
        }

        public Vector2 ReadVector2()
        {
            float x = ReadFloat();
            float y = ReadFloat();
            return new Vector2(x, y);
        }

        public void WriteVector2(Vector2 value)
        {
            WriteFloat(value.X);
            WriteFloat(value.Y);
        }

        public byte[] ReadByteArray()
        {
            int length = ReadVarInt();
            if (bufStream.Length - bufStream.Position < length)
            {
                throw new Exception("ReadPacketBufferError");
            }
            return reader.ReadBytes(length);
        }

        public void WriteByteArray(byte[] value)
        {
            WriteVarInt(value.Length);
            writeBuff.AddRange(value);
        }

        //public (int, int, int) ReadBlockPosition()
        //{
        //    int x = ReadSignedVarInt();
        //    int y = ReadVarInt();
        //    int z = ReadSignedVarInt();
        //    return (x, y, z);
        //}

        public BlockPosition ReadBlockPosition()
        {
            int x = ReadSignedVarInt();
            int y = ReadVarInt();
            int z = ReadSignedVarInt();
            return new BlockPosition(x, y, z);
        }

        public void WriteBlockPosition(BlockPosition blockPos)
        {
            WriteSignedVarInt(blockPos.getX());
            WriteVarInt(blockPos.getY());
            WriteSignedVarInt(blockPos.getZ());
        }

        public void WriteBlockPosition(int x, int y, int z)
        {
            WriteSignedVarInt(x);
            WriteVarInt(y);
            WriteSignedVarInt(z);
        }

        public (float, float, float, float, float, float) ReadPlayerLocation()
        {
            float x = ReadFloat();
            float y = ReadFloat();
            float z = ReadFloat();
            byte pitch = ReadByte();
            byte headYaw = ReadByte();
            byte yaw = ReadByte();
            return (x, y, z, pitch * 0.71f, headYaw * 0.71f, yaw * 0.71f);
        }

        public void WritePlayerLocation(float x, float y, float z, float pitch, float headYaw, float yaw)
        {
            WriteFloat(x);
            WriteFloat(y);
            WriteFloat(z);
            WriteByte((byte)(pitch / 0.71f));
            WriteByte((byte)(headYaw / 0.71f));
            WriteByte((byte)(yaw / 0.71f));
        }

        public Guid ReadUUID()
        {
            if (bufStream.Length - bufStream.Position < 16)
            {
                throw new Exception("ReadPacketBufferError");
            }
            byte[] bytes = reader.ReadBytes(16);
            return new Guid(bytes);
        }

        public void WriteUUID(Guid value)
        {
            byte[] bytes = value.ToByteArray();
            writeBuff.AddRange(bytes);
        }

        public int ReadSignedVarInt()
        {
            int raw = ReadVarInt();
            int temp = (((raw << 31) >> 31) ^ raw) >> 1;
            return temp ^ (raw & (1 << 31));
        }

        public void WriteSignedVarInt(int value)
        {
            WriteVarInt((value << 1) ^ (value >> 31));
        }

        public long ReadSignedVarLong()
        {
            long raw = ReadUnsignedVarLong();
            long temp = (((raw << 63) >> 63) ^ raw) >> 1;
            return temp ^ (raw & (1L << 63));
        }

        public void WriteSignedVarLong(long value)
        {
            WriteUnsignedVarLong((value << 1) ^ (value >> 63));
        }
        public byte[] GetBytes()
        {
            return writeBuff.ToArray();
        }
        public int ReadActorUniqueId()
        {
            return (int) ReadSignedVarLong();
        }

        public void WriteActorUniqueId(int eid)
        {
            WriteSignedVarLong(eid);
        }

        public int ReadActorRuntimeId()
        {
            return (int) ReadUnsignedVarLong(); // $this->getUnsignedVarLong();
        }

        public void WriteActorRuntimeId(int eid)
        {
            WriteUnsignedVarLong(eid); //$this->putUnsignedVarLong($eid);
        }
    }

    public struct Vector3
    {
        public float X, Y, Z;
        public Vector3(float x, float y, float z) { X = x; Y = y; Z = z; }
        public int getFloorX()
        {
            return (int)Math.Floor(X);
        }

        public int getFloorY()
        {
            return (int)Math.Floor(Y);
        }

        public int getFloorZ()
        {
            return (int)Math.Floor(Z);
        }
    }

    public struct Vector2
    {
        public float X, Y;
        public Vector2(float x, float y) { X = x; Y = y; }
    }
}
