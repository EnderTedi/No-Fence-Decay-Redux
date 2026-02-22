using FenceTuner.Data;
using StardewValley;

namespace FenceTuner.Framework.Configuration;

public class Configurations
{
    public bool ShouldDecay { get; set; } = true;
    public float DecaySpeed { get; set; } = 1f;
    public bool RemoveVariation { get; set; }
    public Dictionary<string, PerFenceConfig> PerFenceConfigs { get; set; } = [];

    public static void PopulateConfigs()
    {
        foreach (var fence in DataLoader.Fences(Game1.content).Keys.Where(fence => !FenceTuner.Config.PerFenceConfigs.ContainsKey(fence)))
        {
            FenceTuner.Config.PerFenceConfigs.Add(fence, new());
            ConfigurationIntegration.AddNewPerFenceConfig(fence, FenceTuner.Config.PerFenceConfigs[fence]);
        }
    }
}