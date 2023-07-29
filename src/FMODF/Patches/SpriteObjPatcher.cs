using System;
using System.Diagnostics.CodeAnalysis;
using CDGEngine;
using FullModdedFuriesAPI.Events;
using FullModdedFuriesAPI.Framework.Events;
using FullModdedFuriesAPI.Internal.Patching;
using HarmonyLib;
using SpriteSystem2;

namespace FullModdedFuriesAPI.Patches
{
    /// <summary>A Harmony patch for <see cref="SpriteObj"/> to raise a events when a sprite is not found, which is used to avoid harmony patch due visual.</summary>
    /// <remarks>Patch methods must be static for Harmony to work correctly. See the Harmony documentation before renaming patch arguments.</remarks>
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Argument names are defined by Harmony and methods are named for clarity.")]
    [SuppressMessage("ReSharper", "IdentifierTypo", Justification = "Argument names are defined by Harmony and methods are named for clarity.")]
    internal class SpriteObjPatcher : BasePatcher
    {
        private readonly EventManager EventManager;

        /*********
        ** Fields
        *********/
        /// <summary>A callback to invoke when the load stage changes.</summary>
        private static Action<OnSpriteEventArgs> OnSpriteChange;
        private static Action<OnSpriteEventArgs> OnSpriteNotFound;

        public SpriteObjPatcher()
        {
        }

        private SpriteObjPatcher(Action<OnSpriteEventArgs> OnSpriteChange, Action<OnSpriteEventArgs> OnSpriteNotFound)
        {
            SpriteObjPatcher.OnSpriteChange = OnSpriteChange;
            SpriteObjPatcher.OnSpriteNotFound = OnSpriteNotFound;
        }

        public static SpriteObjPatcher PatchMe(EventManager eventManager)
        {
            return new(
                args => eventManager.SpriteChanged.Raise(args),
                args => eventManager.SpriteNotFound.Raise(args));
        }

        /*********
        ** Public methods
        *********/
        /// <inheritdoc />
        public override void Apply(Harmony harmony, IMonitor monitor)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(SpriteObj), "ChangeSprite", new []{ typeof(string)}),
                postfix: this.GetHarmonyMethod(nameof(SpriteObjPatcher.After_ChangeSprite)) //Transcript would improve memory allocation
            );
        }

        /*********
        ** Private methods
        *********/
        /// <summary><see cref="SpriteObj"/> to raise a events when a sprite is not found, which is used to avoid harmony patch due visual.</summary>
        /// <param name="stringID">stringID from the original locID</param>
        /// <returns>Returns the fixed string.</returns>
        private static void After_ChangeSprite(string spriteName, SpriteObj __instance)
        {
            if (string.IsNullOrEmpty(spriteName))
                return;

            if (SpriteLibrary.HasSprite(spriteName))
                SpriteObjPatcher.OnSpriteChange.Invoke(new OnSpriteEventArgs(__instance, spriteName));
            else
                SpriteObjPatcher.OnSpriteNotFound.Invoke(new OnSpriteEventArgs(__instance, spriteName));
        }
    }
}
