using FenceTuner.Data;
using FenceTuner.Integrations;
using StardewModdingAPI;
using StardewValley;

namespace FenceTuner.Framework.Configuration;

internal static class ConfigurationIntegration
{
    private static IGenericModConfigMenuApi? GenericModConfigMenu
    {
        get => FenceTuner.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
    }

    private static IManifest ModManifest;

    public static void RegisterConfigMenu(IManifest manifest)
    {
        if (GenericModConfigMenu == null)
            return;

        ModManifest = manifest;
        
        GenericModConfigMenu.Register(manifest, () =>
        {
            var dict = FenceTuner.Config.PerFenceConfigs;
            foreach (PerFenceConfig config in dict.Values)
            {
                config.IgnoreGlobal = false;
                config.ShouldDecay = true;
                config.DecaySpeed = 1.0f;
                config.RemoveVariation = false;
            }

            FenceTuner.Config = new()
            {
                PerFenceConfigs = dict
            };
        }, () => 
            FenceTuner.Helper.WriteConfig(FenceTuner.Config));

        GenericModConfigMenu.AddBoolOption(manifest, () => FenceTuner.Config.ShouldDecay,
            val => FenceTuner.Config.ShouldDecay = val, I18n.Config_GlobalShouldDecay_Name,
            I18n.Config_GlobalShouldDecay_Description, "global");
        GenericModConfigMenu.AddNumberOption(manifest, () => FenceTuner.Config.DecaySpeed,
            val => FenceTuner.Config.DecaySpeed = val, I18n.Config_GlobalDecaySpeed_Name,
            I18n.Config_GlobalDecaySpeed_Description, 0.2f, 5f, 0.1f, (val) => $"{val}x");
        GenericModConfigMenu.AddBoolOption(manifest, () => FenceTuner.Config.RemoveVariation,
            val => FenceTuner.Config.RemoveVariation = val, I18n.Config_GlobalRemoveVariation_Name,
            I18n.Config_GlobalRemoveVariation_Description);

        GenericModConfigMenu.AddPageLink(manifest, "perFenceConfigs", I18n.Config_PerFenceConfig_Name,
            I18n.Config_PerFenceConfig_Description);
        GenericModConfigMenu.AddPage(manifest, "perFenceConfigs", I18n.Config_PerFenceConfig_Name);

        foreach (var kvp in FenceTuner.Config.PerFenceConfigs)
            kvp.Value.RegisterModMenuOptions(GenericModConfigMenu, manifest, kvp.Key);
        
        GenericModConfigMenu.OnFieldChanged(manifest, (id, val) =>
        {
            if (val is true)
            {
                Utility.ForEachLocation((loc) =>
                {
                    foreach (Fence f in loc.objects.OfType<Fence>().Where(f => id == "global" || f.ItemId == id))
                        f.health.Value = f.isGate.Value ? f.maxHealth.Value * 2f : f.maxHealth.Value;
                    return true;
                }, true, true);
            }
        });
    }

    public static void AddNewPerFenceConfig(string id, PerFenceConfig config)
    {
        if (GenericModConfigMenu != null)
            config.RegisterModMenuOptions(GenericModConfigMenu, ModManifest, id);
    }
}