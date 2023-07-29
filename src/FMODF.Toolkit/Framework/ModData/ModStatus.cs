namespace FullModdedFuriesAPI.Toolkit.Framework.ModData
{
    /// <summary>Indicates how FMODF should treat a mod.</summary>
    public enum ModStatus
    {
        /// <summary>Don't override the status.</summary>
        None,

        /// <summary>The mod is obsolete and shouldn't be used, regardless of version.</summary>
        Obsolete,

        /// <summary>Assume the mod is not compatible, even if FMODF doesn't detect any incompatible code.</summary>
        AssumeBroken,

        /// <summary>Assume the mod is compatible, even if FMODF detects incompatible code.</summary>
        AssumeCompatible
    }
}
