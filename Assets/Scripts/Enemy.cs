using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    private Player _player;
    private Animator _anim;

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
    private bool _enemyBehindActive = false, _isPickupInFront = false, _enemyAvoidActive = false;
    
    private enum LaserType { OneShot, TwoShots, LaserBeam, HeatSeeking }
    [SerializeField]
    private LaserType _laserType = LaserType.TwoShots;
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
    private float InstantiationTimer = 2.0f;

    private void Start()
    {
        // this is caching the player find component !!
        _AsourceExplosion = GetComponent<AudioSource>(); 
        _player = GameObject.Find("Player").GetComponent<Player>();
       

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
        //    CalculateMovement();

        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(1, 4);
            _canFire = Time.time + _fireRate;

 /*           if (_spawn.enemyLevel ==  )

        //    GameObject LaserEN = Instantiate(LaserTypesDI[_laserType], transform.position, Quaternion.identity);

            Laser[] lasersNEW = LaserEN.GetComponentsInChildren<Laser>();
            
            for (int i = 0; i < lasersNEW.Length; i++)
            {
                lasersNEW[i].AssignEnemyLaser();
            }
*/
            // if enemy is easy - one shot
            // if is medium - two shopts
            // if hard laser beam

            //            LaserTypesDI[_laserType]


            // testing first with random enemy to get random shots

            switch (_spawn.enemyLevel)
            {
                case SpawnManager.EnemyLevels.Easy:
             //old//       GameObject oneS = Instantiate(OneShot, transform.position, Quaternion.identity);
                    Optimise(OneShot, OneShot);
                    break;
                case SpawnManager.EnemyLevels.Medium:
                    Optimise(TwoShots, TwoShots);
                    break;
                case SpawnManager.EnemyLevels.Hard:
                    Optimise(TwoShots, LaserBeam);
                    break;
                case SpawnManager.EnemyLevels.Boss:
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
        float a = InstantiationTimerRange;
        float b = (InstantiationTimerRange - 0.5f) * Time.deltaTime;

        InstantiationTimer -= a; //* Time.deltaTime;
//        Debug.LogError("timer = " + InstantiationTimer);
//        Debug.LogError("a = " + a);
//        Debug.LogError("b = " + b);
                
        if (InstantiationTimer <= a && InstantiationTimer > b)
        //if (InstantiationTimer <= Random.Range(a - 0.1f, a + 0.1f) && InstantiationTimer > Random.Range(b - 0.1f, b + 0.1f))
        {
            GameObject LaserEN1 = Instantiate(fire1, transform.position, Quaternion.identity);
            
 //           if (fire1 == (TwoShots || OneShot) )
 //           {
                Laser[] lasersNEW = LaserEN1.GetComponentsInChildren<Laser>();

                for (int i = 0; i < lasersNEW.Length; i++)
                {
                    lasersNEW[i].AssignEnemyLaser(true, false, false);
                }
 /*           }
            else
            {
                Laser lasersNEW = LaserEN1.GetComponent<Laser>();
                lasersNEW.AssignEnemyLaser(true, false);
            }
 */       }

        if (InstantiationTimer <= b)
        //if (InstantiationTimer <= Random.Range(b - 0.1f, b + 0.1f))
        {
            GameObject LaserEN2 = Instantiate(fire2, transform.position, Quaternion.identity);
            
            if (fire2 == (TwoShots || OneShot) )
            {
                Laser[] lasersNEW2 = LaserEN2.GetComponentsInChildren<Laser>();

                for (int i = 0; i < lasersNEW2.Length; i++)
                {
                    lasersNEW2[i].AssignEnemyLaser(true, true, false);
                }
            }
            else if (fire2 == LaserBeam)
            {
                Laser lasersNEW2 = LaserEN2.GetComponent<Laser>();
                lasersNEW2.AssignEnemyLaser(true, true, true);
            }
      }

        InstantiationTimer = 2f;

    }



    void CalculateMovement() // not used currently .... I USE ENEMY_ROTATING.CS
    {
        //   transform.Translate(Vector3.down * _speed * Time.deltaTime);
        transform.RotateAround(Vector3.forward * _speed, Vector3.down, 10 * Time.deltaTime);


        /*
                if (transform.position.y < -5f)
                {
                    float randomX = Random.Range(-8f, 8f);
                    transform.position = new Vector3(randomX, 7, 0);
                }
         */
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

            //add 10
            if (_player != null)
            {
                _player.AddScore(Random.Range(10, 20));
            }

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 1.0f;
            _AsourceExplosion.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
            //_spawn.killEnemy();
            //_uiManager --- i need a counting killed enemies function
        }
    }           

}
