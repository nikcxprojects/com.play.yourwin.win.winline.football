using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIMenu : MonoBehaviour
{
    [Header("Settings References")]
    [SerializeField] private Text _audioText;

    [Header("Scores")]
    [SerializeField] private Transform _scoreWindowTransform;
    [SerializeField] private GameObject _scorePrefab;
    private List<GameObject> _scoreInitialied = new List<GameObject>();


    private void Awake()
    {
        AudioLoader();
    }

    public void Play()
    {
        SceneManager.LoadScene("Game");
    }

    public void ViewScores()
    {
        List<int> scores = Database.instance.GetScoresDatabase();

        if (scores.Count == 0)
            return;

        for (int i = scores.Count - 1; i > -1; i--)
        {
            GameObject scoreObj = Instantiate(_scorePrefab, _scoreWindowTransform);
            scoreObj.GetComponent<RectTransform>().localScale = Vector3.one;

            Text scoreText = scoreObj.GetComponent<Text>();
            scoreText.text = $"{scores.Count - i}. {scores[i]} points";

            _scoreInitialied.Add(scoreObj);
        }
    }

    /// <summary>
    /// Clears information about records
    /// </summary>
    public void ClearScores()
    {
        foreach (GameObject score in _scoreInitialied)
            Destroy(score);

        _scoreInitialied.Clear();
    }

    #region Settings
    public void ChangeAudio()
    {
        if (PlayerPrefs.GetString(PrefsKey.audio) == "Enable")
        {
            PlayerPrefs.SetString(PrefsKey.audio, "Disable");
            AudioListener.pause = true;
            _audioText.text = "MUSIC: OFF";
        }
        else
        {
            PlayerPrefs.SetString(PrefsKey.audio, "Enable");
            AudioListener.pause = false;
            _audioText.text = "MUSIC: ON";
        }
    }

    private void AudioLoader()
    {
        if (!PlayerPrefs.HasKey(PrefsKey.audio)) PlayerPrefs.SetString(PrefsKey.audio, "Enable");

        if (PlayerPrefs.GetString(PrefsKey.audio) == "Enable")
        {
            AudioListener.pause = false;
            _audioText.text = "MUSIC: ON";
        }
        else
        {
            AudioListener.pause = true;
            _audioText.text = "MUSIC: OFF";
        }
    }

    #endregion
}
