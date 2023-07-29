using System.Collections.Generic;
using Brawler2D;
using Microsoft.Xna.Framework.Content;

namespace FullModdedFuriesAPI.Framework.ModHelpers.CampHelper
{
    public interface ICampHelper
    {
        public void AddInteractable(string stringID, BrawlerGameScreen gameScreen, IModHelper helper);
    }
}
