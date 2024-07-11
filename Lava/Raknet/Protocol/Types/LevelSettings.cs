// THIS CODE WAS AUTO GENERATED. DO NOT CHANGE IT! 

using Lava.Raknet.Protocol;

namespace Lava.Raknet.Protocol.Types
{
    public sealed class LevelSettings
    {
        public int seed;
        public SpawnSettings spawnSettings;
        public int generator;
        public int worldGamemode;
        public bool hardcore = false;
        public int difficulty;
        public BlockPosition spawnPosition;
        public bool hasAchievementsDisabled = true;
        public int editorWorldType;
        public bool createdInEditorMode = false;
        public bool exportedFromEditorMode = false;
        public int time;
        public int eduEditionOffer;
        public bool hasEduFeaturesEnabled = false;
        public string eduProductUUID = "";
        public float rainLevel;
        public float lightningLevel;
        public bool hasConfirmedPlatformLockedContent = false;
        public bool isMultiplayerGame = true;
        public bool hasLANBroadcast = true;
        public int xboxLiveBroadcastMode;
        public int platformBroadcastMode;
        public bool commandsEnabled;
        public bool isTexturePacksRequired = true;
        //public array gameRules;
        //public Experiments experiments;
        //public bool hasBonusChestEnabled = false;
        //public bool hasStartWithMapEnabled = false;
        //public int defaultPlayerPermission;
        //public int serverChunkTickRadius = 4;
        //public bool hasLockedBehaviorPack = false;
        //public bool hasLockedResourcePack = false;
        //public bool isFromLockedWorldTemplate = false;
        //public bool useMsaGamertagsOnly = false;
        //public bool isFromWorldTemplate = false;
        //public bool isWorldTemplateOptionLocked = false;
        //public bool onlySpawnV1Villagers = false;
        //public bool disablePersona = false;
        //public bool disableCustomSkins = false;
        //public bool muteEmoteAnnouncements = false;
        //public string vanillaVersion;
        //public int limitedWorldWidth = 0;
        //public int limitedWorldLength = 0;
        //public bool isNewNether = true;
        ////public EducationUriResource eduSharedUriResource = null;
        //public bool experimentalGameplayOverride = null;
        //public int chatRestrictionLevel;
        //public bool disablePlayerInteractions = false;
        //public string serverIdentifier = "";
        //public string worldIdentifier = "";
        //public string scenarioIdentifier = "";

        public static LevelSettings Read(MinecraftStream ms)
        {
            var result = new LevelSettings();
            result.internalRead(ms);
            return result;
        }

        private void internalRead(MinecraftStream ms)
        {
            this.seed = (int) ms.ReadUnsignedLong();
            this.spawnSettings = SpawnSettings.read(ms);
            this.generator = ms.ReadVarInt();
            this.worldGamemode = ms.ReadVarInt();
            this.hardcore = ms.ReadBoolean();
            this.difficulty = ms.ReadVarInt();
            this.spawnPosition = ms.ReadBlockPosition();
            this.hasAchievementsDisabled = ms.ReadBoolean();
            this.editorWorldType = ms.ReadVarInt();
            this.createdInEditorMode = ms.ReadBoolean();
            this.exportedFromEditorMode = ms.ReadBoolean();
            this.time = ms.ReadVarInt();
            this.eduEditionOffer = ms.ReadVarInt();
            this.hasEduFeaturesEnabled = ms.ReadBoolean();
            this.eduProductUUID = ms.ReadString();
            this.rainLevel = ms.ReadFloat();
            this.lightningLevel = ms.ReadFloat();
            this.hasConfirmedPlatformLockedContent = ms.ReadBoolean();
            this.isMultiplayerGame = ms.ReadBoolean();
            this.hasLANBroadcast = ms.ReadBoolean();
            this.xboxLiveBroadcastMode = ms.ReadVarInt();
            this.platformBroadcastMode = ms.ReadVarInt();
            this.commandsEnabled = ms.ReadBoolean();
            this.isTexturePacksRequired = ms.ReadBoolean();
            this.gameRules = ms.getGameRules();
            //this.experiments = Experiments.read(ms);
            //this.hasBonusChestEnabled = ms.ReadBoolean();
            //this.hasStartWithMapEnabled = ms.ReadBoolean();
            //this.defaultPlayerPermission = ms.ReadVarInt();
            //this.serverChunkTickRadius = ms.getLInt();
            //this.hasLockedBehaviorPack = ms.ReadBoolean();
            //this.hasLockedResourcePack = ms.ReadBoolean();
            //this.isFromLockedWorldTemplate = ms.ReadBoolean();
            //this.useMsaGamertagsOnly = ms.ReadBoolean();
            //this.isFromWorldTemplate = ms.ReadBoolean();
            //this.isWorldTemplateOptionLocked = ms.ReadBoolean();
            //this.onlySpawnV1Villagers = ms.ReadBoolean();
            //this.disablePersona = ms.ReadBoolean();
            //this.disableCustomSkins = ms.ReadBoolean();
            //this.muteEmoteAnnouncements = ms.ReadBoolean();
            //this.vanillaVersion = ms.ReadString();
            //this.limitedWorldWidth = ms.getLInt();
            //this.limitedWorldLength = ms.getLInt();
            //this.isNewNether = ms.ReadBoolean();
            //this.eduSharedUriResource = EducationUriResource.read(ms);
            //this.experimentalGameplayOverride = ms.readOptional(ms.ReadBoolean(...));
            //this.chatRestrictionLevel = ms.ReadByte();
            //this.disablePlayerInteractions = ms.ReadBoolean();
            //this.serverIdentifier = ms.ReadString();
            //this.worldIdentifier = ms.ReadString();
            //this.scenarioIdentifier = ms.ReadString();
        }

