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

    [SerializeField] private List<DestinationDots> Ways;
    [SerializeField] private float MovingDelay;

    [Range(0, 50)]
    [SerializeField] private float MovingSpeed;

    [Range(0, 30)]
    [SerializeField] private float TiltDisplacement;
    [Range(0, 30)]
    [SerializeField] private float TiltSpeed;

    [SerializeField] private GameObject AIMesh;

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

        minimum = -TiltDisplacement;
        maximum = TiltDisplacement;
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
        TiltPingPong();
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

    private float minimum, maximum;
    private float _timePassed;
    void TiltPingPong()
    {
        _timePassed += Time.deltaTime;
        float side = Mathf.PingPong(_timePassed * TiltSpeed, maximum - minimum) + minimum;

        AIMesh.transform.localEulerAngles = new Vector3(side, 0 , 0);
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
