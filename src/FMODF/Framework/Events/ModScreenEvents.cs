using System;
using FullModdedFuriesAPI.Events;

namespace FullModdedFuriesAPI.Framework.Events
{
    /// <summary>Events linked to the game's update loop. The update loop runs roughly ≈60 times/second to run game logic like state changes, action handling, etc. These can be useful, but you should consider more semantic events like <see cref="IInputEvents"/> if possible.</summary>
    internal class ModScreenEvents : ModEventsBase, IScreenEvents
    {
        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="mod">The mod which uses this instance.</param>
        /// <param name="eventManager">The underlying event manager.</param>
        internal ModScreenEvents(IModMetadata mod, EventManager eventManager)
            : base(mod, eventManager) { }

        public event EventHandler<OnScreenLoadArgs> ScreenLoading
        {
            add => this.EventManager.ScreenLoading.Add(value, this.Mod);
            remove => this.EventManager.ScreenLoading.Remove(value);
        }
        public event EventHandler<OnScreenLoadArgs> ScreenLoaded
        {
            add => this.EventManager.ScreenLoaded.Add(value, this.Mod);
            remove => this.EventManager.ScreenLoaded.Remove(value);
        }
    }
}
