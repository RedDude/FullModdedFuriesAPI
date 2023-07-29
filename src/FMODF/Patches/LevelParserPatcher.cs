using System;
using System.Diagnostics.CodeAnalysis;
using Brawler2D;
using HarmonyLib;
using FullModdedFuriesAPI.Events;
using FullModdedFuriesAPI.Framework.Events;
using FullModdedFuriesAPI.Framework.ModHelpers.DatabaseHelper;
using FullModdedFuriesAPI.Internal.Patching;

namespace FullModdedFuriesAPI.Patches
{
    /// <summary>Harmony patches for <see cref="ScreenManagerPatcher"/> which notify FMODF when persistence data is create or loaded.</summary>
    /// <remarks>Patch methods must be static for Harmony to work correctly. See the Harmony documentation before renaming patch arguments.</remarks>
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Argument names are defined by Harmony and methods are named for clarity.")]
    [SuppressMessage("ReSharper", "IdentifierTypo", Justification = "Argument names are defined by Harmony and methods are named for clarity.")]
    internal class LevelParserPatcher : BasePatcher
    {
        /*********
        ** Fields
        *********/
        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="onStageChanged">A callback to invoke when the load stage changes.</param>
        public static LevelParserPatcher PatchMe(EventManager eventManager)
        {
            return new LevelParserPatcher();
        }
        /// <inheritdoc />
        public override void Apply(Harmony harmony, IMonitor monitor)
        {
           this.Patch(harmony);
        }

        private void Patch(Harmony harmony)
        {

            harmony.Patch(
            original: AccessTools.Method(typeof(LevelParser), nameof(LevelParser.getMapScreen)),
            postfix: this.GetHarmonyMethod(nameof(LevelParserPatcher.After_getMapScreen)));

            harmony.Patch(
            original: AccessTools.Method(typeof(LevelParser), nameof(LevelParser.getCutscene)),
            postfix: this.GetHarmonyMethod(nameof(LevelParserPatcher.After_getCutscene)));

            harmony.Patch(
            original: AccessTools.Method(typeof(LevelParser), nameof(LevelParser.getArena)),
            postfix: this.GetHarmonyMethod(nameof(LevelParserPatcher.After_getArena)));

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
        private static ArenaScreen After_getArena(ArenaScreen __result, string arenaName, GameController game)
        {
            string name = LevelParserPatcher.GetLevelName(arenaName);
            Type type = Type.GetType("Brawler2D." + name);
            if (type != null) return __result;

            DatabaseHelper.CustomLevel.TryGetValue(name, out CustomLevelData levelData);
            if (levelData == null) return __result;

            var createScreen = (ArenaScreen) levelData.createClass.Invoke();
            __result = createScreen;
            return createScreen;
        }

        // private static bool After_getCutscene(ref CutsceneScreen __result, string cutsceneName, GameController game)
        // {
        //     string name = LevelParserPatcher.GetLevelName(cutsceneName);
        //     Type type = Type.GetType("Brawler2D." + name);
        //     if (type != null) return false;
        //
        //     DatabaseHelper.CustomLevel.TryGetValue(name, out CustomLevelData levelData);
        //     if(levelData == null) return false;
        //
        //     var createScreen = (CutsceneScreen)levelData.createClass.Invoke();
        //     __result = createScreen;
        //     return true;
        // }

        private static CutsceneScreen After_getCutscene(CutsceneScreen __result, string cutsceneName, GameController game)
        {
            string name = LevelParserPatcher.GetLevelName(cutsceneName);
            Type type = Type.GetType("Brawler2D." + name);
            if (type != null) return __result;

            DatabaseHelper.CustomLevel.TryGetValue(name, out CustomLevelData levelData);
            if(levelData == null) return __result;

            var createScreen = (CutsceneScreen)levelData.createClass.Invoke();
            __result = createScreen;
            return createScreen;
        }

        private static MapScreen After_getMapScreen(MapScreen __result, string mapName, GameController game)
        {
            string name = LevelParserPatcher.GetLevelName(mapName);
            Type type = Type.GetType("Brawler2D." + name);
            if (type != null) return __result;

            DatabaseHelper.CustomLevel.TryGetValue(name, out CustomLevelData levelData);
            if(levelData == null) return __result;

            var createScreen = (MapScreen)levelData.createClass.Invoke();
            return createScreen;

            // string name = LevelParserPatcher.GetLevelName(mapName);
            // Type type = Type.GetType("Brawler2D." + name);
            // if (type != null) return false;
            //
            // DatabaseHelper.CustomLevel.TryGetValue(name, out CustomLevelData levelData);
            // if(levelData == null) return false;
            //
            // var createScreen = (MapScreen)levelData.createClass.Invoke();
            // __result = createScreen;
            // return true;
        }


        private static string GetLevelName(string name)
        {
            if (!name.Contains(".xml") && !name.Contains(".bef")) return name;

            int num = name.LastIndexOf("\\");
            name = name.Substring(num + 1, name.Length - num - 1);
            name = name.Replace(".xml", "");
            name = name.Replace(".bef", "");

            return name;
        }

        // private static BrawlerGameScreen GetScreen(string name, GameController game, BrawlerGameScreen __result)
        // {
        //     if (name.Contains(".xml") || name.Contains(".bef"))
        //     {
        //         int num = name.LastIndexOf("\\");
        //         name = name.Substring(num + 1, name.Length - num - 1);
        //         name = name.Replace(".xml", "");
        //         name = name.Replace(".bef", "");
        //     }
        //     Type type = Type.GetType("Brawler2D." + name);
        //     if (type != null) return __result;
        //
        //     CustomLevelData levelData;
        //     DatabaseHelper.CustomLevel.TryGetValue(name, out levelData);
        //     if(levelData == null) return __result;
        //
        //     var enumerator = levelData.load.Invoke();
        //     foreach (object o in enumerator)
        //     {
        //         ;
        //     }
        //
        //     BrawlerGameScreen createScreen = levelData.createClass.Invoke();
        //     if(createScreen == null) return __result;
        //
        //     // __result = createScreen;
        //     return createScreen;
        // }
        #endregion

    }
}
