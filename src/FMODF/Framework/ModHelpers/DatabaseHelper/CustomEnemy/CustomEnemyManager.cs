using System.Collections.Generic;
using Brawler2D;
using Microsoft.Xna.Framework;
using SpriteSystem2;

namespace FullModdedFuriesAPI.Framework.ModHelpers.DatabaseHelper
{
    public class CustomEnemyManager
    {
        public EnemyObj SpawnCustomEnemy(int index, CustomEnemyData enemyData, Vector2 position)
        {
            var enemyManager = GameController.g_game.EnemyManager;
            int enemyObjIndex = this.PreloadCustomEnemy(index, enemyData, position);
            var enemyObj = enemyManager.enemyArray[enemyObjIndex];
            enemyObj.spawnAnimType = EnemySpawnAnimType.None;
            GameController.g_game.ScreenManager.arenaScreen.AddToCurrentEnemyList(enemyObj);
            EnemySpawnAnimator.SpawnEnemy(enemyObj, true);

            return enemyObj;
        }

        public int PreloadCustomEnemy(int index, CustomEnemyData enemyData, Vector2 position)
        {
            var enemyManager = GameController.g_game.EnemyManager;
            var enemyObj = this.CreateCustomEnemyObj(index, enemyData);
            foreach (object obj in enemyObj.LoadContent())
            {
            }

            enemyObj.Initialize();
            enemyObj.isCameraShy = true;
            enemyObj.Active = false;
            enemyObj.Visible = false;
            enemyObj.Collidable = false;
            enemyObj.forceDraw = false;
            enemyObj.Position = position;
            enemyObj.isSpawning = false;
            enemyManager.AddEnemy(enemyObj);
            return enemyManager.enemyArray_count - 1;
        }

        public EnemyObj CreateCustomEnemyObj(int index, CustomEnemyData enemyData)
        {
            EnemyObj enemyObj = (EnemyObj) null;
            var enumerable = enemyData.loadSpriteSheet.Invoke();
            foreach (object obj in enumerable)

            enemyObj = enemyData.createClass.Invoke();
            enemyObj.objType = index;
            return enemyObj;
        }

        // public IEnumerable<object> LoadEnemySpriteSheetIfNeeded(EnemyType enemyType)
        // {
        //     var enumerator =
        //         this.Helper.Content.LoadSpritesheet($"Assets//Player{className}Spritesheet", ContentSource.ModFolder).GetEnumerator();
        //
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
        // public EnemyObj SpawnEnemy(EnemyType enemyType, Vector2 position)
        // {
        //     var enemyManager = GameController.g_game.EnemyManager;
        //     int enemyObjIndex = enemyManager.PreloadEnemy(enemyType, position);
        //     var enemyObj = enemyManager.enemyArray[enemyObjIndex];
        //     enemyObj.spawnAnimType = EnemySpawnAnimType.None;
        //     GameController.g_game.ScreenManager.arenaScreen.AddToCurrentEnemyList(enemyObj);
        //     EnemySpawnAnimator.SpawnEnemy(enemyObj, true);
        //
        //     return enemyObj;
        // }
    }
}
