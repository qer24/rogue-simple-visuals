using TMPro;
using UnityEngine;

public class OutroUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI LevelText;
    [SerializeField] private TextMeshProUGUI GoldText;

    public static int Level = 1;
    public static int Gold = 0;

    private void Start()
    {
        LevelText.text = $"Level Reached: {Level}";
        GoldText.text = $"Gold Collected: {Gold}";
    }
}
