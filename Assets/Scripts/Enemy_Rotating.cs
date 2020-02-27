using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Rotating : MonoBehaviour
{
    [SerializeField]
    private float _speed = 1.0f;

    //    [SerializeField]
    //    private Vector3 target = new Vector3(5.0f, 0.0f, 0.0f);
    [SerializeField]
    private float nextActionTime = 0.0f;
    [SerializeField]
    private float period = 1.2f;
    [SerializeField]
    private Vector3 shiftLeft, shiftRight;


    // Update is called once per frame
    void Update()
    {
        EnemyMovement(shiftLeft);
        
        //the code bellow would be nice if it would work correctly ...
        
               if (Time.time > nextActionTime)
               {
                   nextActionTime = Time.time + period;
                   EnemyMovement(shiftLeft);
                   //Debug.Log("shifted left");
               }
               else
               {
                   EnemyMovement(shiftRight);
                   //Debug.Log("shifted right");
               }
               
    }

    public void EnemyMovement(Vector3 shifting)
    {

        transform.Translate(shifting * _speed * Time.deltaTime);

    //    transform.RotateAround(target, Vector3.forward, 30 * Time.deltaTime);  // rotating around world axis ...

/* old rotation from course
 * no need anymore ?
 */
        if (transform.position.y < -5f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7, 0);
        }

    }

}
