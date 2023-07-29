using System;
using System.Diagnostics.CodeAnalysis;
using Brawler2D;
using HarmonyLib;
using FullModdedFuriesAPI.Events;
using FullModdedFuriesAPI.Framework.Events;
using FullModdedFuriesAPI.Internal.Patching;

namespace FullModdedFuriesAPI.Patches
{
    /// <summary>Harmony patches for <see cref="ScreenManagerPatcher"/> which notify FMODF when persistence data is create or loaded.</summary>
    /// <remarks>Patch methods must be static for Harmony to work correctly. See the Harmony documentation before renaming patch arguments.</remarks>
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Argument names are defined by Harmony and methods are named for clarity.")]
    [SuppressMessage("ReSharper", "IdentifierTypo", Justification = "Argument names are defined by Harmony and methods are named for clarity.")]
    internal class ScreenManagerPatcher : BasePatcher
    {
        /*********
        ** Fields
        *********/
        /// <summary>A callback to invoke when the load stage changes.</summary>
        private static Action<OnScreenLoadArgs> OnScreenLoading;
        private static Action<OnScreenLoadArgs> OnScreenLoaded;

        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="onStageChanged">A callback to invoke when the load stage changes.</param>
        public ScreenManagerPatcher(
            Action<OnScreenLoadArgs> OnScreenLoading,
            Action<OnScreenLoadArgs> OnScreenLoaded)
        {
            ScreenManagerPatcher.OnScreenLoading = OnScreenLoading;
            ScreenManagerPatcher.OnScreenLoaded = OnScreenLoaded;

        }

        public static ScreenManagerPatcher PatchMe(EventManager eventManager)
        {
            return new ScreenManagerPatcher(
                args => eventManager.ScreenLoading.Raise(args),
                args => eventManager.ScreenLoaded.Raise(args));
        }
        /// <inheritdoc />
        public override void Apply(Harmony harmony, IMonitor monitor)
        {
           this.Patch(harmony);
        }

        private void Patch(Harmony harmony)
        {
            // harmony.Patch(
            //     original: AccessTools.Method(typeof(BrawlerScreenManager), nameof(BrawlerScreenManager.LoadScreen)),
            //     prefix: this.GetHarmonyMethod(nameof(ScreenManagerPatcher.Before_LoadScreen))
            // );
            //
            // harmony.Patch(
            //     original: AccessTools.Method(typeof(BrawlerScreenManager), nameof(BrawlerScreenManager.LoadScreen)),
            //     postfix: this.GetHarmonyMethod(nameof(ScreenManagerPatcher.After_LoadScreen))
            // );

            harmony.Patch(
                original: AccessTools.Method(typeof(LoadingScreen), nameof(LoadingScreen.EndLoading)),
                postfix: this.GetHarmonyMethod(nameof(ScreenManagerPatcher.After_EndLoading))
            );
        }
        /*********
        ** Private methods
        *********/

        #region Config
        /*********
        ** Config
        *********/
        /// <summary>The method to call before <see cref="SaveManager.LoadConfig"/>.</summary>
        /// <returns>Returns whether to execute the original method.</returns>
        /// <remarks>This method must be static for Harmony to work correctly. See the Harmony documentation before renaming arguments.</remarks>
        private static void Before_LoadScreen(ScreenType screenType, string levelName = "", int hostControllerIndex = 0)
        {
            ScreenManagerPatcher.OnScreenLoading(new OnScreenLoadArgs(screenType, levelName, hostControllerIndex));
        }

        /// <summary>The method to call before <see cref="SaveManager.LoadConfig"/>.</summary>
        /// <returns>Returns whether to execute the original method.</returns>
        /// <remarks>This method must be static for Harmony to work correctly. See the Harmony documentation before renaming arguments.</remarks>
        private static void After_LoadScreen(ScreenType screenType, string levelName = "", int hostControllerIndex = 0)
        {
            ScreenManagerPatcher.OnScreenLoaded(new OnScreenLoadArgs(screenType, levelName, hostControllerIndex));
        }

        private static void After_EndLoading()
        {
            ScreenManagerPatcher.OnScreenLoaded(null);
        }

        #endregion

    }
}
