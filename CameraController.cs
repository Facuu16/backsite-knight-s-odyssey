using UnityEngine;

public class CameraController : MonoBehaviour
{

    private Transform target;

    void Awake()
    {
        target = GameObject.Find("Player").transform;
    }

    [SerializeField] private float _damping = 1;
    [SerializeField] private float _lookAheadFactor = 3;
    [SerializeField] private float _lookAheadReturnSpeed = 0.5f;
    [SerializeField] private float _lookAheadMoveThreshold = 0.1f;

    private float _offsetZ;
    private Vector3 _lastTargetPosition;
    private Vector3 _currentVelocity;
    private Vector3 _lookAheadPos;

    private void Start()
    {
        _lastTargetPosition = target.position;
        _offsetZ = (transform.position - target.position).z;
        transform.parent = null;
    }


    private void Update()
    {
        var xMoveDelta = (target.position - _lastTargetPosition).x;

        if (Mathf.Abs(xMoveDelta) > _lookAheadMoveThreshold)
        {
            _lookAheadPos = _lookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
        }
        else
        {
            _lookAheadPos = Vector3.MoveTowards(_lookAheadPos, Vector3.zero, Time.deltaTime * _lookAheadReturnSpeed);
        }

        var aheadTargetPos = target.position + _lookAheadPos + Vector3.forward * _offsetZ;
        var newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref _currentVelocity, _damping);

        transform.position = newPos;
        _lastTargetPosition = target.position;
    }

}
