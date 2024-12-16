using System.Collections.Generic;
using System.IO;
using System.Linq;
using RogueProject.Models.Entities;
using UnityEngine;
using Vector2Int = RogueProject.Utils.Vector2Int;

namespace RogueProject.Models
{
    public static class EnemyDatabase
    {
        private static Enemy[] _enemies;

        /// <summary>
        /// All enemy types in the game.
        /// </summary>
        public static Enemy[] Enemies => _enemies ??= LoadEnemies();

        private static Enemy[] LoadEnemies()
        {
            var enemyDataAssets = Resources.LoadAll<TextAsset>("Data/Entities");

            return enemyDataAssets
                   .Where(asset => asset.name != "Player")
                   .Select(asset => new Enemy(asset.name, Vector2Int.zero))
                   .ToArray();
        }
    }
}
