using UnityEngine;

public class HealthBar : MonoBehaviour
{

    private Attributes _attributes;

    private Vector3 _initialScale;
    private float _initialWidth;
    private Vector3 _initialPosition;

    void Awake()
    {
        _attributes = transform.parent.GetComponent<Attributes>();
        _initialScale = transform.localScale;
        _initialWidth = transform.localScale.x;
        _initialPosition = transform.localPosition;
    }

    void Update()
    {
        if (_attributes.Health == _attributes.MaxHealth)
        {
            transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            return;
        }

        var newScaleX = _initialWidth * ((float) _attributes.Health / _attributes.MaxHealth);

        transform.localScale = new Vector3(newScaleX, _initialScale.y, _initialScale.z);

        var newPosition = _initialPosition;

        newPosition.x += (_initialWidth - newScaleX) / 2f;
        transform.localPosition = newPosition;
    }

}
