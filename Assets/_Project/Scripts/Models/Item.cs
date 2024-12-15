using System;
using System.Collections.Generic;
using RogueProject.Models.Entities;
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

        [Serializable]
        public class ItemData
        {
            public string Character;
            public string Color;
            public string Message;
            public List<EffectData> Effects;
        }

        [Serializable]
        public class EffectData
        {
            public string Key;
            public int Value;
        }

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
            //var jsonString = File.ReadAllText($"Data/Items/{Name}.json");
            var jsonString = Resources.Load<TextAsset>($"Data/Items/{Name}").text;
            var itemData = JsonUtility.FromJson<ItemData>(jsonString);

            Character = itemData.Character[0];
            Color = (ConsoleColor)int.Parse(itemData.Color);

            PickupMessage = itemData.Message;

            _effects = Array.Empty<Action<Player>>();

            if (itemData.Effects == null)
            {
                return;
            }

            var effectsList = new List<Action<Player>>();

            foreach (var effect in itemData.Effects)
            {
                Action<Player> action = effect.Key switch
                {
                    "Health"     => player => player.ChangeHealth(effect.Value),
                    "Strength"   => player => player.Strength += effect.Value,
                    "Armor"      => player => player.Armor += effect.Value,
                    "Gold"       => player => player.Gold += effect.Value,
                    "Experience" => player => player.AddExperience(effect.Value),
                    _            => null
                };

                if (action != null)
                {
                    effectsList.Add(action);
                }
                else
                {
                    Logger.Log($"Warning: Unknown effect type '{effect.Key}' in item {Name}");
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
        }
    }
}
