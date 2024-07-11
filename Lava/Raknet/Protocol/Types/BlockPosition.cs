// THIS CODE WAS AUTO GENERATED. DO NOT CHANGE IT! 

using Lava.Raknet.Protocol;

namespace Lava.Raknet.Protocol.Types
{
    public sealed class BlockPosition
    {
        private int x;
        private int y;
        private int z;

        public BlockPosition(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public int getX()
        {
            return this.x;
        }

        public int getY()
        {
            return this.y;
        }

        public int getZ()
        {
            return this.z;
        }

        public BlockPosition fromVector3(Vector3 vector3)
        {
            return new BlockPosition(vector3.getFloorX(), vector3.getFloorY(), vector3.getFloorZ());
        }

        public bool equals(BlockPosition other)
        {
            return this.x == other.x && this.y == other.y && this.z == other.z;
        }

    }
}
