using System.Collections.Generic;
using RogueProject.Models;
using RogueProject.Utils;

namespace RogueProject.Controllers
{
    public class WorldController : Controller
    {
        public readonly World World;
        public readonly HashSet<Vector2Int> VisibleCells = new();

        public bool PlayerDead;

        public WorldController(World world)
        {
            World = world;
            World.GenerateWorld(false);
        }

        public override void Update()
        {
            PlayerLineOfSight();
            PlayerReveal();
            PlayerStairs();
            PlayerDeath();
        }

        /// <summary>
        /// Hides or reveals floor tiles based on player's line of sight.
        /// </summary>
        private void PlayerLineOfSight()
        {
            var player = World.Entities[0];

            // hide all visible cells
            foreach (var cell in VisibleCells)
            {
                var x = cell.x;
                var y = cell.y;

                World.WorldGrid[x, y].Visible = false;
            }

            // add cells within player's vision to visible cells
            VisibleCells.Clear();

            const int playerVision = Constants.FLOOR_REVEAL_DISTANCE;

            for (int x = player.Position.x - playerVision; x < player.Position.x + playerVision; x++)
            {
                for (int y = player.Position.y - playerVision; y < player.Position.y + playerVision; y++)
                {
                    if (x < 0 || x >= Constants.WORLD_SIZE.x || y < 0 || y >= Constants.WORLD_SIZE.y) continue;

                    var cell = World.WorldGrid[x, y];
                    if (cell.TileType != TileType.Floor) continue;

                    if (!FastDistanceCheck(player.Position, cell.Position, playerVision)) continue;
                    if (World.Linecast(player.Position, cell.Position)) continue;

                    VisibleCells.Add(cell.Position);
                    World.WorldGrid[x, y].Visible = true;
                }
            }
        }

        /// <summary>
        /// Avoids a sqrt operation for distance check.
        /// </summary>
        private static bool FastDistanceCheck(Vector2Int a, Vector2Int b, int distance)
        {
            var dx = a.x - b.x;
            var dy = a.y - b.y;

            return dx * dx + dy * dy < distance * distance;
        }

        /// <summary>
        /// Reveal surrounding tiles or rooms based on player's position.
        /// </summary>
        private void PlayerReveal()
        {
            // reveal surrounding tiles
            foreach (var cell in World.GetPlayerSurroundedTiles())
            {
                var worldCell = World.WorldGrid[cell.x, cell.y];
                if (worldCell.TileType == TileType.Corridor) // reveal corridors
                {
                    World.WorldGrid[cell.x, cell.y].Revealed = true;
                }
                else if (worldCell.TileType == TileType.Door) // reveal rooms
                {
                    if (World.TryGetRoom(worldCell, out var room))
                    {
                        World.RevealRoom(room);
                    }
                }
            }
        }

        /// <summary>
        /// Check if player is on stairs and generate a new world if that's the case.
        /// </summary>
        private void PlayerStairs()
        {
            var playerCell = World.GetPlayercell();

            if (playerCell.TileType != TileType.Stairs) return;

            World.GenerateWorld(true);
            Update();
        }

        /// <summary>
        /// Check if player is dead and set PlayerDead to true.
        /// </summary>
        private void PlayerDeath()
        {
            var player = World.Entities[0];

            if (player.Health <= 0)
            {
                PlayerDead = true;
                Logger.Log("Player has died!");
            }
        }
    }
}
