using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speedL = 8.0f;
    [SerializeField]
    private bool _isEnemylaser = false;
    [SerializeField]
    private bool _isEnemylaser2 = false;
    private bool _hitLeft = false;
    private bool _hitRight = false;

    private LineRenderer lr;
    [SerializeField]
    private bool _isLaserBeam = false;
    [SerializeField]
    private bool _enemyBehindActive = false;
    [SerializeField]
    private bool _isHeatSeekingActive = false;
    [SerializeField]
    private bool _isCloseShotActive = false;

    private Transform target;

    private void Awake()
    {
        target = GameObject.Find("Player").GetComponent<Player>().transform;

        if (target == null)
        {
            Debug.Log("we have NO TARGET !!");
        }
    }

    private void Start()
    {
        //      _LaserLeft = this.transform.parent.tag

        lr = GetComponent<LineRenderer>();

    }

    void Update()
    {
 /*       if (_isHeatSeekingActive == true)
        {
//            Debug.LogError("HeatSeeking is " + _isHeatSeekingActive + "and i use its method !?"); // tested - it works
//            Debug.Break();
            HeatSeekingMethod();
        }
  
        else
        {
*/
//            Debug.LogError("HeatSeeking is" + _isHeatSeekingActive + "and i move to the other IFs bellow!");

            if ((_isEnemylaser == false && _isEnemylaser2 == false && _isLaserBeam == false) || _enemyBehindActive == true)
            {
                /*            if (_enemyBehindActive == true)
                            {
                                Debug.LogWarning("moveup going to show and _enemyBehindActive is " + _enemyBehindActive); /// check if it is behind OR NOT 
                            }
                */
                MoveUp();
                //                    Debug.LogError(_isEnemylaser + "up L1");
                //                   Debug.LogError(_isEnemylaser2 + "up L2");
                //                    Debug.LogError(_isLaserBeam + "up LB");

            }
            else if ((_isEnemylaser == true || _isEnemylaser2 == true) && _isLaserBeam == false)
            {
                MoveDown();
                //            Debug.LogError(_isEnemylaser);
                //            Debug.LogError(_isEnemylaser2);
            }
            else if (_isLaserBeam == true)
            {
                LaserBeamMethod();
                //            Debug.LogWarning("_isLaserBeam is" + _isLaserBeam); // tested - it works
            }
            else if (_isCloseShotActive == true)
            {
            Debug.LogWarning("_isCloseShotActive is " + _isCloseShotActive); // ?? works ?? IT SEEMS IT DOESN GO HERE ... BUT DIRECLTY ON LINE 67 ??
            MoveUp();
            }
            else
            {
                Debug.LogError("bug ? I am on an empty bracket ?!");
            }
 //       }
    }

    public void LaserBeamMethod()
    {
        //                Debug.LogWarning("LaserBeamMethod works");  // tested - it works

        // I MUST SET DIFFERENT RULE BUT INFINITY ... I NEED TO GO FROM TRANSFORM TO BOT !!

        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up); //, Mathf.Infinity);
        lr.SetPosition(0, hit.transform.position);

        //     if (Physics2D.Raycast(transform.position, -transform.up, Mathf.Infinity))
        //     {
        if (hit.collider != null)
        {
            lr.SetPosition(1, hit.point);

            if (hit.transform.tag == "Player")
            {
                //                Debug.Log("Did Hit PLAYER");
            }
            else if (hit.transform.tag == "PowerUP")
            {
                //                Debug.Log("Did Hit PowerUP");

                // I put in OnTriggerEnter2D
                /*               if (hit.transform.parent != null)
                               {
                                   Destroy(hit.transform.parent.gameObject, 1);
                               }

                               Destroy(hit.transform.gameObject, 1);
               */
            }
            else
            {
                //                Debug.Log("Did Hit this collider: " + hit.collider);
                Destroy(this.gameObject, 0.1f);
            }

            //            Destroy(this.gameObject, 0.1f);
        }

        //    }
        else
        {
            lr.SetPosition(1, transform.position + (-transform.up * 500));
            //            Debug.Log("Did NOT Hit");
        }

        // need this ?

        if (hit.transform.position.y < -8f)
        {
            if (hit.transform.parent != null)
            {
                Destroy(transform.parent.gameObject, 1);
            }

            Destroy(this.gameObject, 0.1f);
        }
    }

    public void HeatSeekingMethod() 
    {
        Debug.LogWarning("HeatSeekingMethod is launched !!");
        //        Debug.Break();
        
        Player player = GetComponent<Player>();

        //        if (player != null)
        //       {
 //       if (transform.position.y > -4f)
   //     {

            Debug.LogWarning("player transform is" + player.transform);

            // move towards player
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, _speedL * 5 * Time.deltaTime);
//        }


            if (transform.position.y < -8f)
            {
                if (transform.parent != null)
                {
                    Destroy(transform.parent.gameObject, 3f);
                }

                Destroy(this.gameObject, 3f);
            }
