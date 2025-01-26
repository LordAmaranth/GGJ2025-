using UnityEngine;
using KanKikuchi.AudioManager;

public class Ufo : MonoBehaviour
{
    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private float destroyWidth = 20f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SEManager.Instance.Play(
            audioPath       : SEPath.BLOW_BUBBLE_LOOP1, //再生したいオーディオのパス
            volumeRate      : 0.5f,                //音量の倍率
            delay           : 0,                //再生されるまでの遅延時間
            pitch           : 1.33f,                //ピッチ
            isLoop          : true,             //ループ再生するか
            callback        : null              //再生終了後の処理
        );
    }

    // Update is called once per frame
    void Update()
    {
        // 横方向に移動
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // 一定の横方向の限界を超えたらDestroy
        if (Mathf.Abs(transform.position.x) > destroyWidth) {
            SEManager.Instance.Stop(SEPath.BLOW_BUBBLE_LOOP1);
            Destroy(gameObject);
        }
    }
}
