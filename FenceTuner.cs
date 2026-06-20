using FenceTuner.Data;
using FenceTuner.Framework.Configuration;
using HarmonyLib;
using JetBrains.Annotations;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.GameData.Fences;

namespace FenceTuner
{
    [UsedImplicitly]
    public sealed class FenceTuner : Mod
    {
        public new static IModHelper Helper { get; private set; } = null!;
        public static Configurations Config { get; set; } = null!;

        /// <inheritdoc/>
        public override void Entry(IModHelper helper)
        {
            Helper = helper;
            I18n.Init(helper.Translation);

            Config = helper.ReadConfig<Configurations>();

            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            helper.Events.Content.AssetsInvalidated += OnAssetsInvalidated;

            var harmony = new Harmony(this.ModManifest.UniqueID);
            harmony.PatchAll();
        }

        private static void OnAssetsInvalidated(object? sender, AssetsInvalidatedEventArgs e)
        {
            if (e.NamesWithoutLocale.Any(name => name.IsEquivalentTo("Data/Fences")))
                Configurations.PopulateConfigs();
        }

        public static PerFenceConfig GetPerFenceConfig(string id)
        {
            Configurations.PopulateConfigs();
            return Config.PerFenceConfigs.GetValueOrDefault(id) ?? new();
        }

        private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
        {
            Game1.content.Load<Dictionary<string, FenceData>>("Data/Fences");
            ConfigurationIntegration.RegisterConfigMenu(ModManifest);
            Configurations.PopulateConfigs();
        }
    }
}