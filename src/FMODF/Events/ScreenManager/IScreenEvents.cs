using System;

namespace FullModdedFuriesAPI.Events
{
    /// <summary>Events linked to the game's update loop. The update loop runs roughly ≈60 times/second to run game logic like state changes, action handling, etc. These can be useful, but you should consider more semantic events like <see cref="IInputEvents"/> if possible.</summary>
    public interface IScreenEvents
    {

        /// <summary>Raised Before the game loads a screen</summary>
        event EventHandler<OnScreenLoadArgs> ScreenLoading;

        /// <summary>After Before the game loads a screen</summary>
        event EventHandler<OnScreenLoadArgs> ScreenLoaded;

    }
}
