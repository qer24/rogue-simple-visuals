using System.IO;
using System.Linq;
using UnityEngine;
using Vector2Int = RogueProject.Utils.Vector2Int;

namespace RogueProject.Models
{
    public static class ItemDatabase
    {
        private static Item[] _items;

        /// <summary>
        /// All item types in the game.
        /// </summary>
        public static Item[] Items => _items ??= LoadItems();

        private static Item[] LoadItems()
        {
            var itemDataAssets = Resources.LoadAll<TextAsset>("Data/Items");

            return itemDataAssets.Select(asset => new Item(asset.name, Vector2Int.zero))
                                 .ToArray();
        }
    }
}
