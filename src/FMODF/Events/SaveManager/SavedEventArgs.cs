using System;

namespace FullModdedFuriesAPI.Events
{
    /// <summary>Event arguments for an <see cref="IGameLoopEvents.Saved"/> event.</summary>
    public class SavedEventArgs : EventArgs
    {
        /*********
       ** Accessors
       *********/
        /// <summary>The Profile index from the saved file.</summary>
        public int ProfileIndex { get; }

        /// <summary>Is write to disk.</summary>
        public bool WriteToDisk { get; }

        /// <summary>Is Corrupted file.</summary>
        public bool CorruptedFile { get; }

        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="profileIndex">The Profile index from the saved file.</param>
        /// <param name="writeToDisk">Is write to disk</param>
        /// <param name="corruptedFile">Is Corrupted file</param>
        internal SavedEventArgs(int profileIndex, bool writeToDisk, bool corruptedFile)
        {
            this.ProfileIndex = profileIndex;
            this.WriteToDisk = writeToDisk;
            this.CorruptedFile = corruptedFile;
        }
    }
}
