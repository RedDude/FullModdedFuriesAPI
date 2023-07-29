using Brawler2D;
using HarmonyLib;

namespace FullModdedFuriesAPI.Mods.NoShields
{
    /// <summary>The main entry point for the mod.</summary>
    public class ModEntry : Mod
    {
        private ModConfig Config;

        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            this.Config = this.Helper.ReadConfig<ModConfig>();

            var harmony = new Harmony(this.ModManifest.UniqueID);

            Patches.Initialize(this.Monitor, this.Config);

            harmony.Patch(
                original: AccessTools.Method(typeof(EnemyManager), nameof(EnemyManager.ApplyShield)),
                postfix: new HarmonyMethod(typeof(Patches), nameof(Patches.ApplyShield_Posfix))
            );
        }
    }
}
