using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private string _mapName;
    [SerializeField] private int _enemiesPerRound;
    [SerializeField] private GameObject[] _enemies;

    private int _remainingEnemies;
    private GameData _data;

    private void Update()
    {
        if (!InGame() || _remainingEnemies > 0)
            return;

        for (int i = 0; i < _enemiesPerRound; i++)
        {
            var spawned = Instantiate(_enemies[Random.Range(0, _enemies.Length)]);

            spawned.SetActive(true);
            _remainingEnemies++;
        }

        _data.Round++;
    }

    public bool InGame()
    {
        return _data != null;
    }

    public void StartGame(Difficulty difficulty)
    {
        _data = new GameData(difficulty);
        _remainingEnemies = 0;
    }

    public void Kill()
    {
        _remainingEnemies--;
        _data.Kills++;
    }

    public GameData Data
    {
        get { return _data; }
    }

}

public record GameData
{
    public Difficulty Difficulty { get; }

    public int Kills { set; get; }

    public int Round { set; get; }

    public GameData(Difficulty difficulty)
    {
        Difficulty = difficulty;
        Kills = 0;
        Round = 0;
    }
}

public enum Difficulty
{
    EASY,
    NORMAL,
    HARD
}
