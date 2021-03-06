﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    public float _speed, _positionY = 3.5f;
    public float _initialSpeed;
//    public float _currentSpeed = 0f; // DO I NEED IT ??
    [SerializeField]
    public float _thrusterSpeed = 0.1f;
    public float _speedMultiplier = 2;

    private float _time;
    [Space]
    [SerializeField]
    private GameObject _laserPrefab;
    private int _lasersShot = -1;
    private int _ammo = 15;

    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _targetedShotPrefab;
    [SerializeField]
    private GameObject _FiveShotPrefab;

    [Space]
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private int _shieldLives = 3;
    [Space]
    [SerializeField]
    private GameObject _rightEngine, _leftEngine;
    [SerializeField]
    private GameObject _shieldVisualizer;

    private bool _isTripleShotActive = false;
    private bool _isTargetedShotActive = false; // similar to 5 shot, but it fires when I press caps lock
    private bool _isFiveShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isSlowActive = false;
    private bool _isShieldActive = false;
    private bool _isLifeActive = false;
    private bool _isShiftActive = false;
    private bool _isCloseShotActive = false;
    //    private bool _isFastPickupsActive = false;

    [SerializeField]
    private AudioClip _laserSoundClip;
    private AudioSource _audioSource;
    [Space]
    [SerializeField]
    private int _score;

    private SpawnManager _spawn;
    private CameraShake _cameraShake;
    private UIManager _uiManager;

    [Space]
    //private GameObject _powerUP;
    //private int _powerUPid;
    private GameObject[] _powerUPs;
