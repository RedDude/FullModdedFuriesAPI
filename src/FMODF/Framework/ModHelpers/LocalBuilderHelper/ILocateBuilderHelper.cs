using System.Collections.Generic;
using Brawler2D;
using Microsoft.Xna.Framework.Content;

namespace FullModdedFuriesAPI.Framework.ModHelpers.LocalBuilderHelper
{
    public interface ILocateBuilderHelper
    {
        public object GetCultureInfo(LanguageType languageType);

        public string GetConvertedString(object source, string stringID);

        public string GetResourceString(string stringID);

        public string GetResourceString(string stringID, LanguageType languageType);

        public int GetResourceInt(string stringID);

        public string GetString(string stringID, BrawlerTextObj textObj);

        public string GetLanguageFont(string fontName);

        public void SetString(string stringID, string value);

        public IEnumerable<object> LoadLanguageFile(ContentManager content, string filePath);

        public void RefreshAllText(GameController game);

        public void AddToTextRefreshList(BrawlerTextObj textObj);

        public void RemoveFromTextRefreshList(BrawlerTextObj textObj);

        public void ClearTextRefreshList();

        public bool TextRefreshListContains(BrawlerTextObj textObj);
    }
}
