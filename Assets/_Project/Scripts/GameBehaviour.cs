using System;
using System.Collections.Generic;
using RogueProject.Models;
using RogueProject.Models.Entities;
using UnityEngine;

public abstract class GameBehaviour : MonoBehaviour
{
    [SerializeField] private GameManager GameManager;

    protected World World => GameManager.WorldController.World;
    protected WorldCell[,] WorldGrid => World.WorldGrid;
    protected List<Entity> Entities => World.Entities;
    protected List<Item> Items => World.Items;

    protected Player Player => World.Player;

    private void Awake()
    {
        GameManager.OnInitialize += GameStart;
        GameManager.OnUpdate += GameUpdate;
        GameManager.OnRestart += GameRestart;
    }

    protected virtual void GameStart() { }
    protected virtual void GameRestart() { }

    protected abstract void GameUpdate();
}
