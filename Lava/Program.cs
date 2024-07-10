using Lava.Raknet;
using Lava.Raknet.Packets;
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
            Console.WriteLine("Lava started...");

            //StartGamePacket startGame = new StartGamePacket();
            //var bytes = startGame.Serialize();

            //StartGamePacket start = new StartGamePacket(bytes);
            //start.Deserialize();

            //Console.WriteLine(start.playerPosition.X);
            //Console.WriteLine(start.playerPosition.Y);
            //Console.WriteLine(start.playerPosition.Z);

            while (true) { }
        }
    }
}