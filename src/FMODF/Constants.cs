using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using FullModdedFuriesAPI.Enums;
using FullModdedFuriesAPI.Framework;
using FullModdedFuriesAPI.Framework.ModLoading;
using FullModdedFuriesAPI.Toolkit.Framework;
using FullModdedFuriesAPI.Toolkit.Utilities;
using Brawler2D;
using FullModdedFuriesAPI.Utilities;

namespace FullModdedFuriesAPI
{
    /// <summary>Contains constants that are accessed before the game itself has been loaded.</summary>
    /// <remarks>Most code should use <see cref="Constants"/> instead of this class directly.</remarks>
    internal static class EarlyConstants
    {
        //
        // Note: this class *must not* depend on any external DLL beyond .NET Framework itself.
        // That includes the game or FMODF toolkit, since it's accessed before those are loaded.
        //
        // Adding an external dependency may seem to work in some cases, but will prevent FMODF
        // from showing a human-readable error if the game isn't available. To test this, just
        // rename "Full Metal Furies.exe" in the game folder; you should see an error like "Oops!
        // FMODF can't find the game", not a technical exception.
        //

        /*********
        ** Accessors
        *********/
        /// <summary>The path to the game folder.</summary>
        public static string ExecutionPath { get; } = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        /// <summary>The absolute path to the folder containing FMODF's internal files.</summary>
        public static readonly string InternalFilesPath = Path.Combine(EarlyConstants.ExecutionPath, "fmodf-internal");

        /// <summary>The target game platform.</summary>
        internal static GamePlatform Platform { get; } = (GamePlatform)Enum.Parse(typeof(GamePlatform), LowLevelEnvironmentUtility.DetectPlatform());

        /// <summary>The game framework running the game.</summary>
        internal static GameFramework GameFramework { get; } =
#if FMODF_FOR_XNA
            GameFramework.Xna;
#else
            GameFramework.Fna;
#endif

        /// <summary>The game's assembly name.</summary>
        internal static string GameAssemblyName { get; } = "Brawler2D";

        /// <summary>FMODF's current raw semantic version.</summary>
        internal static string RawApiVersion = "3.12.6";
    }

    /// <summary>Contains FMODF's constants and assumptions.</summary>
    public static class Constants
    {
        /*********
        ** Accessors
        *********/
        /****
        ** Public
        ****/
        /// <summary>FMODF's current semantic version.</summary>
        public static ISemanticVersion ApiVersion { get; } = new Toolkit.SemanticVersion(EarlyConstants.RawApiVersion);

        /// <summary>The minimum supported version of Full Metal Furies.</summary>
        public static ISemanticVersion MinimumGameVersion { get; } = new GameVersion("1.2.2");

        /// <summary>The maximum supported version of Full Metal Furies.</summary>
        public static ISemanticVersion MaximumGameVersion { get; } = new GameVersion("1.2.2");

        /// <summary>The target game platform.</summary>
        public static GamePlatform TargetPlatform { get; } = EarlyConstants.Platform;

        /// <summary>The game framework running the game.</summary>
        public static GameFramework GameFramework { get; } = EarlyConstants.GameFramework;

        /// <summary>The path to the game folder.</summary>
        public static string ExecutionPath { get; } = EarlyConstants.ExecutionPath;

        /// <summary>The directory path containing Full Metal Furies save app data.</summary>
        public static string DataPath { get; } = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)+@"\Documents\Cellar Door Games\Full Metal Furies\";
            //Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Full Metal Furies");

        /// <summary>The directory path in which error logs should be stored.</summary>
        public static string LogDir { get; } = Path.Combine(Constants.DataPath, "Crash Logs");

        /// <summary>The directory path where all saves are stored.</summary>
        public static string SavesPath { get; } = Path.Combine(Constants.DataPath, "Saves");

        /// <summary>The name of the current save folder (if save info is available, regardless of whether the save file exists yet).</summary>
        public static string SaveFolderName => Constants.GetSaveFolderName();

        /// <summary>The absolute path to the current save folder (if save info is available and the save file exists).</summary>
        public static string CurrentSavePath => Constants.GetSaveFolderPathIfExists();

        /****
        ** Internal
        ****/
        /// <summary>Whether FMODF was compiled in debug mode.</summary>
        internal const bool IsDebugBuild =
#if DEBUG
            true;
#else
            false;
#endif

        /// <summary>The URL of the FMODF home page.</summary>
        internal const string HomePageUrl = "https://fmodf.io";

        /// <summary>The absolute path to the folder containing FMODF's internal files.</summary>
        internal static readonly string InternalFilesPath = EarlyConstants.InternalFilesPath;

        /// <summary>The file path for the FMODF configuration file.</summary>
        internal static string ApiConfigPath => Path.Combine(Constants.InternalFilesPath, "config.json");

