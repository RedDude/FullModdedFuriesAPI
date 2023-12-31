using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using FullModdedFuriesAPI.Toolkit.Utilities;
#if FMODF_FOR_WINDOWS
using Microsoft.Win32;
#endif

namespace FullModdedFuriesAPI.Toolkit.Framework.GameScanning
{
    /// <summary>Finds installed game folders.</summary>
    public class GameScanner
    {
        /*********
        ** Fields
        *********/
        /// <summary>The current OS.</summary>
        private readonly Platform Platform;


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        public GameScanner()
        {
            this.Platform = EnvironmentUtility.DetectPlatform();
        }

        /// <summary>Find all valid Full Metal Furies install folders.</summary>
        /// <remarks>This checks default game locations, and on Windows checks the Windows registry for GOG/Steam install data. A folder is considered 'valid' if it contains the Full Metal Furies executable for the current OS.</remarks>
        public IEnumerable<DirectoryInfo> Scan()
        {
            // get install paths
            IEnumerable<string> paths = this
                .GetCustomInstallPaths()
                .Concat(this.GetDefaultInstallPaths())
                .Select(PathUtilities.NormalizePath)
                .Distinct(StringComparer.OrdinalIgnoreCase);

            // yield valid folders
            foreach (string path in paths)
            {
                DirectoryInfo folder = new DirectoryInfo(path);
                if (this.LooksLikeGameFolder(folder))
                    yield return folder;
            }
        }

        /// <summary>Get whether a folder seems to contain the game.</summary>
        /// <param name="dir">The folder to check.</param>
        public bool LooksLikeGameFolder(DirectoryInfo dir)
        {
            return
                dir.Exists
                && dir.EnumerateFiles("Brawler2D.exe").Any();
        }


        /*********
        ** Private methods
        *********/
        /// <summary>The default file paths where Full Metal Furies can be installed.</summary>
        /// <remarks>Derived from the <a href="https://github.com/Pathoschild/Stardew.ModBuildConfig">crossplatform mod config</a>.</remarks>
        private IEnumerable<string> GetDefaultInstallPaths()
        {
            switch (this.Platform)
            {
                case Platform.Linux:
                case Platform.Mac:
                    {
                        string home = Environment.GetEnvironmentVariable("HOME");

                        // Linux
                        yield return $"{home}/GOG Games/Full Metal Furies/game";
                        yield return Directory.Exists($"{home}/.steam/steam/steamapps/common/Full Metal Furies")
                            ? $"{home}/.steam/steam/steamapps/common/Full Metal Furies"
                            : $"{home}/.local/share/Steam/steamapps/common/Full Metal Furies";

                        // macOS
                        yield return "/Applications/Full Metal Furies.app/Contents/MacOS";
                        yield return $"{home}/Library/Application Support/Steam/steamapps/common/Full Metal Furies/Contents/MacOS";
                    }
                    break;

                case Platform.Windows:
                    {
                        // Windows registry
#if FMODF_FOR_WINDOWS
                        IDictionary<string, string> registryKeys = new Dictionary<string, string>
                        {
                            [@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 416600"] = "InstallLocation", // Steam
                            [@"SOFTWARE\WOW6432Node\GOG.com\Games\1453375253"] = "PATH", // GOG on 64-bit Windows
                        };
                        foreach (var pair in registryKeys)
                        {
                            string path = this.GetLocalMachineRegistryValue(pair.Key, pair.Value);
                            if (!string.IsNullOrWhiteSpace(path))
                                yield return path;
                        }

                        // via Steam library path
                        string steamPath = this.GetCurrentUserRegistryValue(@"Software\Valve\Steam", "SteamPath");
                        if (steamPath != null)
                            yield return Path.Combine(steamPath.Replace('/', '\\'), @"steamapps\common\Full Metal Furies");
#endif

                        // default paths
                        foreach (string programFiles in new[] { @"C:\Program Files", @"C:\Program Files (x86)" })
                        {
                            yield return $@"{programFiles}\GalaxyClient\Games\Full Metal Furies";
                            yield return $@"{programFiles}\GOG Galaxy\Games\Full Metal Furies";
                            yield return $@"{programFiles}\GOG Games\Full Metal Furies";
                            yield return $@"{programFiles}\Steam\steamapps\common\Full Metal Furies";
                        }
                    }
                    break;

                default:
                    throw new InvalidOperationException($"Unknown platform '{this.Platform}'.");
            }
        }

        /// <summary>Get the custom install path from the <c>fmf.targets</c> file in the home directory, if any.</summary>
        private IEnumerable<string> GetCustomInstallPaths()
        {
            // get home path
            string homePath = Environment.GetEnvironmentVariable(this.Platform == Platform.Windows ? "USERPROFILE" : "HOME");
            if (string.IsNullOrWhiteSpace(homePath))
                yield break;

            // get targets file
            FileInfo file = new FileInfo(Path.Combine(homePath, "fmf.targets"));
            if (!file.Exists)
                yield break;

            // parse file
            XElement root;
            try
            {
                using FileStream stream = file.OpenRead();
                root = XElement.Load(stream);
            }
            catch
            {
                yield break;
            }

            // get install path
            XElement element = root.XPathSelectElement("//*[local-name() = 'GamePath']"); // can't use '//GamePath' due to the default namespace
            if (!string.IsNullOrWhiteSpace(element?.Value))
                yield return element.Value.Trim();
        }

#if FMODF_FOR_WINDOWS
        /// <summary>Get the value of a key in the Windows HKLM registry.</summary>
        /// <param name="key">The full path of the registry key relative to HKLM.</param>
        /// <param name="name">The name of the value.</param>
        private string GetLocalMachineRegistryValue(string key, string name)
        {
            RegistryKey localMachine = Environment.Is64BitOperatingSystem ? RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64) : Registry.LocalMachine;
            RegistryKey openKey = localMachine.OpenSubKey(key);
            if (openKey == null)
                return null;
            using (openKey)
                return (string)openKey.GetValue(name);
        }

        /// <summary>Get the value of a key in the Windows HKCU registry.</summary>
        /// <param name="key">The full path of the registry key relative to HKCU.</param>
        /// <param name="name">The name of the value.</param>
        private string GetCurrentUserRegistryValue(string key, string name)
        {
            RegistryKey currentuser = Environment.Is64BitOperatingSystem ? RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64) : Registry.CurrentUser;
            RegistryKey openKey = currentuser.OpenSubKey(key);
            if (openKey == null)
                return null;
            using (openKey)
                return (string)openKey.GetValue(name);
        }
#endif
    }
}
