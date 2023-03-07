using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    [SerializeField] private Goalkeeper _goalkeeper;
    public Goalkeeper goalkeeper => _goalkeeper;

    [SerializeField] private int _score = 0;
    [SerializeField] private float _timer = 120.0f;

    [Header("UI References")]
    [SerializeField] private Text _timerText;
    [SerializeField] private Text _scoreText;
    [SerializeField] private GameObject _overWindow;
    [SerializeField] private Text _overTextScore;
    public bool isEnded { get { return _timer <= 0; } }

    private void Awake()
    {
        instance = this;
    }

    public void AddScore() => _score += 1;

    private void Update()
    {
        _scoreText.text = $"score: {_score}";
        if(_timer <= 0)
        {
            _timer = 0.0f;
            _timerText.text = "00:00";
            if (!_overWindow.activeSelf)
            {
                _overTextScore.text = $"{_score}";
                _overWindow.SetActive(true);
                Database.instance.AddNewScoreDatabase(_score);
                Time.timeScale = 0.0f;
            }
            return;
        }

        _timer -= Time.deltaTime;
        float minutes = Mathf.FloorToInt(_timer / 60);
        float seconds = Mathf.FloorToInt(_timer % 60);
        _timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void Restart()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Game");
    }

    public void Menu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Menu");
    }
}
