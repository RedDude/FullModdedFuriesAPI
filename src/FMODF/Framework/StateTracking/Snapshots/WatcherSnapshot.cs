using Microsoft.Xna.Framework;
using FullModdedFuriesAPI.Utilities;

namespace FullModdedFuriesAPI.Framework.StateTracking.Snapshots
{
    /// <summary>A frozen snapshot of the game state watchers.</summary>
    internal class WatcherSnapshot
    {
        /*********
        ** Accessors
        *********/
        /// <summary>Tracks changes to the window size.</summary>
        public SnapshotDiff<Point> WindowSize { get; } = new SnapshotDiff<Point>();

        /// <summary>Tracks changes to the current player.</summary>
        public PlayerSnapshot CurrentPlayer { get; private set; }


        /// <summary>Tracks changes to the time of day (in 24-hour military format).</summary>
        // public SnapshotDiff<int> Time { get; } = new SnapshotDiff<int>();

        /// <summary>Tracks changes to the save ID.</summary>
        // public SnapshotDiff<ulong> SaveID { get; } = new SnapshotDiff<ulong>();

        /// <summary>Tracks changes to the game's locations.</summary>
        // public WorldLocationsSnapshot Locations { get; } = new WorldLocationsSnapshot();

        /// <summary>Tracks changes to <see cref="Game1.activeClickableMenu"/>.</summary>
        // public SnapshotDiff<IClickableMenu> ActiveMenu { get; } = new SnapshotDiff<IClickableMenu>();

        /// <summary>Tracks changes to the cursor position.</summary>
        public SnapshotDiff<ICursorPosition> Cursor { get; } = new SnapshotDiff<ICursorPosition>();

        /// <summary>Tracks changes to the mouse wheel scroll.</summary>
        public SnapshotDiff<int> MouseWheelScroll { get; } = new SnapshotDiff<int>();

        /// <summary>Tracks changes to the content locale.</summary>
        // public SnapshotDiff<LocalizedContentManager.LanguageCode> Locale { get; } = new SnapshotDiff<LocalizedContentManager.LanguageCode>();


        /*********
        ** Public methods
        *********/
        /// <summary>Update the tracked values.</summary>
        /// <param name="watchers">The watchers to snapshot.</param>
        public void Update(WatcherCore watchers)
        {
            // update player instance
            if (watchers.CurrentPlayerTracker == null)
                this.CurrentPlayer = null;
            else if (watchers.CurrentPlayerTracker.Player != this.CurrentPlayer?.Player)
                this.CurrentPlayer = new PlayerSnapshot(watchers.CurrentPlayerTracker.Player);

            // update snapshots
            // this.WindowSize.Update(watchers.WindowSizeWatcher);
            // this.Locale.Update(watchers.LocaleWatcher);
            this.CurrentPlayer?.Update(watchers.CurrentPlayerTracker);
            // this.Time.Update(watchers.TimeWatcher);
            // this.SaveID.Update(watchers.SaveIdWatcher);
            // this.Locations.Update(watchers.LocationsWatcher);
            // this.ActiveMenu.Update(watchers.ActiveMenuWatcher);
            // this.Cursor.Update(watchers.CursorWatcher);
            // this.MouseWheelScroll.Update(watchers.MouseWheelScrollWatcher);
        }
    }
}
