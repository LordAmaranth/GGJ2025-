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
        public float soloSmoothTime = 0.2f;
        public float minZoom = 20.0f;
        public float maxZoom = 60.0f;
        private Vector3 velocity;
        private float fvelocity;
        
        private float defaultOrthographicSize = 5;
        private Vector3 defaultposition = Vector3.zero;
        
        void Awake()
        {
            screenAspect = (float)Screen.height / Screen.width;
            _camera = GetComponent<Camera>();
            // 初期値
            defaultOrthographicSize = _camera.orthographicSize;
            defaultposition = transform.position;
        }

        void Update()
        {
            UpdateCameraPosition();
            UpdateOrthographicSize();
        }

        void UpdateCameraPosition()
        {
            var target = GetTarget();
            if (target.min == null)
            {
                transform.position = Vector3.SmoothDamp(transform.position, defaultposition, ref velocity, smoothTime);
                Debug.Log($"A defaultposition:{defaultposition}");
                return;
            }

            if (target.max == null)
            {
                var tPosition = new Vector3(targets[0].position.x, targets[0].position.y + offset.y, defaultposition.z);
                transform.position = Vector3.SmoothDamp(transform.position, tPosition, ref velocity, soloSmoothTime);
                Debug.Log($"B tPosition:{tPosition}");
                return;
            }
            // 2点間の中心点からカメラの位置を更新
            Vector3 center = Vector3.Lerp(target.min.position, target.max.position, 0.5f);
            var newPosition = center + Vector3.forward * -1;
            transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
        }

        private (Transform min, Transform max) GetTarget()
        {
            if (targets.Count < 1)
            {
                return (null, null);
            }
            if (targets.Count < 2)
            {
                return (targets[0], null);
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
            if (target.min == null || target.max == null)
            {
                _camera.orthographicSize = Mathf.Clamp(
                    Mathf.SmoothDamp(_camera.orthographicSize, defaultOrthographicSize, ref fvelocity, smoothTime),
                    minZoom,
                    maxZoom
                );
                return;
            }
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

            _camera.orthographicSize = Mathf.Clamp(
                Mathf.SmoothDamp(_camera.orthographicSize, targetOrthographicSize, ref fvelocity, smoothTime),
                minZoom,
                maxZoom
            );
        }

        Vector3 AbsPositionDiff(Transform target1, Transform target2)
        {
            Vector3 targetsDiff = target1.position - target2.position;
            return new Vector3(Mathf.Abs(targetsDiff.x), Mathf.Abs(targetsDiff.y));
        }
    }
}