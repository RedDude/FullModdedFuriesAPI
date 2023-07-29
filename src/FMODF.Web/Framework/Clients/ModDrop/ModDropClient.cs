using System.Collections.Generic;
using System.Threading.Tasks;
using Pathoschild.Http.Client;
using FullModdedFuriesAPI.Toolkit.Framework.UpdateData;
using FullModdedFuriesAPI.Web.Framework.Clients.ModDrop.ResponseModels;

namespace FullModdedFuriesAPI.Web.Framework.Clients.ModDrop
{
    /// <summary>An HTTP client for fetching mod metadata from the ModDrop API.</summary>
    internal class ModDropClient : IModDropClient
    {
        /*********
        ** Fields
        *********/
        /// <summary>The underlying HTTP client.</summary>
        private readonly IClient Client;

        /// <summary>The URL for a ModDrop mod page for the user, where {0} is the mod ID.</summary>
        private readonly string ModUrlFormat;


        /*********
        ** Accessors
        *********/
        /// <summary>The unique key for the mod site.</summary>
        public ModSiteKey SiteKey => ModSiteKey.ModDrop;


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="userAgent">The user agent for the API client.</param>
        /// <param name="apiUrl">The base URL for the ModDrop API.</param>
        /// <param name="modUrlFormat">The URL for a ModDrop mod page for the user, where {0} is the mod ID.</param>
        public ModDropClient(string userAgent, string apiUrl, string modUrlFormat)
        {
            this.Client = new FluentClient(apiUrl).SetUserAgent(userAgent);
            this.ModUrlFormat = modUrlFormat;
        }

        /// <summary>Get update check info about a mod.</summary>
        /// <param name="id">The mod ID.</param>
        public async Task<IModPage> GetModData(string id)
        {
            var page = new GenericModPage(this.SiteKey, id);

            if (!long.TryParse(id, out long parsedId))
                return page.SetError(RemoteModStatus.DoesNotExist, $"The value '{id}' isn't a valid ModDrop mod ID, must be an integer ID.");

            // get raw data
            ModListModel response = await this.Client
                .PostAsync("")
                .WithBody(new
                {
                    ModIDs = new[] { parsedId },
                    Files = true,
                    Mods = true
                })
                .As<ModListModel>();
            ModModel mod = response.Mods[parsedId];
            if (mod.Mod?.Title == null || mod.Mod.ErrorCode.HasValue)
                return null;

            // get files
            var downloads = new List<IModDownload>();
            foreach (FileDataModel file in mod.Files)
            {
                if (file.IsOld || file.IsDeleted || file.IsHidden)
                    continue;

                downloads.Add(
                    new GenericModDownload(file.Name, file.Description, file.Version)
                );
            }

            // return info
            string name = mod.Mod?.Title;
            string url = string.Format(this.ModUrlFormat, id);
            return page.SetInfo(name: name, version: null, url: url, downloads: downloads);
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            this.Client?.Dispose();
        }
    }
}
