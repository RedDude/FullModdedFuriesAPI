namespace FullModdedFuriesAPI.Toolkit.Framework.ModScanning
{
    /// <summary>A general mod type.</summary>
    public enum ModType
    {
        /// <summary>The mod is invalid and its type could not be determined.</summary>
        Invalid,

        /// <summary>The folder is ignored by convention.</summary>
        Ignored,

        /// <summary>A mod which uses FMODF directly.</summary>
        Fmodf,

        /// <summary>A mod which contains files loaded by a FMODF mod.</summary>
        ContentPack,

        /// <summary>A legacy mod which replaces game files directly.</summary>
        Xnb
    }
}
