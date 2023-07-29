using System.Diagnostics.CodeAnalysis;
using System.Reflection.Emit;
using Brawler2D;
using FullModdedFuriesAPI.Framework.ModHelpers.DatabaseHelper;
using FullModdedFuriesAPI.Internal.Patching;
using HarmonyLib;
using Microsoft.Xna.Framework;

namespace FullModdedFuriesAPI.Patches
{
    /// <summary>A Harmony patch for <see cref="LocalBuilder"/> methods to remove the {NULLSTRING: localID } string to enable mods to use translation.helper.</summary>
    /// <remarks>Patch methods must be static for Harmony to work correctly. See the Harmony documentation before renaming patch arguments.</remarks>
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Argument names are defined by Harmony and methods are named for clarity.")]
    [SuppressMessage("ReSharper", "IdentifierTypo", Justification = "Argument names are defined by Harmony and methods are named for clarity.")]
    internal class EnemyManagerPatcher : BasePatcher
    {
        /*********
        ** Public methods
        *********/
        /// <inheritdoc />
        public override void Apply(Harmony harmony, IMonitor monitor)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(PlayerEV), "GetColour", new []{typeof(ClassType)}),
                postfix: this.GetHarmonyMethod(nameof(EnemyManagerPatcher.After_GetColour)) //Transcript would improve memory allocation
            );
        }

        // public IEnumerable<object> LoadEnemySpriteSheetIfNeeded(EnemyType enemyType)
        // {
        //     string spritesheetString = enemyType.ToString() + "_Spritesheet";
        //     spritesheetString = spritesheetString.ToLower();
        //     string fullPathString = "Art//Enemies//" + spritesheetString;
        //     if (!SpriteLibrary.IsSpritesheetLoaded(spritesheetString))
        //     {
        //         foreach (object obj in SpriteLibrary.LoadSpritesheetCoro(this.Game.disposableContent, fullPathString))
        //             yield return (object) null;
        //         if (SpriteLibrary.justLoadedSpritesheetOk)
        //             this.Game.ScreenManager.AddLoadedSpritesheetPath(fullPathString);
        //     }
        //     yield return (object) null;
        // }
        //
        // public EnemyObj CreateEnemyObj(EnemyType enemyType)
        // {
        //     EnemyObj enemyObj = (EnemyObj) null;
        //     foreach (object obj in this.LoadEnemySpriteSheetIfNeeded(enemyType))
        //         ;
        //
        //     if (enemyType == EnemyType.Enemy_Dummy_Mercenary)
        //         enemyObj = new Enemy_Dummy_Mercenary(Game);
        //
        //     enemyObj.objType = (int) enemyType;
        //     return enemyObj;
        // }

        /*********
        ** Private methods
        *********/
        /// <summary>Remove {NULLSTRING: locId} from <see cref="LocalBuilder.getResourceString"/> return.</summary>
        /// <param name="stringID">stringID from the original locID</param>
        /// <returns>Returns the fixed string.</returns>
        private static void After_GetColour(ClassType classType, ref Color __result)
        {
            if (__result != Color.White) return;
            if(DatabaseHelper.classColor.ContainsKey((int) classType))
                __result = DatabaseHelper.classColor[(int) classType];
        }
    }
}
