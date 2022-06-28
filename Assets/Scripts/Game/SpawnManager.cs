using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PosForSpawn
{
    public Vector3 Position;
    public float DelayTimer;
    public float Delay;

    public PosForSpawn(Vector3 position, float delay)
    {
        Position = position;
        DelayTimer = delay;
        Delay = delay;
    }
}

public class SpawnManager : MonoBehaviour
{
    public float Difficulty;
    public float IncreaseDifficulty;

    [Range(0, 10)]
    public float SpawnDelay;
    private float TimeRemaining;

    //[HideInInspector]
    public List<PosForSpawn> PositionForSpawn;
    private List<PosForSpawn> FreeForSpawn;

    [SerializeField]
    private GameObject StorageOfPos;

    public List<GameObject> Enemy;

    void Start()
    {
        //Initialize list with pos for spawn
        for (byte i = 0; i < StorageOfPos.transform.childCount; i++)
        {
            PositionForSpawn.Add(new PosForSpawn(StorageOfPos.transform.GetChild(i).transform.position, 5));
        }
        FreeForSpawn = PositionForSpawn;

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
            int randPos = Random.Range(0, FreeForSpawn.Count);
            Instantiate(Enemy[0], FreeForSpawn[randPos].Position, Quaternion.identity);

            TimeRemaining = SpawnDelay;
            return true;
        }

        return false;
    }
}
