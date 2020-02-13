using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    public float _speed = 3.5f;
    public float _speedMultiplier = 2;
    [SerializeField]
    private GameObject _laserPrefab;
    private int _lasersShot;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private int _shieldLives = 3;
    [SerializeField]
    private GameObject _rightEngine, _leftEngine;
   
    private SpawnManager _spawn;
    
    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;
    
//    public static float m_lastPressed; // need this for reffering to press once

    [SerializeField]
    private GameObject _shieldVisualizer;
    
    [SerializeField]
    private AudioClip _laserSoundClip;
    
    private AudioSource _audioSource;


    [SerializeField]
    private int _score;

    private UIManager _uiManager;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawn = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_spawn == null)
        {
            Debug.LogError("spawn manager is null !!");
        }

        if (_uiManager == null)
        {
            Debug.LogError("UI manager is null !!");
        }

        if (_audioSource == null)
        {
            Debug.LogError("Audio Source on player is null !!");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }

    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            if (_lasersShot <= 15)
            {
                FireLaser(true);
                _uiManager.UpdateAmmo(_lasersShot);
            }
            else
            {
                FireLaser(false);
            }
        }

        ShiftBoost();

    }

    void ShiftBoost()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)) //&& Time.time != m_lastPressed
        {
            //m_lastPressed = Time.time;
            _speed *= 1.4f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _speed /= 1.4f;
        }
    }
    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);
        // transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x > 11)
        {
            transform.position = new Vector3(-11, transform.position.y, 0);
        }
        else if (transform.position.x < -11)
        {
            transform.position = new Vector3(11, transform.position.y, 0);
        }
    }

    void FireLaser(bool _haveAmmo)
    {
        if (_haveAmmo == true)
        {
            _canFire = Time.time + _fireRate;

            if (_isTripleShotActive == false)
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);

            }
            else
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            }

            _audioSource.Play(0);
            _lasersShot++;
        }
        else
        {
            Debug.Log("CANNOT FIRE !!");
        }
    }

    private void DamageShield ()
    {
        
            _shieldLives--;
            switch (_shieldLives)
            {
                case 2:
                    _shieldVisualizer.GetComponent<SpriteRenderer>().color = Color.magenta;
                    //Debug.Log("-1st shield"); 
                    break;
                case 1:
                    _shieldVisualizer.GetComponent<SpriteRenderer>().color = Color.red;
                    //Debug.Log("-2nd shield");
                    break;
                case 0:
                    //_shieldLives = 0;
                    _isShieldActive = false;
                    _shieldVisualizer.SetActive(false);
                    return;
            }
        
    }

    public void Damage()
    {

        if (_isShieldActive == true)
        {
            DamageShield();
        }

        else
        {
            _lives--;

            _uiManager.UpdateLives(_lives);

            switch (_lives)
            {
                case 0:
                    _spawn.OnPlayerDeath();
                    Destroy(this.gameObject);

                    //PLAY EXPLOSION !!

                    break;
                case 1:
                    _leftEngine.SetActive(true);
                    break;
                case 2:
                    _rightEngine.SetActive(true);
                    break;
            }
        }
/*
        if (_lives < 1)
        {
            _spawn.OnPlayerDeath();
            Destroy(this.gameObject);
        }
*/
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldLives = 3;
        _shieldVisualizer.SetActive(true);
        _shieldVisualizer.GetComponent<SpriteRenderer>().color = Color.blue;
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
    //method to add 10
    //communicate with UI to update score

}
