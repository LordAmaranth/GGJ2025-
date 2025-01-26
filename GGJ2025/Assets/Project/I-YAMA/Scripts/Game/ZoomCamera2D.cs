using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using GGJ.Common;

namespace Project.GGJ2025
{
    public class ZoomCamera2D : MonoBehaviour
    {
        public List<Transform> targets;
        [SerializeField]
        private Vector2 offset = new Vector2(1, 1);

        [SerializeField] private Vector2 offsetVec = new Vector2(0, 20);

        private float screenAspect = 0;
        private Camera _camera = null;

        public float smoothTime = 0.5f;
        public float soloSmoothTime = 0.2f;
        public float minZoom = 20.0f;
        public float maxZoom = 60.0f;
        private Vector3 velocity;
        private float fvelocity;
        private Vector3 fVec;
        
        private float defaultOrthographicSize = 5;
        private Vector3 defaultposition = Vector3.zero;
        public Vector2 minBounds;
        public Vector2 maxBounds;
        
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
                return;
            }

            if (target.max == null)
            {
                var tPosition = new Vector3(targets[0].position.x, targets[0].position.y + offset.y, defaultposition.z);
                var aspect1 = _camera.aspect;
                tPosition.x = Mathf.Clamp(tPosition.x, minBounds.x + defaultOrthographicSize * aspect1, maxBounds.x - defaultOrthographicSize * aspect1);
                tPosition.y = Mathf.Clamp(tPosition.y, minBounds.y + defaultOrthographicSize, maxBounds.y - defaultOrthographicSize);
                transform.position = Vector3.SmoothDamp(transform.position, tPosition, ref velocity, soloSmoothTime);
                return;
            }
            // 2点間の中心点からカメラの位置を更新
            Vector3 center = Vector3.Lerp(target.min.position, target.max.position, 0.5f);
            center.y += offset.y;
            var newPosition = center + Vector3.forward * -1;
            // カメラの位置を制限
            var orthographicSize = _camera.orthographicSize;
            var aspect = _camera.aspect;
            newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x + orthographicSize * aspect, maxBounds.x - orthographicSize * aspect);
            newPosition.y = Mathf.Clamp(newPosition.y, minBounds.y + orthographicSize, maxBounds.y - orthographicSize);
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
                
                // カメラの位置を制御
                var nowPosition = transform.position;
                var orthographicSize = _camera.orthographicSize;
                var aspect = _camera.aspect;
                nowPosition.x = Mathf.Clamp(nowPosition.x, minBounds.x + orthographicSize * aspect, maxBounds.x - orthographicSize * aspect);
                nowPosition.y = Mathf.Clamp(nowPosition.y, minBounds.y + orthographicSize, maxBounds.y - orthographicSize);
                // transform.position = nowPosition;
                transform.position = Vector3.SmoothDamp(transform.position, nowPosition, ref fVec, smoothTime);
                return;
            }
            // ２点間のベクトルを取得
            Vector3 targetsVector = AbsPositionDiff(target.min, target.max) + (Vector3)offsetVec;

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
            
            // カメラの位置を制御
            var nowPosition2 = transform.position;
            var orthographicSize2 = _camera.orthographicSize;
            var aspect2 = _camera.aspect;
            nowPosition2.x = Mathf.Clamp(nowPosition2.x, minBounds.x + orthographicSize2 * aspect2, maxBounds.x - orthographicSize2 * aspect2);
            nowPosition2.y = Mathf.Clamp(nowPosition2.y, minBounds.y + orthographicSize2, maxBounds.y - orthographicSize2);
            transform.position = Vector3.SmoothDamp(transform.position, nowPosition2, ref fVec, smoothTime);
        }

        Vector3 AbsPositionDiff(Transform target1, Transform target2)
        {
            Vector3 targetsDiff = target1.position - target2.position;
            return new Vector3(Mathf.Abs(targetsDiff.x), Mathf.Abs(targetsDiff.y));
        }
    }
}