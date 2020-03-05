using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private int powerupID;
    [SerializeField]
    private AudioClip _audioClip;

    [SerializeField]
    private bool _isFastPickupsActive = false;

    private Player _player;


    private void Start()
    {
        _player = GameObject.Find("Player").GetComponentInChildren<Player>();

        if (_player == null)
        {
            Debug.LogError("no player !!");
        }
    }


    void Update()
    {
        if (_isFastPickupsActive == true && _player != null)
        {
            Debug.Log("isFastPickupsActive = " + _isFastPickupsActive);
            transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _speed * 5 * Time.deltaTime);
        }

        else if (_isFastPickupsActive == false)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }

        if (transform.position.y < -7)
        {
            Destroy(this.gameObject);
        }
    }

    public void AssignFastPickups(bool grabPickups)
    {
        Debug.Log("AssignFastPickups bool grabPickups = " + grabPickups);
        _isFastPickupsActive = grabPickups;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
           
            AudioSource.PlayClipAtPoint(_audioClip, transform.position, 1.0f); 
            
                        
            if (player != null)
            {
                switch (powerupID)
                {
                    case 0:
                        player.SpeedBoostActive();
                        break;
                    case 1:
                        player.ShieldActive();
                        break;
                    case 2:
                        player.TripleShotActive();
                        break;
                    case 3:
                        player.LifeRefillActive();
                        break;
                    case 4:
                        player.AmmoRefillActive();
                        break;
                    case 5:
                        player.FiveShotActive();
                        break;
                    case 6:
                        player.SlowActive();
                        break;
                    case 7:
                        player.CloseShotActive();
                        break;
                }

                Destroy(this.gameObject);
            }
        }
    }



}
