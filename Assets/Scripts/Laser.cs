using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speedL = 8.0f;
    private bool _isEnemylaser = false;
    
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
                player.Damage();
            }

        }
        

    }

}
