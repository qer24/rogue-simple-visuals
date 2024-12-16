using System.Collections.Generic;
using RogueProject.Models;
using RogueProject.Models.Entities;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;

public class EntityRenderer : GameBehaviour
{
    [SerializeField] private TextMeshPro EntityRendererPrefab;
    [SerializeField] private TilemapRenderer TilemapRenderer;

    [Space]
    [SerializeField] private CinemachineCamera VirtualCamera;
    [SerializeField] private float PlayerMoveSpeed;
    [SerializeField] private float PlayerMoveScale = 1.1f;

    private TextMeshPro _playerRenderer;
    private Vector3 _targetPosition;

    private struct RendererObject
    {
        public GameObject GameObject;
        public IRenderable Entity;

        public RendererObject(GameObject gameObject, IRenderable entity)
        {
            GameObject = gameObject;
            Entity = entity;
        }
    }

    private readonly List<RendererObject> _rendererObjects = new();

    protected override void GameRestart()
    {
        GameStart();
    }

    protected override void GameStart()
    {
        // delete all existing entity renderers
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        _rendererObjects.Clear();

        foreach (var entity in Entities)
        {
            var entityRenderer = Instantiate(EntityRendererPrefab, transform);
            entityRenderer.text = entity.Character.ToString();
            entityRenderer.color = ColorConverter.ToUnityColor(entity.Color);
            entityRenderer.transform.position = TilemapRenderer.GetPosition(entity.Position);

            if (entity is Player)
            {
                entityRenderer.name = "Player";
                _playerRenderer = entityRenderer;

                VirtualCamera.Follow = _playerRenderer.transform;
            }
            else
            {
                _rendererObjects.Add(new RendererObject(entityRenderer.gameObject, entity));

                entity.OnDeath += e =>
                {
                    _rendererObjects.RemoveAll(rend => rend.Entity == e);
                    Destroy(entityRenderer.gameObject);
                };
            }
        }

        foreach (var item in Items)
        {
            var itemRenderer = Instantiate(EntityRendererPrefab, transform);
            itemRenderer.text = item.Character.ToString();
            itemRenderer.color = ColorConverter.ToUnityColor(item.Color);
            itemRenderer.transform.position = TilemapRenderer.GetPosition(item.Position);

            _rendererObjects.Add(new RendererObject(itemRenderer.gameObject, item));

            item.OnPickup += i =>
            {
                _rendererObjects.RemoveAll(rend => rend.Entity == i);
                Destroy(itemRenderer.gameObject);
            };
        }
    }

    protected override void GameUpdate()
    {
        _targetPosition = TilemapRenderer.GetPosition(World.Player.Position);
        _playerRenderer.transform.localScale = Vector3.one * PlayerMoveScale;

        foreach (var rend in _rendererObjects)
        {
            rend.GameObject.SetActive(rend.Entity.IsVisible(World));
        }
    }

    private void Update()
    {
        if (_playerRenderer is null)
        {
            return;
        }

        _playerRenderer.transform.position = Vector3.Lerp(_playerRenderer.transform.position, _targetPosition, PlayerMoveSpeed * Time.deltaTime);
        _playerRenderer.transform.localScale = Vector3.Lerp(_playerRenderer.transform.localScale, Vector3.one, PlayerMoveSpeed * Time.deltaTime);
    }
}
