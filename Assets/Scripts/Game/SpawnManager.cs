using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public float Difficulty = 1;
    [Range(0, 10)]
    public float IncreaseDifficulty = 1;

    [Range(0, 10)]
    public float StartSpawnDelay;
    public float SpawnDelay
    {
        set
        {
            if (value < 0.4) { spawnDelay = 0.4f; }
            else { spawnDelay = value; }
        }
        get { return spawnDelay; }
    }
    private float spawnDelay;
    private float TimeRemaining;

    public Player player;

    [HideInInspector] public List<List<Vector3>> PosForSpawn;
    [HideInInspector] public List<List<Vector3>> AvailablePosForSpawn;

    [HideInInspector] public List<GameObject> Enemy;

    [SerializeField] private List<GameObject> StorageOfPos;

    public List<GameObject> TypesOfEnemy;

    [SerializeField] private int MaxEnemyCountToLose;

    [HideInInspector] public bool IsSpawnFreeze;

    void Start()
    {
        PosForSpawn = new List<List<Vector3>>();
        AvailablePosForSpawn = new List<List<Vector3>>();

        //Initialize list with pos for spawn
        for (byte i = 0; i < StorageOfPos.Count; i++)
        {
            PosForSpawn.Add(new List<Vector3>());
            AvailablePosForSpawn.Add(new List<Vector3>());

            for (byte k = 0; k < StorageOfPos[i].transform.childCount; k++)
            {
                PosForSpawn[i].Add(StorageOfPos[i].transform.GetChild(k).transform.position);
                AvailablePosForSpawn[i].Add(StorageOfPos[i].transform.GetChild(k).transform.position);
            }
        }

        spawnDelay = StartSpawnDelay;

        //Initialize timer
        TimeRemaining = StartSpawnDelay;
    }

    void Update()
    {
        if (player.IsLose == false && IsSpawnFreeze == false)
        {
            SpawnEnemy();
            if (player.GameNotStarted == false)
            {
                IncreaseDifficult();
                print(SpawnDelay);
            }
        }
    }

    void IncreaseDifficult()
    {
        Difficulty += IncreaseDifficulty / 10 * Time.deltaTime;
        if (SpawnDelay - Difficulty / 50 * Time.deltaTime >= 0)
        {
            SpawnDelay -= Difficulty / 50 * Time.deltaTime;
        }
    }

    bool SpawnEnemy()
    {
        //timer
        if (TimeRemaining > 0)
        {
            TimeRemaining -= Time.deltaTime;
        }
        else
        {
            if (Enemy.Count > MaxEnemyCountToLose)
            {
                player.Lose();
                player.IsLose = true;
            }
            else
            {
                #region Spawn Enemy
                int randEnemy = Random.Range(0, TypesOfEnemy.Count);
                int randPos = Random.Range(0, AvailablePosForSpawn[randEnemy].Count);

                if (AvailablePosForSpawn[randEnemy].Count != 0)
                {
                    Transform obj = Instantiate(TypesOfEnemy[randEnemy], AvailablePosForSpawn[randEnemy][randPos], Quaternion.identity).transform;

                    obj.GetComponent<AI>().IndexTypesOfEnemy = randEnemy;
                    obj.GetComponent<AI>().spawnManager = this;
                    obj.GetComponent<AI>().MovingDelay -= Difficulty / 5;
                    obj.GetComponent<AI>().enabled = true;
                    obj.GetComponent<EnemyProfile>().spawnManager = this;
                    obj.GetComponent<EnemyProfile>().MaxHP *= Difficulty;
                    obj.GetComponent<EnemyProfile>().enabled = true;

                    Enemy.Add(obj.gameObject);

                    //Deleting point from list of available for spawning
                    AvailablePosForSpawn[randEnemy].RemoveAt(randPos);
                }
                #endregion

                //Restart timer
                TimeRemaining = SpawnDelay;
                return true;
            }
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        if (AvailablePosForSpawn != null)
        {
            for (int i = 0; i < AvailablePosForSpawn.Count; i++)
            {
                for (int k = 0; k < AvailablePosForSpawn[i].Count; k++)
                {
                    Gizmos.DrawSphere(AvailablePosForSpawn[i][k], 1);
                }
            }
        }

    }
}
