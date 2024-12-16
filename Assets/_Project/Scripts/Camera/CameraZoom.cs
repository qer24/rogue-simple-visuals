using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraZoom : GameBehaviour
{
    [SerializeField] private CinemachineCamera VirtualCamera;
    [SerializeField] private float ZoomedInSize = 12f;
    [SerializeField] private float ZoomTime = 1f;
    [SerializeField] private Easings.EaseType ZoomEaseType = Easings.EaseType.Linear;

    private float? _zoomedOutSize;
    private float _zoomTimer;

    protected override void GameStart()
    {
        _zoomedOutSize ??= VirtualCamera.Lens.OrthographicSize;
        _zoomTimer = 0f;
    }

    protected override void GameUpdate()
    {

    }

    private void Update()
    {
        if (_zoomedOutSize is null || _zoomTimer >= ZoomTime)
        {
            return;
        }

        var t = _zoomTimer / ZoomTime;
        VirtualCamera.Lens.OrthographicSize = Mathf.Lerp(_zoomedOutSize.Value, ZoomedInSize, ZoomEaseType.Ease(t));

        _zoomTimer += Time.deltaTime;
    }
}
