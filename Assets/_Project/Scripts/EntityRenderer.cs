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

    private TextMeshPro _playerRenderer;
    private Vector3 _targetPosition;

    protected override void GameStart()
    {
        // delete all existing entity renderers
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

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
        }
    }

    protected override void GameUpdate()
    {
        _targetPosition = TilemapRenderer.GetPosition(World.Player.Position);
    }

    private void Update()
    {
        if (_playerRenderer is null)
        {
            return;
        }

        _playerRenderer.transform.position = Vector3.Lerp(_playerRenderer.transform.position, _targetPosition, PlayerMoveSpeed * Time.deltaTime);
    }
}
