using System;
using System.Diagnostics.CodeAnalysis;
using Brawler2D;
using HarmonyLib;
using FullModdedFuriesAPI.Events;
using FullModdedFuriesAPI.Framework.Events;
using FullModdedFuriesAPI.Internal.Patching;

namespace FullModdedFuriesAPI.Patches
{
    /// <summary>Harmony patches for <see cref="SaveManager"/> which notify FMODF when persistence data is create or loaded.</summary>
    /// <remarks>Patch methods must be static for Harmony to work correctly. See the Harmony documentation before renaming patch arguments.</remarks>
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Argument names are defined by Harmony and methods are named for clarity.")]
    [SuppressMessage("ReSharper", "IdentifierTypo", Justification = "Argument names are defined by Harmony and methods are named for clarity.")]
    internal class SaveManagerPatcher : BasePatcher
    {
        /*********
        ** Fields
        *********/
        /// <summary>A callback to invoke when the load stage changes.</summary>
        private static Action<ProfileLoadedEventArgs> OnProfileLoaded;
        private static Action<ProfileDeletedEventArgs> OnDeleteProfile;
        private static Action<ProfileResetEventArgs> OnResetStatsFile;
        private static Action<ProfileResetEventArgs> OnResetStats;

        private static Action<SaveLoadingEventArgs> OnSaveLoading;
        private static Action<SaveLoadedEventArgs> OnSaveLoaded;
        private static Action<SavedEventArgs> OnSaved;
        private static Action<SavingEventArgs> OnSaving;

        private static Action OnConfigSaving;
        private static Action OnConfigSaved;
        private static Action OnConfigLoading;
        private static Action OnConfigLoaded;

        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="onStageChanged">A callback to invoke when the load stage changes.</param>
        public SaveManagerPatcher(
            Action<ProfileLoadedEventArgs> OnProfileLoaded,
            Action<ProfileDeletedEventArgs> OnDeleteProfile,
            Action<ProfileResetEventArgs>  OnResetStatsFile,
            Action<ProfileResetEventArgs>  OnResetStats,

            Action<SaveLoadingEventArgs> OnSaveLoading,
            Action<SaveLoadedEventArgs> OnSaveLoaded,
            Action<SavedEventArgs> OnSaved,
            Action<SavingEventArgs> OnSaving,

            Action OnConfigSaving,
            Action OnConfigSaved,
            Action OnConfigLoading,
            Action OnConfigLoaded)
        {
            SaveManagerPatcher.OnProfileLoaded = OnProfileLoaded;
            SaveManagerPatcher.OnDeleteProfile = OnDeleteProfile;
            SaveManagerPatcher.OnResetStatsFile = OnResetStatsFile;
            SaveManagerPatcher.OnResetStats = OnResetStats;

            SaveManagerPatcher.OnSaveLoading = OnSaveLoading;
            SaveManagerPatcher.OnSaveLoaded = OnSaveLoaded;
            SaveManagerPatcher.OnSaved = OnSaved;
            SaveManagerPatcher.OnSaving = OnSaving;

            SaveManagerPatcher.OnConfigSaving = OnConfigSaving;
            SaveManagerPatcher.OnConfigSaved = OnConfigSaved;
            SaveManagerPatcher.OnConfigLoading = OnConfigLoading;
            SaveManagerPatcher.OnConfigLoaded = OnConfigLoaded;
        }

        public static SaveManagerPatcher PatchMe(EventManager eventManager)
        {
            return new SaveManagerPatcher(
                args => eventManager.ProfileLoaded.Raise(args),
                args => eventManager.DeleteProfile.Raise(args),
                args => eventManager.ResetStatsFile.Raise(args),
                args => eventManager.ResetStats.Raise(args),

                args => eventManager.SaveLoading.Raise(args),
                args => eventManager.SaveLoaded.Raise(args),
                args => eventManager.Saved.Raise(args),
                args => eventManager.Saving.Raise(args),

                () => eventManager.ConfigSaving.Raise(null),
                () => eventManager.ConfigSaved.Raise(null),
                () => eventManager.ConfigLoading.Raise(null),
                () => eventManager.ConfigLoaded.Raise(null));
        }
        /// <inheritdoc />
        public override void Apply(Harmony harmony, IMonitor monitor)
        {
            this.ConfigPatch(harmony);
            this.SavePatch(harmony);
            this.ProfilePatch(harmony);
        }

