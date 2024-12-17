using UnityEngine;

public class SineScale : MonoBehaviour
{
    [SerializeField] private float ScaleAmount = 1.1f;
    [SerializeField] private float ScaleSpeed = 1f;

    private void Update()
    {
        transform.localScale = Vector3.one * (1 + Mathf.Sin(Time.time * ScaleSpeed) * ScaleAmount);
    }
}
