using System;
using Brawler2D;

namespace FullModdedFuriesAPI.Events
{
    /// <summary>Event arguments for an <see cref="IGameLoopEvents.SavingEvent"/> event.</summary>
    public class OnScreenLoadArgs : EventArgs
    {
        /*********
       ** Accessors
       *********/
        /// <summary>The ScreenType from screenManager.</summary>
        public ScreenType ScreenType { get; }

        /// <summary>The level name.</summary>
        public string levelName { get; }

        /// <summary>Is hostControllerIndex.</summary>
        public int hostControllerIndex { get; }

        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="ScreenType">The ScreenType from screenManager</param>
        /// <param name="writeToDisk">The level name.</param>
        /// <param name="corruptedFile">Is hostControllerIndex.</param>
        internal OnScreenLoadArgs(ScreenType screenType, string levelName, int hostControllerIndex)
        {
            this.ScreenType = screenType;
            this.levelName = levelName;
            this.hostControllerIndex = hostControllerIndex;
        }
    }
}
