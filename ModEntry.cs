using HarmonyLib;
using StardewModdingAPI;
using StardewValley;
using StardewValley.GameData.Fences;
using StardewValley.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoFenceDecayRedux
{
    internal sealed class ModEntry : Mod
    {
        public override void Entry(IModHelper helper)
        {
            // Init Harmony
            var Harmony = new Harmony(this.ModManifest.UniqueID);

            /**********************************
             * Harmony prefix Fence.minutesElapsed
             * minutesElapsed applies fence decay
             **********************************/
            Harmony.Patch(
                original: AccessTools.Method(typeof(StardewValley.Fence), nameof(StardewValley.Fence.minutesElapsed)),
                prefix: new HarmonyMethod(typeof(ModEntry), nameof(ModEntry.minutesElapsed_Patched))
                );
        }

        
        private static bool minutesElapsed_Patched(int minutes)
        {
            // Return false, stopping fence decay from applying to fences
            return false;
        }
    }
}
