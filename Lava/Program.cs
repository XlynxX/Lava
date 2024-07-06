using Lava.Raknet;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Lava
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            RaknetListener raknetListener = new RaknetListener(new IPEndPoint(IPAddress.Parse("0.0.0.0"), 19132));
            Console.WriteLine("UDP-сервер запущен...");
            //raknetListener.
            
            while (true) { }
        }

        //    static void OnRecievePing(IPEndPoint address, UnconnectedPing packet)
        //    {
        //        string message = "--- " + address.Address + " ---";

        //        Console.WriteLine(message);

        //        Console.WriteLine("Ping Time: " + packet.time);
        //        Console.WriteLine("Ping Magic: " + packet.magic);
        //        Console.WriteLine("Ping GUID: " + packet.guid);

        //        Console.WriteLine(string.Concat(Enumerable.Repeat("-", message.Length)));
        //    }

        //    static void OnDisconnected(RaknetSession session)
        //    {
        //        Console.WriteLine(session.PeerEndPoint);
        //    }

        //    static void WriteString(BinaryWriter writer, string value)
        //    {
        //        var bytes = Encoding.UTF8.GetBytes(value);
        //        var length = (short)bytes.Length;

        //        // Convert to big-endian
        //        var lengthBytes = BitConverter.GetBytes(length);
        //        if (BitConverter.IsLittleEndian)
        //        {
        //            Array.Reverse(lengthBytes);
        //        }

        //        writer.Write(lengthBytes);
        //        writer.Write(bytes);
        //    }

        //    static byte[] StringToByteArray(string hex)
        //    {
        //        int NumberChars = hex.Length;
        //        byte[] bytes = new byte[NumberChars / 2];
        //        for (int i = 0; i < NumberChars; i += 2)
        //            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        //        return bytes;
        //    }
        //}
    }
}

//while (true)
//{

//    // получаем данные
//    var result = await udpServer.ReceiveAsync();
//    // предположим, что отправлена строка, преобразуем байты в строку
//    var message = result.Buffer;

//    if (result.Buffer[0] == 0x01)
//    {

//        using (var ms = new MemoryStream())
//        using (var writer = new BinaryWriter(ms))
//        {
//            writer.Write(PacketId);
//            writer.Write(_time);
//            writer.Write(_serverGuid);
//            writer.Write([0x00, 0xFF, 0xFF, 0x00, 0xFE, 0xFE, 0xFE, 0xFE, 0xFD, 0xFD, 0xFD, 0xFD, 0x12, 0x34, 0x56, 0x78]);
//            //writer.Write((short)Encoding.UTF8.GetBytes(_serverIdString).Length);
//            WriteString(writer, _serverIdString);
//            //WriteString(writer, _serverIdString);

//            var bytesgavna = ms.ToArray();
//            udpServer.Send(bytesgavna, bytesgavna.Length, result.RemoteEndPoint);
//            Console.WriteLine($"Отправлено: {bytesgavna.Length}");
//            foreach (byte _byte in bytesgavna)
//            {
//                Console.Write($"0x{_byte:X2} ");
//            }
//            Console.WriteLine(" SENT");

//            // 0x1C PACKET ID
//            // 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 TIME
//            // 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 SERVER GUID
//            // 0x00 0xFF 0xFF 0x00 0xFE 0xFE 0xFE 0xFE 0xFD 0xFD 0xFD 0xFD 0x12 0x34 0x56 0x78 MAGIC
//            // 0x61 0x00 0x4D 0x43 0x50 0x45 0x3B 0x44 0x65 0x64 0x69 0x63 0x61 0x74 0x65 0x64 0x20 0x53 0x65 0x72 0x76 0x65 0x72 0x3B 0x33 0x39 0x30 0x3B 0x31 0x2E 0x31 0x34 0x2E 0x36 0x30 0x3B 0x30 0x3B 0x310x30 0x3B 0x31 0x33 0x32 0x35 0x33 0x38 0x36 0x30 0x38 0x39 0x32 0x33 0x32 0x38 0x39 0x33 0x30 0x38 0x36 0x35 0x3B 0x420x65 0x64 0x72 0x6F 0x63 0x6B 0x20 0x6C 0x65 0x76 0x65 0x6C 0x3B 0x53 0x75 0x72 0x76 0x69 0x76 0x61 0x6C 0x3B 0x31 0x3B 0x31 0x39 0x31 0x33 0x32 0x3B 0x31 0x39 0x31 0x33 0x33 0x3B
//            // SENT
//        }
//    }

//    Console.WriteLine($"Получено {result.Buffer.Length} байт");
//    Console.WriteLine($"Удаленный адрес: {result.RemoteEndPoint}");
//    foreach (byte _byte in result.Buffer)
//    {
//        Console.Write($"0x{_byte:X2} ");
//    }
//    Console.WriteLine(" DAUN");
//    //Console.WriteLine(Encoding.UTF8.GetString(message));
//    // 0x01 PACKET ID
//    // 0x00 0x00 0x00 0x00 0x0C 0x91 0xB1 0x19 TIME
//    // 0x00 0xFF 0xFF 0x00 0xFE 0xFE 0xFE 0xFE 0xFD 0xFD 0xFD 0xFD 0x12 0x34 0x56 0x78 MAGIC
//    // 0x8E 0xE1 0x8D 0x2C 0x11 0xFF 0x88 0x81 CLIENT GUID
//}
