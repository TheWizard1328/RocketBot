#region using directives

using System.Collections.Generic;
using PoGo.NecroBot.Logic.Model.Settings;
using POGOProtos.Enums;
using POGOProtos.Inventory.Item;

#endregion

namespace PoGo.NecroBot.Logic.Interfaces.Configuration
{
    public interface ILogicSettings
    {
        bool UseWebsocket { get; }
        bool CatchPokemon { get; }
        bool ByPassCatchFlee { get; }
        int OutOfBallCatchBlockTime { get; }
        int CatchPokemonLimit { get; }
        int CatchPokemonLimitMinutes { get; }
        int PokeStopLimit { get; }
        int PokeStopLimitMinutes { get; }
        bool TransferWeakPokemon { get; }
        bool DisableHumanWalking { get; }
        bool CheckForUpdates { get; }
        bool AutoUpdate { get; }
        float KeepMinIvPercentage { get; }
        int KeepMinCp { get; }
        int KeepMinLvl { get; }
        bool UseKeepMinLvl { get; }
        string KeepMinOperator { get; }
        bool KeepPokemonsToBeEvolved { get; }
        double WalkingSpeedInKilometerPerHour { get; }
        bool UseWalkingSpeedVariant { get; }
        double WalkingSpeedVariant { get; }
        bool ShowVariantWalking { get; }
        bool RandomlyPauseAtStops { get; }
        bool FastSoftBanBypass { get; }
        int ByPassSpinCount { get; }
        bool UseTransferFilterToCatch { get; }
        bool TransferDuplicatePokemon { get; }
        bool TransferDuplicatePokemonOnCapture { get; }
        bool UseBulkTransferPokemon { get; }
        bool UseEggIncubators { get; }
        bool UseLimitedEggIncubators { get; }
        int UseGreatBallAboveCp { get; }
        string UseBallOperator { get; }
        int UseUltraBallAboveCp { get; }
        int UseMasterBallAboveCp { get; }
        double UseGreatBallAboveIv { get; }
        double UseUltraBallAboveIv { get; }
        double UseMasterBallBelowCatchProbability { get; }
        double UseUltraBallBelowCatchProbability { get; }
        double UseGreatBallBelowCatchProbability { get; }
        bool EnableHumanizedThrows { get; }
        bool EnableMissedThrows { get; }
        int ThrowMissPercentage { get; }
        int NiceThrowChance { get; }
        int GreatThrowChance { get; }
        int ExcellentThrowChance { get; }
        int CurveThrowChance { get; }
        double ForceGreatThrowOverIv { get; }
        double ForceExcellentThrowOverIv { get; }
        int ForceGreatThrowOverCp { get; }
        int ForceExcellentThrowOverCp { get; }
        int DelayBetweenPokemonCatch { get; }
        int DelayBetweenPokemonUpgrade { get; }
        bool AutomaticallyLevelUpPokemon { get; }
        bool OnlyUpgradeFavorites { get; }
        bool UseLevelUpList { get; }
        string LevelUpByCPorIv { get; }
        float UpgradePokemonCpMinimum { get; }
        float UpgradePokemonIvMinimum { get; }
        int DelayBetweenPlayerActions { get; }
        bool UsePokemonToNotCatchFilter { get; }
        bool UsePokemonToCatchLocallyListOnly { get; }
        int KeepMinDuplicatePokemon { get; }
        int KeepMaxDuplicatePokemon { get; }
        bool PrioritizeIvOverCp { get; }
        int AmountOfTimesToUpgradeLoop { get; }
        int GetMinStarDustForLevelUp { get; }
        bool UseLuckyEggConstantly { get; }
        bool UseIncenseConstantly { get; }
        string UpgradePokemonMinimumStatsOperator { get; }
        int MaxTravelDistanceInMeters { get; }
        bool StartFromLastPosition { get; }
        bool UseGpxPathing { get; }
        string GpxFile { get; }
        #region Evolve
        bool EvolvePokemonsThatMatchFilter { get; }
        bool EvolveAnyPokemonAboveIv { get; }
        float EvolveAnyPokemonAboveIvValue { get; }
        bool TriggerEvolveAsSoonAsFilterIsMatched { get; }
        bool TriggerEvolveOnEvolutionCount { get; }
        int TriggerEvolveOnEvolutionCountValue { get; }
        bool TriggerEvolveOnStorageUsagePercentage { get; }
        double TriggerEvolveOnStorageUsagePercentageValue { get; }
        bool TriggerEvolveOnStorageUsageAbsolute { get; }
        int TriggerEvolveOnStorageUsageAbsoluteValue { get; }
        bool TriggerEvolveIfLuckyEggIsActive { get; }
        bool EvolvePreserveMinCandiesFromFilter { get; }
        bool EvolveApplyLuckyEggOnEvolutionCount { get; }
        int EvolveApplyLuckyEggOnEvolutionCountValue { get; }
        #endregion
        bool DumpPokemonStats { get; }
        bool RenamePokemon { get; }
        bool RenamePokemonRespectTransferRule { get; }
        bool RenameOnlyAboveIv { get; }
        float FavoriteMinIvPercentage { get; }
        float FavoriteMinCp { get; }
        int FavoriteMinLevel { get; }
        string FavoriteOperator { get; }
        bool AutoFavoritePokemon { get; }
        bool AutoFavoriteShinyOnCatch { get; }
        string RenameTemplate { get; }
        int AmountOfPokemonToDisplayOnStart { get; }
        string TranslationLanguageCode { get; }
        string ProfilePath { get; }
        string ProfileConfigPath { get; }
        string GeneralConfigPath { get; }
        int SchemaVersion { get; }
        bool UseTelegramAPI { get; }
        string TelegramAPIKey { get; }
        string TelegramPassword { get; }
        int MaxPokeballsPerPokemon { get; }
        bool RandomizeRecycle { get; }
        int RandomRecycleValue { get; }

