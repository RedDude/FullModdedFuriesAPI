using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FullModdedFuriesAPI.Toolkit;
using FullModdedFuriesAPI.Toolkit.Framework.UpdateData;
using FullModdedFuriesAPI.Web.Framework.Clients;

namespace FullModdedFuriesAPI.Web.Framework
{
    /// <summary>Handles fetching data from mod sites.</summary>
    internal class ModSiteManager
    {
        /*********
        ** Fields
        *********/
        /// <summary>The mod sites which provide mod metadata.</summary>
        private readonly IDictionary<ModSiteKey, IModSiteClient> ModSites;


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="modSites">The mod sites which provide mod metadata.</param>
        public ModSiteManager(IModSiteClient[] modSites)
        {
            this.ModSites = modSites.ToDictionary(p => p.SiteKey);
        }

        /// <summary>Get the mod info for an update key.</summary>
        /// <param name="updateKey">The namespaced update key.</param>
        public async Task<IModPage> GetModPageAsync(UpdateKey updateKey)
        {
            // get site
            if (!this.ModSites.TryGetValue(updateKey.Site, out IModSiteClient client))
                return new GenericModPage(updateKey.Site, updateKey.ID).SetError(RemoteModStatus.DoesNotExist, $"There's no mod site with key '{updateKey.Site}'. Expected one of [{string.Join(", ", this.ModSites.Keys)}].");

            // fetch mod
            IModPage mod;
            try
            {
                mod = await client.GetModData(updateKey.ID);
            }
            catch (Exception ex)
            {
                mod = new GenericModPage(updateKey.Site, updateKey.ID).SetError(RemoteModStatus.TemporaryError, ex.ToString());
            }

            // handle errors
            return mod ?? new GenericModPage(updateKey.Site, updateKey.ID).SetError(RemoteModStatus.DoesNotExist, $"Found no {updateKey.Site} mod with ID '{updateKey.ID}'.");
        }

        /// <summary>Parse version info for the given mod page info.</summary>
        /// <param name="page">The mod page info.</param>
        /// <param name="subkey">The optional update subkey to match in available files. (If no file names or descriptions contain the subkey, it'll be ignored.)</param>
        /// <param name="mapRemoteVersions">Maps remote versions to a semantic version for update checks.</param>
        /// <param name="allowNonStandardVersions">Whether to allow non-standard versions.</param>
        public ModInfoModel GetPageVersions(IModPage page, string subkey, bool allowNonStandardVersions, IDictionary<string, string> mapRemoteVersions)
        {
            // get base model
            ModInfoModel model = new ModInfoModel()
                .SetBasicInfo(page.Name, page.Url)
                .SetError(page.Status, page.Error);
            if (page.Status != RemoteModStatus.Ok)
                return model;

            // fetch versions
            bool hasVersions = this.TryGetLatestVersions(page, subkey, allowNonStandardVersions, mapRemoteVersions, out ISemanticVersion mainVersion, out ISemanticVersion previewVersion);
            if (!hasVersions && subkey != null)
                hasVersions = this.TryGetLatestVersions(page, null, allowNonStandardVersions, mapRemoteVersions, out mainVersion, out previewVersion);
            if (!hasVersions)
                return model.SetError(RemoteModStatus.InvalidData, $"The {page.Site} mod with ID '{page.Id}' has no valid versions.");

            // return info
            return model.SetVersions(mainVersion, previewVersion);
        }

