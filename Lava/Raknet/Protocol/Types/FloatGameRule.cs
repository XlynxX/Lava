// THIS CODE WAS AUTO GENERATED. DO NOT CHANGE IT! 

using Lava.Raknet.Protocol;

namespace Lava.Raknet.Protocol.Types
{
    public sealed class FloatGameRule : GameRule 
    {
        public const int ID = GameRuleType.FLOAT;
        private float value;
        private bool isPlayerModifiable;

        public FloatGameRule(float value, bool isPlayerModifiable) : base()
        {
            this.isPlayerModifiable = isPlayerModifiable;
            hujna.__construct(isPlayerModifiable);
            this.value = value;
        }

        public float getValue()
        {
            return this.value;
        }

        public void encode(MinecraftStream ms)
        {
            ms.WriteFloat(this.value);
        }

        public FloatGameRule decode(MinecraftStream ms, bool isPlayerModifiable)
        {
            return new FloatGameRule(ms.ReadFloat(), isPlayerModifiable);
        }

    }
}
