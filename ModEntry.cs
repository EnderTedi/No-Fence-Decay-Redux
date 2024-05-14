using HarmonyLib;
using StardewModdingAPI;
using StardewValley;
using StardewValley.GameData.Fences;

namespace NoFenceDecayRedux
{
    internal sealed class ModEntry : Mod
    {
        public override void Entry(IModHelper helper)
        {
            var harmony = new Harmony(this.ModManifest.UniqueID);
            harmony.PatchAll(typeof(ModEntry).Assembly);
        }
    }

    [HarmonyPatch(typeof(Fence), nameof(Fence.minutesElapsed))]
    public static class NoFenceDecay
    {
        private static bool Prefix(Fence __instance, ref bool __result)
        {
            return false;
        }
        private static void Postfix(Fence __instance, ref bool __result)
        {
            if (__instance.health.Value < __instance.maxHealth.Value && Game1.IsMasterGame)
            {
                FenceData data = __instance.GetData();
                __instance.ResetHealth(data.RepairHealthAdjustmentMaximum);
                __result = false;
            }
        }
    }
}
