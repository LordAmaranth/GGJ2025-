using UnityEngine;

public class BubbleSpawnerStart : MonoBehaviour {
    [SerializeField] private GameObject bubblePrefab; // Bubbleのプレハブ
    [SerializeField] private int spawnCount = 10; // 生成数
    [SerializeField] private float spawnRangeX = 5.0f; // X方向のランダム生成範囲
    [SerializeField] private float spawnHeight = -5.0f; // 生成位置のY座標
    [SerializeField] private BubbleSettings bubbleSettings; // BubbleSettingsを適用

    private float timer;

    void OnEnable() {
        for (int i = 0; i < spawnCount; i++) {
            SpawnBubble();
        }
    }

    private void SpawnBubble() {
        // ランダムなX座標を生成
        float randomX = Random.Range(-spawnRangeX, spawnRangeX);

        // 生成位置を決定
        Vector3 spawnPosition = new Vector3(randomX, spawnHeight, 0);

        // Bubbleを生成
        GameObject newBubble = Instantiate(bubblePrefab, spawnPosition, Quaternion.identity);

        // BubbleSettingsを適用
        Bubble bubbleComponent = newBubble.GetComponent<Bubble>();
        if (bubbleComponent != null) {
            bubbleComponent.settings = bubbleSettings;
        }
    }
}
