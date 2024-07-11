// THIS CODE WAS AUTO GENERATED. DO NOT CHANGE IT! 

using Lava.Raknet.Protocol;

namespace Lava.Raknet.Protocol.Types
{
    abstract public class GameRule
    {
        private bool isPlayerModifiable;

        public GameRule(bool isPlayerModifiable)
        {
            this.isPlayerModifiable = isPlayerModifiable;
        }

        public bool isPlayerModifiable()
        {
            return this.isPlayerModifiable;
        }

        abstract public int getTypeId();
        abstract public void encode(MinecraftStream ms);
    }
}
