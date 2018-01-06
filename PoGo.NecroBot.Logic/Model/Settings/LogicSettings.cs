#region using directives

using System.Collections.Generic;
using System.Linq;
using PoGo.NecroBot.Logic.Interfaces.Configuration;
using POGOProtos.Enums;
using POGOProtos.Inventory.Item;
using System;

#endregion

namespace PoGo.NecroBot.Logic.Model.Settings
{
    public class LogicSettings : ILogicSettings
    {
        private readonly GlobalSettings _settings;
        private Random _rand;

        public LogicSettings(GlobalSettings settings)
        {
            _settings = settings;
            _rand = new Random();
        }

        public double GenRandom(double min, double max)
        {
            return _rand.NextDouble() * (max - min) + min;
        }

        public int GenRandom(int val)
        {
            double min = val - (_settings.PlayerConfig.RandomizeSettingsByPercent / 100.0 * val);
            int newVal = (int)Math.Floor(GenRandom(min, val));
            if (newVal < 0)
                newVal = 0;
            return newVal;
        }

        public double GenRandom(double val)
        {
            double min = val - (_settings.PlayerConfig.RandomizeSettingsByPercent / 100.0 * val);
            double newVal = GenRandom(min, val);
            if (newVal < 0)
                newVal = 0;
            return newVal;
        }

        public float GenRandom(float val)
        {
            double min = val - (_settings.PlayerConfig.RandomizeSettingsByPercent / 100.0 * val);
            double newVal = GenRandom(min, val);
            if (newVal < 0)
                newVal = 0;
            return (float)newVal;
        }

        public string ProfilePath => _settings.ProfilePath;
        public string ProfileConfigPath => _settings.ProfileConfigPath;
        public string GeneralConfigPath => _settings.GeneralConfigPath;
        public int SchemaVersion => _settings.UpdateConfig.SchemaVersion;
        public bool CheckForUpdates => _settings.UpdateConfig.CheckForUpdates;
        public bool AutoUpdate => _settings.UpdateConfig.AutoUpdate;
        public bool UseWebsocket => _settings.WebsocketsConfig.UseWebsocket;
        public bool CatchPokemon => _settings.PokemonConfig.CatchPokemon;
        public int OutOfBallCatchBlockTime => GenRandom(_settings.PokemonConfig.OutOfBallCatchBlockTime);
        public int CatchPokemonLimit => GenRandom(_settings.PokemonConfig.CatchPokemonLimit);
        public int CatchPokemonLimitMinutes => GenRandom(_settings.PokemonConfig.CatchPokemonLimitMinutes);
        public int PokeStopLimit => GenRandom(_settings.PokeStopConfig.PokeStopLimit);
        public int PokeStopLimitMinutes => GenRandom(_settings.PokeStopConfig.PokeStopLimitMinutes);
        public bool TransferWeakPokemon => _settings.PokemonConfig.TransferWeakPokemon;
        public bool DisableHumanWalking => _settings.LocationConfig.DisableHumanWalking;
        public float KeepMinIvPercentage => _settings.PokemonConfig.KeepMinIvPercentage;
        public string KeepMinOperator => _settings.PokemonConfig.KeepMinOperator;
        public int KeepMinCp => _settings.PokemonConfig.KeepMinCp;
        public int KeepMinLvl => _settings.PokemonConfig.KeepMinLvl;
        public bool UseKeepMinLvl => _settings.PokemonConfig.UseKeepMinLvl;
        public bool KeepPokemonsToBeEvolved => _settings.PokemonConfig.KeepPokemonsToBeEvolved;
        public bool AutomaticallyLevelUpPokemon => _settings.PokemonConfig.AutomaticallyLevelUpPokemon;
        public bool OnlyUpgradeFavorites => _settings.PokemonConfig.OnlyUpgradeFavorites;
        public double UpgradePokemonLvlMinimum => _settings.PokemonConfig.UpgradePokemonLvlMinimum;
        public bool UseLevelUpList => _settings.PokemonConfig.UseLevelUpList;
        public int AmountOfTimesToUpgradeLoop => _settings.PokemonConfig.AmountOfTimesToUpgradeLoop;
        public string LevelUpByCPorIv => _settings.PokemonConfig.LevelUpByCPorIv;
        public int GetMinStarDustForLevelUp => _settings.PokemonConfig.GetMinStarDustForLevelUp;
        public bool UseLuckyEggConstantly => _settings.PokemonConfig.UseLuckyEggConstantly;
        public bool UseIncenseConstantly => _settings.PokemonConfig.UseIncenseConstantly;
        public string UseBallOperator => _settings.PokemonConfig.UseBallOperator.ToString();
        public float UpgradePokemonIvMinimum => _settings.PokemonConfig.UpgradePokemonIvMinimum;
        public float UpgradePokemonCpMinimum => _settings.PokemonConfig.UpgradePokemonCpMinimum;
        public string UpgradePokemonMinimumStatsOperator => _settings.PokemonConfig.UpgradePokemonMinimumStatsOperator;
        public double WalkingSpeedInKilometerPerHour => _settings.LocationConfig.WalkingSpeedInKilometerPerHour;
        public bool UseWalkingSpeedVariant => _settings.LocationConfig.UseWalkingSpeedVariant;
        public double WalkingSpeedVariant => _settings.LocationConfig.WalkingSpeedVariant;
        public bool ShowVariantWalking => _settings.LocationConfig.ShowVariantWalking;
        public bool FastSoftBanBypass => _settings.SoftBanConfig.FastSoftBanBypass;
        public int ByPassSpinCount => _settings.SoftBanConfig.ByPassSpinCount;
        public bool ByPassCatchFlee => _settings.PokemonConfig.ByPassCatchFlee;
        public bool TransferDuplicatePokemon => _settings.PokemonConfig.TransferDuplicatePokemon;
        public bool TransferDuplicatePokemonOnCapture => _settings.PokemonConfig.TransferDuplicatePokemonOnCapture;
        public bool UseBulkTransferPokemon => _settings.PokemonConfig.UseBulkTransferPokemon;
        public string DefaultBuddyPokemon => _settings.PokemonConfig.DefaultBuddyPokemon;
        public bool AutoFinishTutorial => _settings.PlayerConfig.AutoFinishTutorial;
        public bool SkipFirstTimeTutorial => _settings.PlayerConfig.SkipFirstTimeTutorial;
        public bool SkipCollectingLevelUpRewards => _settings.PlayerConfig.SkipCollectingLevelUpRewards;
        public int BulkTransferSize => _settings.PokemonConfig.BulkTransferSize;
        public int BulkTransferStogareBuffer => _settings.PokemonConfig.BulkTransferStogareBuffer;
        public bool AutoWalkAI => _settings.PlayerConfig.AutoWalkAI;
        public int AutoWalkDist => _settings.PlayerConfig.AutoWalkDist;

