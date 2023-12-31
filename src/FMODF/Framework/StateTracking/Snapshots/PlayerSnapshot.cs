using System;
using System.Collections.Generic;
using System.Linq;
using FullModdedFuriesAPI.Enums;
using FullModdedFuriesAPI.Events;
using Brawler2D;

namespace FullModdedFuriesAPI.Framework.StateTracking.Snapshots
{
    /// <summary>A frozen snapshot of a tracked player.</summary>
    internal class PlayerSnapshot
    {
        /*********
        ** Fields
        *********/
        /// <summary>An empty item list diff.</summary>
        // private readonly SnapshotItemListDiff EmptyItemListDiff = new SnapshotItemListDiff(new Item[0], new Item[0], new ItemStackSizeChange[0]);


        /*********
        ** Accessors
        *********/
        /// <summary>The player being tracked.</summary>
        public PlayerObj Player { get; }

        public SnapshotDiff<ClassType> ClassType = new();
        public SnapshotDiff<PlayerClassObj> ClassObj = new();

        public string Location { get; }

        /// <summary>The player's current location.</summary>
        // public SnapshotDiff<GameLocation> Location { get; } = new SnapshotDiff<GameLocation>();

        /// <summary>Tracks changes to the player's skill levels.</summary>
        // public IDictionary<SkillType, SnapshotDiff<int>> Skills { get; } =
        //     Enum
        //         .GetValues(typeof(SkillType))
        //         .Cast<SkillType>()
        //         .ToDictionary(skill => skill, skill => new SnapshotDiff<int>());

        /// <summary>Get a list of inventory changes.</summary>
        // public SnapshotItemListDiff Inventory { get; private set; }


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="player">The player being tracked.</param>
        public PlayerSnapshot(PlayerObj player)
        {
            this.Player = player;
        }

        /// <summary>Update the tracked values.</summary>
        /// <param name="watcher">The player watcher to snapshot.</param>
        public void Update(PlayerTracker watcher)
        {
            this.ClassType.Update(watcher.ClassTypeWatcher);
            this.ClassObj.Update(watcher.ClassObjWatcher);
            // this.Location.Update(watcher.LocationWatcher);
            // foreach (var pair in this.Skills)
            // pair.Value.Update(watcher.SkillWatchers[pair.Key]);

            // this.Inventory = watcher.TryGetInventoryChanges(out SnapshotItemListDiff itemChanges)
            // ? itemChanges
            // : this.EmptyItemListDiff;
        }
    }
}
