using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private Color gizmoColor = Color.red;
    
    [SerializeField]
    private enum SpawnTypes 
    {
        Normal, Once, Wave, TimedWave
    }
    [SerializeField]
    public enum EnemyLevels
    {
        Easy, Medium, Hard, Boss
    }
    [SerializeField]
    public EnemyLevels enemyLevel = EnemyLevels.Easy;


    [SerializeField]
    private GameObject _enemyPrefab; // old
    [SerializeField]
    private GameObject EasyEnemy, MediumEnemy, HardEnemy, BossEnemy;
    private Dictionary<EnemyLevels, GameObject> Enemies = new Dictionary<EnemyLevels, GameObject>(4);

    [SerializeField]
    private GameObject _rotPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] powerUps;

    [Space]

    [SerializeField]
    private int powerUpsRate = 1;
    
    private bool _stopSpawning = false;

    [ReadOnly] [SerializeField]
    private int totalEnemy = 6;
    [ReadOnly] [SerializeField]
    private int totalEnemyinWave = 2;    
    [SerializeField]
    private int initialEnemy = 6;
    [SerializeField]
    private int initialEnemyinWave = 2;

    [ReadOnly] [SerializeField] 
    private int numEnemy = 0, spawnedEnemy = 0;

    // The ID of the spawner
    [ReadOnly] [SerializeField] 
    private int SpawnID;

    [ReadOnly] [SerializeField] 
    private bool waveSpawn, isShielded = false;
    //    [SerializeField] private bool Spawn = true;
    [SerializeField]
    private SpawnTypes spawnType = SpawnTypes.Normal;
    // timed wave controls
//    [SerializeField]
    private float waveTimer = 30.0f;