        int TotalAmountOfPokeballsToKeep { get; }
        int TotalAmountOfPotionsToKeep { get; }
        int TotalAmountOfRevivesToKeep { get; }
        int TotalAmountOfBerriesToKeep { get; }
        int TotalAmountOfEvolutionToKeep { get; }

        bool UseRecyclePercentsInsteadOfTotals { get; }
        int PercentOfInventoryPokeballsToKeep { get; }
        int PercentOfInventoryPotionsToKeep { get; }
        int PercentOfInventoryRevivesToKeep { get; }
        int PercentOfInventoryBerriesToKeep { get; }
        int PercentOfInventoryEvolutionToKeep { get; }

        bool DetailedCountsBeforeRecycling { get; }
        bool VerboseRecycling { get; }
        double RecycleInventoryAtUsagePercentage { get; }
        bool UsePokeStopLimit { get; }
        bool UseCatchLimit { get; }
        bool UseNearActionRandom { get; }
        ICollection<KeyValuePair<ItemId, int>> ItemRecycleFilter { get; }

        ICollection<PokemonId> PokemonsToLevelUp { get; }
        CatchSettings PokemonToCatchLocally { get; }

        NotificationConfig NotificationConfig { get; }
        ICollection<PokemonId> PokemonsNotToTransfer { get; }

        ICollection<PokemonId> PokemonsNotToCatch { get; }

        ICollection<PokemonId> PokemonToUseMasterball { get; }

        Dictionary<PokemonId, TransferFilter> PokemonsTransferFilter { get; }
        Dictionary<PokemonId, EvolveFilter> PokemonEvolveFilters { get; }
        Dictionary<PokemonId, UpgradeFilter> PokemonUpgradeFilters { get; }

        Dictionary<PokemonId, BotSwitchPokemonFilter> BotSwitchPokemonFilters { get; }
        bool StartupWelcomeDelay { get; }
        bool UseGoogleWalk { get; }
        double DefaultStepLength { get; }
        bool UseGoogleWalkCache { get; }
        string GoogleApiKey { get; }
        string GoogleHeuristic { get; }
        string GoogleElevationApiKey { get; }

        bool UseYoursWalk { get; }
        string YoursWalkHeuristic { get; }

        bool UseMapzenWalk { get; }
        string MapzenTurnByTurnApiKey { get; }
        string MapzenWalkHeuristic { get; }
        string MapzenElevationApiKey { get; }

        int ResumeTrack { get; }
        int ResumeTrackSeg { get; }
        int ResumeTrackPt { get; }

        int EvolveActionDelay { get; }
        int TransferActionDelay { get; }
        int RecycleActionDelay { get; }
        int RenamePokemonActionDelay { get; }

        GymConfig GymConfig { get; }
        MultipleBotConfig MultipleBotConfig { get; }
        List<AuthConfig> Bots { get; }
        CaptchaConfig CaptchaConfig { get; }
        int BulkTransferStogareBuffer { get; }
        int BulkTransferSize { get; }

        string DefaultBuddyPokemon { get; }
        bool AutoFinishTutorial { get;  }
        bool SkipFirstTimeTutorial { get; }
        bool SkipCollectingLevelUpRewards { get; }
        Dictionary<ItemId, ItemUseFilter> ItemUseFilters { get; }
        double UpgradePokemonLvlMinimum { get; }

        bool UseHumanlikeDelays { get; }
        int CatchSuccessDelay { get; }
        int CatchErrorDelay { get; }
        int CatchEscapeDelay { get; }
        int CatchFleeDelay { get; }
        int CatchMissedDelay { get; }
        int BeforeCatchDelay { get; }
        bool AutoWalkAI { get; }
        int AutoWalkDist { get; }
    }
}