        private void ConfigPatch(Harmony harmony)
        {
            /*********
           ** Config
           *********/
            harmony.Patch(
                original: AccessTools.Method(typeof(SaveManager), nameof(SaveManager.LoadConfig)),
                prefix: this.GetHarmonyMethod(nameof(SaveManagerPatcher.Before_LoadConfig))
            );

            harmony.Patch(
                original: AccessTools.Method(typeof(SaveManager), nameof(SaveManager.LoadConfig)),
                postfix: this.GetHarmonyMethod(nameof(SaveManagerPatcher.After_LoadConfig))
            );

            // harmony.Patch(
            //     original: AccessTools.Method(typeof(SaveManager), nameof(SaveManager.SaveConfig)),
            //     prefix: this.GetHarmonyMethod(nameof(SaveManagerPatcher.Before_SaveConfig))
            // );

            harmony.Patch(
                original: AccessTools.Method(typeof(SaveManager), nameof(SaveManager.SaveConfig)),
                postfix: this.GetHarmonyMethod(nameof(SaveManagerPatcher.After_SaveConfig))
            );
        }

        private void SavePatch(Harmony harmony)
        {
            /*********
           ** Save
           *********/
            harmony.Patch(
                original: AccessTools.Method(typeof(SaveManager), nameof(SaveManager.LoadGame)),
                prefix: this.GetHarmonyMethod(nameof(SaveManagerPatcher.Before_LoadSave))
            );

            harmony.Patch(
                original: AccessTools.Method(typeof(SaveManager), nameof(SaveManager.LoadGame)),
                postfix: this.GetHarmonyMethod(nameof(SaveManagerPatcher.After_LoadSave))
            );

            harmony.Patch(
                original: AccessTools.Method(typeof(SaveManager), nameof(SaveManager.SaveGame)),
                prefix: this.GetHarmonyMethod(nameof(SaveManagerPatcher.Before_Save))
            );

            harmony.Patch(
                original: AccessTools.Method(typeof(SaveManager), nameof(SaveManager.SaveGame)),
                postfix: this.GetHarmonyMethod(nameof(SaveManagerPatcher.After_Save))
            );
        }