//        }
//        else { Debug.LogError("no heatseeking !!"); }
    } /// somehow PLAYER.TRANSFORM .. does not work :( ....

    private GameObject FindClosestEnemy()  //FUNCTION THAT RETURNS A GO !!
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;                          // initialise a null GO
        float distance = Mathf.Infinity;
        Vector3 position = transform.position; // TRANSFORM POSITION OF WHO ???? GENERIC ? having it in laser it should be lasers position ... having it in player is player position ...
        
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;  // THIS GETS OUT OF THE FUNCTION ?!!
    }

    private void MoveUp()
    {
//        Debug.LogWarning("MOVE UP is launched !!");
//        Debug.Break();
        
        //Debug.LogWarning("MOVEUP() isCloseShotActive is " + _isCloseShotActive);

        if (_isCloseShotActive == true)
        {
            //Debug.LogWarning("MOVEUP() _isCloseShotActive is " + _isCloseShotActive);
            //Debug.LogWarning("MOVEUP() is launched !! inside IF 1");
            //Debug.Break();

            GameObject target = FindClosestEnemy();
                       
            if (target != null)
            {
                //Debug.LogWarning("MOVE UP is launched !! inside IF 2");

                //Debug.LogError("MOVEUP() we have the closest enemy named > " + target.name);
                //Debug.LogError("MOVEUP() and closest enemy position is> " + target.transform.position);
                //Debug.Break();

                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, _speedL * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.up * _speedL * Time.deltaTime);
            }
        }
        else
        {
            transform.Translate(Vector3.up * _speedL * Time.deltaTime);
        }

        if (transform.position.y > 8f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject, 3);
            }

            Destroy(this.gameObject, 3);
        }
    }

    private void MoveDown()
    {
        if (target != null && _isHeatSeekingActive == true)
        {
            //Debug.LogWarning("target is: " + target);
            //Debug.Break();

             //Debug.LogWarning("HeatSeekingMethod is launched !!");
             //Debug.Break();

             //Transform target = GameObject.Find("Player").GetComponent<Player>().transform;

                // move towards player BUT IF PLAYER'S SPEED is > 5 (so if it SHIFT or got the speed ... it will go down !!
                if ( (target.GetComponent<Player>()._speed <= target.GetComponent<Player>()._initialSpeed) && (transform.position.y > -3f) )
                {
                    //Debug.LogWarning("YOU SHOULD MOVE FASTER !!");
                    transform.position = Vector3.MoveTowards(transform.position, target.position, _speedL /4 * Time.deltaTime);
                }
                else 
                {
                    //Debug.LogWarning("You ESCAPEd for now ... You Lucky ...!!");
                    transform.Translate(Vector3.down * _speedL /2 * Time.deltaTime); 
                }
        }

        else
        {
            transform.Translate(Vector3.down * _speedL / 2 * Time.deltaTime); // from 1A course
        }
        
        if (transform.position.y < -8f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject, 3f);
            }

            Destroy(this.gameObject, 3f);
        }
    }

    public void AssignEnemyLaser(bool laser1, bool laser2, bool laserB)
    {
        _isEnemylaser = laser1; // _isEnemylaser = true;
        _isEnemylaser2 = laser2; //_isEnemylaser2 = true;
        _isLaserBeam = laserB;
    }

    public void AssignEnemyBehaviours(bool enBehind, bool heat)
    {
        _enemyBehindActive = enBehind;
        _isHeatSeekingActive = heat;
    }

    public void AssignLaserBehaviours(bool close)
    {
        _isCloseShotActive = close;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isEnemylaser == true || _isEnemylaser2 == true || _isLaserBeam == true)
        {
            if (other.tag == "Player") //&& (_isEnemylaser == true || _isEnemylaser2 == true || _isLaserBeam == true))
            {
                Player player = other.GetComponent<Player>();

                if (player != null)
                {
                    player.Damage(0, 1, 1);
                }

//                Debug.LogWarning("_enemyBehindActive is " + _enemyBehindActive); /// check if it is behind OR NOT
//                Debug.Break();
                Destroy(this.gameObject, 0.01f);

//                Debug.LogWarning("destroyed laser GO " + this.gameObject); /// check if DESTROY ON IMPACT
//                Debug.Break();

                // old code - see bellow /////////////******************************//////////////
            }

            else if (other.tag == "PowerUP")
            {

                if (other.transform.parent != null)
                {
                    Destroy(other.transform.parent.gameObject, 0.01f);
                }
                Destroy(other.gameObject, 0.01f);
                
                Destroy(this.gameObject, 0.01f);

//                Debug.LogWarning("destroyed laser GO " + this.gameObject); /// check if DESTROY ON IMPACT
//                Debug.Break();
            }
        }

        else
        {
            Debug.LogWarning("what did I colided with ?!?!");
        }

    }

              /*
                        if (player != null)
                        {
                           if (transform.parent.name == "Laser_Left")
                            {
             //                  player.Damage(0,0,1);
                                _hitLeft = true;
                                Debug.Log("hit left");
                            }
                            else if (transform.parent.name == "Laser_Right")
                            {
             //                   player.Damage(0,1,0);
                                _hitRight = true;
                                Debug.Log("hit right");
                            }
                            else
                            {
                               player.Damage(0, 1, 1);
                                _hitLeft = true;
                                _hitRight = true;
                                Debug.Log("BOTH HIT !!");
                            }
                          // if hit both same time?
                           if (_hitLeft == true && _hitRight == true)
                            {
                                player.Damage(0, 1, 1);
                                Debug.Log("success in code BOTH HIT !!");
                            }
                            else
                            {
                                Debug.Log("I am doing something wrong !!!!");
                            }

                        }
            */

}

