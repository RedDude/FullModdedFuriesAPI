using System;
using System.Collections.Generic;
using Brawler2D;

namespace FullModdedFuriesAPI.Framework.ModHelpers.DatabaseHelper
{
    public class CustomEnemyData
    {
        public string name;
        public EnemyData data;
        public Func<IEnumerable<object>> loadSpriteSheet;
        public Func<EnemyObj> createClass;
    }
}
