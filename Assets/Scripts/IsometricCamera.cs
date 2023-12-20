using System.Collections;
using UnityEngine;

internal struct CamMoveArgs
{
    public Vector3 Position;
    public float TimeSec;
}

public class IsometricCamera : MonoBehaviour
{
    public Transform targetTransform;

    private Transform _transform;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    private void Update()
    {
        FollowTargetUpdate();
    }

    private void FollowTargetUpdate()
    {
        if (targetTransform == null) {
            return;
        }
        
        var thisPosition = _transform.position;
        var targetPosition = targetTransform.position;
        var distance = Vector3.Distance(thisPosition, targetPosition);
        _transform.position = Vector3.Lerp(thisPosition, targetPosition, Time.deltaTime * distance);
    }
    
    public void MoveTo(Vector3 position, float moveDurationSec = 0f)
    {
        if (targetTransform == null)
        {
            return;
        }
        
        if (moveDurationSec <= 0f)
        {
            _transform.position = position;
            return;
        }
        
        var args = new CamMoveArgs
        {
            Position = position,
            TimeSec = moveDurationSec
        };
        
        StartCoroutine(nameof(MoveTowards), args);
    }
    
    private IEnumerator MoveTowards(CamMoveArgs args)
    {
        var thisPosition = _transform.position;
        var time = 0f;
        while (time < args.TimeSec)
        {
            time += Time.deltaTime;
            _transform.position = Vector3.Lerp(thisPosition, args.Position, time / args.TimeSec);
            yield return null;
        }
    }
}
