using System;
using System.Collections.Generic;
using System.Linq;
using FullModdedFuriesAPI.Enums;
using FullModdedFuriesAPI.Framework.StateTracking.Comparers;
using FullModdedFuriesAPI.Framework.StateTracking.FieldWatchers;
using Brawler2D;

namespace FullModdedFuriesAPI.Framework.StateTracking
{
    /// <summary>Tracks changes to a player's data.</summary>
    internal class PlayerTracker : IDisposable
    {
        /*********
        ** Fields
        *********/
        /// <summary>The player's inventory as of the last reset.</summary>
        // private IDictionary<Item, int> PreviousInventory;

        /// <summary>The player's inventory change as of the last update.</summary>
        // private IDictionary<Item, int> CurrentInventory;

        /// <summary>The player's last valid location.</summary>
        // private GameLocation LastValidLocation;

        /// <summary>The underlying watchers.</summary>
        private readonly List<IWatcher> Watchers = new List<IWatcher>();


        /*********
        ** Accessors
        *********/
        /// <summary>The player being tracked.</summary>
        public PlayerObj Player { get; }

        public IValueWatcher<ClassType> ClassTypeWatcher { get; }
        public IValueWatcher<PlayerClassObj> ClassObjWatcher { get; }

        public IValueWatcher<string> LocationWatcher { get; }
        /// <summary>The player's current location.</summary>
        // public IValueWatcher<GameLocation> LocationWatcher { get; }

        /// <summary>Tracks changes to the player's skill levels.</summary>
        public IDictionary<SkillType, IValueWatcher<int>> SkillWatchers { get; }


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="player">The player to track.</param>
        public PlayerTracker(PlayerObj player)
        {
            // init player data
            this.Player = player;
            // this.PreviousInventory = this.GetInventory();

            // init trackers
            this.ClassObjWatcher = WatcherFactory.ForReference(() => player.currentPlayerClass);
            this.ClassTypeWatcher = WatcherFactory.ForGenericEquality(() => player.currentClassType);
            this.LocationWatcher = WatcherFactory.ForEquatable(() => player.currentStage);
            // this.LocationWatcher = WatcherFactory.ForReference(this.GetCurrentLocation);
            // this.SkillWatchers = new Dictionary<SkillType, IValueWatcher<int>>
            // {
            //     [SkillType.Combat] = WatcherFactory.ForNetValue(player.combatLevel),
            //     [SkillType.Farming] = WatcherFactory.ForNetValue(player.farmingLevel),
            //     [SkillType.Fishing] = WatcherFactory.ForNetValue(player.fishingLevel),
            //     [SkillType.Foraging] = WatcherFactory.ForNetValue(player.foragingLevel),
            //     [SkillType.Luck] = WatcherFactory.ForNetValue(player.luckLevel),
            //     [SkillType.Mining] = WatcherFactory.ForNetValue(player.miningLevel)
            // };

            // track watchers for convenience
            this.Watchers.Add(this.ClassObjWatcher);
            this.Watchers.Add(this.ClassTypeWatcher);
            this.Watchers.Add(this.LocationWatcher);
            // this.Watchers.AddRange(this.SkillWatchers.Values);
        }

        /// <summary>Update the current values if needed.</summary>
        public void Update()
        {
            // update valid location
            // this.LastValidLocation = this.GetCurrentLocation();

            // update watchers
            foreach (IWatcher watcher in this.Watchers)
                watcher.Update();

            // update inventory
            // this.CurrentInventory = this.GetInventory();
        }

        /// <summary>Reset all trackers so their current values are the baseline.</summary>
        public void Reset()
        {
            foreach (IWatcher watcher in this.Watchers)
                watcher.Reset();

            // this.PreviousInventory = this.CurrentInventory;
        }

        /// <summary>Get the player's current location, ignoring temporary null values.</summary>
        /// <remarks>The game will set <see cref="Character.currentLocation"/> to null in some cases, e.g. when they're a secondary player in multiplayer and transition to a location that hasn't been synced yet. While that's happening, this returns the player's last valid location instead.</remarks>
        // public GameLocation GetCurrentLocation()
        // {
            // return this.Player.currentLocation ?? this.LastValidLocation;
        // }

        /// <summary>Get the inventory changes since the last update, if anything changed.</summary>
        /// <param name="changes">The inventory changes, or <c>null</c> if nothing changed.</param>
        /// <returns>Returns whether anything changed.</returns>
        // public bool TryGetInventoryChanges(out SnapshotItemListDiff changes)
        // {
        //     IDictionary<Item, int> current = this.GetInventory();
        //
        //     ISet<Item> added = new HashSet<Item>(new ObjectReferenceComparer<Item>());
        //     ISet<Item> removed = new HashSet<Item>(new ObjectReferenceComparer<Item>());
        //     foreach (Item item in this.PreviousInventory.Keys.Union(current.Keys))
        //     {
        //         if (!this.PreviousInventory.ContainsKey(item))
        //             added.Add(item);
        //         else if (!current.ContainsKey(item))
        //             removed.Add(item);
        //     }
        //
        //     return SnapshotItemListDiff.TryGetChanges(added: added, removed: removed, stackSizes: this.PreviousInventory, out changes);
        // }

        /// <summary>Release watchers and resources.</summary>
        public void Dispose()
        {
            // this.PreviousInventory.Clear();
            // this.CurrentInventory?.Clear();

            foreach (IWatcher watcher in this.Watchers)
                watcher.Dispose();
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Get the player's current inventory.</summary>
        // private IDictionary<Item, int> GetInventory()
        // {
        //     return this.Player.Items
        //         .Where(n => n != null)
        //         .Distinct()
        //         .ToDictionary(n => n, n => n.Stack);
        // }
    }
}
