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

    public List<List<Vector3>> PosForSpawn;
    public List<List<Vector3>> FreePosForSpawn;

    [SerializeField]
    private List<GameObject> StorageOfPos;

    public List<GameObject> Enemy;

    void Start()
    {
        PosForSpawn = new List<List<Vector3>>();
        FreePosForSpawn = new List<List<Vector3>>();

        //Initialize list with pos for spawn
        for (byte i = 0; i < StorageOfPos.Count; i++)
        {
            PosForSpawn.Add(new List<Vector3>());

            for (byte k = 0; k < StorageOfPos[i].transform.childCount; k++)
            {
                PosForSpawn[i].Add(StorageOfPos[i].transform.GetChild(i).transform.position);
            }
        }
        FreePosForSpawn = PosForSpawn;

        //Initialize timer
        TimeRemaining = SpawnDelay;
    }

    void Update()
    {
        SpawnEnemy();
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
            #region Spawn Enemy
            int randEnemy = Random.Range(0, Enemy.Count);
            int randPos = Random.Range(0, FreePosForSpawn[randEnemy].Count);
            Instantiate(Enemy[randEnemy], FreePosForSpawn[randEnemy][randPos], Quaternion.identity);
            #endregion

            //Deleting point from list of available for spawning
            FreePosForSpawn[randEnemy].RemoveAt(randPos);

            //Restart timer
            TimeRemaining = SpawnDelay;
            return true;
        }

        return false;
    }
}
