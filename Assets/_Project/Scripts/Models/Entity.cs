﻿using System;
using System.Collections.Generic;
using RogueProject.Utils;
using UnityEngine;
using Vector2Int = RogueProject.Utils.Vector2Int;

namespace RogueProject.Models
{
    public abstract class Entity : IRenderable
    {
        public readonly string Name;
        public Vector2Int Position;

        public int Health { get; protected set; } = -1;
        public int MaxHealth = -1;

        public int Strength = -1;
        public int Armor = -1;

        public char Character { get; protected set; }
        public ConsoleColor Color { get; protected set; }

        public virtual bool IsVisible(World world)
        {
            var cell = world.GetCell(Position);
            return cell is { Visible: true, Revealed: true };
        }

        protected Entity(string name, Vector2Int position)
        {
            Name = name;
            Position = position;

            LoadStats();
        }

        /// <summary>
        /// Load stats from json file associated with entity.
        /// </summary>
        protected virtual void LoadStats()
        {
            //var jsonString = File.ReadAllText($"Data/Entities/{Name}.json");
            var jsonString = Resources.Load<TextAsset>($"Data/Entities/{Name}").text;
            var json = JsonUtility.FromJson<Dictionary<string, string>>(jsonString);

            MaxHealth = json["MaxHealth"].ToInt();
            Health = MaxHealth;

            Strength = json["Strength"].ToInt();
            Armor = json["Armor"].ToInt();

            Character = json["Character"][0];
            Color = (ConsoleColor)json["Color"].ToInt();
        }

        public void ChangeHealth(int amount)
        {
            Health += amount;
            Health = Math.Clamp(Health, 0, MaxHealth);
        }
    }
}
