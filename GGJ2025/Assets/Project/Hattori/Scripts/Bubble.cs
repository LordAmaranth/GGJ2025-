using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour {
    private Animator animator;
    private bool isAirColliding = false;
    private bool isHit = false;
    private Vector3 originalScale;
    [SerializeField] private Rigidbody2D myRigidBody;
    private List<Collider2D> windSources = new();

    [SerializeField]
    public BubbleSettings settings;

    void Start() {
        animator = GetComponent<Animator>();
        originalScale = transform.localScale;
    }

    void Update() {
        // 上方向に移動
        transform.Translate(Vector3.up * settings.upSpeed * Time.deltaTime);

        // Airが当たっている間、スケールを大きくする
        if (isAirColliding && transform.localScale.x < settings.maxScale) {
            transform.localScale += Vector3.one * settings.scaleIncreaseSpeed * Time.deltaTime;
        }

        // 最大スケールを超えないように制限
        if (transform.localScale.x > settings.maxScale) {
            transform.localScale = Vector3.one * settings.maxScale;
        }


        for (int i = windSources.Count - 1; i >= 0; i--) {
            Collider2D windSource = windSources[i];
            if (windSource.enabled) {
                int directionMultiplier = 0;
                if (windSource.transform.position.x < transform.position.x) {
                    directionMultiplier = 1;
                }
                else if (windSource.transform.position.x > transform.position.x) {
                    directionMultiplier = -1;
                }
                myRigidBody.AddForceX(settings.WindStrength * directionMultiplier);
            }
            else {
                windSources.Remove(windSource);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            // 接触点のワールド座標を取得
            Vector2 contactPoint = collision.GetContact(0).point;

            // 接触点をローカル座標に変換
            Vector2 localContactPoint = transform.InverseTransformPoint(contactPoint);

            // 接触位置に応じてセクターを判定し、アニメーションのトリガーを設定
            SetDeformAnimation(localContactPoint);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // シャボン玉を膨らます時の当たり判定
        if (other.gameObject.CompareTag("Straw")) {
            isAirColliding = true;
            animator.SetTrigger("IsAir");
        }

        // シャボン玉をプスする時の当たり判定
        if (other.gameObject.CompareTag("Weapon")) {
            Destroy(gameObject);
        }

        if (other.CompareTag("Air") && !windSources.Contains(other)) {
            windSources.Add(other);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            SetDeformAnimationExit();
            isAirColliding = false;
        }
        // シャボン玉を膨らます時の当たり判定
        if (other.gameObject.CompareTag("Straw")) {
            isAirColliding = false;
            animator.SetTrigger("Idle");
        }

        if (other.CompareTag("Air") && windSources.Contains(other)) {
            windSources.Remove(other);
        }
    }

    private void SetDeformAnimation(Vector2 localContactPoint) {
        // 接触点からの角度を計算
        float angle = Mathf.Atan2(localContactPoint.y, localContactPoint.x) * Mathf.Rad2Deg;

        // 角度を0～360度の範囲に正規化
        if (angle < 0)
            angle += 360;

        // セクターの中心角度（10度）に基づいてセクター番号を計算
        int sector = Mathf.FloorToInt(angle / 10f);
        Debug.Log("DeformSector" + localContactPoint.x);

        // 対応するアニメーショントリガーを設定
        // if (sector >= 11)
        // {
        //     sector = 2;
        // }
        // else
        // {
        //     sector = 1;
        // }
        sector = 1;

        animator.SetTrigger("Hit" + sector.ToString());
        // Debug.Log("DeformSector" + sector);
    }

    private void SetDeformAnimationExit() {
        animator.SetTrigger("Idle");
    }
}
