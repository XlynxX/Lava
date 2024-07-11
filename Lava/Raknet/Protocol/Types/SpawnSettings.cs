// THIS CODE WAS AUTO GENERATED. DO NOT CHANGE IT! 

using Lava.Raknet.Protocol;

namespace Lava.Raknet.Protocol.Types
{
    public sealed class SpawnSettings
    {
        public const int BIOME_TYPE_DEFAULT = 0;
        public const int BIOME_TYPE_USER_DEFINED = 1;
        private int biomeType;
        private string biomeName;
        private int dimension;

        public SpawnSettings(int biomeType, string biomeName, int dimension)
        {
            this.biomeType = biomeType;
            this.biomeName = biomeName;
            this.dimension = dimension;
        }

        public int getBiomeType()
        {
            return this.biomeType;
        }

        public string getBiomeName()
        {
            return this.biomeName;
        }

        public int getDimension()
        {
            return this.dimension;
        }

        public static SpawnSettings read(MinecraftStream ms)
        {
            var biomeType = ms.ReadShort();
            var biomeName = ms.ReadString();
            var dimension = ms.ReadVarInt();
            return new SpawnSettings(biomeType, biomeName, dimension);
        }

        public void write(MinecraftStream ms)
        {
            ms.WriteShort((short) this.biomeType);
            ms.WriteString(this.biomeName);
            ms.WriteVarInt(this.dimension);
        }

    }
}
