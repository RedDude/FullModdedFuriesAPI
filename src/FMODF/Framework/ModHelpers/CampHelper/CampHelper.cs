using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Brawler2D;
using CDGEngine;
using FullModdedFuriesAPI.Utilities;
using Microsoft.Xna.Framework.Content;

namespace FullModdedFuriesAPI.Framework.ModHelpers.CampHelper
{
    /// <summary>Provides access to the intern LocateBuilder to access the game Locate information.</summary>
    internal class CampHelper : BaseHelper , ICampHelper
    {
        private readonly IMonitor Monitor;

        /*********
        ** Fields
        *********/
        /// <summary> The game assembly </summary>
        private readonly Assembly Assembly;

        /// <summary> Reference to the LocalBuilder </summary>
        private readonly Type LocalBuilder;

        /// <summary> Cache the MethodInfos from LocalBuilder </summary>
        private MethodInfo _GetResourceStringSingleParameter;
        private MethodInfo _GetResourceString;
        private MethodInfo _ConvertString;
        private MethodInfo _CultureInfo;
        private MethodInfo _GetResourceInt;
        private MethodInfo _GetConvertedString;
        private MethodInfo _GetString;
        private MethodInfo _GetLanguageFont;
        private MethodInfo _SetString;
        private MethodInfo _LoadLanguageFile;
        private MethodInfo _RefreshAllText;
        private MethodInfo _AddToTextRefreshList;
        private MethodInfo _RemoveFromTextRefreshList;
        private MethodInfo _ClearTextRefreshList;
        private MethodInfo _TextRefreshListContains;
        private PropertyInfo _languageType;

        private Type CampBaseType;
        private FieldInfo LayerArray;

        /*********
        ** Accessors
        *********/
        public LanguageType LanguageType
        {
            get => (LanguageType) this._languageType.GetValue(this.LocalBuilder);
            set => this._languageType.SetValue(this.LocalBuilder, value);
        }

        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="modID">The unique ID of the relevant mod.</param>
        /// <param name="locale">The initial locale.</param>
        /// <param name="languageCode">The game's current language code.</param>
        public CampHelper(string modID, IMonitor monitor)
            : base(modID)
        {
            this.Monitor = monitor;

            this.Assembly = typeof(GameController).Assembly;
            this.CampBaseType = this.Assembly.GetType("Brawler2D.Camp_Base");

            FieldInfo fieldInfo = this.CampBaseType.GetField("m_skillNPC");
            fieldInfo.GetValue(GameController.g_game.ScreenManager.arenaScreen);

            this.LayerArray = this.CampBaseType.GetField("m_layerArray");
            // var arenaScreen = GameController.g_game.ScreenManager.arenaScreen as Camp_Base;
        }

        public void AddInteractable(string stringID, BrawlerGameScreen gameScreen, IModHelper helper)
        {
            var interactableObject = new InteractableObject(GameController.g_game, gameScreen, helper, this.Monitor);
            this.AddInteractable(stringID, helper, interactableObject);
            // SpriteLibrary.m_ssObjDict[key].AddHitboxData(index, x, y, width, height, rotation, type);
        }

        public void AddInteractable(string stringID, IModHelper helper, InteractableObject interactableObject)
        {
        }

        public int GetResourceInt(string stringID)
        {
            throw new NotImplementedException();
        }
    }
}
