using System.Collections.Generic;

namespace FullModdedFuriesAPI.Toolkit.Framework.ModData
{
    /// <summary>The FMODF predefined metadata.</summary>
    internal class MetadataModel
    {
        /********
        ** Accessors
        ********/
        /// <summary>Extra metadata about mods.</summary>
        public IDictionary<string, ModDataModel> ModData { get; set; }
    }
}
