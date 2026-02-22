using FenceTuner.Integrations;
using StardewModdingAPI;
using StardewValley;
using StardewValley.GameData.Objects;
using StardewValley.TokenizableStrings;

namespace FenceTuner.Data;

public class PerFenceConfig
{
    public bool IgnoreGlobal { get; set; }
    public bool ShouldDecay { get; set; } = true;
    public float DecaySpeed { get; set; } = 1.0f;
    public bool RemoveVariation { get; set; }
    

    private static string Name(string id) => !Game1.objectData.TryGetValue(id, out ObjectData? value) ? id : TokenParser.ParseText(value.DisplayName ?? id);
    public void RegisterModMenuOptions(IGenericModConfigMenuApi genericModConfigMenu, IManifest manifest, string id)
    {   
        genericModConfigMenu.AddPageLink(manifest, id, () => Name(id));
        genericModConfigMenu.AddPage(manifest, id, () => Name(id));

        genericModConfigMenu.AddBoolOption(manifest, () => IgnoreGlobal, val => IgnoreGlobal = val,
            I18n.Config_IgnoreGlobal_Name, I18n.Config_IgnoreGlobal_Description);
        genericModConfigMenu.AddBoolOption(manifest, () => ShouldDecay, val => ShouldDecay = val,
            I18n.Config_ShouldDecay_Name, I18n.Config_ShouldDecay_Description, id);
        genericModConfigMenu.AddNumberOption(manifest, () => DecaySpeed, (val) => DecaySpeed = val,
            I18n.Config_DecaySpeed_Name, I18n.Config_DecaySpeed_Description, 0.2f, 5f, 0.1f, (val) => $"{val}x");
        genericModConfigMenu.AddBoolOption(manifest, () => RemoveVariation, val => RemoveVariation = val,
            I18n.Config_RemoveVariation_Name, I18n.Config_RemoveVariation_Description);

        genericModConfigMenu.AddPage(manifest, "perFenceConfigs", I18n.Config_PerFenceConfig_Name);
    }
}