using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.GameData.Fences;
using StardewValley.TerrainFeatures;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

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
        private static void Postfix(Fence __instance, ref bool __result)
        {
            if (Game1.IsMasterGame)
            {
                __instance.health.Value = __instance.maxHealth.Value;
                if (__instance.isGate.Value)
                    __instance.health.Value = __instance.maxHealth.Value * 2f;
                __result = false;
            }
        }
    }
}