        /// <summary>The file path for the overrides file for <see cref="ApiConfigPath"/>, which is applied over it.</summary>
        internal static string ApiUserConfigPath => Path.Combine(Constants.InternalFilesPath, "config.user.json");

        /// <summary>The file path for the FMODF metadata file.</summary>
        internal static string ApiMetadataPath => Path.Combine(Constants.InternalFilesPath, "metadata.json");

        /// <summary>The filename prefix used for all FMODF logs.</summary>
        internal static string LogNamePrefix { get; } = "FMODF-";

        /// <summary>The filename for FMODF's main log, excluding the <see cref="LogExtension"/>.</summary>
        internal static string LogFilename { get; } = $"{Constants.LogNamePrefix}latest";

        /// <summary>The filename extension for FMODF log files.</summary>
        internal static string LogExtension { get; } = "txt";

        /// <summary>The file path for the log containing the previous fatal crash, if any.</summary>
        internal static string FatalCrashLog => Path.Combine(Constants.LogDir, "FMODF-crash.txt");

        /// <summary>The file path which stores a fatal crash message for the next run.</summary>
        internal static string FatalCrashMarker => Path.Combine(Constants.InternalFilesPath, "FullModdedFuriesAPI.crash.marker");

        /// <summary>The file path which stores the detected update version for the next run.</summary>
        internal static string UpdateMarker => Path.Combine(Constants.InternalFilesPath, "FullModdedFuriesAPI.update.marker");

        /// <summary>The default full path to search for mods.</summary>
        internal static string DefaultModsPath { get; } = Path.Combine(Constants.ExecutionPath, "Mods");

        /// <summary>The actual full path to search for mods.</summary>
        internal static string ModsPath { get; set; }

        /// <summary>The game's current semantic version.</summary>
        internal static ISemanticVersion GameVersion { get; }  = new GameVersion("1.2.2");//= new GameVersion(Game1.version);

        /// <summary>The target game platform as a FMODF toolkit constant.</summary>
        internal static Platform Platform { get; } = (Platform)Constants.TargetPlatform;

        /// <summary>The language code for non-translated mod assets.</summary>
        internal static LocalizedContentManager.LanguageCode DefaultLanguage { get; } = LocalizedContentManager.LanguageCode.en;


        /*********
        ** Internal methods
        *********/
        /// <summary>Get the FMODF version to recommend for an older game version, if any.</summary>
        /// <param name="version">The game version to search.</param>
        /// <returns>Returns the compatible FMODF version, or <c>null</c> if none was found.</returns>
        internal static ISemanticVersion GetCompatibleApiVersion(ISemanticVersion version)
        {
            // This covers all officially supported public game updates. It might seem like version
            // ranges would be better, but the given FMODF versions may not be compatible with
            // intermediate unlisted versions (e.g. private beta updates).
            //
            // Nonstandard versions are normalized by GameVersion (e.g. 1.07 => 1.0.7).
            switch (version.ToString())
            {
                case "1.4.1":
                case "1.4.0":
                    return new SemanticVersion("3.0.1");

                case "1.3.36":
                    return new SemanticVersion("2.11.2");

                case "1.3.33":
                case "1.3.32":
                    return new SemanticVersion("2.10.2");

                case "1.3.28":
                    return new SemanticVersion("2.7.0");

                case "1.2.33":
                case "1.2.32":
                case "1.2.31":
                case "1.2.30":
                    return new SemanticVersion("2.5.5");

                case "1.2.29":
                case "1.2.28":
                case "1.2.27":
                case "1.2.26":
                    return new SemanticVersion("1.13.1");

                case "1.1.1":
                case "1.1.0":
                    return new SemanticVersion("1.9.0");

                case "1.0.7.1":
                case "1.0.7":
                case "1.0.6":
                case "1.0.5.2":
                case "1.0.5.1":
                case "1.0.5":
                case "1.0.4":
                case "1.0.3":
                case "1.0.2":
                case "1.0.1":
                case "1.0.0":
                    return new SemanticVersion("0.40.0");

                default:
                    return null;
            }
        }

        /// <summary>Configure the Mono.Cecil assembly resolver.</summary>
        /// <param name="resolver">The assembly resolver.</param>
        internal static void ConfigureAssemblyResolver(AssemblyDefinitionResolver resolver)
        {
            // add search paths
            resolver.AddSearchDirectory(Constants.ExecutionPath);
            resolver.AddSearchDirectory(Constants.InternalFilesPath);

            // add FMODF explicitly
            // Normally this would be handled automatically by the search paths, but for some reason there's a specific
            // case involving unofficial 64-bit Full Metal Furies when launched through Steam (for some players only)
            // where Mono.Cecil can't resolve references to FMODF.
            resolver.Add(AssemblyDefinition.ReadAssembly(typeof(SGame).Assembly.Location));

            // make sure game assembly names can be resolved
            // The game assembly can have one of three names depending how the mod was compiled:
            //   - 'Brawler2D': assembly name
            resolver.AddWithExplicitNames(AssemblyDefinition.ReadAssembly(typeof(GameController).Assembly.Location), "Brawler2D"
            );
        }