        public bool UseEggIncubators => _settings.PokemonConfig.UseEggIncubators;
        public bool UseLimitedEggIncubators => _settings.PokemonConfig.UseLimitedEggIncubators;
        public int UseGreatBallAboveCp => GenRandom(_settings.PokemonConfig.UseGreatBallAboveCp);
        public int UseUltraBallAboveCp => GenRandom(_settings.PokemonConfig.UseUltraBallAboveCp);
        public int UseMasterBallAboveCp => GenRandom(_settings.PokemonConfig.UseMasterBallAboveCp);
        public double UseGreatBallAboveIv => GenRandom(_settings.PokemonConfig.UseGreatBallAboveIv);
        public double UseUltraBallAboveIv => GenRandom(_settings.PokemonConfig.UseUltraBallAboveIv);
        public double UseMasterBallBelowCatchProbability => GenRandom(_settings.PokemonConfig.UseMasterBallBelowCatchProbability);
        public double UseUltraBallBelowCatchProbability => GenRandom(_settings.PokemonConfig.UseUltraBallBelowCatchProbability);
        public double UseGreatBallBelowCatchProbability => GenRandom(_settings.PokemonConfig.UseGreatBallBelowCatchProbability);
        public bool EnableHumanizedThrows => _settings.CustomCatchConfig.EnableHumanizedThrows;
        public bool EnableMissedThrows => _settings.CustomCatchConfig.EnableMissedThrows;
        public int ThrowMissPercentage => _settings.CustomCatchConfig.ThrowMissPercentage;
        public int NiceThrowChance => GenRandom(_settings.CustomCatchConfig.NiceThrowChance);
        public int GreatThrowChance => GenRandom(_settings.CustomCatchConfig.GreatThrowChance);
        public int ExcellentThrowChance => GenRandom(_settings.CustomCatchConfig.ExcellentThrowChance);
        public int CurveThrowChance => GenRandom(_settings.CustomCatchConfig.CurveThrowChance);
        public double ForceGreatThrowOverIv => GenRandom(_settings.CustomCatchConfig.ForceGreatThrowOverIv);
        public double ForceExcellentThrowOverIv => GenRandom(_settings.CustomCatchConfig.ForceExcellentThrowOverIv);
        public int ForceGreatThrowOverCp => GenRandom(_settings.CustomCatchConfig.ForceGreatThrowOverCp);
        public int ForceExcellentThrowOverCp => GenRandom(_settings.CustomCatchConfig.ForceExcellentThrowOverCp);
        public int DelayBetweenPokemonUpgrade => GenRandom(_settings.PokemonConfig.DelayBetweenPokemonUpgrade);
        public int DelayBetweenPokemonCatch => GenRandom(_settings.PokemonConfig.DelayBetweenPokemonCatch);

