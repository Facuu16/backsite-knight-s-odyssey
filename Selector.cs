using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Selector : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler
{

    [SerializeField] private float _sensitivity = 10.0f;
    [SerializeField] private int _firstOption = 0;
    [SerializeField] private bool _isDraggable;

    [SerializeField] private GameObject _textObj, _imageObj;

    [SerializeField] private string[] _options;
    [SerializeField] private Sprite[] _images;

    private bool _isMouseOver;
    private Vector2 _lastMousePos;

    private TextMeshProUGUI _text;
    private Image _image;

    private int _option;

    void Awake()
    {
        if (_textObj != null)
            _text = _textObj.GetComponent<TextMeshProUGUI>();

        if (_imageObj != null)
            _image = _imageObj.GetComponent<Image>();

        ShowOption(_firstOption);
    }

    public void OnPointerEnter(PointerEventData data)
    {
        _isMouseOver = true;
        _lastMousePos = data.position;
    }

    public void OnPointerExit(PointerEventData data)
    {
        _isMouseOver = false;
    }

    public void OnPointerClick(PointerEventData data)
    {
        if (_isDraggable || !_isMouseOver)
            return;

        ShowOption(true);
    }

    public void OnDrag(PointerEventData data)
    {
        if (!_isDraggable || !_isMouseOver)
            return;

        var pos = data.position;
        var delta = pos - _lastMousePos;

        if (delta.x > _sensitivity)
            ShowOption(false);

        else if (delta.x < -_sensitivity)
            ShowOption(true);

        _lastMousePos = pos;
    }

    public void ShowOption(bool increment)
    {
        if (increment)
            ShowOption(_option == (_options.Length - 1) ? 0 : _option + 1);

        else
            ShowOption(_option == 0 ? _options.Length - 1 : _option - 1);
    }

    public void ShowOption(int index)
    {
        var option = _options[index];

        if (_text != null)
            _text.text = option;

        if (_image != null)
            _image.sprite = _images[index];

        _option = index;
    }

    public string GetOption()
    {
        return _options[_option];
    }

}