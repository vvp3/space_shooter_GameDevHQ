using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Rotating : MonoBehaviour
{
    [SerializeField]
    private float _speed = 1.0f;

    [SerializeField]
    private float nextActionTime = 2.0f;
    //    [SerializeField]
    //    private float stepTime = 0.1f;
    [SerializeField]
    private float timer = 0.0f;

    [SerializeField]
    private Vector3 shiftLeft, shiftRight;
    [SerializeField]
    private Vector3 shiftDown, shiftUp;
    [SerializeField]
    private Vector3 zigLeft, zigRight;

    private Vector3[] movementArrayL;
    private Vector3[] movementArrayR;
    private int indexLeft;
    private int indexRight;

    private bool _enemyAgressiveActive = false;
    private Player _player;


    void Start()
    {
        indexLeft = Random.Range(0, 2);
        indexRight = Random.Range(0, 2);

        movementArrayL = new Vector3[3];
        movementArrayR = new Vector3[3];

        movementArrayL[0] = shiftLeft;
        movementArrayL[1] = shiftDown;
        movementArrayL[2] = zigLeft;

        movementArrayR[0] = shiftRight;
        movementArrayR[1] = shiftUp;
        movementArrayR[2] = zigRight;

        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null)
        {
            Debug.LogError("no player !!");
        }

    }


    void Update()
    {
        timer += Time.deltaTime;

        AgressiveEnemyMovement(); // this is putting _enemyAgressiveActive true OR false

        if (_enemyAgressiveActive == false)
        {
            if (timer >= nextActionTime)
            {
                //            Debug.Log("shifting right");

                // IF AGRESSIVE COME WITH INFO FROM ENEMY.CS


                //IF NOT AGRESSIVE DO BELLOW

                EnemyMovement(movementArrayR[indexRight]);
            }
            else
            {
                //            Debug.Log("shifting left");

                EnemyMovement(movementArrayL[indexLeft]);
            }
        }
    }

    private void EnemyMovement(Vector3 shifting)
    {
               
        if (timer < nextActionTime * 2)
        {
            transform.Translate(shifting * _speed * Time.deltaTime);
            transform.Rotate(shifting * Time.deltaTime, Space.Self);  // rotating around current GO
        }
        else
        {
            StartCoroutine(UpdateIndex());
        }

        //REUSE THE ENEMIES !!//

        if (transform.position.y < -5f || transform.position.x < -10f || transform.position.x > 10f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7, 0);
        }
    }

    IEnumerator UpdateIndex()
    {
        yield return new WaitForSeconds(Random.Range(0, 0.5f));

        indexLeft = Random.Range(0, 2);
        indexRight = Random.Range(0, 2);
        timer = -1.0f;
    }

    private void AgressiveEnemyMovement() //(bool _enemyAgressiveActive)
    {

        float step = _speed * 2 * Time.deltaTime;

        if (_player != null)
        {
            if (((transform.position.x - 1.5f) <= _player.transform.position.x) && ((transform.position.y - 1.5f) <= _player.transform.position.y))
            {
//                Debug.LogError("near player -- enemy will ramm into the player");
                _enemyAgressiveActive = true;

                // move towards player
                transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, step);
            }
            else
            {
//                Debug.LogError("NOT agressive !");
                _enemyAgressiveActive = false;
            }
        }
        else { Debug.Log("Player is Dead !!"); }   
    }

}
