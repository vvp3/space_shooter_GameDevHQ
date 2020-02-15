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
    private int _lasersShot = -1;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _targetedShotPrefab;
    [SerializeField]
    private GameObject _FiveShotPrefab;
    

    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private int _shieldLives = 3;
    [SerializeField]
    private GameObject _rightEngine, _leftEngine;
   
    private SpawnManager _spawn;
    private CameraShake _cameraShake;

    private bool _isTripleShotActive = false;
    private bool _isTargetedShotActive = false;
    private bool _isFiveShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;
    private bool _isLifeActive = false;
    
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
        _cameraShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();

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
        if (_cameraShake == null)
        {
            Debug.LogError("Camera Shake is null !!");
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
            AmmoCheck();
        }

        if (Input.GetKeyDown(KeyCode.CapsLock) && Time.time > _canFire)
        {
            _isTargetedShotActive = true;
            AmmoCheck();
        }

        ShiftBoost();

    }

    void AmmoCheck()
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
                        
            if (_isTripleShotActive == true)
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            }
            else if (_isTargetedShotActive == true)
            {
                Instantiate(_targetedShotPrefab, transform.position, Quaternion.identity);
                _isTargetedShotActive = false;
            }
            if (_isFiveShotActive == true)
            {
                Instantiate(_FiveShotPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
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


    public void Damage(int _oneShot, int _hitLeft, int _hitRight)
    {
        _cameraShake._shakeDuration = 0.25f;
        StartCoroutine(CameraShakePowerDownRoutine(0));

        if (_isShieldActive == true)
        {
            DamageShield();
            // what if we powerup a LIFE ??
        }

        else
        {
            //_oneShot = 1;
            if (_oneShot == 1)
            {
                _lives = _lives - _oneShot;
            }
            else if (_hitLeft == 1 && _hitRight == 1)
            {
                _lives--;
                Debug.Log("live decrease 1");
            }
            else
            {
                Debug.Log("wtf ??");
            }


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
                default:
                    Debug.Log("I am doing something wrong ?!");
                    break;

            }
        }
//        _cameraShake._decreaseFactor = 0f;
    }

    IEnumerator CameraShakePowerDownRoutine(float factor)
    {
        yield return new WaitForSeconds(_cameraShake._shakeDuration);
        _cameraShake._decreaseFactor = factor;
        //_cameraShake

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

    public void AmmoRefillActive()
    {
        _lasersShot = 0;
        _uiManager.UpdateAmmo(_lasersShot);

    }

    public void LifeRefillActive()
    {
        _isLifeActive = true;
        if (_lives<3 && _isLifeActive == true)
        {
            _lives++;
            _uiManager.UpdateLives(_lives);
            _isLifeActive = false;
            //Debug.LogError(_lives);

            switch (_lives)
            {
                case 2:
                    _leftEngine.SetActive(false);
                    break;
                case 3:
                    _rightEngine.SetActive(false);
                    break;
            }
        }
        else
        {
            Debug.Log("Max lives are 3! All Good!");
        }
    }

    public void FiveShotActive()
    {
        _isFiveShotActive = true;
        Debug.Log("I got 5 on it !!");
        StartCoroutine(FiveShotPowerDownRoutine());
    }

    IEnumerator FiveShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isFiveShotActive = false;
    }


}
