using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using FullModdedFuriesAPI.Events;

namespace FullModdedFuriesAPI.Framework.Events
{
    /// <summary>Manages FMODF events.</summary>
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Private fields are deliberately named to simplify organisation.")]
    internal class EventManager
    {
        /*********
        ** Events
        *********/
        /****
        ** Display
        ****/
        /// <summary>Raised after a game menu is opened, closed, or replaced.</summary>
        // public readonly ManagedEvent<MenuChangedEventArgs> MenuChanged;

        // /// <summary>Raised before the game draws anything to the screen in a draw tick, as soon as the sprite batch is opened. The sprite batch may be closed and reopened multiple times after this event is called, but it's only raised once per draw tick. This event isn't useful for drawing to the screen, since the game will draw over it.</summary>
        // public readonly ManagedEvent<RenderingEventArgs> Rendering;

        // /// <summary>Raised after the game draws to the sprite patch in a draw tick, just before the final sprite batch is rendered to the screen. Since the game may open/close the sprite batch multiple times in a draw tick, the sprite batch may not contain everything being drawn and some things may already be rendered to the screen. Content drawn to the sprite batch at this point will be drawn over all vanilla content (including menus, HUD, and cursor).</summary>
        // public readonly ManagedEvent<RenderedEventArgs> Rendered;

        // /// <summary>Raised before the game world is drawn to the screen.</summary>
        // public readonly ManagedEvent<RenderingWorldEventArgs> RenderingWorld;

        // /// <summary>Raised after the game world is drawn to the sprite patch, before it's rendered to the screen.</summary>
        // public readonly ManagedEvent<RenderedWorldEventArgs> RenderedWorld;

        // /// <summary>When a menu is open (<see cref="Brawler2D.Game1.activeClickableMenu"/> isn't null), raised before that menu is drawn to the screen.</summary>
        // public readonly ManagedEvent<RenderingActiveMenuEventArgs> RenderingActiveMenu;

        // /// <summary>When a menu is open (<see cref="Brawler2D.Game1.activeClickableMenu"/> isn't null), raised after that menu is drawn to the sprite batch but before it's rendered to the screen.</summary>
        // public readonly ManagedEvent<RenderedActiveMenuEventArgs> RenderedActiveMenu;

        // /// <summary>Raised before drawing the HUD (item toolbar, clock, etc) to the screen.</summary>
        // public readonly ManagedEvent<RenderingHudEventArgs> RenderingHud;

        // /// <summary>Raised after drawing the HUD (item toolbar, clock, etc) to the sprite batch, but before it's rendered to the screen.</summary>
        // public readonly ManagedEvent<RenderedHudEventArgs> RenderedHud;

        /// <summary>Raised after the game window is resized.</summary>
        public readonly ManagedEvent<WindowResizedEventArgs> WindowResized;

        /****
       ** ScreenManager
       ****/

        public readonly ManagedEvent<OnScreenLoadArgs> ScreenLoading;
        public readonly ManagedEvent<OnScreenLoadArgs> ScreenLoaded;

         /****
        ** Persistence
        ****/

          public readonly ManagedEvent<ProfileLoadedEventArgs> ProfileLoaded;
          public readonly ManagedEvent<ProfileDeletedEventArgs> DeleteProfile;
          public readonly ManagedEvent<ProfileResetEventArgs> ResetStatsFile;
          public readonly ManagedEvent<ProfileResetEventArgs> ResetStats;
          public readonly ManagedEvent<SaveLoadingEventArgs> SaveLoading;

          /// <summary>Raised after the player loads a save slot and the world is initialized.</summary>
          public readonly ManagedEvent<SaveLoadedEventArgs> SaveLoaded;

          /// <summary>Raised after the game finishes writing data to the save file (except the initial save creation).</summary>
          public readonly ManagedEvent<SavedEventArgs> Saved;

          /// <summary>Raised before the game begins writes data to the save file (except the initial save creation).</summary>
          public readonly ManagedEvent<SavingEventArgs> Saving;

          public readonly ManagedEvent<ConfigEventArgs> ConfigSaving;
          public readonly ManagedEvent<ConfigEventArgs> ConfigSaved;
          public readonly ManagedEvent<ConfigEventArgs> ConfigLoading;
          public readonly ManagedEvent<ConfigEventArgs> ConfigLoaded;

        /// <summary>Raised after the game finishes creating the save file.</summary>
        public readonly ManagedEvent<SaveCreatedEventArgs> SaveCreated;

