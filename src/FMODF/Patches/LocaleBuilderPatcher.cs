using System.Diagnostics.CodeAnalysis;
using System.Reflection.Emit;
using HarmonyLib;
using FullModdedFuriesAPI.Internal.Patching;
using Brawler2D;
using FullModdedFuriesAPI.Utilities;
using Brawler2D.Resources;

namespace FullModdedFuriesAPI.Mods.ErrorHandler.Patches
{
    /// <summary>A Harmony patch for <see cref="LocalBuilder"/> methods to remove the {NULLSTRING: localID } string to enable mods to use translation.helper.</summary>
    /// <remarks>Patch methods must be static for Harmony to work correctly. See the Harmony documentation before renaming patch arguments.</remarks>
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Argument names are defined by Harmony and methods are named for clarity.")]
    [SuppressMessage("ReSharper", "IdentifierTypo", Justification = "Argument names are defined by Harmony and methods are named for clarity.")]
    internal class LocaleBuilderPatcher : BasePatcher
    {
        /*********
        ** Public methods
        *********/
        /// <inheritdoc />
        public override void Apply(Harmony harmony, IMonitor monitor)
        {
            var Assembly = typeof(GameController).Assembly;
            var LocaleBuilder = Assembly.GetType("Brawler2D.LocaleBuilder");
            harmony.Patch(
                original: AccessTools.Method(LocaleBuilder, "getResourceString", new []{ typeof(string)}),
                postfix: this.GetHarmonyMethod(nameof(LocaleBuilderPatcher.After_getResourceString)) //Transcript would improve memory allocation
            );

            harmony.Patch(
                original: AccessTools.Method(LocaleBuilder, "getResourceString", new []{ typeof(string)}),
                postfix: this.GetHarmonyMethod(nameof(LocaleBuilderPatcher.After_getResourceString)) //Transcript would improve memory allocation
            );

            harmony.Patch(
                original: AccessTools.Method(LocaleBuilder, "RefreshAllText", new []{ typeof(GameController) }),
                postfix: this.GetHarmonyMethod(nameof(LocaleBuilderPatcher.After_RefreshAllText)) //Transcript would improve memory allocation
            );
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Remove {NULLSTRING: locId} from <see cref="LocalBuilder.getResourceString"/> return.</summary>
        /// <param name="stringID">stringID from the original locID</param>
        /// <returns>Returns the fixed string.</returns>
        private static string After_getResourceString(string stringID, ref string __result)
        {
            return __result.StartsWith("{N") ? stringID.StartsWith("{N") ? stringID.Remove(stringID.Length - 1, 1).Remove(0,13) : stringID : __result;
        }

        //Write a before return just the locString directly or a trascript ignoring the if
        // private static bool Before_getResourceString(string stringID, ref string __result)
        // {
        //     __result = LocStrings.ResourceManager.GetString(stringID, LocStrings.Culture);
        //     if (stringID.StartsWith("@"))
        //     {
        //         __result = stringID.Remove(0, 1);
        //         return true;
        //     }
        //     return false;
        // }



        /*********
        ** Private methods
        *********/
        /// <summary>Update FMODF language after it has been changed <see cref="LocalBuilder.RefreshAllText"/></summary>
        /// <param name="game">The game controller (for some reason)</param>
        private static void After_RefreshAllText(GameController game)
        {
            //would be really nice to raise a LanguageChangeEvent here
            // monitor this not using
            LocalizedContentManager.UpdateCurrentLanguageCode();
        }
    }
}
