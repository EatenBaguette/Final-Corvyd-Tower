using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    [SerializeField] Transform _target;

    [SerializeField] float _rotationSpeed = 1.5f;

    float _rotationY;
    Vector3 _offset;


    private void Start()
    {
        _rotationY = transform.eulerAngles.y;
        
        _offset = _target.position - transform.position;
    }

    private void LateUpdate()
    {
        float horInput = Input.GetAxis("Horizontal");
        
        if (!Mathf.Approximately(horInput, 0))
            // Slowly rotate camera with arrow keys
            _rotationY += horInput * _rotationSpeed;
        // Otherwise, use the mouse X axis
        else
            _rotationY += Input.GetAxis("Mouse X") * _rotationSpeed * 3;
        
        Quaternion rotation = Quaternion.Euler(0, _rotationY, 0);

        // Quaternion times vector rotates the vector.
        // same equation as calculate the offset,
        // now solving for new position and rotating the offset vector.
        transform.position = _target.position - (rotation * _offset);
        
        transform.LookAt(_target);
    }
}
