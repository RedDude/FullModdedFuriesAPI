
using System;

namespace FullModdedFuriesAPI.Events
{
    /// <summary>Event arguments for an <see cref="ProfileLoadedEventArgs"/> event.</summary>
    public class ProfileDeletedEventArgs : EventArgs
    {
        /*********
   ** Accessors
   *********/
        /// <summary>The Profile index from the saved file.</summary>
        public int ProfileIndex { get; }

        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="profileIndex">The Profile index from the saved file.</param>
        internal ProfileDeletedEventArgs(int profileIndex)
        {
            this.ProfileIndex = profileIndex;
        }
    }
}
