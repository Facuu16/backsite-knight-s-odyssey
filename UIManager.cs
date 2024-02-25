using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    private Attributes _attributes;
    private GameManager _manager;

    void Awake()
    {
        _attributes = GameObject.Find("Player").GetComponent<Attributes>();
        _manager = GameObject.Find("API").GetComponent<GameManager>();
    }

    void Update()
    {
        var data = _manager.Data;

        GameObject.Find("UI")
            .transform
            .Find("Info")
            .GetComponent<TextMeshProUGUI>()
            .text = $"HEALTH: {_attributes.Health}/{_attributes.MaxHealth} \nSTAMINA: {_attributes.Stamina}/{_attributes.MaxStamina} \nDIFFICULTY: {data.Difficulty} \nROUND: {data.Round} \nKILLS: {data.Kills}";
    }

}
