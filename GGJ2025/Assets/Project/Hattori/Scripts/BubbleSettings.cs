using UnityEngine;

[CreateAssetMenu(fileName = "BubbleSettings", menuName = "ScriptableObjects/BubbleSettings", order = 1)]
public class BubbleSettings : ScriptableObject
{
    public float maxScale = 2f; // 最大スケール
    public float scaleIncreaseSpeed = 1f; // スケールの増加速度
    public float upSpeed = 0.1f; // 上昇速度
}