        /****
        ** Game loop
        ****/
        /// <summary>Raised after the game is launched, right before the first update tick.</summary>
        public readonly ManagedEvent<GameLaunchedEventArgs> GameLaunched;

        /// <summary>Raised before the game performs its overall update tick (≈60 times per second).</summary>
        public readonly ManagedEvent<UpdateTickingEventArgs> UpdateTicking;

        /// <summary>Raised after the game performs its overall update tick (≈60 times per second).</summary>
        public readonly ManagedEvent<UpdateTickedEventArgs> UpdateTicked;

        /// <summary>Raised once per second before the game performs its overall update tick.</summary>
        public readonly ManagedEvent<OneSecondUpdateTickingEventArgs> OneSecondUpdateTicking;

        /// <summary>Raised once per second after the game performs its overall update tick.</summary>
        public readonly ManagedEvent<OneSecondUpdateTickedEventArgs> OneSecondUpdateTicked;

        /// <summary>Raised before the game creates the save file.</summary>
        public readonly ManagedEvent<SaveCreatingEventArgs> SaveCreating;

        /// <summary>Raised after the game returns to the title screen.</summary>
        public readonly ManagedEvent<ReturnedToTitleEventArgs> ReturnedToTitle;

        /****
        ** Input
        ****/
        /// <summary>Raised after the player presses or releases any buttons on the keyboard, controller, or mouse.</summary>
        // public readonly ManagedEvent<ButtonsChangedEventArgs> ButtonsChanged;

        // /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        // public readonly ManagedEvent<ButtonPressedEventArgs> ButtonPressed;

        // /// <summary>Raised after the player released a button on the keyboard, controller, or mouse.</summary>
        // public readonly ManagedEvent<ButtonReleasedEventArgs> ButtonReleased;

        /// <summary>Raised after the player moves the in-game cursor.</summary>
        public readonly ManagedEvent<CursorMovedEventArgs> CursorMoved;

        /// <summary>Raised after the player scrolls the mouse wheel.</summary>
        public readonly ManagedEvent<MouseWheelScrolledEventArgs> MouseWheelScrolled;

        /****
        ** Multiplayer
        ****/
        /// <summary>Raised after the mod context for a peer is received. This happens before the game approves the connection (<see cref="IMultiplayerEvents.PeerConnected"/>), so the player doesn't yet exist in the game. This is the earliest point where messages can be sent to the peer via FMODF.</summary>
        public readonly ManagedEvent<PeerContextReceivedEventArgs> PeerContextReceived;

        /// <summary>Raised after a peer connection is approved by the game.</summary>
        public readonly ManagedEvent<PeerConnectedEventArgs> PeerConnected;

        /// <summary>Raised after a mod message is received over the network.</summary>
        public readonly ManagedEvent<ModMessageReceivedEventArgs> ModMessageReceived;

        /// <summary>Raised after the connection with a peer is severed.</summary>
        public readonly ManagedEvent<PeerDisconnectedEventArgs> PeerDisconnected;

        /****
        ** Player
        ****/
        /// <summary>Raised after items are added or removed to a player's inventory.</summary>
        public readonly ManagedEvent<InventoryChangedEventArgs> InventoryChanged;

        /// <summary>Raised after a player skill level changes. This happens as soon as they level up, not when the game notifies the player after their character goes to bed.</summary>
        public readonly ManagedEvent<LevelChangedEventArgs> LevelChanged;

        /// <summary>Raised after a player warps to a new location.</summary>
        // public readonly ManagedEvent<WarpedEventArgs> Warped;

        // /****
        // ** World
        // ****/
        // /// <summary>Raised after a game location is added or removed.</summary>
        // public readonly ManagedEvent<LocationListChangedEventArgs> LocationListChanged;

        // /// <summary>Raised after buildings are added or removed in a location.</summary>
        // public readonly ManagedEvent<BuildingListChangedEventArgs> BuildingListChanged;

        // /// <summary>Raised after debris are added or removed in a location.</summary>
        // public readonly ManagedEvent<DebrisListChangedEventArgs> DebrisListChanged;

        // /// <summary>Raised after large terrain features (like bushes) are added or removed in a location.</summary>
        // public readonly ManagedEvent<LargeTerrainFeatureListChangedEventArgs> LargeTerrainFeatureListChanged;

        // /// <summary>Raised after NPCs are added or removed in a location.</summary>
        // public readonly ManagedEvent<NpcListChangedEventArgs> NpcListChanged;

        // /// <summary>Raised after objects are added or removed in a location.</summary>
        // public readonly ManagedEvent<ObjectListChangedEventArgs> ObjectListChanged;