        public void write(MinecraftStream ms)
        {
            ms.WriteUnsignedLong((ulong) this.seed);
            this.spawnSettings.write(ms);
            ms.WriteVarInt(this.generator);
            ms.WriteVarInt(this.worldGamemode);
            ms.WriteBoolean(this.hardcore);
            ms.WriteVarInt(this.difficulty);
            ms.WriteBlockPosition(this.spawnPosition);
            ms.WriteBoolean(this.hasAchievementsDisabled);
            ms.WriteVarInt(this.editorWorldType);
            ms.WriteBoolean(this.createdInEditorMode);
            ms.WriteBoolean(this.exportedFromEditorMode);
            ms.WriteVarInt(this.time);
            ms.WriteVarInt(this.eduEditionOffer);
            ms.WriteBoolean(this.hasEduFeaturesEnabled);
            ms.WriteString(this.eduProductUUID);
            ms.WriteFloat(this.rainLevel);
            ms.WriteFloat(this.lightningLevel);
            ms.WriteBoolean(this.hasConfirmedPlatformLockedContent);
            ms.WriteBoolean(this.isMultiplayerGame);
            ms.WriteBoolean(this.hasLANBroadcast);
            ms.WriteVarInt(this.xboxLiveBroadcastMode);
            ms.WriteVarInt(this.platformBroadcastMode);
            ms.WriteBoolean(this.commandsEnabled);
            ms.WriteBoolean(this.isTexturePacksRequired);
            //ms.putGameRules(this.gameRules);
            //this.experiments.write(ms);
            //ms.WriteBoolean(this.hasBonusChestEnabled);
            //ms.WriteBoolean(this.hasStartWithMapEnabled);
            //ms.WriteVarInt(this.defaultPlayerPermission);
            //ms.putLInt(this.serverChunkTickRadius);
            //ms.WriteBoolean(this.hasLockedBehaviorPack);
            //ms.WriteBoolean(this.hasLockedResourcePack);
            //ms.WriteBoolean(this.isFromLockedWorldTemplate);
            //ms.WriteBoolean(this.useMsaGamertagsOnly);
            //ms.WriteBoolean(this.isFromWorldTemplate);
            //ms.WriteBoolean(this.isWorldTemplateOptionLocked);
            //ms.WriteBoolean(this.onlySpawnV1Villagers);
            //ms.WriteBoolean(this.disablePersona);
            //ms.WriteBoolean(this.disableCustomSkins);
            //ms.WriteBoolean(this.muteEmoteAnnouncements);
            //ms.WriteString(this.vanillaVersion);
            //ms.putLInt(this.limitedWorldWidth);
            //ms.putLInt(this.limitedWorldLength);
            //ms.WriteBoolean(this.isNewNether);
            //this.eduSharedUriResource ?? new EducationUriResource("", "").write(ms);
            //ms.writeOptional(this.experimentalGameplayOverride, ms.WriteBoolean(...));
            //ms.WriteByte(this.chatRestrictionLevel);
            //ms.WriteBoolean(this.disablePlayerInteractions);
            //ms.WriteString(this.serverIdentifier);
            //ms.WriteString(this.worldIdentifier);
            //ms.WriteString(this.scenarioIdentifier);
        }

    }
}
