using System.Collections; 
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    [SerializeField]
    // 生成するオブジェクトのプレハブ
    public GameObject prefab;

    // ランダムなY座標の範囲
    public float minY = -5f;
    public float maxY = 5f;

    // 生成するX座標
    public float minX = 0f;
    public float maxX = 0f;

    // ランダムな間隔の範囲（秒）
    public float minInterval = 1f;
    public float maxInterval = 5f;

    void Start()
    {
        // ランダムな間隔でオブジェクトを生成するコルーチンを開始
        StartCoroutine(SpawnObjectsRandomly());
    }

    IEnumerator SpawnObjectsRandomly()
    {
        while (true)
        {
            // ランダムなX,Y座標を計算
            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, maxY);
            
            // 生成位置
            Vector3 spawnPosition = new Vector3(randomX, randomY, 0f);

            // オブジェクトを生成
            Instantiate(prefab, spawnPosition, Quaternion.identity);

            // 次の生成までのランダムな時間を計算
            float randomInterval = Random.Range(minInterval, maxInterval);

            // 次の生成まで待機
            yield return new WaitForSeconds(randomInterval);
        }
    }
}
