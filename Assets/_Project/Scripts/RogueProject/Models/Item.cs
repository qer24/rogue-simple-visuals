using System;
using System.Collections.Generic;
using RogueProject.Models.Entities;
using RogueProject.Utils;
using SimpleJSON;
using UnityEngine;
using Logger = RogueProject.Utils.Logger;
using Vector2Int = RogueProject.Utils.Vector2Int;

namespace RogueProject.Models
{
    public class Item : IRenderable
    {
        public readonly string Name;
        public Vector2Int Position;

        public char Character { get; private set; }
        public ConsoleColor Color { get; private set; }
        public string PickupMessage;

        private Action<Player>[] _effects;

        public event Action<Item> OnPickup;

        public Item(string name, Vector2Int position)
        {
            Name = name;
            Position = position;

            LoadStats();
        }

        public Item Clone(Vector2Int position)
        {
            return new Item(Name, position);
        }

        public bool IsVisible(World world)
        {
            var cell = world.GetCell(Position);
            return cell is { Visible: true, Revealed: true };
        }

        /// <summary>
        /// Load stats from json file associated with item.
        /// </summary>
        private void LoadStats()
        {
            // Load JSON text
            var jsonString = Resources.Load<TextAsset>($"Data/Items/{Name}").text;

            // Parse JSON using SimpleJSON
            var json = JSON.Parse(jsonString);

            // Main properties
            Character = json["Character"].Value[0];
            Color = (ConsoleColor)int.Parse(json["Color"]);
            PickupMessage = json["Message"];

            // Parse Effects manually
            var effectsNode = json["Effects"];
            _effects = Array.Empty<Action<Player>>();

            if (effectsNode == null)
            {
                Logger.Log("No effects found.");
                return;
            }

            var effectsList = new List<Action<Player>>();

            foreach (var key in effectsNode.Keys)
            {
                var value = effectsNode[key];

                Action<Player> action = key switch
                {
                    "Health"     => player => player.ChangeHealth(value.AsInt),
                    "Strength"   => player => player.Strength += value.AsInt,
                    "Armor"      => player => player.Armor += value.AsInt,
                    "Gold"       => player => player.Gold += value.AsInt,
                    "Experience" => player => player.AddExperience(value.AsInt),
                    _            => null
                };

                if (action != null)
                {
                    effectsList.Add(action);
                }
                else
                {
                    Logger.Log($"Warning: Unknown effect type '{key}' in item {Name}");
                }
            }

            _effects = effectsList.ToArray();
        }


        public void ApplyEffect(Player player)
        {
            Logger.Log(PickupMessage);
            Logger.Log($"Applying effects of {Name} to {player.Name}");

            foreach (var effect in _effects)
            {
                effect(player);
            }

            OnPickup?.Invoke(this);
            OnPickup = null;
        }
    }
}
