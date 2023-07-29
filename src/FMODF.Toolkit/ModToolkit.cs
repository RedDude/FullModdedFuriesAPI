using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FullModdedFuriesAPI.Toolkit.Framework.Clients.Wiki;
using FullModdedFuriesAPI.Toolkit.Framework.GameScanning;
using FullModdedFuriesAPI.Toolkit.Framework.ModData;
using FullModdedFuriesAPI.Toolkit.Framework.ModScanning;
using FullModdedFuriesAPI.Toolkit.Framework.UpdateData;
using FullModdedFuriesAPI.Toolkit.Serialization;

namespace FullModdedFuriesAPI.Toolkit
{
    /// <summary>A convenience wrapper for the various tools.</summary>
    public class ModToolkit
    {
        /*********
        ** Fields
        *********/
        /// <summary>The default HTTP user agent for the toolkit.</summary>
        private readonly string UserAgent;

        /// <summary>Maps vendor keys (like <c>Nexus</c>) to their mod URL template (where <c>{0}</c> is the mod ID). This doesn't affect update checks, which defer to the remote web API.</summary>
        private readonly IDictionary<ModSiteKey, string> VendorModUrls = new Dictionary<ModSiteKey, string>()
        {
            [ModSiteKey.Chucklefish] = "https://community.playstarbound.com/resources/{0}",
            [ModSiteKey.GitHub] = "https://github.com/{0}/releases",
            [ModSiteKey.Nexus] = "https://www.nexusmods.com/stardewvalley/mods/{0}"
        };


        /*********
        ** Accessors
        *********/
        /// <summary>Encapsulates FMODF's JSON parsing.</summary>
        public JsonHelper JsonHelper { get; } = new JsonHelper();


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        public ModToolkit()
        {
            ISemanticVersion version = new SemanticVersion(this.GetType().Assembly.GetName().Version);
            this.UserAgent = $"FMODF Mod Handler Toolkit/{version}";
        }

        /// <summary>Find valid Full Metal Furies install folders.</summary>
        /// <remarks>This checks default game locations, and on Windows checks the Windows registry for GOG/Steam install data. A folder is considered 'valid' if it contains the Full Metal Furies executable for the current OS.</remarks>
        public IEnumerable<DirectoryInfo> GetGameFolders()
        {
            return new GameScanner().Scan();
        }

        /// <summary>Extract mod metadata from the wiki compatibility list.</summary>
        public async Task<WikiModList> GetWikiCompatibilityListAsync()
        {
            var client = new WikiClient(this.UserAgent);
            return await client.FetchModsAsync();
        }

        /// <summary>Get FMODF's internal mod database.</summary>
        /// <param name="metadataPath">The file path for the FMODF metadata file.</param>
        public ModDatabase GetModDatabase(string metadataPath)
        {
            MetadataModel metadata = JsonConvert.DeserializeObject<MetadataModel>(File.ReadAllText(metadataPath));
            ModDataRecord[] records = metadata.ModData.Select(pair => new ModDataRecord(pair.Key, pair.Value)).ToArray();
            return new ModDatabase(records, this.GetUpdateUrl);
        }

        /// <summary>Extract information about all mods in the given folder.</summary>
        /// <param name="rootPath">The root folder containing mods.</param>
        public IEnumerable<ModFolder> GetModFolders(string rootPath)
        {
            return new ModScanner(this.JsonHelper).GetModFolders(rootPath);
        }

        /// <summary>Extract information about all mods in the given folder.</summary>
        /// <param name="rootPath">The root folder containing mods. Only the <paramref name="modPath"/> will be searched, but this field allows it to be treated as a potential mod folder of its own.</param>
        /// <param name="modPath">The mod path to search.</param>
        public IEnumerable<ModFolder> GetModFolders(string rootPath, string modPath)
        {
            return new ModScanner(this.JsonHelper).GetModFolders(rootPath, modPath);
        }

        /// <summary>Get an update URL for an update key (if valid).</summary>
        /// <param name="updateKey">The update key.</param>
        public string GetUpdateUrl(string updateKey)
        {
            UpdateKey parsed = UpdateKey.Parse(updateKey);
            if (!parsed.LooksValid)
                return null;

            if (this.VendorModUrls.TryGetValue(parsed.Site, out string urlTemplate))
                return string.Format(urlTemplate, parsed.ID);

            return null;
        }
    }
}