        public int DelayBetweenPlayerActions => GenRandom(_settings.PlayerConfig.DelayBetweenPlayerActions);
        public int EvolveActionDelay => GenRandom(_settings.PlayerConfig.EvolveActionDelay);
        public int TransferActionDelay => GenRandom(_settings.PlayerConfig.TransferActionDelay);
        public int RecycleActionDelay => GenRandom(_settings.PlayerConfig.RecycleActionDelay);
        public int RenamePokemonActionDelay => GenRandom(_settings.PlayerConfig.RenamePokemonActionDelay);
        public bool UseNearActionRandom => _settings.PlayerConfig.UseNearActionRandom;
        public bool UsePokemonToNotCatchFilter => _settings.PokemonConfig.UsePokemonToNotCatchFilter;
        public bool UsePokemonToCatchLocallyListOnly => _settings.PokemonConfig.UsePokemonToCatchLocallyListOnly;
        public CatchSettings PokemonToCatchLocally => _settings.PokemonToCatchLocally;
        public int KeepMinDuplicatePokemon => _settings.PokemonConfig.KeepMinDuplicatePokemon;
        public int KeepMaxDuplicatePokemon => _settings.PokemonConfig.KeepMaxDuplicatePokemon;
        public bool PrioritizeIvOverCp => _settings.PokemonConfig.PrioritizeIvOverCp;
        public int MaxTravelDistanceInMeters => GenRandom(_settings.LocationConfig.MaxTravelDistanceInMeters);
        public bool StartFromLastPosition => _settings.LocationConfig.StartFromLastPosition;
        public string GpxFile => _settings.GPXConfig.GpxFile;
        public bool UseGpxPathing => _settings.GPXConfig.UseGpxPathing;
        public bool RenamePokemon => _settings.PokemonConfig.RenamePokemon;
        public bool RenamePokemonRespectTransferRule => _settings.PokemonConfig.RenamePokemonRespectTransferRule;
        public bool RenameOnlyAboveIv => _settings.PokemonConfig.RenameOnlyAboveIv;
        public float FavoriteMinIvPercentage => _settings.PokemonConfig.FavoriteMinIvPercentage;
        public float FavoriteMinCp => _settings.PokemonConfig.FavoriteMinCp;
        public int FavoriteMinLevel => _settings.PokemonConfig.FavoriteMinLevel;
        public string FavoriteOperator => _settings.PokemonConfig.FavoriteOperator.ToString();

        public bool AutoFavoritePokemon => _settings.PokemonConfig.AutoFavoritePokemon;
        public bool AutoFavoriteShinyOnCatch => _settings.PokemonConfig.AutoFavoriteShinyOnCatch;
        public string RenameTemplate => _settings.PokemonConfig.RenameTemplate;
        public int AmountOfPokemonToDisplayOnStart => _settings.ConsoleConfig.AmountOfPokemonToDisplayOnStart;
        public bool DumpPokemonStats => _settings.PokemonConfig.DumpPokemonStats;
        public string TranslationLanguageCode => _settings.ConsoleConfig.TranslationLanguageCode;
        public bool DetailedCountsBeforeRecycling => _settings.ConsoleConfig.DetailedCountsBeforeRecycling;
        public bool VerboseRecycling => _settings.RecycleConfig.VerboseRecycling;
        public double RecycleInventoryAtUsagePercentage => GenRandom(_settings.RecycleConfig.RecycleInventoryAtUsagePercentage);
        public Dictionary<ItemId, ItemUseFilter> ItemUseFilters => _settings.ItemUseFilters;

