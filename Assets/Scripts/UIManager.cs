using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //handle to get text value
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _ammoCount;
    [SerializeField]
    private Image _LivesImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;

    private GameManager _gm;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _ammoCount.text = "Ammo Count: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _gm = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_gm == null)
        {
            Debug.LogError("GAME Manager is null !!");
        }
    }

    //method to update score in UI
    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateAmmo(int ammoFired)
    {
        if (ammoFired <= 15)
        {
            _ammoCount.color = Color.white;
            _ammoCount.text = "Ammo Count:" + ammoFired.ToString();
        }
        else
        {
            _ammoCount.text = "Ammo depleted! Refill?!";
            _ammoCount.color = Color.red;
        }
    }

    public void UpdateLives(int currentLives)
    {
        _LivesImg.sprite = _liveSprites[currentLives];
        //Debug.LogError(_liveSprites);
        //Debug.LogError(currentLives);

        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    void GameOverSequence()
    {
        _gm.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }
    IEnumerator GameOverFlickerRoutine()
    {
            while (true)
        {
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }
    }

}