        /// <summary>Get a semantic local version for update checks.</summary>
        /// <param name="version">The version to parse.</param>
        /// <param name="map">A map of version replacements.</param>
        /// <param name="allowNonStandard">Whether to allow non-standard versions.</param>
        public ISemanticVersion GetMappedVersion(string version, IDictionary<string, string> map, bool allowNonStandard)
        {
            // try mapped version
            string rawNewVersion = this.GetRawMappedVersion(version, map, allowNonStandard);
            if (SemanticVersion.TryParse(rawNewVersion, allowNonStandard, out ISemanticVersion parsedNew))
                return parsedNew;

            // return original version
            return SemanticVersion.TryParse(version, allowNonStandard, out ISemanticVersion parsedOld)
                ? parsedOld
                : null;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Get the mod version numbers for the given mod.</summary>
        /// <param name="mod">The mod to check.</param>
        /// <param name="subkey">The optional update subkey to match in available files. (If no file names or descriptions contain the subkey, it'll be ignored.)</param>
        /// <param name="allowNonStandardVersions">Whether to allow non-standard versions.</param>
        /// <param name="mapRemoteVersions">Maps remote versions to a semantic version for update checks.</param>
        /// <param name="main">The main mod version.</param>
        /// <param name="preview">The latest prerelease version, if newer than <paramref name="main"/>.</param>
        private bool TryGetLatestVersions(IModPage mod, string subkey, bool allowNonStandardVersions, IDictionary<string, string> mapRemoteVersions, out ISemanticVersion main, out ISemanticVersion preview)
        {
            main = null;
            preview = null;

            // parse all versions from the mod page
            IEnumerable<(string name, string description, ISemanticVersion version)> GetAllVersions()
            {
                if (mod != null)
                {
                    ISemanticVersion ParseAndMapVersion(string raw)
                    {
                        raw = this.NormalizeVersion(raw);
                        return this.GetMappedVersion(raw, mapRemoteVersions, allowNonStandardVersions);
                    }

                    // get mod version
                    ISemanticVersion modVersion = ParseAndMapVersion(mod.Version);
                    if (modVersion != null)
                        yield return (name: null, description: null, version: ParseAndMapVersion(mod.Version));

                    // get file versions
                    foreach (IModDownload download in mod.Downloads)
                    {
                        ISemanticVersion cur = ParseAndMapVersion(download.Version);
                        if (cur != null)
                            yield return (download.Name, download.Description, cur);
                    }
                }
            }
            var versions = GetAllVersions()
                .OrderByDescending(p => p.version, SemanticVersionComparer.Instance)
                .ToArray();

            // get main + preview versions
            void TryGetVersions(out ISemanticVersion mainVersion, out ISemanticVersion previewVersion, Func<(string name, string description, ISemanticVersion version), bool> filter = null)
            {
                mainVersion = null;
                previewVersion = null;

                // get latest main + preview version
                foreach (var entry in versions)
                {
                    if (filter?.Invoke(entry) == false)
                        continue;

                    if (entry.version.IsPrerelease())
                        previewVersion ??= entry.version;
                    else
                        mainVersion ??= entry.version;

                    if (mainVersion != null)
                        break; // any other values will be older
                }

                // normalize values
                if (previewVersion is not null)
                {
                    mainVersion ??= previewVersion; // if every version is prerelease, latest one is the main version
                    if (!previewVersion.IsNewerThan(mainVersion))
                        previewVersion = null;
                }
            }

            if (subkey is not null)
                TryGetVersions(out main, out preview, entry => entry.name?.Contains(subkey, StringComparison.OrdinalIgnoreCase) == true || entry.description?.Contains(subkey, StringComparison.OrdinalIgnoreCase) == true);
            if (main is null)
                TryGetVersions(out main, out preview);

            return main != null;
        }

        /// <summary>Get a semantic local version for update checks.</summary>
        /// <param name="version">The version to map.</param>
        /// <param name="map">A map of version replacements.</param>
        /// <param name="allowNonStandard">Whether to allow non-standard versions.</param>
        private string GetRawMappedVersion(string version, IDictionary<string, string> map, bool allowNonStandard)
        {
            if (version == null || map == null || !map.Any())
                return version;

            // match exact raw version
            if (map.ContainsKey(version))
                return map[version];

            // match parsed version
            if (SemanticVersion.TryParse(version, allowNonStandard, out ISemanticVersion parsed))
            {
                if (map.ContainsKey(parsed.ToString()))
                    return map[parsed.ToString()];

                foreach ((string fromRaw, string toRaw) in map)
                {
                    if (SemanticVersion.TryParse(fromRaw, allowNonStandard, out ISemanticVersion target) && parsed.Equals(target) && SemanticVersion.TryParse(toRaw, allowNonStandard, out ISemanticVersion newVersion))
                        return newVersion.ToString();
                }
            }

            return version;
        }

        /// <summary>Normalize a version string.</summary>
        /// <param name="version">The version to normalize.</param>
        private string NormalizeVersion(string version)
        {
            if (string.IsNullOrWhiteSpace(version))
                return null;

            version = version.Trim();
            if (Regex.IsMatch(version, @"^v\d", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)) // common version prefix
                version = version.Substring(1);

            return version;
        }
    }
}
