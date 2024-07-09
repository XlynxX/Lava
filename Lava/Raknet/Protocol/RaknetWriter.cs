using SharpRakNet.Protocol.Raknet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Lava.Raknet.Protocol;
using System.Buffers.Binary;

namespace Lava.Raknet
{
    public class RaknetWriter
    {
        private List<byte> buf = new List<byte>();

        public void Write(byte[] v)
        {
            buf.AddRange(v);
        }


        public void WriteU8(PacketID id)
        {
            WriteU8((byte)id);
        }
        public void WriteU8(GamePacketID id)
        {
            WriteU8((byte)id);
        }

        public void WriteU8(byte v)
        {
            buf.Add(v);
        }

        public void WriteI16(short v, Endian n)
        {
            byte[] bytes;
            if (n == Endian.Big)
            {
                bytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(v));
            }
            else
            {
                bytes = BitConverter.GetBytes(v);
            }
            buf.AddRange(bytes);
        }

        public void WriteShort(short value)
        {
            byte[] buffer = new byte[sizeof(short)];
            BinaryPrimitives.WriteInt16LittleEndian(buffer, value);
            Write(buffer);
        }

        public void WriteU16(ushort v, Endian n)
        {
            byte[] bytes;
            if (n == Endian.Big)
            {
                bytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)v));
            }
            else
            {
                bytes = BitConverter.GetBytes(v);
            }
            buf.AddRange(bytes);
        }

        public void WriteU24(uint v, Endian n)
        {
            byte[] bytes;
            if (n == Endian.Big)
            {
                bytes = new byte[]
                {
                (byte)(v >> 16 & 0xFF),
                (byte)(v >> 8 & 0xFF),
                (byte)(v & 0xFF)
                };
            }
            else
            {
                bytes = new byte[]
                {
                (byte)(v & 0xFF),
                (byte)(v >> 8 & 0xFF),
                (byte)(v >> 16 & 0xFF)
                };
            }
            buf.AddRange(bytes);
        }

        public void WriteI32(int v, Endian n)
        {
            byte[] bytes;
            if (n == Endian.Big)
            {
                bytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(v));
            }
            else
            {
                bytes = BitConverter.GetBytes(v);
            }
            buf.AddRange(bytes);
        }

        public void WriteU32(uint v, Endian n)
        {
            byte[] bytes;
            if (n == Endian.Big)
            {
                bytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((int)v));
            }
            else
            {
                bytes = BitConverter.GetBytes(v);
            }
            buf.AddRange(bytes);
        }

        public void WriteI64(long v, Endian n)
        {
            byte[] bytes;
            if (n == Endian.Big)
            {
                bytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(v));
            }
            else
            {
                bytes = BitConverter.GetBytes(v);
            }
            buf.AddRange(bytes);
        }

        public void WriteMagic()
        {
            byte[] magic = new byte[]
            {
            0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe,
            0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78
            };
            buf.AddRange(magic);
        }

        public void WriteU64(ulong v, Endian n)
        {
            byte[] bytes;
            if (n == Endian.Big)
            {
                bytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((long)v));
            }
            else
            {
                bytes = BitConverter.GetBytes(v);
            }
            buf.AddRange(bytes);
        }

        public void WriteString(string body)
        {
            byte[] raw = Encoding.UTF8.GetBytes(body);
            WriteU16((ushort)raw.Length, Endian.Big);
            //WriteVarInt(raw.Length);
            buf.AddRange(raw);
        }

        public void WriteF32(float v, Endian n)
        {
            byte[] bytes = BitConverter.GetBytes(v);
            if (n == Endian.Big && BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            buf.AddRange(bytes);
        }
        public float ReadF32(byte[] data, int startIndex, Endian n)
        {
            byte[] bytes = new byte[4];
            Array.Copy(data, startIndex, bytes, 0, 4);
            if (n == Endian.Big && BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            return BitConverter.ToSingle(bytes, 0);
        }
        public void WriteBool(bool v)
        {
            buf.Add(v ? (byte)1 : (byte)0);
        }
        public bool ReadBool(byte[] data, int startIndex)
        {
            return data[startIndex] != 0;
        }

        public void WriteAddress(IPEndPoint address)
        {
            if (address.AddressFamily == AddressFamily.InterNetwork)
            {
                WriteU8(0x4);
                byte[] ipBytes = address.Address.GetAddressBytes();
                for (int i = 0; i < ipBytes.Length; i++)
                {
                    WriteU8((byte)(0xFF - ipBytes[i]));
                }
                WriteU16((ushort)address.Port, Endian.Big);
            }
            else
            {
                WriteI16(23, Endian.Little);
                WriteU16((ushort)address.Port, Endian.Big);
                WriteI32(0, Endian.Big);
                byte[] ipBytes = address.Address.GetAddressBytes();
                Write(ipBytes);
                WriteI32(0, Endian.Big);
            }
        }
        
        public void WriteVarInt(int value)
        {
            do
            {
                byte temp = (byte)(value & 0x7F);
                value >>= 7;

                if (value != 0)
                {
                    temp |= 0x80;
                }

                WriteU8(temp);
            }
            while (value != 0);
        }

        public void WriteUVarInt(uint value)
        {
            while ((value & -128) != 0)
            {
                WriteU8((byte)((value & 0x7F) | 0x80));
                value >>= 7;
            }

            WriteU8((byte)value);
        }

        public void WriteSequences(List<AckRange> sequences)
        {
            WriteU16((ushort)sequences.Count, Endian.Big);

            foreach (var sequence in sequences)
            {
                // Check if Start and End properties are not null before accessing them
                if (sequence != null)
                {
                    byte singleSequenceNumber = (byte)(sequence.Start == sequence.End ? 0x01 : 0x00);
                    WriteU8(singleSequenceNumber);
                    WriteU24(sequence.Start, Endian.Little);

                    if (singleSequenceNumber == 0x00)
                    {
                        WriteU24(sequence.End, Endian.Little);
                    }
                }
            }
        }

        public byte[] GetRawPayload()
        {
            return buf.ToArray();
        }

        public ulong Position
        {
            get { return (ulong)buf.Count; }
        }
    }

    public enum Endian
    {
        Big,
        Little
    }
}
