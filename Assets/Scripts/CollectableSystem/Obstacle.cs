using DG.Tweening;
using UnityEngine;

public class Obstacle : Collectable
{
    [SerializeField] private ObstacleMotion motion;
    [SerializeField] private float radius;
    [SerializeField] private float motionPeriod;

    private Tween _tween;
    private Transform _parent;

    private void Start()
    {
        _tween = ModelTransform.DOLocalRotate(new Vector3(90, 0, 360), .5f, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Incremental)
            .SetEase(Ease.Linear);

        SetMotion();
    }
    
    private void OnDestroy()
    {
        _tween.Kill();
        if (_parent)
        {
            Destroy(_parent.gameObject);
        }
    }

    /// <summary>
    /// Build a tween for selected ObstacleMotion enum on the inspector..
    /// </summary>
    private void SetMotion()
    {
        switch (motion)
        {
            case ObstacleMotion.Horizontal:
            {
                transform.position = new Vector3(-radius / 2, transform.position.y, transform.position.z);
                Tween = transform.DOMoveX(radius / 2, motionPeriod / 2).SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.InOutQuad);
                break;
            }
            case ObstacleMotion.Circular:
            {
                _parent = new GameObject("ObstacleParent").transform;
                _parent.SetParent(transform.parent);
                _parent.position = transform.position;
                transform.SetParent(_parent);
                transform.position = new Vector3(-radius, transform.position.y, transform.position.z);
                Tween = _parent.DORotate(new Vector3(0, 360, 0), motionPeriod, RotateMode.FastBeyond360)
                    .SetEase(Ease.Linear)
                    .SetLoops(-1, LoopType.Incremental);
                break;
            }
            case ObstacleMotion.Vertical:
            {
                transform.position = new Vector3(transform.position.x, transform.position.y,
                    transform.position.z + radius / 2);
                Tween = transform.DOMoveZ(transform.position.z - radius, motionPeriod / 2).SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.InOutQuad);
                break;
            }
        }
    }

    /// <summary>
    /// Gizmos for motion preview.
    /// </summary>
    private void OnDrawGizmos()
    {
        if (UnityEngine.Application.isPlaying) return;
        Gizmos.color = Color.yellow;
        switch (motion)
        {
            case ObstacleMotion.Horizontal:
            {
                var position = ModelTransform.position;
                var startPoint = new Vector3(-radius / 2, position.y, position.z);
                var endPoint = new Vector3(radius / 2, position.y, position.z);
                Gizmos.DrawLine(startPoint, endPoint);
                break;
            }
            case ObstacleMotion.Circular:
            {
                DrawCircle();
                break;
            }
            case ObstacleMotion.Vertical:
            {
                var position = ModelTransform.position;
                var startPoint = new Vector3(position.x, position.y, position.z - radius / 2);
                var endPoint = new Vector3(position.x, position.y, position.z + radius / 2);
                Gizmos.DrawLine(startPoint, endPoint);
                break;
            }
        }
    }

    /// <summary>
    /// Draw wire circle for gizmos.
    /// </summary>
    private void DrawCircle()
    {
        float theta = 0;
        float x = radius * Mathf.Cos(theta);
        float y = radius * Mathf.Sin(theta);
        Vector3 pos = ModelTransform.position + new Vector3(x, 0, y);
        Vector3 lastPos = pos;
        for (theta = 0.1f; theta < Mathf.PI * 2; theta += 0.1f)
        {
            x = radius * Mathf.Cos(theta);
            y = radius * Mathf.Sin(theta);
            var newPos = ModelTransform.position + new Vector3(x, 0, y);
            Gizmos.DrawLine(pos, newPos);
            pos = newPos;
        }

        Gizmos.DrawLine(pos, lastPos);
    }

    private enum ObstacleMotion
    {
        Static = 0,
        Horizontal = 1,
        Circular = 2,
        Vertical = 3
    }
}