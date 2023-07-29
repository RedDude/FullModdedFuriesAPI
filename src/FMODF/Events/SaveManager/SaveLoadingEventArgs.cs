using System;

namespace FullModdedFuriesAPI.Events
{
    /// <summary>Event arguments for an <see cref="IGameLoopEvents.SaveLoaded"/> event.</summary>
    public class SaveLoadingEventArgs : EventArgs
    {
        /*********
   ** Accessors
   *********/
        /// <summary>The Profile index from the saved file.</summary>
        public int ProfileIndex { get; }
        public bool SaveToPlayer { get; }

        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="profileIndex">The Profile index from the saved file.</param>
        /// <param name="saveToPlayer"></param>
        internal SaveLoadingEventArgs(int profileIndex, bool saveToPlayer)
        {
            this.ProfileIndex = profileIndex;
            this.SaveToPlayer = saveToPlayer;
        }
    }
}