//    private GameObject[] _laserS; // should I check an array of fired lasers ???
    

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawn = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _cameraShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();
        _initialSpeed = _speed;
        _time = 0;

  
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
        _time += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ShiftBoost(true);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            ShiftBoost(false);
        }

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
        
        if (Input.GetKeyDown(KeyCode.C))
        {
//            Debug.Log("C key was pressed.");
            
            if (_powerUPs == null || _lasersShot >=10) // IF WE HAVE low ammo then yeeeeessss we want to gather a lot of those !!
            {
//IT WORKS                Debug.LogError("powerUPs is null and I am searching for powerUPs !!!");
                _powerUPs = GameObject.FindGameObjectsWithTag("PowerUP");

//IT WORKS                Debug.LogWarning("number of powerups found is: " + _powerUPs.Length);

            }
//            _isFastPickupsActive = true;
            FastPickups();
        }
                       
    }

    void AmmoCheck()
    {
        if (_lasersShot < 15)
        {
            FireLaser(true);
    //        _ammo = 15 - _lasersShot;
            _uiManager.UpdateAmmo(_lasersShot, _ammo);
        }
        else
        {
            FireLaser(false);
        }
    }

    void ShiftBoost(bool startShift)
    {
        if (startShift == true)
        {
            _speed += _thrusterSpeed;
            StartCoroutine(ShiftBoostPowerUpRoutine());
        }
        else
        {
            StartCoroutine(ShiftBoostPowerDownRoutine());
        }

        /*
         * if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    StopCoroutine(ShiftBoostPowerDownRoutine());
                    _isShiftActive = true;
                    StartCoroutine(ShiftBoostPowerUpRoutine());
                }

                if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    StopCoroutine(ShiftBoostPowerUpRoutine());
                    _isShiftActive = false;
                    //ui update slider going left to 0%
                    StartCoroutine(ShiftBoostPowerDownRoutine());
                }
        */
    }

    IEnumerator ShiftBoostPowerUpRoutine()
    {
        while (_speed > _initialSpeed && _speed <= 10)
        {
            
            _speed += (_thrusterSpeed) * _time / 60;

//            _speed = Mathf.SmoothStep(_initialSpeed, 10, _time / 60);
//            Debug.LogError(_speed);

            //ui update slider going right to 100%
            _uiManager.UpdateThrusters(_thrusterSpeed, _speed);
        }   
        yield return null;
 //       _isShiftActive = false;
    }
    IEnumerator ShiftBoostPowerDownRoutine()
    {
        while (_speed > _initialSpeed)
        {
            _speed -= (_thrusterSpeed); //*Time.deltaTime
            _uiManager.UpdateThrusters(-_thrusterSpeed, _speed);
        }
        yield return null;
        _speed = _initialSpeed;
    }


    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);
        // transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, _positionY), 0);

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

            if (_isTargetedShotActive == true)
            {
                //Debug.LogWarning("bool 1");

                Instantiate(_targetedShotPrefab, transform.position, Quaternion.identity);
                // need this false ? i think yes becasue i do not have coroutine and I want to have it all the time at my disposal ?!
                _isTargetedShotActive = false;
            }

            else if (_isCloseShotActive == true)
            {
                //Debug.LogWarning("!!! BOOL 2 !!! : _isCloseShotActive is " + _isCloseShotActive);
                
                GameObject _closeLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
                //Debug.LogWarning("_closeLaser Go name is >> " + _closeLaser.name);

                // need this false ? i have coroutine to stop it in 5s so i think not ?!
                //                 _isCloseShotActive = false;

                // either I MAKE A NEW PREFAB for this, either i get the info from current LASER !!

                //               Laser _laserClose = GameObject.FindGameObjectWithTag("Laser").GetComponent<Laser>(); /// no need becasue i know exactly my instantiated laser prefab as _closeLaser !!

                Laser _laserClose = _closeLaser.GetComponent<Laser>();
                
                if (_laserClose != null)
                {
                    //Debug.LogWarning("enter the good bracket !! _laserClose is NOT null !!! >> " + _laserClose.name);
                    _laserClose.AssignLaserBehaviours(true);
                    _isCloseShotActive = false;
                }
                else
                {
                    Debug.LogWarning("_laserClose is null !!! >> " + _laserClose.name);
                }

            }
            else if (_isFiveShotActive == true)
            {
                //Debug.LogWarning("bool 3");

                Instantiate(_FiveShotPrefab, transform.position, Quaternion.identity);
            }
            else if (_isTripleShotActive == true)
            {
                //Debug.LogWarning("bool 4");

                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                //Debug.LogWarning("bool 5");

                Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
            }

            _audioSource.Play(0);
            _lasersShot++;
            _ammo = 15 - _lasersShot;
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
            if (_hitLeft == 1 || _hitRight == 1)
            {
                _lives--;
            //    Debug.Log("live decrease 1");
            }
            else if (_oneShot == 1)
            {
                _lives--;
               // _lives = _lives - _oneShot;
            }
            else
            {
                Debug.Log("something is wrong ??");
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
    
    public void SlowActive()
    {
        _isSlowActive = true;
        _speed /= (_speedMultiplier * 2);
        StartCoroutine(SlowPowerDownRoutine());
    }

    IEnumerator SlowPowerDownRoutine()
    {
        while (_speed <= _initialSpeed)
        {
            _speed ++;
        }
        yield return new WaitForSeconds(1.0f);
        _isSlowActive = false;
        _speed = _initialSpeed;
    }

    public void ShieldActive() // this is a POWER UP
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
        _ammo = 15;
        _uiManager.UpdateAmmo(_lasersShot, _ammo);

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
//        Debug.Log("I got 5 on it !!");
        StartCoroutine(FiveShotPowerDownRoutine());
    }

    IEnumerator FiveShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isFiveShotActive = false;
    }
    
    public void CloseShotActive()
    {
        _isCloseShotActive = true;
//        Debug.LogWarning("_isCloseShotActive is " + _isCloseShotActive);

        // i need to pass tO THE INSTANTIED LASER a behaviour ?!
        
        StartCoroutine(CloseShotPowerDownRoutine());
    }

    IEnumerator CloseShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(20.0f);
        _isCloseShotActive = false;

//        Debug.LogWarning("coRutine STOPPED !! _isCloseShotActive is " + _isCloseShotActive);

    }

    private void FastPickups()
    {
        //IT WORKS        Debug.Log("FastPickups started !!");

            foreach (GameObject powerUp in _powerUPs)
            {
                //IT WORKS            Debug.Log("FastPickup no " + powerUp.name + " started !!");
                Powerup _powerUPscript = powerUp.GetComponent<Powerup>();
                _powerUPscript.AssignFastPickups(true);
                //        _isFastPickupsActive = false;
            }
/* clear the array ? not neceessary - better the found solution !
        {
            Array.Clear(_powerUPs, 0, _powerUPs.Length);
            _powerUPs = null;
        }
*/
    }
             
}
