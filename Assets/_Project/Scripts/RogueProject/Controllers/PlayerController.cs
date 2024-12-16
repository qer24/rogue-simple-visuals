using System;
using RogueProject.Models;
using RogueProject.Models.Entities;
using UnityEngine;
using Logger = RogueProject.Utils.Logger;
using Vector2Int = RogueProject.Utils.Vector2Int;

namespace RogueProject.Controllers
{
    public class PlayerController : Controller
    {
        private Vector2Int _movementDirection;
        private readonly World _world;
        private readonly Player _player;

        private float _lastMoveTime;

        public event Action OnInput;

        public PlayerController(World world, Player player)
        {
            _world = world;
            _player = player;
        }

        /// <summary>
        /// Escape - Exit the game.
        /// R - Reveal the map.
        /// G - Generate a new world.
        /// Arrow keys - Move the player.
        /// </summary>
        private bool GetInput()
        {
            _movementDirection = new Vector2Int(0, 0);
            // var key = Console.ReadKey(true).Key;
            //
            // switch (key)
            // {
            //     case ConsoleKey.Escape:
            //         Environment.Exit(0);
            //         return;
            //     case ConsoleKey.R:
            //     {
            //         // reveal map
            //         foreach (var cell in _world.WorldGrid)
            //         {
            //             var x = cell.Position.x;
            //             var y = cell.Position.y;
            //
            //             _world.WorldGrid[x, y].Revealed = true;
            //         }
            //         return;
            //     }
            //     case ConsoleKey.G:
            //         _world.GenerateWorld(true);
            //         return;
            //     case ConsoleKey.K:
            //         _player.ChangeHealth(-999);
            //         return;
            //     default:
            //         _movementDirection = key switch
            //         {
            //             ConsoleKey.UpArrow    => new Vector2Int(0, -1),
            //             ConsoleKey.LeftArrow  => new Vector2Int(-1, 0),
            //             ConsoleKey.DownArrow  => new Vector2Int(0, 1),
            //             ConsoleKey.RightArrow => new Vector2Int(1, 0),
            //             _                     => new Vector2Int(0, 0)
            //         };
            //
            //         break;
            // }

            // Arrow keys
            if (Input.GetKey(KeyCode.UpArrow) && Time.time - _lastMoveTime > Constants.PLAYER_MOVE_DELAY)
            {
                _movementDirection = new Vector2Int(0, 1);
                _lastMoveTime = Time.time;
            }
            else if (Input.GetKey(KeyCode.LeftArrow) && Time.time - _lastMoveTime > Constants.PLAYER_MOVE_DELAY)
            {
                _movementDirection = new Vector2Int(-1, 0);
                _lastMoveTime = Time.time;
            }
            else if (Input.GetKey(KeyCode.DownArrow) && Time.time - _lastMoveTime > Constants.PLAYER_MOVE_DELAY)
            {
                _movementDirection = new Vector2Int(0, -1);
                _lastMoveTime = Time.time;
            }
            else if (Input.GetKey(KeyCode.RightArrow) && Time.time - _lastMoveTime > Constants.PLAYER_MOVE_DELAY)
            {
                _movementDirection = new Vector2Int(1, 0);
                _lastMoveTime = Time.time;
            }
            // Escape
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
            // R
            else if (Input.GetKeyDown(KeyCode.R))
            {
                // reveal map
                foreach (var cell in _world.WorldGrid)
                {
                    var x = cell.Position.x;
                    var y = cell.Position.y;

                    _world.WorldGrid[x, y].Revealed = true;
                }
            }
            // G
            else if (Input.GetKeyDown(KeyCode.G))
            {
                _world.GenerateWorld(true);
            }
            // K
            else if (Input.GetKeyDown(KeyCode.K))
            {
                _player.ChangeHealth(-999);
            }
            else
            {
                // Don't do anything if no input
                return false;
            }

            return true;
        }

        public override void Update()
        {
            var input = GetInput();

            if (input)
            {
                Move(_movementDirection);
                OnInput?.Invoke();
            }
        }

        public void Move(Vector2Int direction)
        {
            var newPosition = _player.Position + direction;

            // Check if new position is out of bounds or collides with a wall
            if (_world.CollisionCheck(newPosition)) return;

            // Attack enemy if there is one on the cell
            if (_world.EnemyCheck(newPosition))
            {
                AttackEnemy(_world.GetEntityOnCell(newPosition) as Enemy);
                return;
            }

            // Move player if there is no collision or enemy
            _player.Position = newPosition;
            TryPickupItems(_player.Position);
        }

        private void TryPickupItems(Vector2Int position)
        {
            var item = _world.GetItemOnCell(position);
            if (item == null)
            {
                return;
            }

            Logger.Log($"Picked up {item.Name}");

            item.ApplyEffect(_player);
            UiMessage.Instance.ShowMessage(item.PickupMessage, 5);

            _world.Items.Remove(item);
        }

        private void AttackEnemy(Enemy enemy)
        {
            _world.Attack(_player, enemy);
        }
    }
}
