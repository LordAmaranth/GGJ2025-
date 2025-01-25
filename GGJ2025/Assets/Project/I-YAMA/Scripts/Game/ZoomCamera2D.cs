using UnityEngine;
using System.Collections.Generic;
using GGJ.Common;

namespace Project.GGJ2025
{
    public class ZoomCamera2D : MonoBehaviour
    {
        public List<Transform> targets;
        [SerializeField] Vector2 offset = new Vector2(1, 1);

        private float screenAspect = 0;
        private Camera _camera = null;

        public float smoothTime = 0.5f;
        private Vector3 velocity;
        private float fvelocity;
        
        void Awake()
        {
            screenAspect = (float)Screen.height / Screen.width;
            _camera = GetComponent<Camera>();
        }

        void Update()
        {
            UpdateCameraPosition();
            UpdateOrthographicSize();
        }

        void UpdateCameraPosition()
        {
            var target = GetTarget();
            // 2点間の中心点からカメラの位置を更新
            Vector3 center = Vector3.Lerp(target.min.position, target.max.position, 0.5f);
            var newPosition = center + Vector3.forward * -1;
            transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
        }

        private (Transform min, Transform max) GetTarget()
        {
            if (targets.Count <= 2)
            {
                Debug.LogError("Target Count is count ....");
                return (null, null);
            }
            var transform1 = targets[0];
            var transform2 = targets[1];
            var maxDistance = 0f;
            var max = targets.Count;
            for (int i = 0; i < max; i++)
            {
                for (int j = i + 1; j < max; j++)
                {
                    var distance = Vector3.Distance(targets[i].position.GetPositionRate(), targets[j].position.GetPositionRate());
                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                        transform1 = targets[i];
                        transform2 = targets[j];
                    }
                }
            }
            return (transform1, transform2);
        }

        void UpdateOrthographicSize()
        {
            var target = GetTarget();
            // ２点間のベクトルを取得
            Vector3 targetsVector = AbsPositionDiff(target.min, target.max) + (Vector3)offset;

            // アスペクト比が縦長ならyの半分、横長ならxとアスペクト比でカメラのサイズを更新
            float targetsAspect = targetsVector.y / targetsVector.x;
            float targetOrthographicSize = 0;
            if (screenAspect < targetsAspect)
            {
                targetOrthographicSize = targetsVector.y * 0.5f;
            }
            else
            {
                targetOrthographicSize = targetsVector.x * (1 / _camera.aspect) * 0.5f;
            }

            _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, targetOrthographicSize, ref fvelocity, smoothTime);
        }

        Vector3 AbsPositionDiff(Transform target1, Transform target2)
        {
            Vector3 targetsDiff = target1.position - target2.position;
            return new Vector3(Mathf.Abs(targetsDiff.x), Mathf.Abs(targetsDiff.y));
        }
    }
}