        public ICollection<KeyValuePair<ItemId, int>> ItemRecycleFilter => _settings.ItemRecycleFilter.Select(itemRecycleFilter => new KeyValuePair<ItemId, int>(itemRecycleFilter.Key, itemRecycleFilter.Value)).ToList();
        public ICollection<PokemonId> PokemonsToLevelUp => _settings.PokemonsToLevelUp;
        public ICollection<PokemonId> PokemonsNotToTransfer => _settings.PokemonsNotToTransfer;
        public ICollection<PokemonId> PokemonsNotToCatch => _settings.PokemonsToIgnore;

        public ICollection<PokemonId> PokemonToUseMasterball => _settings.PokemonToUseMasterball;
        public Dictionary<PokemonId, TransferFilter> PokemonsTransferFilter => _settings.PokemonsTransferFilter;
        public Dictionary<PokemonId, UpgradeFilter> PokemonUpgradeFilters => _settings.PokemonUpgradeFilters;
        public Dictionary<PokemonId, EvolveFilter> PokemonEvolveFilters => _settings.PokemonEvolveFilter;
        public bool StartupWelcomeDelay => _settings.ConsoleConfig.StartupWelcomeDelay;

        public bool UseGoogleWalk => _settings.GoogleWalkConfig.UseGoogleWalk;
        public double DefaultStepLength => _settings.GoogleWalkConfig.DefaultStepLength;
        public bool UseGoogleWalkCache => _settings.GoogleWalkConfig.Cache;
        public string GoogleApiKey => _settings.GoogleWalkConfig.GoogleAPIKey;
        public string GoogleHeuristic => _settings.GoogleWalkConfig.GoogleHeuristic;
        public string GoogleElevationApiKey => _settings.GoogleWalkConfig.GoogleElevationAPIKey;

        public bool UseYoursWalk => _settings.YoursWalkConfig.UseYoursWalk;
        public string YoursWalkHeuristic => _settings.YoursWalkConfig.YoursWalkHeuristic;

        public bool UseMapzenWalk => _settings.MapzenWalkConfig.UseMapzenWalk;
        public string MapzenTurnByTurnApiKey => _settings.MapzenWalkConfig.MapzenTurnByTurnApiKey;
        public string MapzenWalkHeuristic => _settings.MapzenWalkConfig.MapzenWalkHeuristic;
        public string MapzenElevationApiKey => _settings.MapzenWalkConfig.MapzenElevationApiKey;
        public bool UseTelegramAPI => _settings.TelegramConfig.UseTelegramAPI;
        public string TelegramAPIKey => _settings.TelegramConfig.TelegramAPIKey;
        public string TelegramPassword => _settings.TelegramConfig.TelegramPassword;
        public int MaxPokeballsPerPokemon => GenRandom(_settings.PokemonConfig.MaxPokeballsPerPokemon);
        public bool RandomlyPauseAtStops => _settings.LocationConfig.RandomlyPauseAtStops;
        public bool RandomizeRecycle => _settings.RecycleConfig.RandomizeRecycle;
        public int RandomRecycleValue => _settings.RecycleConfig.RandomRecycleValue;
        public int TotalAmountOfPokeballsToKeep => GenRandom(_settings.RecycleConfig.TotalAmountOfPokeballsToKeep);
        public int TotalAmountOfPotionsToKeep => GenRandom(_settings.RecycleConfig.TotalAmountOfPotionsToKeep);
        public int TotalAmountOfRevivesToKeep => GenRandom(_settings.RecycleConfig.TotalAmountOfRevivesToKeep);
        public int TotalAmountOfBerriesToKeep => GenRandom(_settings.RecycleConfig.TotalAmountOfBerriesToKeep);
        public int TotalAmountOfEvolutionToKeep => GenRandom(_settings.RecycleConfig.TotalAmountOfEvolutionToKeep);

        public bool UseRecyclePercentsInsteadOfTotals => _settings.RecycleConfig.UseRecyclePercentsInsteadOfTotals;
        public int PercentOfInventoryPokeballsToKeep => GenRandom(_settings.RecycleConfig.PercentOfInventoryPokeballsToKeep);
        public int PercentOfInventoryPotionsToKeep => GenRandom(_settings.RecycleConfig.PercentOfInventoryPotionsToKeep);
        public int PercentOfInventoryRevivesToKeep => GenRandom(_settings.RecycleConfig.PercentOfInventoryRevivesToKeep);
        public int PercentOfInventoryBerriesToKeep => GenRandom(_settings.RecycleConfig.PercentOfInventoryBerriesToKeep);
        public int PercentOfInventoryEvolutionToKeep => GenRandom(_settings.RecycleConfig.PercentOfInventoryEvolutionToKeep);

