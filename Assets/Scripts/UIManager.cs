using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Slider _thrusterSlider;
    private int valueMAX = 100;
//    [SerializeField]
//    private int _thrusterStep = 1;
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

    float thrusterStep;
    float thrusterValue;

    private GameManager _gm;

    // Start is called before the first frame update
    void Start()
    {  
        _scoreText.text = "Score: " + 0;
        _ammoCount.text = "Ammo Count: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _gm = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _thrusterSlider = GameObject.Find("Thruster_HUD_Slider").GetComponent<Slider>();

        if (_gm == null)
        {
            Debug.LogError("GAME Manager is null !!");
        }
    }

    private void Update()
    {
        UpdateThrusters(thrusterStep, thrusterValue);
    }

    public void UpdateThrusters(float thrusterStep, float thrusterValue)
    {
       if (thrusterValue >= 5 && thrusterValue <= 10)
       {
            _thrusterSlider.value = thrusterValue;
            //Debug.Log("it works !!");
            Debug.LogError(thrusterValue);
       }
       else
       {
            Debug.LogError(thrusterValue);
       }
    
    }


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
