using System;
using Brawler2D;
using CDGEngine;

namespace FullModdedFuriesAPI.Events
{
    /// <summary>Event arguments for an <see cref="ISpriteEvents.OnSpriteArgs"/> event.</summary>
    public class OnSpriteEventArgs : EventArgs
    {
        /*********
       ** Accessors
       *********/
        /// <summary>The ScreenType from screenManager.</summary>
        public SpriteObj SpriteObj { get; }

        /// <summary>The spriteName.</summary>
        public string spriteName { get; }

        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="ScreenType">The ScreenType from screenManager</param>
        /// <param name="writeToDisk">The level name.</param>
        /// <param name="corruptedFile">Is hostControllerIndex.</param>
        internal OnSpriteEventArgs(SpriteObj spriteObj, string spriteName)
        {
            this.SpriteObj = spriteObj;
            this.spriteName = spriteName;
        }
    }
}
