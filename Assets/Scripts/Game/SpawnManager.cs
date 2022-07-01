using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public float Difficulty;
    public float IncreaseDifficulty;

    [Range(0, 10)]
    public float SpawnDelay;
    private float TimeRemaining;

    public Player player;

    [HideInInspector] public List<List<Vector3>> PosForSpawn;
    [HideInInspector] public List<List<Vector3>> AvailablePosForSpawn;

    [HideInInspector] public List<GameObject> Enemy;

    [SerializeField] private List<GameObject> StorageOfPos;

    public List<GameObject> TypesOfEnemy;

    [SerializeField] private int MaxEnemyCountToLose;

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

        //Initialize timer
        TimeRemaining = SpawnDelay;
    }

    void Update()
    {
        if (player.IsLose == false)
        {
            SpawnEnemy();
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
                obj.GetComponent<AI>().enabled = true;
                obj.GetComponent<EnemyProfile>().spawnManager = this;
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
            for (int i = 0; i < AvailablePosForSpawn[0].Count; i++)
            {
                Gizmos.DrawSphere(AvailablePosForSpawn[0][i], 1);
            }
        }
    }
}
