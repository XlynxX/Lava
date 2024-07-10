// THIS CODE WAS AUTO GENERATED. DO NOT CHANGE IT! 

using Lava.Raknet.Protocol;

namespace Lava.Raknet.Protocol.Types
{
	public class StructureEditorData
	{
		public const int TYPE_DATA = 0;
		public const int TYPE_SAVE = 1;
		public const int TYPE_LOAD = 2;
		public const int TYPE_CORNER = 3;
		public const int TYPE_INVALID = 4;
		public const int TYPE_EXPORT = 5;
		public string structureName;
		public string structureDataField;
		public bool includePlayers;
		public bool showBoundingBox;
		public int structureBlockType;
		public StructureSettings structureSettings;
		public int structureRedstoneSaveMode;
	}
}
