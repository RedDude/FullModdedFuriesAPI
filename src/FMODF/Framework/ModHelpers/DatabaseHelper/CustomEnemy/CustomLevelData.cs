using System;
using System.Collections.Generic;
using Brawler2D;

namespace FullModdedFuriesAPI.Framework.ModHelpers.DatabaseHelper
{
    public class CustomLevelData
    {
        public string name;
        public string path;
        public ScreenType type;
        public Func<IEnumerable<object>> load;
        public Func<BrawlerGameScreen> createClass;
    }
}
