using UnityEngine;

[CreateAssetMenu(fileName = "BubbleSettings", menuName = "ScriptableObjects/BubbleSettings", order = 1)]
public class BubbleSettings : ScriptableObject {
    public float maxScale = 2f; // 最大スケール
    public float startMaxScale = 2f; // 最初の最大スケール
    public float startMinScale = 1f; // 最初の最小スケール
    public float scaleIncreaseSpeed = 1f; // スケールの増加速度
    public float upSpeed = 0.1f; // 上昇速度
    public float WindStrength = 1;
    public float destroyHeight = 50f; // 達したらDestroyされる高さ
    public float destroyWidth = 100f; // 達したらDestroyされる幅
}