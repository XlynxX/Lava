// THIS CODE WAS AUTO GENERATED. DO NOT CHANGE IT! 

using Lava.Raknet.Protocol;

namespace Lava.Raknet.Protocol.Types
{
	public class ScorePacketEntry
	{
		public const int TYPE_PLAYER = 1;
		public const int TYPE_ENTITY = 2;
		public const int TYPE_FAKE_PLAYER = 3;
		public int scoreboardId;
		public string objectiveName;
		public int score;
		public int type;
		public int actorUniqueId;
		public string customName;
	}
}
