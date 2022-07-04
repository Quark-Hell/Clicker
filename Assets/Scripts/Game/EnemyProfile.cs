using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EnemyProfile : MonoBehaviour
{
    public float MaxHP;
    private float currentHP;
    [HideInInspector] public float CurrentHP
    {
        set
        {
            if (value < 0 || value > MaxHP) { currentHP = 0; }
            else { currentHP = value; }
        }
        get { return currentHP; }
    }
    public int ScoreForKill;

    public GameObject IceCube;
    private Vector3 StartIceSize;

    [SerializeField] private GameObject Booster;

    [Range(0, 1)]
    [SerializeField] private float ChangeOfDrop;
    [SerializeField] private bool CanDropBooster;
    
    [HideInInspector] public SpawnManager spawnManager;

    [Range(0, 5)]
    [SerializeField] private float SpeedOfChangingHPBar;

    [SerializeField] private Image HPBar;

    private AI ai;

    void Start()
    {
        CurrentHP = MaxHP;

        StartIceSize = IceCube.transform.localScale;

        ai = gameObject.transform.GetComponent<AI>();
    }

    // Update is called once per frame
    void Update()
    {
        HPbarAnimation();

        if (spawnManager.IsSpawnFreeze)
        {
            Unfreezy();
        }

        if (CurrentHP == 0)
        {
            Death();
        }
    }

    public void BeHit(float damage)
    {
        CurrentHP -= damage;
    }

    private float elapsed = 0;
    void HPbarAnimation()
    {
        if (elapsed < (MaxHP - CurrentHP) / MaxHP)
        {
            elapsed += Time.deltaTime * SpeedOfChangingHPBar;
        }

        HPBar.fillAmount = Mathf.Lerp(1, 0, elapsed);
    }

    void DropBooster()
    {
        Quaternion rotation = new Quaternion(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360), Quaternion.identity.w);
        Transform obj = Instantiate(Booster, transform.position, rotation).transform;

        obj.GetComponent<Boosters>().spawnManager = spawnManager;

        #region Set random type booster
        System.Array values = System.Enum.GetValues(typeof(TypesBooster));
        System.Random random = new System.Random();
        TypesBooster randomType = (TypesBooster)values.GetValue(random.Next(values.Length));

        obj.GetComponent<Boosters>().typesBooster = randomType;
        #endregion
    }

    public void Unfreezy()
    {
        Vector3 speed;
        speed.x = StartIceSize.x / spawnManager.TimeForUnfreezy * Time.deltaTime;
        speed.y = StartIceSize.y / spawnManager.TimeForUnfreezy * Time.deltaTime;
        speed.z = StartIceSize.z / spawnManager.TimeForUnfreezy * Time.deltaTime;

        IceCube.transform.localScale -= speed;
        if (IceCube.transform.localScale.x <= 0)
        {
            spawnManager.IsSpawnFreeze = false;
            IceCube.SetActive(false);
            IceCube.transform.localScale = StartIceSize;
        }
    }

    public void DeathWithoutDropBooster()
    {
        //Get score
        spawnManager.player.Score += ScoreForKill;
        spawnManager.player.ScoreText.text = "Score: " + spawnManager.player.Score;

        spawnManager.AvailablePosForSpawn[ai.IndexTypesOfEnemy].Add(ai.CurrentPoint);

        //Delete self
        spawnManager.Enemy.Remove(gameObject);
        Destroy(gameObject);
    }

    public void Death()
    {
        //Get score
        spawnManager.player.Score += ScoreForKill;
        spawnManager.player.ScoreText.text = "Score: " + spawnManager.player.Score;

        //Drop booster
        if (CanDropBooster)
        {
            float rand = Random.Range(0, 1f);
            if (rand <= ChangeOfDrop)
            {
                DropBooster();
            }
        }

        spawnManager.AvailablePosForSpawn[ai.IndexTypesOfEnemy].Add(ai.CurrentPoint);

        //Delete self
        spawnManager.Enemy.Remove(gameObject);
        Destroy(gameObject);
    }
}
