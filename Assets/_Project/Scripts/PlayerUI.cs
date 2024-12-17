using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : GameBehaviour
{
    [SerializeField] private Slider HealthSlider;
    [SerializeField] private TextMeshProUGUI HealthText;

    private int _currentPlayerHp;

    protected override void GameStart()
    {
        UpdatePlayerHealth(Player.Health);
    }

    protected override void GameUpdate()
    {
        if (_currentPlayerHp != Player.Health)
        {
            UpdatePlayerHealth(Player.Health);
        }
    }

    private void UpdatePlayerHealth(int newHealth)
    {
        _currentPlayerHp = newHealth;
        HealthSlider.value = _currentPlayerHp / (float)Player.MaxHealth;
        HealthText.text = $"HP: {_currentPlayerHp}/{Player.MaxHealth}";
    }
}
