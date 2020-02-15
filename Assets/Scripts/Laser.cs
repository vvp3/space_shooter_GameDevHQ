using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speedL = 8.0f;
    [SerializeField]
    private bool _isEnemylaser = false;
    private bool _hitLeft = false;
    private bool _hitRight = false;

    private void Start()
    {
  //      _LaserLeft = this.transform.parent.tag

    }

    void Update()
    {
       
        if (_isEnemylaser == false)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }
        

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

    public void AssignEnemyLaser()
    {
        _isEnemylaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       if (other.tag == "Player" && _isEnemylaser == true)
        {
            Player player = other.GetComponent<Player>();

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
