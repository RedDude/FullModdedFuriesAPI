using System;
using System.Collections.Generic;
using Brawler2D;
using Microsoft.Xna.Framework;

namespace FullModdedFuriesAPI.Framework.ModHelpers.DatabaseHelper
{
    public interface IDatabaseHelper
    {
        public int RegisterClass(string id, PlayerClassData data, Color color);
        public void RegisterSkill(Dictionary<string, SkillData> data);
        public int RegisterSkill(string id, SkillData data);
        public void RegisterEquipment(Dictionary<string, EquipmentData> data);
        public int RegisterEquipment(string id, EquipmentData data);

        public int RegisterEnemy(string id, CustomEnemyData customEnemyData);

        public EnemyObj SpawnCustomEnemy(string name, Vector2 position);

        public EnemyObj SpawnCustomEnemy(int index, Vector2 position);
        // public void RegisterEnemy(Dictionary<string, EnemyData> data);
        // public int RegisterEnemy(string id, EnemyData data);

        public int GetCustomSkillIndex(string name);
        public SkillData GetCustomSkill(string name);
        public int GetCustomEquipmentIndex(string name);
        public EquipmentData GetCustomEquipment(string name);
        public int GetCustomEnemyIndex(string name);
        public EnemyData GetEnemy(string name);

        public CustomEnemyData GetCustomEnemyData(string name);

        public CustomEnemyData GetCustomEnemyData(int index);

        public int Register(string id, PlayerClassData data, Color color);

        public int Register(string id, SkillData data);

        public void Register(Dictionary<string, SkillData> data);

        public int Register(string id, EquipmentData data);

        public void Register(Dictionary<string, EquipmentData> data);
        // public int Register(string id, EnemyData data);
        // public void Register(Dictionary<string, EnemyData> data);

        public int Register(string id, CustomLevelData data);
        public int RegisterLevel(string id, CustomLevelData data);

        public void RegisterSkillTree(string id, Func<int[,]> get, Action<int[,]> set);

        public int[,] GetSkillTree(string id);

        public Dictionary<string, Func<int[,]>> GetSkillTrees();

        public string GetClassTypeName(int classType);
        public string GetClassTypeName(ClassType classType);

        public Dictionary<string, int> GetCustomClasses();

        public Dictionary<string, int> GetCustomEnemies();

        public Dictionary<string, int> GetCustomEquipments();

        public Dictionary<string, int> GetCustomSkills();

        public List<string> GetClassesNames();

        public Dictionary<int, Color> GetClassesColors();

    }
}
