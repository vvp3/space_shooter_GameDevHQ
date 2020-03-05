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
    private Text _livesCount;
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
    private SpawnManager _spawn;

    // Start is called before the first frame update
    void Start()
    {  
        _scoreText.text = "Score: " + 0;
        _ammoCount.text = "Ammo Count: 15 / 15";
        _livesCount.text = "BOSS Lives: 10 / 10";
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _gm = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _spawn = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _thrusterSlider = GameObject.Find("Thruster_HUD_Slider").GetComponent<Slider>();

        if (_gm == null)
        {
            Debug.LogError("GAME Manager is null !!");
        }
        if (_thrusterSlider == null)
        {
            Debug.LogError("SLIDER is null !!");
        }



    }

    private void Update()
    {
        UpdateThrusters(thrusterStep, thrusterValue);
    }

    public void UpdateBossLives(int livesCurrent)
    {
        if (livesCurrent > 0)
        {
            _livesCount.color = Color.white;
            _livesCount.text = "BOSS Lives: " + livesCurrent.ToString() + " / 10";
        }
        else
        {
            _livesCount.text = "BOSS DEAD !! YEEEEY !!";
            _livesCount.color = Color.red;

            GameOverSequence();
            
        }
    }




    public void UpdateThrusters(float thrusterStep, float thrusterValue)
    {
       if (thrusterValue >= 5 && thrusterValue <= 10)
       {
            _thrusterSlider.value = thrusterValue;
            //Debug.Log("it works !!");
            //Debug.LogError(thrusterValue);
       }
       else
       {
            //Debug.LogError(thrusterValue);
       }
    
    }


    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }
      
    public void UpdateAmmo(int ammoFired, int ammoCurrent)
    {
        if (ammoCurrent > 0)
        {
            _ammoCount.color = Color.white;
            _ammoCount.text = "Ammo Count: " + ammoCurrent.ToString() + " / 15";
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
        yield return new WaitForSeconds(0.5f);
        _spawn.OnBossDeath();

        while (true)
        {
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }
    }

}
