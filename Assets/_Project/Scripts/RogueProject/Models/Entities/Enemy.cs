using System;
using System.Collections.Generic;
using RogueProject.Utils;
using UnityEngine;
using Vector2Int = RogueProject.Utils.Vector2Int;

namespace RogueProject.Models.Entities
{
    public class Enemy : Entity
    {
        [Serializable]
        private class EnemyStats
        {
            public int Experience;
            public int Gold;
        }

        public Enemy(string name, Vector2Int position) : base(name, position) { }

        public int Experience { get; private set; }
        public int Gold { get; private set; }

        public Enemy Clone(Vector2Int position)
        {
            return new Enemy(Name, position)
            {
                Health = Health,
                MaxHealth = MaxHealth,
                Strength = Strength,
                Armor = Armor,
                Character = Character,
                Color = Color
            };
        }

        protected override void LoadStats()
        {
            base.LoadStats();

            //var jsonString = File.ReadAllText($"Data/Entities/{Name}.json");
            var jsonString = Resources.Load<TextAsset>($"Data/Entities/{Name}").text;
            var json = JsonUtility.FromJson<EnemyStats>(jsonString);

            Experience = json.Experience;
            Gold = json.Gold;
        }

        public void ScaleStats(int floorNumber)
        {
            var scaleFactor = 1 + floorNumber * 0.2f;

            MaxHealth = (int)(MaxHealth * scaleFactor);
            Health = MaxHealth;

            Strength = (int)(Strength * scaleFactor);
            Armor = (int)(Armor * scaleFactor);

            Experience = (int)(Experience * scaleFactor);
            Gold = (int)(Gold * scaleFactor);
        }
    }
}
