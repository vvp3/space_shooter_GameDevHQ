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
            
    }

    void Update()
    {
        CalculateMovement();

        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3, 4);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserEnemy, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i= 0; i< lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }

            // Debug.Break();

        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7, 0);
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
        }
    }           
    


}
