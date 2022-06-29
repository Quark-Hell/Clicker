using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DestinationDots
{
    public Transform From;
    public List<Transform> To;
}

public class AI : MonoBehaviour
{
    public List<DestinationDots> Ways;
    public float MovingDelay;

    [Range(0,50)]
    public float MovingSpeed;

    private float MovingDelayTimer;

    private Vector3 FromPoint;
    private Vector3 ToPoint;

    private int IndexCurrentPoint;

    [HideInInspector]
    public SpawnManager spawnManger;

    [HideInInspector]
    public int IndexTypesOfEnemy;

    // Start is called before the first frame update
    void Start()
    {
        FromPoint = transform.position;
        FindingIndexOfSpawnPointInGeneralList();

        MovingDelayTimer = MovingDelay;
    }

    bool FindingIndexOfSpawnPointInGeneralList()
    {
        for (int i = 0; i < spawnManger.PosForSpawn[IndexTypesOfEnemy].Count; i++)
        {
            if (transform.position == spawnManger.PosForSpawn[IndexTypesOfEnemy][i])
            {
                IndexCurrentPoint = i;
                return true;
            }
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {

        CheckPosition();
        Moving();
    }

    bool CheckPosition()
    {
        //timer
        if (MovingDelayTimer > 0)
        {
            MovingDelayTimer -= Time.deltaTime;
        }
        else
        {
            int randDest = Random.Range(0, Ways[IndexCurrentPoint].To.Count);
            for (int i = 0; i < spawnManger.AvailablePosForSpawn[IndexTypesOfEnemy].Count; i++)
            {
                //check available destination
                if (Ways[IndexCurrentPoint].To[randDest].position == spawnManger.AvailablePosForSpawn[IndexTypesOfEnemy][i])
                {
                    //Preparing to moving
                    FromPoint = transform.position;
                    ToPoint = Ways[IndexCurrentPoint].To[randDest].position;
                    spawnManger.AvailablePosForSpawn[IndexTypesOfEnemy].RemoveAt(i);
                    IsMoving = true;

                    //Restart timer
                    MovingDelayTimer = MovingDelay;

                    return true;
                }
            }

            //Restart timer
            MovingDelayTimer = MovingDelay;
        }
        return false;
    }

    private bool IsMoving;
    void Moving()
    {
        if (IsMoving)
        {
            gameObject.transform.position = Vector3.MoveTowards(transform.position, ToPoint, MovingSpeed * Time.deltaTime);
            if (gameObject.transform.position == ToPoint)
            {
                FindingIndexOfSpawnPointInGeneralList();
                spawnManger.AvailablePosForSpawn[IndexTypesOfEnemy].Add(FromPoint);
                IsMoving = false;
            }
        }
    }

    void OnDrawGizmos()
    {
        for (byte i = 0; i < Ways.Count; i++)
        {
            for (byte k = 0; k < Ways[i].To.Count; k++)
            {
                // Draws a blue line from this transform to the target
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(Ways[i].From.position, Ways[i].To[k].position);
            }
        }
    }
}
