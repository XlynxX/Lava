namespace Lava.Raknet.Protocol.Types
{
    public class LevelSettings
    {
        public long Seed { get; set; }
        public SpawnSettings SpawnSettings { get; set; }
        public int Generator { get; set; } = GeneratorType.OVERWORLD;
        public int WorldGamemode { get; set; }
        public bool Hardcore { get; set; } = false;
        public int Difficulty { get; set; }
        public BlockPosition SpawnPosition { get; set; }
        public bool HasAchievementsDisabled { get; set; } = true;
        public int EditorWorldType { get; set; } = Types.EditorWorldType.NON_EDITOR;
        public bool CreatedInEditorMode { get; set; } = false;
        public bool ExportedFromEditorMode { get; set; } = false;
        public int Time { get; set; } = -1;
        public int EduEditionOffer { get; set; } = EducationEditionOffer.NONE;
        public bool HasEduFeaturesEnabled { get; set; } = false;
        public string EduProductUUID { get; set; } = "";
        public float RainLevel { get; set; }
        public float LightningLevel { get; set; }
        public bool HasConfirmedPlatformLockedContent { get; set; } = false;
        public bool IsMultiplayerGame { get; set; } = true;
        public bool HasLANBroadcast { get; set; } = true;
        public int XboxLiveBroadcastMode { get; set; } = MultiplayerGameVisibility.PUBLIC;
        public int PlatformBroadcastMode { get; set; } = MultiplayerGameVisibility.PUBLIC;
        public bool CommandsEnabled { get; set; }
        public bool IsTexturePacksRequired { get; set; } = true;
        public Dictionary<string, GameRule> GameRules { get; set; } = new Dictionary<string, GameRule>();
        public Experiments Experiments { get; set; }
        public bool HasBonusChestEnabled { get; set; } = false;
        public bool HasStartWithMapEnabled { get; set; } = false;
        public int DefaultPlayerPermission { get; set; } = PlayerPermissions.MEMBER;
        public int ServerChunkTickRadius { get; set; } = 4;
        public bool HasLockedBehaviorPack { get; set; } = false;
        public bool HasLockedResourcePack { get; set; } = false;
        public bool IsFromLockedWorldTemplate { get; set; } = false;
        public bool UseMsaGamertagsOnly { get; set; } = false;
        public bool IsFromWorldTemplate { get; set; } = false;
        public bool IsWorldTemplateOptionLocked { get; set; } = false;
        public bool OnlySpawnV1Villagers { get; set; } = false;
        public bool DisablePersona { get; set; } = false;
        public bool DisableCustomSkins { get; set; } = false;
        public bool MuteEmoteAnnouncements { get; set; } = false;
        public string VanillaVersion { get; set; } = "1.21.";
        public int LimitedWorldWidth { get; set; } = 0;
        public int LimitedWorldLength { get; set; } = 0;
        public bool IsNewNether { get; set; } = true;
        public EducationUriResource? EduSharedUriResource { get; set; } = null;
        public bool? ExperimentalGameplayOverride { get; set; } = null;
        public int ChatRestrictionLevel { get; set; } = Types.ChatRestrictionLevel.NONE;
        public bool DisablePlayerInteractions { get; set; } = false;
        public string ServerIdentifier { get; set; } = "";
        public string WorldIdentifier { get; set; } = "";
        public string ScenarioIdentifier { get; set; } = "";

        public static LevelSettings Read(byte[] bytes)
        {
            var result = new LevelSettings();
            result.InternalRead(bytes);
            return result;
        }

        private void InternalRead(byte[] bytes)
        {
            MinecraftStream ms = new MinecraftStream(bytes);
            //Seed = ms.readLLong();
            //SpawnSettings = SpawnSettings.Read(ms);
            //Generator = ms.readVarInt();
            //WorldGamemode = ms.readVarInt();
            //Hardcore = ms.readBool();
            //Difficulty = ms.readVarInt();
            //SpawnPosition = ms.readBlockPosition();
            //HasAchievementsDisabled = ms.readBool();
            //EditorWorldType = ms.readVarInt();
            //CreatedInEditorMode = ms.readBool();
            //ExportedFromEditorMode = ms.readBool();
            //Time = ms.readVarInt();
            //EduEditionOffer = ms.readVarInt();
            //HasEduFeaturesEnabled = ms.readBool();
            //EduProductUUID = ms.readString();
            //RainLevel = ms.readLFloat();
            //LightningLevel = ms.readLFloat();
            //HasConfirmedPlatformLockedContent = ms.readBool();
            //IsMultiplayerGame = ms.readBool();
            //HasLANBroadcast = ms.readBool();
            //XboxLiveBroadcastMode = ms.readVarInt();
            //PlatformBroadcastMode = ms.readVarInt();
            //CommandsEnabled = ms.readBool();
            //IsTexturePacksRequired = ms.readBool();
            //GameRules = ms.readGameRules();
            //Experiments = Experiments.Read(ms);
            //HasBonusChestEnabled = ms.readBool();
            //HasStartWithMapEnabled = ms.readBool();
            //DefaultPlayerPermission = ms.readVarInt();
            //ServerChunkTickRadius = ms.readLInt();
            //HasLockedBehaviorPack = ms.readBool();
            //HasLockedResourcePack = ms.readBool();
            //IsFromLockedWorldTemplate = ms.readBool();
            //UseMsaGamertagsOnly = ms.readBool();
            //IsFromWorldTemplate = ms.readBool();
            //IsWorldTemplateOptionLocked = ms.readBool();
            //OnlySpawnV1Villagers = ms.readBool();
            //DisablePersona = ms.readBool();
            //DisableCustomSkins = ms.readBool();
            //MuteEmoteAnnouncements = ms.readBool();
            //VanillaVersion = ms.readString();
            //LimitedWorldWidth = ms.readLInt();
            //LimitedWorldLength = ms.readLInt();
            //IsNewNether = ms.readBool();
            //EduSharedUriResource = EducationUriResource.Read(ms);
            //ExperimentalGameplayOverride = ms.ReadOptional(ms.readBool);
            //ChatRestrictionLevel = ms.readByte();
            //DisablePlayerInteractions = ms.readBool();
            //ServerIdentifier = ms.readString();
            //WorldIdentifier = ms.readString();
            //ScenarioIdentifier = ms.readString();
        }

        public void Write()
        {
            //output.PutLLong(Seed);
            //SpawnSettings.Write(output);
            //output.PutVarInt(Generator);
            //output.PutVarInt(WorldGamemode);
            //output.PutBool(Hardcore);
            //output.PutVarInt(Difficulty);
            //output.PutBlockPosition(SpawnPosition);
            //output.PutBool(HasAchievementsDisabled);
            //output.PutVarInt(EditorWorldType);
            //output.PutBool(CreatedInEditorMode);
            //output.PutBool(ExportedFromEditorMode);
            //output.PutVarInt(Time);
            //output.PutVarInt(EduEditionOffer);
            //output.PutBool(HasEduFeaturesEnabled);
            //output.PutString(EduProductUUID);
            //output.PutLFloat(RainLevel);
            //output.PutLFloat(LightningLevel);
            //output.PutBool(HasConfirmedPlatformLockedContent);
            //output.PutBool(IsMultiplayerGame);
            //output.PutBool(HasLANBroadcast);
            //output.PutVarInt(XboxLiveBroadcastMode);
            //output.PutVarInt(PlatformBroadcastMode);
            //output.PutBool(CommandsEnabled);
            //output.PutBool(IsTexturePacksRequired);
            //output.PutGameRules(GameRules);
            //Experiments.Write(output);
            //output.PutBool(HasBonusChestEnabled);
            //output.PutBool(HasStartWithMapEnabled);
            //output.PutVarInt(DefaultPlayerPermission);
            //output.PutLInt(ServerChunkTickRadius);
            //output.PutBool(HasLockedBehaviorPack);
            //output.PutBool(HasLockedResourcePack);
            //output.PutBool(IsFromLockedWorldTemplate);
            //output.PutBool(UseMsaGamertagsOnly);
            //output.PutBool(IsFromWorldTemplate);
            //output.PutBool(IsWorldTemplateOptionLocked);
            //output.PutBool(OnlySpawnV1Villagers);
            //output.PutBool(DisablePersona);
            //output.PutBool(DisableCustomSkins);
            //output.PutBool(MuteEmoteAnnouncements);
            //output.PutString(VanillaVersion);
            //output.PutLInt(LimitedWorldWidth);
            //output.PutLInt(LimitedWorldLength);
            //output.PutBool(IsNewNether);
            //(EduSharedUriResource ?? new EducationUriResource("", "")).Write(output);
            //output.WriteOptional(ExperimentalGameplayOverride, output.PutBool);
            //output.PutByte(ChatRestrictionLevel);
            //output.PutBool(DisablePlayerInteractions);
            //output.PutString(ServerIdentifier);
            //output.PutString(WorldIdentifier);
            //output.PutString(ScenarioIdentifier);
        }
    }
}