        private void ProfilePatch(Harmony harmony)
        {
            /*********
             ** Profile
             *********/
            harmony.Patch(
                original: AccessTools.Method(typeof(SaveManager), nameof(SaveManager.ProfileLoaded)),
                postfix: this.GetHarmonyMethod(nameof(SaveManagerPatcher.After_ProfileLoaded))
            );

            harmony.Patch(
                original: AccessTools.Method(typeof(SaveManager), nameof(SaveManager.DeleteProfile)),
                postfix: this.GetHarmonyMethod(nameof(SaveManagerPatcher.After_DeleteProfile))
            );

            harmony.Patch(
                original: AccessTools.Method(typeof(SaveManager), nameof(SaveManager.ResetStats)),
                postfix: this.GetHarmonyMethod(nameof(SaveManagerPatcher.After_ResetStats))
            );

            harmony.Patch(
                original: AccessTools.Method(typeof(SaveManager), nameof(SaveManager.ResetStatsFile)),
                postfix: this.GetHarmonyMethod(nameof(SaveManagerPatcher.After_ResetStatsFile))
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
        private static void Before_LoadConfig()
        {
            SaveManagerPatcher.OnConfigLoading();
        }

        /// <summary>The method to call before <see cref="SaveManager.LoadConfig"/>.</summary>
        /// <returns>Returns whether to execute the original method.</returns>
        /// <remarks>This method must be static for Harmony to work correctly. See the Harmony documentation before renaming arguments.</remarks>
        private static void After_LoadConfig()
        {
            SaveManagerPatcher.OnConfigLoaded();
        }


        /// <summary>The method to call before <see cref="SaveManager.LoadConfig"/>.</summary>
        /// <returns>Returns whether to execute the original method.</returns>
        /// <remarks>This method must be static for Harmony to work correctly. See the Harmony documentation before renaming arguments.</remarks>
        private static void Before_SaveConfig()
        {
            SaveManagerPatcher.OnConfigSaving();
        }

        /// <summary>The method to call before <see cref="SaveManager.LoadConfig"/>.</summary>
        /// <returns>Returns whether to execute the original method.</returns>
        /// <remarks>This method must be static for Harmony to work correctly. See the Harmony documentation before renaming arguments.</remarks>
        private static void After_SaveConfig()
        {
            SaveManagerPatcher.OnConfigSaved();
        }
        #endregion

        #region Profile
        /*********
              ** Profile
              *********/

        /// <summary>The method to call before <see cref="SaveManager.LoadConfig"/>.</summary>
        /// <returns>Returns whether to execute the original method.</returns>
        /// <remarks>This method must be static for Harmony to work correctly. See the Harmony documentation before renaming arguments.</remarks>
        private static void After_ProfileLoaded(
            int profileIndex)
        {
            SaveManagerPatcher.OnProfileLoaded(new ProfileLoadedEventArgs(profileIndex));
        }

        /// <summary>The method to call before <see cref="SaveManager.LoadConfig"/>.</summary>
        /// <returns>Returns whether to execute the original method.</returns>
        /// <remarks>This method must be static for Harmony to work correctly. See the Harmony documentation before renaming arguments.</remarks>
        private static void After_DeleteProfile(
            int profileIndex)
        {
            SaveManagerPatcher.OnDeleteProfile(new ProfileDeletedEventArgs(profileIndex));
        }

        /// <summary>The method to call before <see cref="SaveManager.LoadConfig"/>.</summary>
        /// <returns>Returns whether to execute the original method.</returns>
        /// <remarks>This method must be static for Harmony to work correctly. See the Harmony documentation before renaming arguments.</remarks>
        private static void After_ResetStats(PlayerObj player)
        {
            SaveManagerPatcher.OnResetStats(new ProfileResetEventArgs(player));
        }

        private static void After_ResetStatsFile()
        {
            SaveManagerPatcher.OnResetStatsFile(new ProfileResetEventArgs(null));
        }

        #endregion

        #region Save
        /*********
        ** Save
        *********/
        private static bool Before_LoadSave(
            int profileIndex, bool saveToPlayer)
        {
            SaveManagerPatcher.OnSaveLoading(new SaveLoadingEventArgs(profileIndex, saveToPlayer));
            return true;
        }

        /// <summary>The method to call before <see cref="SaveManager.LoadConfig"/>.</summary>
        /// <returns>Returns whether to execute the original method.</returns>
        /// <remarks>This method must be static for Harmony to work correctly. See the Harmony documentation before renaming arguments.</remarks>
        private static void After_LoadSave(
            int profileIndex, bool saveToPlayer)
        {
            SaveManagerPatcher.OnSaveLoaded(new SaveLoadedEventArgs(profileIndex, saveToPlayer));
        }


        /// <summary>The method to call before <see cref="SaveManager.LoadConfig"/>.</summary>
        /// <returns>Returns whether to execute the original method.</returns>
        /// <remarks>This method must be static for Harmony to work correctly. See the Harmony documentation before renaming arguments.</remarks>
        private static void Before_Save(
            int profileIndex,
            bool writeToDisk = true,
            bool corruptedFile = false,
            PlayerObj forcedPlayer = null
        )
        {
            int profileIndexArg = -1;
            var playerObj = forcedPlayer ?? GameController.g_game.PlayerManager.GetPlayerAtProfileIndex(profileIndex, true);
            if (playerObj == null)
            {
                if (writeToDisk)
                    throw new Exception("Cannot save player at index: " + (object) profileIndex + ".  No player found");
                playerObj = GameController.g_game.PlayerManager.GetPlayerAtPlayerIndex(0);
            }

            profileIndexArg = playerObj.profileIndex;

            SaveManagerPatcher.OnSaving(new SavingEventArgs(
                profileIndexArg,
                writeToDisk,
                corruptedFile
            ));
        }


        /// <summary>The method to call before <see cref="SaveManager.LoadConfig"/>.</summary>
        /// <returns>Returns whether to execute the original method.</returns>
        /// <remarks>This method must be static for Harmony to work correctly. See the Harmony documentation before renaming arguments.</remarks>
        private static void After_Save(
            int profileIndex,
            bool writeToDisk = true,
            bool corruptedFile = false,
            PlayerObj forcedPlayer = null
        )
        {
            int profileIndexArg = -1;
            var playerObj = forcedPlayer ?? GameController.g_game.PlayerManager.GetPlayerAtProfileIndex(profileIndex, true);
            if (playerObj == null)
            {
                if (writeToDisk)
                    throw new Exception("Cannot save player at index: " + (object) profileIndex + ".  No player found");
                playerObj = GameController.g_game.PlayerManager.GetPlayerAtPlayerIndex(0);
            }

            profileIndexArg = playerObj.profileIndex;

            SaveManagerPatcher.OnSaved(new SavedEventArgs(
                profileIndexArg,
                writeToDisk,
                corruptedFile
            ));
        }


        #endregion

    }
}
