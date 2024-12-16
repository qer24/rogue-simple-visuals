using System;
using System.Collections.Generic;
using RogueProject.Models;
using UnityEngine;

public abstract class GameBehaviour : MonoBehaviour
{
    [SerializeField] private GameManager GameManager;

    protected World World => GameManager.WorldController.World;
    protected WorldCell[,] WorldGrid => World.WorldGrid;
    protected List<Entity> Entities => World.Entities;
    protected List<Item> Items => World.Items;

    private void Awake()
    {
        GameManager.OnUpdate += GameUpdate;
    }

    protected abstract void GameUpdate();
}