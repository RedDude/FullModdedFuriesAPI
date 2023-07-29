using System;
using System.Collections.Generic;
using System.Linq;
using Brawler2D;
using FullModdedFuriesAPI.Patches;
using Microsoft.Xna.Framework;

namespace FullModdedFuriesAPI.Framework.ModHelpers.DatabaseHelper
{
    /// <summary>Provides access to the intern LocateBuilder to access the game Locate information.</summary>
    class DatabaseHelper : BaseHelper, IDatabaseHelper
    {
        /*********
        ** Fields
        *********/
        private static int SkillCountDefault;
        private static int SkillCountAdded;

        private static int CharacterCountDefault;
        private static int CharacterCountAdded;

        private static int enemyCountDefault;
        private static int enemyCountAdded;

        private static int equipCountDefault;
        private static int equipCountAdded;

        private static int levelCountDefault;
        private static int levelCountAdded;


        public static readonly Dictionary<string, int> CustomCharacter = new();
        public static readonly Dictionary<string, int> CustomEquipment = new();
        public static readonly Dictionary<string, int> CustomSkills = new();
        public static readonly Dictionary<string, int> CustomEnemies = new();
        public static readonly Dictionary<int, CustomEnemyData> CustomEnemiesData = new();
        public static readonly Dictionary<string, CustomLevelData> CustomLevel = new();

        private static readonly Dictionary<string, Func<int[,]>> SkillTrees = new();
        private static readonly Dictionary<string, Action<int[,]>> SkillTreesSet = new();

        public static readonly List<string> classNames = new();
        public static readonly Dictionary<int, Color> classColor = new();

        public static CustomEnemyManager CustomEnemyManager;
        /*********
        ** Accessors
        *********/

        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        public DatabaseHelper(string modID)
            : base(modID)
        {
            enemyCountDefault = EnemyEV.EnemyData.Count;
            SkillCountDefault = SkillEV.SkillData.Count;
            equipCountDefault = EquipmentEV.EquipmentData.Count;
            CharacterCountDefault = 4;
            if (DatabaseHelper.SkillTrees.Count != 0) return;
            this.RegisterSkillTree("Engineer", () => SkillEV.EngineerUpgradeTree, skillTree =>
            {
                SkillEV.EngineerUpgradeTree = skillTree;
            });
            this.RegisterSkillTree("Fighter", () => SkillEV.FighterUpgradeTree, skillTree =>
            {
                SkillEV.FighterUpgradeTree = skillTree;
            });
            this.RegisterSkillTree("Sniper", () => SkillEV.SniperUpgradeTree, skillTree =>
            {
                SkillEV.SniperUpgradeTree = skillTree;
            });
            this.RegisterSkillTree("Tank", () => SkillEV.TankUpgradeTree, skillTree =>
            {
                SkillEV.TankUpgradeTree = skillTree;
            });

            DatabaseHelper.classNames.Add("None");
            DatabaseHelper.classNames.Add("Engineer");
            DatabaseHelper.classNames.Add("Fighter");
            DatabaseHelper.classNames.Add("Sniper");
            DatabaseHelper.classNames.Add("Tank");

            CustomEnemyManager = new CustomEnemyManager();
        }

        public int RegisterClass(string id, PlayerClassData data, Color color)
        {
            PlayerEV.PlayerData.Add(id.StartsWith("Player_") ? id : "Player_" + id, data);
            DatabaseHelper.CharacterCountAdded++;

            int index = DatabaseHelper.CharacterCountDefault + DatabaseHelper.CharacterCountAdded;
            DatabaseHelper.CustomCharacter.Add(id.Replace("Player_", ""), index);
            DatabaseHelper.classNames.Add(id.Replace("Player_", ""));
            DatabaseHelper.classColor.Add(index, color);
            return index;
        }

        public void RegisterSkill(Dictionary<string, SkillData> data)
        {
            foreach (var keyValuePair in data)
                this.RegisterSkill(keyValuePair.Key, keyValuePair.Value);
        }

        public int RegisterSkill(string id, SkillData data)
        {
            SkillEV.SkillData.Add(data);

            int index = SkillCountDefault + SkillCountAdded;
            SkillCountAdded++;
            CustomSkills.Add(id, index);
            return index;
        }

        // public int RegisterEnemy(string id, EnemyData data)
        // {
        //     throw new NotImplementedException();
        // }

        public int GetCustomSkillIndex(string skill)
        {
            return CustomSkills[skill];
        }

        public SkillData GetCustomSkill(string skill)
        {
            return SkillEV.SkillData[CustomSkills[skill]];
        }

        public void RegisterEquipment(Dictionary<string, EquipmentData> data)
        {
            foreach (var keyValuePair in data)
                this.RegisterEquipment(keyValuePair.Key, keyValuePair.Value);
        }

