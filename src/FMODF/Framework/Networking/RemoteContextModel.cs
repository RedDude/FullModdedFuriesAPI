namespace FullModdedFuriesAPI.Framework.Networking
{
    /// <summary>Metadata about the game, FMODF, and installed mods exchanged with connected computers.</summary>
    internal class RemoteContextModel
    {
        /*********
        ** Accessors
        *********/
        /// <summary>Whether this player is the host player.</summary>
        public bool IsHost { get; set; }

        /// <summary>The game's platform version.</summary>
        public GamePlatform Platform { get; set; }

        /// <summary>The installed version of Full Metal Furies.</summary>
        public ISemanticVersion GameVersion { get; set; }

        /// <summary>The installed version of FMODF.</summary>
        public ISemanticVersion ApiVersion { get; set; }

        /// <summary>The installed mods.</summary>
        public RemoteContextModModel[] Mods { get; set; }
    }
}