        public bool UsePokeStopLimit => _settings.PokeStopConfig.UsePokeStopLimit;
        public bool UseCatchLimit => _settings.PokemonConfig.UseCatchLimit;
        public int ResumeTrack => _settings.LocationConfig.ResumeTrack;
        public int ResumeTrackSeg => _settings.LocationConfig.ResumeTrackSeg;
        public int ResumeTrackPt => _settings.LocationConfig.ResumeTrackPt;
        public GymConfig GymConfig => _settings.GymConfig;

        public bool UseTransferFilterToCatch => _settings.CustomCatchConfig.UseTransferFilterToCatch;
        public MultipleBotConfig MultipleBotConfig => _settings.MultipleBotConfig;
        public List<AuthConfig> Bots => _settings.Auth.Bots;
        public Dictionary<PokemonId, BotSwitchPokemonFilter> BotSwitchPokemonFilters => _settings.BotSwitchPokemonFilters;
        public NotificationConfig NotificationConfig => _settings.NotificationConfig;
        public CaptchaConfig CaptchaConfig => _settings.CaptchaConfig;

        public bool UseHumanlikeDelays => _settings.HumanlikeDelays.UseHumanlikeDelays;
        public int CatchSuccessDelay => _settings.HumanlikeDelays.CatchSuccessDelay;
        public int CatchErrorDelay => _settings.HumanlikeDelays.CatchErrorDelay;
        public int CatchEscapeDelay => _settings.HumanlikeDelays.CatchEscapeDelay;
        public int CatchFleeDelay => _settings.HumanlikeDelays.CatchFleeDelay;
        public int CatchMissedDelay => _settings.HumanlikeDelays.CatchMissedDelay;
        public int BeforeCatchDelay => _settings.HumanlikeDelays.BeforeCatchDelay;
        #region Evolve
        public bool EvolvePokemonsThatMatchFilter => _settings.PokemonConfig.EvolveConfig.EvolvePokemonsThatMatchFilter;
        public bool EvolveAnyPokemonAboveIv => _settings.PokemonConfig.EvolveConfig.EvolveAnyPokemonAboveIv;
        public float EvolveAnyPokemonAboveIvValue => _settings.PokemonConfig.EvolveConfig.EvolveAnyPokemonAboveIvValue;
        public bool TriggerEvolveAsSoonAsFilterIsMatched => _settings.PokemonConfig.EvolveConfig.TriggerAsSoonAsFilterIsMatched;
        public bool TriggerEvolveOnEvolutionCount => _settings.PokemonConfig.EvolveConfig.TriggerOnEvolutionCount;
        public int TriggerEvolveOnEvolutionCountValue => _settings.PokemonConfig.EvolveConfig.TriggerOnEvolutionCountValue;
        public bool TriggerEvolveOnStorageUsagePercentage => _settings.PokemonConfig.EvolveConfig.TriggerOnStorageUsagePercentage;
        public double TriggerEvolveOnStorageUsagePercentageValue => GenRandom(_settings.PokemonConfig.EvolveConfig.TriggerOnStorageUsagePercentageValue);
        public bool TriggerEvolveOnStorageUsageAbsolute => _settings.PokemonConfig.EvolveConfig.TriggerOnStorageUsageAbsolute;
        public int TriggerEvolveOnStorageUsageAbsoluteValue => GenRandom(_settings.PokemonConfig.EvolveConfig.TriggerOnStorageUsageAbsoluteValue);
        public bool TriggerEvolveIfLuckyEggIsActive => _settings.PokemonConfig.EvolveConfig.TriggerIfLuckyEggIsActive;
        public bool EvolvePreserveMinCandiesFromFilter => _settings.PokemonConfig.EvolveConfig.PreserveMinCandiesFromFilter;
        public bool EvolveApplyLuckyEggOnEvolutionCount => _settings.PokemonConfig.EvolveConfig.ApplyLuckyEggOnEvolutionCount;
        public int EvolveApplyLuckyEggOnEvolutionCountValue => _settings.PokemonConfig.EvolveConfig.ApplyLuckyEggOnEvolutionCountValue;
        #endregion
    }
}
