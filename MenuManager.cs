using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    private Selector _difficultySelector;
    private Selector _mapSelector;

    private GameObject _menus;
    private GameObject _menu;

    void Awake()
    {
        _menus = GameObject.Find("Menu");

        var mainMenu = GetMenu("Main Menu");
        var newGameMenu = GetMenu("New Game Menu");

        _difficultySelector = newGameMenu
            .transform
            .Find("Difficulty Text")
            .GetComponent<Selector>();

        _mapSelector = newGameMenu
            .transform
            .Find("Map Image")
            .GetComponent<Selector>();

        for (int i = 0; i < _menus.transform.childCount; i++)
            _menus.transform.GetChild(i).gameObject.SetActive(false);

        GetTransform(mainMenu, "Play")
            .GetComponent<Button>()
            .onClick
            .AddListener(() => ShowMenu(newGameMenu));

        GetTransform(newGameMenu, "Back")
            .GetComponent<Button>()
            .onClick
            .AddListener(() => ShowMenu(mainMenu));

        GetTransform(newGameMenu, "Start Game")
            .GetComponent<Button>()
            .onClick
            .AddListener(() =>
            {
                SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

                var loaded = SceneManager.LoadSceneAsync(_mapSelector.GetOption(), LoadSceneMode.Single);

                loaded.completed += (operation) => {
                    var manager = GameObject.Find("API").GetComponent<GameManager>();
                    var difficulty = Enum.Parse(typeof(Difficulty), _difficultySelector.GetOption().ToUpper());

                    manager.StartGame((Difficulty) difficulty);
                };
            });

        ShowMenu(mainMenu);
    }

    public Transform GetTransform(string menu, string id)
    {
        return GetTransform(GetMenu(menu), id);
    }

    public Transform GetTransform(GameObject menu, string id)
    {
        return menu.transform.Find(id);
    }

    public GameObject GetCurrentMenu()
    {
        return _menu;
    }

    public GameObject GetMenu(string menu)
    {
        return _menus.transform.Find(menu).gameObject;
    }

    public void ShowMenu(string menu)
    {
        ShowMenu(GetMenu(menu));
    }

    public void ShowMenu(GameObject menu)
    {
        if (_menu != null)
            _menu.SetActive(false);

        menu.SetActive(true);
        _menu = menu;
    }

}