        // /// <summary>Raised after items are added or removed from a chest.</summary>
        // public readonly ManagedEvent<ChestInventoryChangedEventArgs> ChestInventoryChanged;

        // /// <summary>Raised after terrain features (like floors and trees) are added or removed in a location.</summary>
        // public readonly ManagedEvent<TerrainFeatureListChangedEventArgs> TerrainFeatureListChanged;

        // /// <summary>Raised after furniture are added or removed in a location.</summary>
        // public readonly ManagedEvent<FurnitureListChangedEventArgs> FurnitureListChanged;

        /****
        ** Specialized
        ****/
        /// <summary>Raised when the low-level stage in the game's loading process has changed. See notes on <see cref="ISpecializedEvents.LoadStageChanged"/>.</summary>
        public readonly ManagedEvent<LoadStageChangedEventArgs> LoadStageChanged;

        public readonly ManagedEvent<OnSpriteEventArgs> SpriteChanged;

        public readonly ManagedEvent<OnSpriteEventArgs> SpriteNotFound;

        /// <summary>Raised before the game performs its overall update tick (≈60 times per second). See notes on <see cref="ISpecializedEvents.UnvalidatedUpdateTicking"/>.</summary>
        // public readonly ManagedEvent<UnvalidatedUpdateTickingEventArgs> UnvalidatedUpdateTicking;

