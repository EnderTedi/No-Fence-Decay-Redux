using FenceTuner.Data;
using HarmonyLib;
using JetBrains.Annotations;
using StardewValley;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Emit;

namespace FenceTuner.Framework.FenceTweaks;

[HarmonyPatch(typeof(Fence)), SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Named for harmony")]
public static class FenceTweaks
{
    [UsedImplicitly]
    [HarmonyTranspiler, HarmonyPatch(nameof(Fence.minutesElapsed))]
    private static IEnumerable<CodeInstruction> MinutesElapsedTranspiler(IEnumerable<CodeInstruction> insns, ILGenerator gen)
    {
        CodeMatcher matcher = new(insns, gen);

        matcher.MatchStartForward([
            new(OpCodes.Ldarg_0),
            new(OpCodes.Ldfld, AccessTools.Field(typeof(Fence), nameof(Fence.health))),
        ]);
        matcher.Advance(1);
        matcher.RemoveInstructions(9);
        matcher.Insert([
            new(OpCodes.Ldarg_1),
            new(OpCodes.Call, AccessTools.Method(typeof(FenceTweaks), nameof(GetAmount))),
        ]);

        return matcher.Instructions();
    }

    public static void GetAmount(Fence f, int minutesElapsed)
    {
        PerFenceConfig thisConfig = FenceTuner.GetPerFenceConfig(f.ItemId);

        if (thisConfig is { IgnoreGlobal: true, ShouldDecay: true })
            f.health.Value -= (float)minutesElapsed / 1440 * thisConfig.DecaySpeed;
        else if (!thisConfig.IgnoreGlobal && FenceTuner.Config.ShouldDecay)
            f.health.Value -= (float)minutesElapsed / 1400 * FenceTuner.Config.DecaySpeed;
    }

    [UsedImplicitly]
    [HarmonyPostfix, HarmonyPatch(nameof(Fence.ResetHealth))]
    private static void ResetHealthPostfix(Fence __instance)
    {
        PerFenceConfig thisConfig = FenceTuner.GetPerFenceConfig(__instance.ItemId);

        if (thisConfig is { IgnoreGlobal: true, RemoveVariation: true } || (!thisConfig.IgnoreGlobal && FenceTuner.Config.RemoveVariation))
        {
            __instance.health.Value = __instance.GetData()?.Health ?? 100;
            __instance.health.Value *= 2f;
            __instance.maxHealth.Value = __instance.health.Value;
        }
    }
}