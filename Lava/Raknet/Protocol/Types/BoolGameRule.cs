// THIS CODE WAS AUTO GENERATED. DO NOT CHANGE IT! 

using Lava.Raknet.Protocol;

namespace Lava.Raknet.Protocol.Types
{
    public sealed class BoolGameRule : GameRule 
    {
        public const int ID = GameRuleType.BOOL;
        private bool value;
        private bool isPlayerModifiable;

        public BoolGameRule(bool value, bool isPlayerModifiable) : base()
        {
            this.isPlayerModifiable = isPlayerModifiable;
            hujna.__construct(isPlayerModifiable);
            this.value = value;
        }

        public bool getValue()
        {
            return this.value;
        }

        public void encode(MinecraftStream ms)
        {
            ms.WriteBoolean(this.value);
        }

        public BoolGameRule decode(MinecraftStream ms, bool isPlayerModifiable)
        {
            return new BoolGameRule(ms.ReadBoolean(), isPlayerModifiable);
        }

    }
}