        // /// <summary>Raised after the game performs its overall update tick (≈60 times per second). See notes on <see cref="ISpecializedEvents.UnvalidatedUpdateTicked"/>.</summary>
        // public readonly ManagedEvent<UnvalidatedUpdateTickedEventArgs> UnvalidatedUpdateTicked;


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="modRegistry">The mod registry with which to identify mods.</param>
        public EventManager(ModRegistry modRegistry)
        {
            // create shortcut initializers
            ManagedEvent<TEventArgs> ManageEventOf<TEventArgs>(string typeName, string eventName, bool isPerformanceCritical = false)
            {
                return new ManagedEvent<TEventArgs>($"{typeName}.{eventName}", modRegistry, isPerformanceCritical);
            }

            // init events (new)
            // this.MenuChanged = ManageEventOf<MenuChangedEventArgs>(nameof(IModEvents.Display), nameof(IDisplayEvents.MenuChanged));
            // this.Rendering = ManageEventOf<RenderingEventArgs>(nameof(IModEvents.Display), nameof(IDisplayEvents.Rendering), isPerformanceCritical: true);
            // this.Rendered = ManageEventOf<RenderedEventArgs>(nameof(IModEvents.Display), nameof(IDisplayEvents.Rendered), isPerformanceCritical: true);
            // this.RenderingWorld = ManageEventOf<RenderingWorldEventArgs>(nameof(IModEvents.Display), nameof(IDisplayEvents.RenderingWorld), isPerformanceCritical: true);
            // this.RenderedWorld = ManageEventOf<RenderedWorldEventArgs>(nameof(IModEvents.Display), nameof(IDisplayEvents.RenderedWorld), isPerformanceCritical: true);
            // this.RenderingActiveMenu = ManageEventOf<RenderingActiveMenuEventArgs>(nameof(IModEvents.Display), nameof(IDisplayEvents.RenderingActiveMenu), isPerformanceCritical: true);
            // this.RenderedActiveMenu = ManageEventOf<RenderedActiveMenuEventArgs>(nameof(IModEvents.Display), nameof(IDisplayEvents.RenderedActiveMenu), isPerformanceCritical: true);
            // this.RenderingHud = ManageEventOf<RenderingHudEventArgs>(nameof(IModEvents.Display), nameof(IDisplayEvents.RenderingHud), isPerformanceCritical: true);
            // this.RenderedHud = ManageEventOf<RenderedHudEventArgs>(nameof(IModEvents.Display), nameof(IDisplayEvents.RenderedHud), isPerformanceCritical: true);
            // this.WindowResized = ManageEventOf<WindowResizedEventArgs>(nameof(IModEvents.Display), nameof(IDisplayEvents.WindowResized));

            //Persistence
            this.ProfileLoaded = ManageEventOf<ProfileLoadedEventArgs>(nameof(IModEvents.Persistence), nameof(IPersistenceEvents.ProfileLoaded));
            this.DeleteProfile = ManageEventOf<ProfileDeletedEventArgs>(nameof(IModEvents.Persistence), nameof(IPersistenceEvents.DeleteProfile));
            this.ResetStats = ManageEventOf<ProfileResetEventArgs>(nameof(IModEvents.Persistence), nameof(IPersistenceEvents.ResetStats));
            this.ResetStatsFile = ManageEventOf<ProfileResetEventArgs>(nameof(IModEvents.Persistence), nameof(IPersistenceEvents.ResetStatsFile));

            this.SaveLoading = ManageEventOf<SaveLoadingEventArgs>(nameof(IModEvents.Persistence), nameof(IPersistenceEvents.SaveLoading));
            // this.SaveCreating = ManageEventOf<SaveCreatingEventArgs>(nameof(IModEvents.GameLoop), nameof(IPersistenceEvents.SaveCreating));
            // this.SaveCreating = ManageEventOf<SaveCreatingEventArgs>(nameof(IModEvents.GameLoop), nameof(IPersistenceEvents.SaveCreating));
            this.Saving = ManageEventOf<SavingEventArgs>(nameof(IModEvents.GameLoop), nameof(IPersistenceEvents.Saving));
            this.Saved = ManageEventOf<SavedEventArgs>(nameof(IModEvents.GameLoop), nameof(IPersistenceEvents.Saved));
            this.SaveLoaded = ManageEventOf<SaveLoadedEventArgs>(nameof(IModEvents.GameLoop), nameof(IPersistenceEvents.SaveLoaded));

            this.ConfigSaving = ManageEventOf<ConfigEventArgs>(nameof(IModEvents.Persistence), nameof(IPersistenceEvents.ConfigSaving));
            this.ConfigSaved = ManageEventOf<ConfigEventArgs>(nameof(IModEvents.Persistence), nameof(IPersistenceEvents.ConfigSaved));
            this.ConfigLoading = ManageEventOf<ConfigEventArgs>(nameof(IModEvents.Persistence), nameof(IPersistenceEvents.ConfigLoading));
            this.ConfigLoaded = ManageEventOf<ConfigEventArgs>(nameof(IModEvents.Persistence), nameof(IPersistenceEvents.ConfigLoaded));

            this.GameLaunched = ManageEventOf<GameLaunchedEventArgs>(nameof(IModEvents.GameLoop), nameof(IGameLoopEvents.GameLaunched));
            this.UpdateTicking = ManageEventOf<UpdateTickingEventArgs>(nameof(IModEvents.GameLoop), nameof(IGameLoopEvents.UpdateTicking), isPerformanceCritical: true);
            this.UpdateTicked = ManageEventOf<UpdateTickedEventArgs>(nameof(IModEvents.GameLoop), nameof(IGameLoopEvents.UpdateTicked), isPerformanceCritical: true);
            this.OneSecondUpdateTicking = ManageEventOf<OneSecondUpdateTickingEventArgs>(nameof(IModEvents.GameLoop), nameof(IGameLoopEvents.OneSecondUpdateTicking), isPerformanceCritical: true);
            this.OneSecondUpdateTicked = ManageEventOf<OneSecondUpdateTickedEventArgs>(nameof(IModEvents.GameLoop), nameof(IGameLoopEvents.OneSecondUpdateTicked), isPerformanceCritical: true);

            // this.DayStarted = ManageEventOf<DayStartedEventArgs>(nameof(IModEvents.GameLoop), nameof(IGameLoopEvents.DayStarted));
            // this.DayEnding = ManageEventOf<DayEndingEventArgs>(nameof(IModEvents.GameLoop), nameof(IGameLoopEvents.DayEnding));
            // this.TimeChanged = ManageEventOf<TimeChangedEventArgs>(nameof(IModEvents.GameLoop), nameof(IGameLoopEvents.TimeChanged));
            this.ReturnedToTitle = ManageEventOf<ReturnedToTitleEventArgs>(nameof(IModEvents.GameLoop), nameof(IGameLoopEvents.ReturnedToTitle));

            // this.ButtonsChanged = ManageEventOf<ButtonsChangedEventArgs>(nameof(IModEvents.Input), nameof(IInputEvents.ButtonsChanged));
            // this.ButtonPressed = ManageEventOf<ButtonPressedEventArgs>(nameof(IModEvents.Input), nameof(IInputEvents.ButtonPressed));
            // this.ButtonReleased = ManageEventOf<ButtonReleasedEventArgs>(nameof(IModEvents.Input), nameof(IInputEvents.ButtonReleased));
            // this.CursorMoved = ManageEventOf<CursorMovedEventArgs>(nameof(IModEvents.Input), nameof(IInputEvents.CursorMoved), isPerformanceCritical: true);
            // this.MouseWheelScrolled = ManageEventOf<MouseWheelScrolledEventArgs>(nameof(IModEvents.Input), nameof(IInputEvents.MouseWheelScrolled));

            // this.PeerContextReceived = ManageEventOf<PeerContextReceivedEventArgs>(nameof(IModEvents.Multiplayer), nameof(IMultiplayerEvents.PeerContextReceived));
            // this.PeerConnected = ManageEventOf<PeerConnectedEventArgs>(nameof(IModEvents.Multiplayer), nameof(IMultiplayerEvents.PeerConnected));
            // this.ModMessageReceived = ManageEventOf<ModMessageReceivedEventArgs>(nameof(IModEvents.Multiplayer), nameof(IMultiplayerEvents.ModMessageReceived));
            // this.PeerDisconnected = ManageEventOf<PeerDisconnectedEventArgs>(nameof(IModEvents.Multiplayer), nameof(IMultiplayerEvents.PeerDisconnected));

            this.InventoryChanged = ManageEventOf<InventoryChangedEventArgs>(nameof(IModEvents.Player), nameof(IPlayerEvents.InventoryChanged));
            this.LevelChanged = ManageEventOf<LevelChangedEventArgs>(nameof(IModEvents.Player), nameof(IPlayerEvents.LevelChanged));
            // this.Warped = ManageEventOf<WarpedEventArgs>(nameof(IModEvents.Player), nameof(IPlayerEvents.Warped));

            this.SpriteChanged = ManageEventOf<OnSpriteEventArgs>(nameof(IModEvents.Specialized), nameof(ISpecializedEvents.SpriteChanged));
            this.SpriteNotFound = ManageEventOf<OnSpriteEventArgs>(nameof(IModEvents.Specialized), nameof(ISpecializedEvents.SpriteNotFound));

            // this.BuildingListChanged = ManageEventOf<BuildingListChangedEventArgs>(nameof(IModEvents.World), nameof(IWorldEvents.LocationListChanged));
            // this.DebrisListChanged = ManageEventOf<DebrisListChangedEventArgs>(nameof(IModEvents.World), nameof(IWorldEvents.DebrisListChanged));
            // this.LargeTerrainFeatureListChanged = ManageEventOf<LargeTerrainFeatureListChangedEventArgs>(nameof(IModEvents.World), nameof(IWorldEvents.LargeTerrainFeatureListChanged));
            // this.LocationListChanged = ManageEventOf<LocationListChangedEventArgs>(nameof(IModEvents.World), nameof(IWorldEvents.BuildingListChanged));
            // this.NpcListChanged = ManageEventOf<NpcListChangedEventArgs>(nameof(IModEvents.World), nameof(IWorldEvents.NpcListChanged));
            // this.ObjectListChanged = ManageEventOf<ObjectListChangedEventArgs>(nameof(IModEvents.World), nameof(IWorldEvents.ObjectListChanged));
            // this.ChestInventoryChanged = ManageEventOf<ChestInventoryChangedEventArgs>(nameof(IModEvents.World), nameof(IWorldEvents.ChestInventoryChanged));
            // this.TerrainFeatureListChanged = ManageEventOf<TerrainFeatureListChangedEventArgs>(nameof(IModEvents.World), nameof(IWorldEvents.TerrainFeatureListChanged));
            // this.FurnitureListChanged = ManageEventOf<FurnitureListChangedEventArgs>(nameof(IModEvents.World), nameof(IWorldEvents.FurnitureListChanged));

            // this.LoadStageChanged = ManageEventOf<LoadStageChangedEventArgs>(nameof(IModEvents.Specialized), nameof(ISpecializedEvents.LoadStageChanged));
            // this.UnvalidatedUpdateTicking = ManageEventOf<UnvalidatedUpdateTickingEventArgs>(nameof(IModEvents.Specialized), nameof(ISpecializedEvents.UnvalidatedUpdateTicking), isPerformanceCritical: true);
            // this.UnvalidatedUpdateTicked = ManageEventOf<UnvalidatedUpdateTickedEventArgs>(nameof(IModEvents.Specialized), nameof(ISpecializedEvents.UnvalidatedUpdateTicked), isPerformanceCritical: true);
        }

        /// <summary>Get all managed events.</summary>
        public IEnumerable<IManagedEvent> GetAllEvents()
        {
            foreach (FieldInfo field in this.GetType().GetFields())
                yield return (IManagedEvent)field.GetValue(this);
        }
    }
}