        /// <summary>Get metadata for mapping assemblies to the current platform.</summary>
        /// <param name="targetPlatform">The target game platform.</param>
        /// <param name="framework">The game framework running the game.</param>
        internal static PlatformAssemblyMap GetAssemblyMap(Platform targetPlatform, GameFramework framework)
        {
            var removeAssemblyReferences = new List<string>();
            var targetAssemblies = new List<Assembly>();

            // get assembly renamed in FMODF 3.0
            removeAssemblyReferences.Add("FullModdedFuriesAPI.Toolkit.CoreInterfaces");
            targetAssemblies.Add(typeof(FullModdedFuriesAPI.IManifest).Assembly);

            // get changes for platform
            if (Constants.Platform != Platform.Windows)
            {
                removeAssemblyReferences.AddRange(new[]
                {
                    "Brawler2D"
                });
                targetAssemblies.Add(
                    typeof(Brawler2D.GameController).Assembly // note: includes Netcode types on Linux/macOS
                );
            }
            else
            {
                removeAssemblyReferences.Add(
                    "Brawler2D"
                );
                targetAssemblies.AddRange(new[]
                {
                    typeof(Brawler2D.GameController).Assembly
                });
            }

            // get changes for game framework
            switch (framework)
            {
                case GameFramework.Fna:
                    removeAssemblyReferences.AddRange(new[]
                    {
                        "Microsoft.Xna.Framework.Game",
                        "Microsoft.Xna.Framework.Graphics",
                        "Microsoft.Xna.Framework.Xact"
                    });
                    // targetAssemblies.Add(
                    //     typeof(Microsoft.Xna.Framework.Vector2).Assembly
                    // );
                    break;

                case GameFramework.Xna:
                    // removeAssemblyReferences.Add(
                    //     "MonoGame.Framework"
                    // );
                    // targetAssemblies.AddRange(new[]
                    // {
                    //     typeof(Microsoft.Xna.Framework.Vector2).Assembly,
                    //     typeof(Microsoft.Xna.Framework.Game).Assembly,
                    //     typeof(Microsoft.Xna.Framework.Graphics.SpriteBatch).Assembly
                    // });
                    break;

                default:
                    throw new InvalidOperationException($"Unknown game framework '{framework}'.");
            }

            return new PlatformAssemblyMap(targetPlatform, removeAssemblyReferences.ToArray(), targetAssemblies.ToArray());
        }

        /// <summary>Get whether the game assembly was patched by Stardew64Installer.</summary>
        /// <param name="version">The version of Stardew64Installer which was applied to the game assembly, if any.</param>
        internal static bool IsPatchedByStardew64Installer(out ISemanticVersion version)
        {
               version = null;
            return false;

            // PropertyInfo property = typeof(Game1).GetProperty("Stardew64InstallerVersion");
            // if (property == null)
            // {
            //     version = null;
            //     return false;
            // }

            // version = new SemanticVersion((string)property.GetValue(null));
            // return true;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Get the name of the save folder, if any.</summary>
        private static string GetSaveFolderName()
        {
            return Constants.GetSaveFolder()?.Name;
        }

        /// <summary>Get the path to the current save folder, if any.</summary>
        private static string GetSaveFolderPathIfExists()
        {
            DirectoryInfo saveFolder = Constants.GetSaveFolder();
            return saveFolder?.Exists == true
                ? saveFolder.FullName
                : null;
        }

        /// <summary>Get the current save folder, if any.</summary>
        private static DirectoryInfo GetSaveFolder()
        {
            // save not available
            if (Context.LoadStage == LoadStage.None)
                return null;
            return null;
            // get basic info
            // string rawSaveName = Game1.GetSaveGameName(set_value: false);
            // ulong saveID = Context.LoadStage == LoadStage.SaveParsed
            //     ? SaveGame.loaded.uniqueIDForThisGame
            //     : Game1.uniqueIDForThisGame;

            // // get best match (accounting for rare case where folder name isn't sanitized)
            // DirectoryInfo folder = null;
            // foreach (string saveName in new[] { rawSaveName, new string(rawSaveName.Where(char.IsLetterOrDigit).ToArray()) })
            // {
            //     try
            //     {
            //         folder = new DirectoryInfo(Path.Combine(Constants.SavesPath, $"{saveName}_{saveID}"));
            //         if (folder.Exists)
            //             return folder;
            //     }
            //     catch (ArgumentException)
            //     {
            //         // ignore invalid path
            //     }
            // }

            // // if save doesn't exist yet, return the default one we expect to be created
            // return folder;
        }
    }
}
