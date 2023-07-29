using System;
using System.Collections.Generic;
using System.Reflection;
using Brawler2D;
using FullModdedFuriesAPI.Utilities;
using Microsoft.Xna.Framework.Content;

namespace FullModdedFuriesAPI.Framework.ModHelpers.LocalBuilderHelper
{
    /// <summary>Provides access to the intern LocateBuilder to access the game Locate information.</summary>
    internal class LocateBuilderHelper : BaseHelper , ILocateBuilderHelper
    {
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
        public LocateBuilderHelper(string modID)
            : base(modID)
        {
            this.Assembly = typeof(GameController).Assembly;
            this.LocalBuilder = this.Assembly.GetType("Brawler2D.LocaleBuilder");

            this._languageType =
                this.LocalBuilder.GetProperty("languageType", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
        }

        public object GetCultureInfo(LanguageType languageType)
        {
            if (this._CultureInfo == null)
                this._CultureInfo =
                    this.LocalBuilder.GetMethod("GetCultureInfo", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, null, new[]
                    {
                        typeof(LanguageType)
                    }, null);

            return this._CultureInfo?.Invoke(this.LocalBuilder, new object[] {languageType});
        }

        public string GetConvertedString(object source, string stringID)
        {
            if (this._GetConvertedString == null)
                this._CultureInfo =
                    this.LocalBuilder.GetMethod("GetConvertedString", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, null, new[]
                    {
                        typeof(string),
                        typeof(LanguageType)
                    }, null);

            return (string) this._GetConvertedString?.Invoke(this.LocalBuilder, new object[] {source, stringID});
        }

        public string GetResourceString(string stringID)
        {
            if (this._GetResourceStringSingleParameter == null)
                this._GetResourceStringSingleParameter =
                    this.LocalBuilder.GetMethod("getResourceString", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, null, new[]
                    {
                        typeof(string),
                    }, null);

            return (string) this._GetResourceStringSingleParameter?.Invoke(this.LocalBuilder, new object[] {stringID});
        }

        public string GetResourceString(string stringID, LanguageType languageType)
        {
            if (this._GetResourceString == null)
                this._GetResourceString =
                    this.LocalBuilder.GetMethod("getResourceString", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, null, new[]
                    {
                        typeof(string),
                        typeof(LanguageType)
                    }, null);

            return (string) this._GetResourceString?.Invoke(this.LocalBuilder, new object[] {stringID, languageType});
        }

        public int GetResourceInt(string stringID)
        {
            if (this._GetResourceInt == null)
                this._GetResourceInt =
                    this.LocalBuilder.GetMethod("getResourceInt", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, null, new[]
                    {
                        typeof(string),
                    }, null);

            return (int) this._GetResourceInt?.Invoke(this.LocalBuilder, new object[] {stringID});
        }

        public string GetString(string stringID, BrawlerTextObj textObj)
        {
            if (this._GetString == null)
                this._GetString =
                    this.LocalBuilder.GetMethod("getString", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, null, new[]
                    {
                        typeof(string),
                        typeof(BrawlerTextObj)
                    }, null);


            return (string) this._GetString?.Invoke(this.LocalBuilder, new object[] {stringID, textObj});
        }

        public string GetLanguageFont(string fontName)
        {
            if (this._GetLanguageFont == null)
                this._GetLanguageFont =
                    this.LocalBuilder.GetMethod("GetLanguageFont", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, null, new[]
                    {
                        typeof(string),
                    }, null);

            return (string) this._GetLanguageFont?.Invoke(this.LocalBuilder, new object[] {fontName});
        }

        public void SetString(string stringID, string value)
        {
            if (this._SetString == null)
                this._SetString =
                    this.LocalBuilder.GetMethod("setString", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, null, new[]
                    {
                        typeof(string),
                        typeof(string)
                    }, null);

            this._SetString?.Invoke(this.LocalBuilder, new object[] {stringID, value});
        }

        public IEnumerable<object> LoadLanguageFile(
            ContentManager content,
            string filePath)
        {
            if (this._LoadLanguageFile == null)
                this._LoadLanguageFile =
                    this.LocalBuilder.GetMethod("LoadLanguageFile", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, null, new[]
                    {
                        typeof(ContentManager),
                        typeof(string)
                    }, null);

            return (IEnumerable<object>) this._LoadLanguageFile?.Invoke(this.LocalBuilder, new object[] {content, filePath});
        }

        public void RefreshAllText(GameController game)
        {
            if (this._RefreshAllText == null)
                this._RefreshAllText =
                    this.LocalBuilder.GetMethod("RefreshAllText", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, null, new[]
                    {
                        typeof(GameController),
                    }, null);

            this._RefreshAllText?.Invoke(this.LocalBuilder, new object[] {game});
        }

        public void AddToTextRefreshList(BrawlerTextObj textObj)
        {
            if (this._AddToTextRefreshList == null)
                this._AddToTextRefreshList =
                    this.LocalBuilder.GetMethod("AddToTextRefreshList", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, null, new[]
                    {
                        typeof(BrawlerTextObj),
                    }, null);

            this._AddToTextRefreshList?.Invoke(this.LocalBuilder, new object[] {textObj});
        }

        public void RemoveFromTextRefreshList(BrawlerTextObj textObj)
        {
            if (this._RemoveFromTextRefreshList == null)
                this._RemoveFromTextRefreshList =
                    this.LocalBuilder.GetMethod("RemoveFromTextRefreshList", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, null, new[]
                    {
                        typeof(BrawlerTextObj),
                    }, null);

            this._RemoveFromTextRefreshList?.Invoke(this.LocalBuilder, new object[] {textObj});
        }

        public void ClearTextRefreshList()
        {
            if (this._ClearTextRefreshList == null)
                this._ClearTextRefreshList =
                    this.LocalBuilder.GetMethod("ClearTextRefreshList", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);

            this._ClearTextRefreshList?.Invoke(this.LocalBuilder, new object[]{ });
        }

        public bool TextRefreshListContains(BrawlerTextObj textObj)
        {
            if (this._TextRefreshListContains == null)
                this._TextRefreshListContains =
                    this.LocalBuilder.GetMethod("TextRefreshListContains", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, null, new[]
                    {
                        typeof(BrawlerTextObj),
                    }, null);

            return (bool) this._TextRefreshListContains?.Invoke(this.LocalBuilder, new object[] {textObj});
        }
    }
}
