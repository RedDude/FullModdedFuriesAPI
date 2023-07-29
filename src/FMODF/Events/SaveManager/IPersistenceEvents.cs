using System;

namespace FullModdedFuriesAPI.Events
{
    /// <summary>Events linked to the game's update loop. The update loop runs roughly ≈60 times/second to run game logic like state changes, action handling, etc. These can be useful, but you should consider more semantic events like <see cref="IInputEvents"/> if possible.</summary>
    public interface IPersistenceEvents
    {
        event EventHandler<ProfileLoadedEventArgs> ProfileLoaded;
        event EventHandler<ProfileDeletedEventArgs> DeleteProfile;
        event EventHandler<ProfileResetEventArgs> ResetStatsFile;

        event EventHandler<ProfileResetEventArgs> ResetStats;
        event EventHandler<SaveLoadingEventArgs> SaveLoading;
        event EventHandler<ConfigEventArgs> ConfigSaving;
        event EventHandler<ConfigEventArgs> ConfigSaved;
        event EventHandler<ConfigEventArgs> ConfigLoading;
        event EventHandler<ConfigEventArgs> ConfigLoaded;

        /// <summary>Raised before the game creates a new save file.</summary>
        event EventHandler<SaveCreatingEventArgs> SaveCreating;

        /// <summary>Raised after the game finishes creating the save file.</summary>
        event EventHandler<SaveCreatedEventArgs> SaveCreated;

        /// <summary>Raised before the game begins writing data to the save file (except the initial save creation).</summary>
        event EventHandler<SavingEventArgs> Saving;

        /// <summary>Raised after the game finishes writing data to the save file (except the initial save creation).</summary>
        event EventHandler<SavedEventArgs> Saved;

        /// <summary>Raised after the player loads a save slot and the world is initialized.</summary>
        event EventHandler<SaveLoadedEventArgs> SaveLoaded;

    }
}