//    [SerializeField]
    private float timeTillWave = 0.0f;
    //Wave controls
    [SerializeField]
    private int totalWaves = 3;
    [ReadOnly] [SerializeField]
    private int numWaves = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        // sets a random number for the id of the spawner
        SpawnID = Random.Range(1, 500);
        Enemies.Add(EnemyLevels.Easy, EasyEnemy);
        Enemies.Add(EnemyLevels.Medium, MediumEnemy);
        Enemies.Add(EnemyLevels.Hard, HardEnemy);
        Enemies.Add(EnemyLevels.Boss, BossEnemy);
    }

    // Draws a cube to show where the spawn point is
    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(transform.position, new Vector3(0.5f, 0.5f, 0.5f));
    }
    
    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(1.5f);
        while (_stopSpawning == false)
        {
            //Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0); //course-1a

            // Spawns enemies everytime one dies
            if (spawnType == SpawnTypes.Normal) // not so interesting
            {
                if (numEnemy < totalEnemy)
                {
                    spawnEnemy(enemyLevel);
                }
            }
            // Spawns enemies only once
            else if (spawnType == SpawnTypes.Once)  // not so interesting
            {
                if (spawnedEnemy >= totalEnemy)
                {
                    _stopSpawning = true;
                }
                else
                {
                    spawnEnemy(enemyLevel);
                }
            }

            //spawns enemies in waves --------------------------------- THIS IS OUR CURRENT SPAWNING SYSTEM
            else if (spawnType == SpawnTypes.Wave)
            {
                spawnLogic();
            }
            
            // Spawns enemies in waves but based on time. >>>>>> TO BE COSIDERED  <<<<<<<<<
            else if (spawnType == SpawnTypes.TimedWave)
            {
                // checks if the number of waves is bigger than the total waves
                if (numWaves <= totalWaves)
                {
                    // Increases the timer to allow the timed waves to work
                    timeTillWave += Time.deltaTime;
                    if (waveSpawn)
                    {
                        //spawns an enemy
                        spawnEnemy(enemyLevel);
                    }
                    // checks if the time is equal to the time required for a new wave
                    if (timeTillWave >= waveTimer)
                    {
                        // enables the wave spawner
                        waveSpawn = true;
                        // sets the time back to zero
                        timeTillWave = 0.0f;
                        // increases the number of waves
                        numWaves++;
                        // A hack to get it to spawn the same number of enemies regardless of how many have been killed
                        numEnemy = 0;
                    }
                    if (numEnemy >= totalEnemy)
                    {
                        // diables the wave spawner
                        waveSpawn = false;
                    }
                }
               
            }
                                         
                yield return new WaitForSeconds(2.0f);
        }
    }


    private void spawnLogic ()
    {
        if (numWaves < totalWaves)
        {
            totalEnemy = initialEnemy;
            totalEnemyinWave = initialEnemyinWave;

            switch (enemyLevel)
            {
                case EnemyLevels.Easy:
                    totalEnemy += 0;
                    totalEnemyinWave += 0;
                    powerUpsRate += 0;
                    break;
                case EnemyLevels.Medium:
                    totalEnemy += 3;
                    totalEnemyinWave += 1;
                    powerUpsRate += 1;
                    break;
                case EnemyLevels.Hard:
                    totalEnemy += 6;
                    totalEnemyinWave += 2;
                    powerUpsRate += 2; // = 0 + 1 + 2
                    break;
                case EnemyLevels.Boss:

                    totalEnemy = 1;
                    totalEnemyinWave = 1;
                    totalWaves = 1;
                    powerUpsRate += 2; // = 0 + 1 + 2 + 2
                    break;
            }
            
            if (waveSpawn == true)
            {
                spawnEnemy(enemyLevel);
//                Debug.LogError(enemyLevel);
            }
            if (numEnemy == 0)
            {
                waveSpawn = true;
//                numWaves++;
//                Debug.Log("numEnemy == 0");
            }
            if (numEnemy == totalEnemyinWave)
            {
                waveSpawn = true;
                numEnemy = 0;
                numWaves++;
//                Debug.Log("numEnemy == totalEnemyinWave");
            }
        }

        else if (spawnedEnemy >= totalEnemy)
        {
            switch (enemyLevel)
            {
                case EnemyLevels.Easy:
                    Optimising(EnemyLevels.Medium, true, 0, 0);
                    break;
                case EnemyLevels.Medium:
                    Optimising(EnemyLevels.Hard, true, 0, 0);
                    break;
                case EnemyLevels.Hard:
                    Optimising(EnemyLevels.Boss, true, 0, 0);
                    break;
                case EnemyLevels.Boss:
                    _stopSpawning = true;
                    Debug.Log("keep the boss in the screen ... move it veeeeeery slow !!");
                    break;
            }

 //           Debug.Log("LEVEL " + enemyLevel);
        }

        else // else if (x) >> (x == boss dead ??)
        {
            _stopSpawning = true;
            Debug.Log("STOP SPAWN - NOT GOOOD !!!");
        }
        
    }

    private void Optimising(EnemyLevels eL, bool wS, int nE, int nW) //, bool sH)
    {
        enemyLevel = eL;
        waveSpawn = wS;
        numEnemy = nE;
        numWaves = nW;
 //       isShielded = sH;
        Debug.Log("We are in LEVEL " + eL);
    }

    private void spawnEnemy(EnemyLevels _enemyL)
    {
        Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0); // new - wave esting

        GameObject newRot = Instantiate(_rotPrefab, posToSpawn, Quaternion.identity); // new - quizz 
        //GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity); // course-1a

        newRot.transform.parent = _enemyContainer.transform;

        //        GameObject newEnemy = Instantiate(Enemies[enemyLevel], posToSpawn, Quaternion.identity); 
        GameObject newEnemy = Instantiate(Enemies[_enemyL], posToSpawn, Quaternion.identity);

        if (enemyLevel != EnemyLevels.Boss)
        {
            newEnemy.transform.parent = newRot.transform;
        }
        else
        { Debug.LogError("I am fighting boss !!"); }

        // newEnemy.transform.parent = _enemyContainer.transform; // old - course-1a

        //newEnemy.SendMessage("setName", SpawnID); // HOW TO MAKE IT WORK WITH SPAWN ID IN ENEMY.CS ?
        numEnemy++; 
        spawnedEnemy++; 
    }


    IEnumerator SpawnPowerUpRoutine()
    {
        int x = powerUpsRate;
        yield return new WaitForSeconds(1/x);
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            
            // i need to pass to spawnpoweruproutine rate for each powerup
            // i should have random some of them
            // health rare
            // ammo often


            Instantiate(powerUps[Random.Range(0, 8)], posToSpawn, Quaternion.identity);

            if (powerUps[4])
            {
                yield return new WaitForSeconds(Random.Range(2/x, 5/x));
//                Debug.Log("powerUP Ammo is UP");
            }
            else if (powerUps[5])
            {
                yield return new WaitForSeconds(Random.Range(5/x, 20/x));
//                Debug.Log("powerUP 5Shot is UP");
            }
            else if (powerUps[3])
            {
                if (powerUpsRate >=3) // IF HARD OR BOSS WE NEED LIVES !!
                {
                    yield return new WaitForSeconds(Random.Range(5/x,7/x));
                }
                else 
                {
                    yield return new WaitForSeconds(Random.Range(20/x, 30/x));
                }
//                Debug.Log("powerUP Life is UP");
            }
            else if (powerUps[6])
            {
                yield return new WaitForSeconds(Random.Range(10/x, 20/x));
//                Debug.Log("powerUP SLOW is UP");
            }            
            else if (powerUps[7])
            {
                yield return new WaitForSeconds(Random.Range(30/x, 40/x));
//                Debug.Log("powerUP CLOSE is UP");
            }
            else
            {
                yield return new WaitForSeconds(Random.Range(3/x, 8/x));
//                Debug.Log("powerUP - others");
            }

        }
    }

    
    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }


    // -----------------------------------------------------------------------------
/*
    // Call this function from the enemy when it "dies" to remove an enemy count
    public void killEnemy(int sID)
    {
        // if the enemy's spawnId is equal to this spawnersID then remove an enemy count
        if (SpawnID == sID)
        {
            numEnemy--;
        }
    }
    //enable the spawner based on spawnerID
    public void enableSpawner(int sID)
    {
        if (SpawnID == sID)
        {
            _stopSpawning = false;
        }
    }
    //disable the spawner based on spawnerID
    public void disableSpawner(int sID)
    {
        if (SpawnID == sID)
        {
            _stopSpawning = true;
        }
    }
    // returns the Time Till the Next Wave, for a interface, ect.
    public float TimeTillWave
    {
        get
        {
            return timeTillWave;
        }
    }
    // Enable the spawner, useful for trigger events because you don't know the spawner's ID.
    public void enableTrigger()
    {
        _stopSpawning = false;
    }
*/


}
