using System;
using RogueProject.Controllers;
using RogueProject.Models;
using RogueProject.Models.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool EndGame { get; private set; }
    public WorldController WorldController { get; private set; }
    public PlayerController PlayerController { get; private set; }

    public event Action OnInitialize;
    public event Action OnUpdate;
    public event Action OnRestart;

    private void Init()
    {
        var world = new World();
        WorldController = new WorldController(world);

        world.OnWorldGenerated += (regenerate) =>
        {
            if (regenerate)
            {
                OnRestart?.Invoke();
            }
        };

        var player = world.Entities[0] as Player;
        PlayerController = new PlayerController(world, player);

        OutroUI.Level = 1;
        OutroUI.Gold = 0;

        EndGame = false;

        OnInitialize?.Invoke();
    }

    private void Start()
    {
        Init();

        void GameUpdate()
        {
            WorldController.Update();

            if (WorldController.PlayerDead)
            {
                EndGame = true;
            }

            if (EndGame)
            {
                OutroUI.Level = WorldController.World.Player.Level;
                OutroUI.Gold = WorldController.World.Player.Gold;

                // Load next scene
                SceneManager.LoadScene(2);
            }
            else
            {
                OnUpdate?.Invoke();
            }
        }

        // force first update before players makes any input
        GameUpdate();

        // Can't use this while loop because unity doesn't block the main thread while waiting for input
        // while (!_endGame)
        // {
        //     Update();
        // }

        // Instead we use an event to update the game
        PlayerController.OnInput += () =>
        {
            if (EndGame) return;
            GameUpdate();
        };
    }

    private void Update()
    {
        PlayerController.Update();
    }
}
