using System.Collections.Generic;

namespace FullModdedFuriesAPI.Framework.StateTracking
{
    /// <summary>A watcher which tracks changes to a dictionary.</summary>
    internal interface IDictionaryWatcher<TKey, TValue> : ICollectionWatcher<KeyValuePair<TKey, TValue>> { }
}
