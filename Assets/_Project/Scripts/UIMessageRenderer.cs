using System;
using RogueProject.Models;
using TMPro;
using UnityEngine;

public class UIMessageRenderer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI UIMessageText;
    [SerializeField] private CanvasGroup UIMessageCanvasGroup;

    [Space]
    [SerializeField] private Easings.EaseType FadeEaseType = Easings.EaseType.Linear;
    [SerializeField] private Easings.EaseType ScaleEaseType = Easings.EaseType.Linear;
    [SerializeField] private float ScaleDuration = 0.5f;
    [SerializeField] private float ScaleAmount = 1.1f;

    private UiMessage _uiMessage;

    private void Awake()
    {
        _uiMessage = UiMessage.Instance;
        _uiMessage.OnMessageChanged += OnMessageChanged;

        UIMessageCanvasGroup.alpha = 0f;
    }

    private void OnMessageChanged(string message)
    {
        _uiMessage.Priority = false;
        UIMessageText.text = message;
        UIMessageText.transform.localScale = Vector3.one * ScaleAmount;
        UIMessageCanvasGroup.alpha = 1f;
    }

    private void Update()
    {
        if (_uiMessage.RemainingDuration <= 0)
        {
            return;
        }

        var t = _uiMessage.RemainingDuration / _uiMessage.MaxDuration;
        UIMessageCanvasGroup.alpha = Mathf.Lerp(0f, 1f, FadeEaseType.Ease(t));

        var scaleT = Mathf.Clamp01((_uiMessage.MaxDuration - _uiMessage.RemainingDuration) / ScaleDuration);
        UIMessageText.transform.localScale = Vector3.one * Mathf.Lerp(ScaleAmount, 1f, ScaleEaseType.Ease(scaleT));

        _uiMessage.RemainingDuration -= Time.deltaTime;
    }
}
