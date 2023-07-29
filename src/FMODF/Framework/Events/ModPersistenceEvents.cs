using System;
using FullModdedFuriesAPI.Events;

namespace FullModdedFuriesAPI.Framework.Events
{
    /// <summary></summary>
    internal class ModPersistenceEvents : ModEventsBase, IPersistenceEvents
    {
        public event EventHandler<ProfileLoadedEventArgs> ProfileLoaded
        {
            add => this.EventManager.ProfileLoaded.Add(value, this.Mod);
            remove => this.EventManager.ProfileLoaded.Remove(value);
        }
        public event EventHandler<ProfileDeletedEventArgs> DeleteProfile
        {
            add => this.EventManager.DeleteProfile.Add(value, this.Mod);
            remove => this.EventManager.DeleteProfile.Remove(value);
        }
        public event EventHandler<ProfileResetEventArgs> ResetStatsFile
        {
            add => this.EventManager.ResetStatsFile.Add(value, this.Mod);
            remove => this.EventManager.ResetStatsFile.Remove(value);
        }

        public event EventHandler<ProfileResetEventArgs> ResetStats
        {
            add => this.EventManager.ResetStats.Add(value, this.Mod);
            remove => this.EventManager.ResetStats.Remove(value);
        }

        public event EventHandler<ConfigEventArgs> ConfigSaving
        {
            add => this.EventManager.ConfigSaving.Add(value, this.Mod);
            remove => this.EventManager.ConfigSaving.Remove(value);
        }

        public event EventHandler<ConfigEventArgs> ConfigSaved
        {
            add => this.EventManager.ConfigSaved.Add(value, this.Mod);
            remove => this.EventManager.ConfigSaved.Remove(value);
        }

        public event EventHandler<ConfigEventArgs> ConfigLoading
        {
            add => this.EventManager.ConfigLoading.Add(value, this.Mod);
            remove => this.EventManager.ConfigLoading.Remove(value);
        }

        public event EventHandler<ConfigEventArgs> ConfigLoaded
        {
            add => this.EventManager.ConfigLoaded.Add(value, this.Mod);
            remove => this.EventManager.ConfigLoaded.Remove(value);
        }

        public event EventHandler<SaveCreatingEventArgs> SaveCreating;
        public event EventHandler<SaveCreatedEventArgs> SaveCreated;

        public event EventHandler<SaveLoadingEventArgs> SaveLoading
        {
            add => this.EventManager.SaveLoading.Add(value, this.Mod);
            remove => this.EventManager.SaveLoading.Remove(value);
        }

        /*********
        ** Accessors
        *********/
        /// <summary>Raised before the game begins writes data to the save file.</summary>
        public event EventHandler<SavingEventArgs> Saving
        {
            add => this.EventManager.Saving.Add(value, this.Mod);
            remove => this.EventManager.Saving.Remove(value);
        }

        /// <summary>Raised after the game finishes writing data to the save file.</summary>
        public event EventHandler<SavedEventArgs> Saved
        {
            add => this.EventManager.Saved.Add(value, this.Mod);
            remove => this.EventManager.Saved.Remove(value);
        }

        /// <summary>Raised after the player loads a save slot and the world is initialized.</summary>
        public event EventHandler<SaveLoadedEventArgs> SaveLoaded
        {
            add => this.EventManager.SaveLoaded.Add(value, this.Mod);
            remove => this.EventManager.SaveLoaded.Remove(value);
        }

        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="mod">The mod which uses this instance.</param>
        /// <param name="eventManager">The underlying event manager.</param>
        internal ModPersistenceEvents(IModMetadata mod, EventManager eventManager)
            : base(mod, eventManager) { }
    }
}
