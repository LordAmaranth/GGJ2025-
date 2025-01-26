using UnityEngine;

public class Ufo : MonoBehaviour
{
    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private float destroyWidth = 20f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 横方向に移動
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // 一定の横方向の限界を超えたらDestroy
        if (Mathf.Abs(transform.position.x) > destroyWidth) {
            Destroy(gameObject);
        }
    }
}
