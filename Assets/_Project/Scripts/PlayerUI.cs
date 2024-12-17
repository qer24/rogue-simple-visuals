using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : GameBehaviour
{
    [SerializeField] private Slider HealthSlider;
    [SerializeField] private TextMeshProUGUI HealthText;

    [Space]
    [SerializeField] private Slider ExperienceSlider;
    [SerializeField] private TextMeshProUGUI ExperienceText;

    [Space]
    [SerializeField] private TextMeshProUGUI GoldText;
    [SerializeField] private TextMeshProUGUI StrengthText;
    [SerializeField] private TextMeshProUGUI ArmorText;

    private int _currentPlayerHp;
    private int _currentPlayerExp;
    private int _currentPlayerGold;
    private int _currentPlayerStrength;
    private int _currentPlayerArmor;

    protected override void GameStart()
    {
        UpdatePlayerHealth(Player.Health);
        UpdatePlayerExperience(Player.Experience);
        UpdateStats(Player.Gold, Player.Strength, Player.Armor);
    }

    protected override void GameUpdate()
    {
        if (_currentPlayerHp != Player.Health)
        {
            UpdatePlayerHealth(Player.Health);
        }

        if (_currentPlayerExp != Player.Experience)
        {
            UpdatePlayerExperience(Player.Experience);
        }

        if (_currentPlayerGold != Player.Gold || _currentPlayerStrength != Player.Strength || _currentPlayerArmor != Player.Armor)
        {
            UpdateStats(Player.Gold, Player.Strength, Player.Armor);
        }
    }

    private void UpdatePlayerHealth(int newHealth)
    {
        _currentPlayerHp = newHealth;
        HealthSlider.value = _currentPlayerHp / (float)Player.MaxHealth;
        HealthText.text = $"HP: {_currentPlayerHp}/{Player.MaxHealth}";
    }

    private void UpdatePlayerExperience(int newExperience)
    {
        _currentPlayerExp = newExperience;
        ExperienceSlider.value = _currentPlayerExp / (float)Player.ExperienceToNextLevel;
        ExperienceText.text = $"Level: {Player.Level}";
    }

    private void UpdateStats(int newGold, int newStrength, int newArmor)
    {
        _currentPlayerGold = newGold;
        _currentPlayerStrength = newStrength;
        _currentPlayerArmor = newArmor;

        GoldText.text = $"{Player.Gold}";
        StrengthText.text = $"{Player.Strength}";
        ArmorText.text = $"{Player.Armor}";
    }
}
