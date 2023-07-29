using System;
using System.Threading.Tasks;

namespace FullModdedFuriesAPI.Web.Framework.Clients.Pastebin
{
    /// <summary>An API client for Pastebin.</summary>
    internal interface IPastebinClient : IDisposable
    {
        /// <summary>Fetch a saved paste.</summary>
        /// <param name="id">The paste ID.</param>
        Task<PasteInfo> GetAsync(string id);
    }
}
