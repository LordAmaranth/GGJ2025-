using UnityEngine;

public class Bubble : MonoBehaviour
{
    private Animator animator;

    public float upSpeed = 0.1f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 上方向に移動
        transform.Translate(Vector3.up * upSpeed * Time.deltaTime);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
        // 接触点のワールド座標を取得
            Vector2 contactPoint = collision.GetContact(0).point;

            // 接触点をローカル座標に変換
            Vector2 localContactPoint = transform.InverseTransformPoint(contactPoint);

            // 接触位置に応じてセクターを判定し、アニメーションのトリガーを設定
            SetDeformAnimation(localContactPoint);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // シャボン玉を膨らます時の当たり判定
        // if (other.gameObject.CompareTag("Air"))
        // {
            
        // }

        // シャボン玉をプスする時のの当たり判定
        // if (other.gameObject.CompareTag("Attack"))
        // {
            
        // }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SetDeformAnimationExit();
        }
    }


    private void SetDeformAnimation(Vector2 localContactPoint)
    {
        // 接触点からの角度を計算
        float angle = Mathf.Atan2(localContactPoint.y, localContactPoint.x) * Mathf.Rad2Deg;

        // 角度を0～360度の範囲に正規化
        if (angle < 0)
            angle += 360;

        // セクターの中心角度（10度）に基づいてセクター番号を計算
        int sector = Mathf.FloorToInt(angle / 10f);

        // 対応するアニメーショントリガーを設定
        if(sector >= 12){
            sector = 2;
        }
        else{
            sector = 1;
        }

        animator.SetTrigger(sector.ToString());
        Debug.Log("DeformSector" + sector);
    }

    private void SetDeformAnimationExit()
    {
        animator.SetTrigger("0");
        Debug.Log("0");
    }
}
