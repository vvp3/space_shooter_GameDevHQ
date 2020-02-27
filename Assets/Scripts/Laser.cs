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

    private void Start()
    {
        //      _LaserLeft = this.transform.parent.tag

        lr = GetComponent<LineRenderer>();

    }

    void Update()
    {

        if (_isEnemylaser == false && _isEnemylaser2 == false && _isLaserBeam == false)
        {
            MoveUp();
            //            Debug.LogError(_isEnemylaser);
            //            Debug.LogError(_isEnemylaser2);
        }
        else if ((_isEnemylaser || _isEnemylaser2) == true && _isLaserBeam == false)
        {
            MoveDown();
//            Debug.LogError(_isEnemylaser);
//            Debug.LogError(_isEnemylaser2);
        }
        else if (_isLaserBeam == true)
        {
            laserBeam();
        }
        else
        {
            Debug.LogError(_isEnemylaser + "bug ?");
            Debug.LogError(_isEnemylaser2 + "bug2 ?");
        }
        

    }

    void laserBeam()
    {
        lr.SetPosition(0, transform.position);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider)
            {
                lr.SetPosition(1, hit.point);
            }
        }
        else lr.SetPosition(1, transform.forward * 5000);
    }


    void MoveUp()
    {
        transform.Translate(Vector3.up * _speedL * Time.deltaTime);

        if (transform.position.y > 8f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject, 3);
            }

            Destroy(this.gameObject, 3);
        }
    }

    void MoveDown()
    {
        transform.Translate(Vector3.down * _speedL * Time.deltaTime);

        if (transform.position.y < -8f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject, 3);
            }

            Destroy(this.gameObject, 3);
        }
    }

    public void AssignEnemyLaser(bool laser1, bool laser2, bool laserB)
    {
        _isEnemylaser = laser1; // _isEnemylaser = true;
        _isEnemylaser2 = laser2; //_isEnemylaser2 = true;
        _isLaserBeam = laserB;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       if (other.tag == "Player" && (_isEnemylaser == true || _isEnemylaser2 == true))
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
/*               if (transform.parent.name == "Laser_Left")
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
*/                    player.Damage(0, 1, 1);
/*                    _hitLeft = true;
                    _hitRight = true;
                    Debug.Log("BOTH HIT !!");
                }
 */               // if hit both same time?

 /*               if (_hitLeft == true && _hitRight == true)
                {
                    player.Damage(0, 1, 1);
                    Debug.Log("success in code BOTH HIT !!");
                }
                else
                {
                    Debug.Log("I am doing something wrong !!!!");
                }
*/
            }

        }
        

    }

}
