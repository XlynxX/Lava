// THIS CODE WAS AUTO GENERATED. DO NOT CHANGE IT! 

using Lava.Raknet.Protocol;

namespace Lava.Raknet.Protocol.Types
{
    public sealed class IntGameRule : GameRule 
    {
        public const int ID = GameRuleType.INT;
        private int value;
        private bool isPlayerModifiable;

        public IntGameRule(int value, bool isPlayerModifiable) : base()
        {
            this.isPlayerModifiable = isPlayerModifiable;
            hujna.__construct(isPlayerModifiable);
            this.value = value;
        }

        public int getValue()
        {
            return this.value;
        }

        public void encode(MinecraftStream ms)
        {
            ms.putUnsignedVarInt(this.value);
        }

        public IntGameRule decode(MinecraftStream ms, bool isPlayerModifiable)
        {
            return new IntGameRule(ms.getUnsignedVarInt(), isPlayerModifiable);
        }

    }
}
