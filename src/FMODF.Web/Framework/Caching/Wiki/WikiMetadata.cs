namespace FullModdedFuriesAPI.Web.Framework.Caching.Wiki
{
    /// <summary>The model for cached wiki metadata.</summary>
    internal class WikiMetadata
    {
        /*********
        ** Accessors
        *********/
        /// <summary>The current stable Stardew Valley version.</summary>
        public string StableVersion { get; set; }

        /// <summary>The current beta Stardew Valley version.</summary>
        public string BetaVersion { get; set; }


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        public WikiMetadata() { }

        /// <summary>Construct an instance.</summary>
        /// <param name="stableVersion">The current stable Stardew Valley version.</param>
        /// <param name="betaVersion">The current beta Stardew Valley version.</param>
        public WikiMetadata(string stableVersion, string betaVersion)
        {
            this.StableVersion = stableVersion;
            this.BetaVersion = betaVersion;
        }
    }
}