        public int RegisterEquipment(string id, EquipmentData data)
        {
            EquipmentEV.EquipmentData.Add(data);
            equipCountAdded++;

            int index = equipCountDefault - 1 + equipCountAdded;
            CustomEquipment.Add(id, index);
            return index;
        }

        public int GetCustomEquipmentIndex(string skill)
        {
            return CustomEquipment[skill];
        }

        public EquipmentData GetCustomEquipment(string skill)
        {
            return EquipmentEV.EquipmentData[CustomEquipment[skill]];
        }

        public void RegisterEnemy(Dictionary<string, EnemyData> data)
        {
            // foreach (var keyValuePair in data)
                // this.RegisterEnemy(keyValuePair.Key, keyValuePair.Value);
        }

        public int RegisterEnemy(string id, CustomEnemyData customEnemyData)
        {
            EnemyEV.EnemyData.Add(id, customEnemyData.data);
            enemyCountAdded++;
            int index = enemyCountDefault - 1 + enemyCountAdded;
            CustomEnemies.Add(id, index);
            CustomEnemiesData.Add(index, customEnemyData);
            return index;
        }

        public int GetCustomEnemyIndex(string name)
        {
            return CustomEnemies[name];
        }

        public EnemyData GetEnemy(string name)
        {
            return EnemyEV.EnemyData[name];
        }

        public CustomEnemyData GetCustomEnemyData(string name)
        {
            int index = this.GetCustomEnemyIndex(name);
            return CustomEnemiesData[index];
        }

        public CustomEnemyData GetCustomEnemyData(int index)
        {
            return CustomEnemiesData[index];
        }

        public EnemyObj SpawnCustomEnemy(string name, Vector2 position)
        {
            int index = this.GetCustomEnemyIndex(name);
            return this.SpawnCustomEnemy(index, position);
        }

        public EnemyObj SpawnCustomEnemy(int index, Vector2 position){
            var data = this.GetCustomEnemyData(index);
            return DatabaseHelper.CustomEnemyManager.SpawnCustomEnemy(index, data, position);
        }

        public int Register(string id, PlayerClassData data, Color color)
        {
            return this.RegisterClass(id, data, color);
        }

        public int Register(string id, SkillData data)
        {
            return this.RegisterSkill(id, data);
        }

        public void Register(Dictionary<string, SkillData> data)
        {
            this.RegisterSkill(data);
        }

        public int Register(string id, EquipmentData data)
        {
            return this.RegisterEquipment(id, data);
        }

        public void Register(Dictionary<string, EquipmentData> data)
        {
            this.RegisterEquipment(data);
        }

        public int Register(string id, CustomLevelData data)
        {
            return this.RegisterLevel(id, data);
        }

        public int RegisterLevel(string id, CustomLevelData data)
        {
            // EquipmentEV.EquipmentData.Add(data);
            levelCountAdded++;

            int index = equipCountDefault - 1 + levelCountAdded;
            DatabaseHelper.CustomLevel.Add(id, data);
            return index;
        }

        // public int Register(string id, EnemyData data)
        // {
        //     return this.RegisterEnemy(id, data);
        // }
        //
        // public void Register(Dictionary<string, EnemyData> data)
        // {
        //     this.RegisterEnemy(data);
        // }

        public void RegisterSkillTree(string id, Func<int[,]> get, Action<int[,]> set)
        {
            SkillTrees[id] = get;
            SkillTreesSet[id] = set;
        }

        public int[,] GetSkillTree(string id)
        {
            if (!SkillTrees.ContainsKey(id))
            {
                int[,] empty = new int[SkillEV.TREE_ROWS, SkillEV.TREE_COLS];
                return empty;
            }
            return SkillTrees[id].Invoke();
        }

        public Dictionary<string, Func<int[,]>> GetSkillTrees()
        {
            return SkillTrees;
        }

        public string GetClassTypeName(int classType)
        {
            return DatabaseHelper.classColor.ContainsKey(classType) ? DatabaseHelper.classNames[classType] : null;
        }

        public string GetClassTypeName(ClassType classType)
        {
            return this.GetClassTypeName((int)classType);
        }

        public Dictionary<string, int> GetCustomClasses()
        {
            return CustomCharacter;
        }

        public Dictionary<string, int> GetCustomEnemies()
        {
            return CustomEnemies;
        }

        public Dictionary<string, int> GetCustomEquipments()
        {
            return CustomEquipment;
        }

        public Dictionary<string, int> GetCustomSkills()
        {
            return CustomSkills;
        }

        public List<string> GetClassesNames()
        {
            return DatabaseHelper.classNames;
        }

        public Dictionary<int, Color> GetClassesColors()
        {
            return DatabaseHelper.classColor;
        }
    }
}
