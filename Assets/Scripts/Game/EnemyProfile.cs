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
    public float ScoreForKill;
    private bool CanDropBooster;

    [Range(0, 10)]
    [SerializeField] private float SpeedOfChangingHPbar;

    [SerializeField]  private Image HPBar;

    void Start()
    {
        CurrentHP = MaxHP;
    }

    // Update is called once per frame
    void Update()
    {
        Hit();
    }

    private float elapsed = 0;
    void Hit()
    {
        if (elapsed < (MaxHP - CurrentHP) / MaxHP)
        {
            elapsed += Time.deltaTime * SpeedOfChangingHPbar;
        }

        HPBar.fillAmount = Mathf.Lerp(1, 0, elapsed);
    }

    void Death()
    {

    }
}
