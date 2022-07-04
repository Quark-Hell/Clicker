using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypesBooster
{
    Bomb,
    Freezer,
    AutomaticGun
}

public class Boosters : MonoBehaviour
{
    [HideInInspector] public TypesBooster typesBooster;
    [HideInInspector] public SpawnManager spawnManager;

    private Vector3 StartSize;
    private Vector3 StartCollisionSize;

    [Range(0, 1)]
    [SerializeField] private float SpeedDisappear;
    [SerializeField] private Vector3 MinSizeToDeath;

    private int Durability = 3;

    private BoxCollider LinkToCollider;

    void Start()
    {
        LinkToCollider = transform.GetComponent<BoxCollider>();

        StartSize = transform.localScale;
        StartCollisionSize = LinkToCollider.size;
    }

    // Update is called once per frame
    void Update()
    {
        Disappear();
        if (transform.localScale.x <= MinSizeToDeath.x)
        {
            Destroy(gameObject);
        }
        if (Durability <= 0)
        {
            GetRewerd();
            Destroy(gameObject);
        }
    }

    public void BeHit()
    {
        Durability--;
    }

    private const float t = 0.2f;
    private void Disappear()
    {
        Vector3 bufScale = transform.localScale;
        bufScale.x -= Mathf.Lerp(0, StartSize.x, (SpeedDisappear / StartSize.x * Time.deltaTime) * t);
        bufScale.y -= Mathf.Lerp(0, StartSize.y, (SpeedDisappear / StartSize.y * Time.deltaTime) * t);
        bufScale.z -= Mathf.Lerp(0, StartSize.z, (SpeedDisappear / StartSize.z * Time.deltaTime) * t);
        transform.localScale = bufScale;

        Vector3 bufCollisionScale = LinkToCollider.size;
        bufCollisionScale.x -= Mathf.Lerp(0, StartCollisionSize.x, (SpeedDisappear * StartCollisionSize.x * Time.deltaTime) * t);
        bufCollisionScale.y -= Mathf.Lerp(0, StartCollisionSize.y, (SpeedDisappear * StartCollisionSize.y * Time.deltaTime) * t);
        bufCollisionScale.z -= Mathf.Lerp(0, StartCollisionSize.z, (SpeedDisappear * StartCollisionSize.z * Time.deltaTime) * t);
        LinkToCollider.size = bufCollisionScale;
    }

    public void ReturnSize()
    {
        transform.localScale = StartSize;
        transform.GetComponent<BoxCollider>().size = StartCollisionSize;
    }

    private void GetRewerd()
    {
        if (typesBooster == TypesBooster.Bomb)
        {
            for (int i = spawnManager.Enemy.Count - 1; i >= 0; i--)
            {
                spawnManager.Enemy[i].GetComponent<EnemyProfile>().DeathWithoutDropBooster();
            }
        }
        if (typesBooster == TypesBooster.Freezer)
        {
            for (int i = 0; i < spawnManager.Enemy.Count; i++)
            {
                spawnManager.Enemy[i].GetComponent<EnemyProfile>().IceCube.SetActive(true);
                spawnManager.IsSpawnFreeze = true;
            }
        }
        if (typesBooster == TypesBooster.AutomaticGun)
        {
            spawnManager.player.GetRifle();
        }
    }
}
