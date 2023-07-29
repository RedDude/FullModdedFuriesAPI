using System;
using Brawler2D;

namespace FullModdedFuriesAPI.Events
{
    /// <summary>Event arguments for an <see cref="ProfileResetEventArgs"/> event.</summary>
    public class ProfileResetEventArgs : EventArgs
    {
        /*********
       ** Accessors
       *********/
        /// <summary>The Profile index from the saved file.</summary>
        public PlayerObj Player { get; }

        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="player">The PlayerObj which is reseted.</param>
        internal ProfileResetEventArgs(PlayerObj player)
        {
            this.Player = player;
        }
    }
}
