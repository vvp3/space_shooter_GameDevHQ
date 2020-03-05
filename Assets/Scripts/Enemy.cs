using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    private Player _player;
    private Animator _anim;
//    private Transform targetAgressive; // not used

    [SerializeField]
//    private AudioClip _AclipExplosion;
    private AudioSource _AsourceExplosion;

    [SerializeField]
    private GameObject _laserEnemy;
    private float _fireRate = 3.0f;
    private float _canFire = 1.0f;

    [SerializeField]
    private bool _enemyShieldActive = true, _enemyAgressiveActive = true;

    [SerializeField]
    private bool _enemyBehindActive = false;// , _isPickupInFront = false, _enemyAvoidActive = false;
    
    public enum LaserType { OneShot_en, TwoShots_en, LaserBeam_en, HeatSeeking_en }
    [ReadOnly][SerializeField]
    public LaserType _laserType1 = LaserType.OneShot_en;
    [ReadOnly][SerializeField]
    public LaserType _laserType2 = LaserType.TwoShots_en;
    
    [SerializeField]
    private GameObject OneShot, TwoShots, LaserBeam, HeatSeeking;
    private Dictionary<LaserType, GameObject> LaserTypesDI = new Dictionary<LaserType, GameObject>(4);

    private enum MovementType { Normal, ZigZag, Wave }
    [SerializeField]
    private MovementType _movementTypeEN = MovementType.Normal;


    private SpawnManager _spawn;
    private UIManager _uiManager;
    [SerializeField]
    [Range(-10f, 10f)]
    private float InstantiationTimerRange = 0.5f;
    [SerializeField]
    [Range(-10f, 10f)]
    private float InstantiationTimer = 2.0f;

    [SerializeField]
    private GameObject _shieldVisualizerEnemy;
    private bool Boolean;

    [SerializeField]
    private int _shieldLives = 10;

    private void Start()
    {
        // this is caching the player find component !!
        _AsourceExplosion = GetComponent<AudioSource>(); 
        _player = GameObject.Find("Player").GetComponent<Player>();
        Boolean = (Random.value > 0.5f);

        if (_player == null)
        {
            Debug.LogError("no player !!");
        }

        _anim = GetComponent<Animator>();

        if (_anim == null)
        {
            Debug.LogError("no anim !!");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _spawn = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

    }

    void Update()
    {
        CalculateMovement();

        if (Time.time > _canFire && _player != null)
        {
            _fireRate = Random.Range(1, 4);
            _canFire = Time.time + _fireRate;
            Boolean = (Random.value > 0.5f);

            /*           if (_spawn.enemyLevel ==  )

                   //    GameObject LaserEN = Instantiate(LaserTypesDI[_laserType], transform.position, Quaternion.identity);
                       Laser[] lasersNEW = LaserEN.GetComponentsInChildren<Laser>();

                       for (int i = 0; i < lasersNEW.Length; i++)
                       {
                           lasersNEW[i].AssignEnemyLaser();
                       }
           */

            //            LaserTypesDI[_laserType]
            // testing first with random enemy to get random shots

            switch (_spawn.enemyLevel)
            {
                case SpawnManager.EnemyLevels.Easy:
                    _enemyShieldActive = Boolean;
                    _shieldVisualizerEnemy.SetActive(Boolean);
//                    Debug.LogWarning("easy level and BOOLEAN is: " + Boolean);
                    Optimise(OneShot, TwoShots);
                    // testing the enum changing
                    //        _laserType1 = LaserType.OneShot_en; 
                    //        _laserType2 = LaserType.TwoShots_en;
                    break;
                case SpawnManager.EnemyLevels.Medium:
                    _enemyShieldActive = Boolean;
                    _shieldVisualizerEnemy.SetActive(Boolean);
//                    Debug.LogWarning("medium level and BOOLEAN is: " + Boolean);
                    Optimise(TwoShots, LaserBeam);
                    break;
                case SpawnManager.EnemyLevels.Hard:
                    _enemyShieldActive = Boolean;
                    _shieldVisualizerEnemy.SetActive(Boolean);
                    Optimise(LaserBeam, HeatSeeking);
                    break;
                case SpawnManager.EnemyLevels.Boss:
                    _enemyShieldActive = true;
                    _shieldVisualizerEnemy.SetActive(true);
                    _shieldLives = 10;
                    _shieldVisualizerEnemy.GetComponent<SpriteRenderer>().color = Color.blue;
                    Optimise(LaserBeam, HeatSeeking);
                    break;
                    /*                default: //old - course 1A;
                                        GameObject enemyLaser = Instantiate(_laserEnemy, transform.position, Quaternion.identity);
                                        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

                                        for (int i = 0; i < lasers.Length; i++)
                                        {
                                            lasers[i].AssignEnemyLaser();
                                        }
                                        break;
                                }
                    */
                    
            }
            Debug.Log("we are in level " + _spawn.enemyLevel);
            //Debug.Break();
        }
    }

    private void Optimise(GameObject fire1, GameObject fire2)
    {

//        fire1.SetActive(false);
//        fire2.SetActive(false);

        float a = InstantiationTimer;
        float b = 0f;
        float step = _speed * 5 * Time.deltaTime;

        InstantiationTimer -= InstantiationTimerRange; //* Time.deltaTime;
                                                       //        Debug.LogWarning("timer = " + InstantiationTimer);
                                                       //        Debug.LogWarning("a = " + a);
                                                       //        Debug.LogWarning("b = " + b);

        EnemyBehaviours(fire1); // this is putting _enemyBehindActive true OR false //
                                // enemy agressive will fire as usual though
                                // enemy avoiding will fire as usual though

        if (_enemyBehindActive == false) // not interesting in other behaviours to change the Instantiation of fires
        {
            if (InstantiationTimer <= a && InstantiationTimer > b)
            {
                //            Debug.LogError(" L1 works");

                GameObject LaserEN1 = Instantiate(fire1, transform.position, Quaternion.identity);

                if (fire1 == TwoShots || fire1 == OneShot)
                {
                    Laser[] lasersNEW = LaserEN1.GetComponentsInChildren<Laser>();

                    for (int i = 0; i < lasersNEW.Length; i++)
                    {
                        lasersNEW[i].AssignEnemyLaser(true, false, false);
                    }

                }
                else if (fire1 == LaserBeam)
                {
//                    Debug.LogWarning("fire 1 is laserbeam");
                    //                Debug.Break();

                    Laser lasersNEW = LaserEN1.GetComponentInChildren<Laser>();
                    lasersNEW.AssignEnemyLaser(true, false, true);
                }
                else if (fire1 == HeatSeeking)
                {
                    Laser lasersNEW = LaserEN1.GetComponentInChildren<Laser>();
                    lasersNEW.AssignEnemyBehaviours(false, true);
                    lasersNEW.AssignEnemyLaser(true, false, false);
                }
            }


            if (InstantiationTimer <= b)
            {
                //            Debug.LogError(" L2 works");

                GameObject LaserEN2 = Instantiate(fire2, transform.position, Quaternion.identity);
                            

                if (fire2 == TwoShots || fire2 == OneShot)
                {
                    Laser[] lasersNEW2 = LaserEN2.GetComponentsInChildren<Laser>();

                    for (int i = 0; i < lasersNEW2.Length; i++)
                    {
                        lasersNEW2[i].AssignEnemyLaser(false, true, false);
                    }
                }
                else if (fire2 == LaserBeam) // and here
                {
//                    Debug.LogWarning("fire 2 is laserbeam");
                    //                Debug.Break();

                    Laser lasersNEW2 = LaserEN2.GetComponentInChildren<Laser>();
                    lasersNEW2.AssignEnemyLaser(false, true, true);
                }
                else if (fire2 == HeatSeeking)
                {
//                    Debug.LogWarning("fire 2 is HEAT SEEKING");
//WORKS                    Debug.Break();

                    Laser lasersNEW2 = LaserEN2.GetComponentInChildren<Laser>();
                    lasersNEW2.AssignEnemyBehaviours(false, true);
                    lasersNEW2.AssignEnemyLaser(false, true, false);
                }

                InstantiationTimer = 2f;  ///IMPORTANT !! it must be only on this bracket to work so I RESET IT !!
            }
        }      
    }


    // not used currently for normal enemies.... I USE ENEMY_ROTATING.CS
   void CalculateMovement() 
       {
           //   transform.Translate(Vector3.down * _speed * Time.deltaTime);
           //transform.RotateAround(Vector3.forward * _speed, Vector3.down, 10 * Time.deltaTime);
      /*     
                   if (transform.position.y < -5f)
                   {
                       float randomX = Random.Range(-8f, 8f);
                       transform.position = new Vector3(randomX, 7, 0);
                   }        
    */
        if (_spawn.enemyLevel == SpawnManager.EnemyLevels.Boss)
        {
            float step = _speed * 2 * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, 5, 0), step);
        }
        else { Debug.LogError("I am moving differently !?");  }
    
    }

  
    public void EnemyBehaviours(GameObject fireBehind) // SMART -- FIRE BEHIND ... SIMILAR
    {
        if (_player != null)
        {
//            float step = _speed * 5 * Time.deltaTime;

            if (transform.position.y < (_player.transform.position.y))
            {
                Debug.LogError("bellow player -- enemy will shoot the player from behind");
                _enemyBehindActive = true;

                GameObject LaserENb = Instantiate(fireBehind, transform.position, Quaternion.identity);
                Laser[] lasersB = LaserENb.GetComponentsInChildren<Laser>();

                for (int i = 0; i < lasersB.Length; i++)
                {
                    lasersB[i].AssignEnemyLaser(true, false, false);
                    lasersB[i].AssignEnemyBehaviours(true, false);
                }
            }
            else
            {
//                Debug.LogError("NOT BEHIND !");
                _enemyBehindActive = false;
            }

//maybe have here an if _enemyshieldACTIVE ?

        }
        else { Debug.LogError("PLAYER IS DEAD ?!"); }
    }



    private void DamageShield()
    {
        _shieldLives--;
        switch (_shieldLives)
        {
            case int n when (n <= 10 && n >= 6):
                _shieldVisualizerEnemy.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.blue, Color.cyan, 0.1f);
                //Debug.Log("-1st shield"); 
                break;
            case int n when (n <= 5 && n >= 3):
                _shieldVisualizerEnemy.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.cyan, Color.magenta, 0.3f);
                //Debug.Log("-1st shield"); 
                break;
            case int n when (n <= 2 && n >= 1):
                _shieldVisualizerEnemy.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.magenta, Color.red, 0.5f);
                //Debug.Log("-2nd shield");
                break;
            case 0:
                _AsourceExplosion.Play();
                //_shieldLives = 0;
                _enemyShieldActive = false;
                _shieldVisualizerEnemy.SetActive(false);
                return;
        }

    }

  

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage(1,0,0);
            }

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0.5f;
            _AsourceExplosion.Play();
            Destroy(this.gameObject, 2.8f);
            //_spawn.killEnemy(VAR); I need a VAR to pass the killed enemies IDs so that the counting of numEnemy decreases
            //_uiManager --- i need a counting killed enemies function
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);

            if (_enemyShieldActive == true)
            {
                if (_spawn.enemyLevel == SpawnManager.EnemyLevels.Boss)  // IA DAMAGE SI AICI SI MAI JOS !!!!
                {
                    Debug.LogWarning("TESTING IF boss damage !!");
                    Debug.Break();

                    DamageShield();
                }
                else
                {
                    _AsourceExplosion.Play();

                    // set shield inacitve
                    _enemyShieldActive = false;
                    _shieldVisualizerEnemy.SetActive(false);
                }
            }


            if (_player != null)
            {

                //had player check if null here before ...
                _player.AddScore(Random.Range(10, 20));

                _anim.SetTrigger("OnEnemyDeath");
                _speed = 1.0f;
                _AsourceExplosion.Play();
                Destroy(GetComponent<Collider2D>());
                Destroy(this.gameObject, 2.8f);

                Debug.LogWarning("TESTING full dmg !!");
                Debug.Break();

                //_spawn.killEnemy();
                //_uiManager --- i need a counting killed enemies function
            }


        }
    }           

}
