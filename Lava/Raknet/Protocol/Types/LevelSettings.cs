namespace Lava.Raknet.Protocol.Types
{
    public class LevelSettings
    {
        public ulong Seed { get; set; }
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

        public static LevelSettings Read(MinecraftStream ms)
        {
            var result = new LevelSettings();
            result.InternalRead(ms);
            return result;
        }

        private void InternalRead(MinecraftStream ms)
        {
            Seed = ms.ReadUnsignedLong(); // .ReadUnsignedLong(); //.readLLong(); // unsigned 64bit
            //SpawnSettings = SpawnSettings.Read(ms);
            // SPAWN SETTINGS

            ms.ReadShort();
            ms.ReadString();
            ms.ReadVarInt();
            //$biomeType = $in->getLShort();
		    //$biomeName = $in->getString();
		    //$dimension = $in->getVarInt();

            // SPAWN SETTINGS
            Generator = ms.ReadVarInt();
            WorldGamemode = ms.ReadVarInt();
            Hardcore = ms.ReadBoolean();
            Difficulty = ms.ReadVarInt();
            ms.ReadBlockCoordinates(); //SpawnPosition = ms.ReadBlockCoordinates();
            HasAchievementsDisabled = ms.ReadBoolean();
            EditorWorldType = ms.ReadVarInt();
            CreatedInEditorMode = ms.ReadBoolean();
            ExportedFromEditorMode = ms.ReadBoolean();
            Time = ms.ReadVarInt();
            EduEditionOffer = ms.ReadVarInt();
            HasEduFeaturesEnabled = ms.ReadBoolean();
            EduProductUUID = ms.ReadString();
            RainLevel = ms.ReadFloat();
            LightningLevel = ms.ReadFloat();
            HasConfirmedPlatformLockedContent = ms.ReadBoolean();
            IsMultiplayerGame = ms.ReadBoolean();
            HasLANBroadcast = ms.ReadBoolean();
            XboxLiveBroadcastMode = ms.ReadVarInt();
            PlatformBroadcastMode = ms.ReadVarInt();
            CommandsEnabled = ms.ReadBoolean();
            IsTexturePacksRequired = ms.ReadBoolean();
            //GameRules = ms.readGameRules();
            //Experiments = Experiments.Read(ms);
            HasBonusChestEnabled = ms.ReadBoolean();
            HasStartWithMapEnabled = ms.ReadBoolean();
            DefaultPlayerPermission = ms.ReadVarInt();
            ServerChunkTickRadius = ms.ReadInt();
            HasLockedBehaviorPack = ms.ReadBoolean();
            HasLockedResourcePack = ms.ReadBoolean();
            IsFromLockedWorldTemplate = ms.ReadBoolean();
            UseMsaGamertagsOnly = ms.ReadBoolean();
            IsFromWorldTemplate = ms.ReadBoolean();
            IsWorldTemplateOptionLocked = ms.ReadBoolean();
            OnlySpawnV1Villagers = ms.ReadBoolean();
            DisablePersona = ms.ReadBoolean();
            DisableCustomSkins = ms.ReadBoolean();
            MuteEmoteAnnouncements = ms.ReadBoolean();
            VanillaVersion = ms.ReadString();
            LimitedWorldWidth = ms.ReadInt();
            LimitedWorldLength = ms.ReadInt();
            IsNewNether = ms.ReadBoolean();
            //EduSharedUriResource = EducationUriResource.Read(ms);
            //ExperimentalGameplayOverride = ms.ReadOptional(ms.ReadBoolean);
            ChatRestrictionLevel = ms.ReadByte();
            DisablePlayerInteractions = ms.ReadBoolean();
            ServerIdentifier = ms.ReadString();
            WorldIdentifier = ms.ReadString();
            ScenarioIdentifier = ms.ReadString();
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
