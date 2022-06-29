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

    [HideInInspector] public float elapsed = 1;
    private float bufHP;
    void Hit()
    {
        if (elapsed < 1)
        {
            elapsed += Time.deltaTime * SpeedOfChangingHPbar;
        }
        else
        {
            bufHP = CurrentHP / MaxHP;
        }
        HPBar.fillAmount = Mathf.Lerp(bufHP, CurrentHP / MaxHP, elapsed);
    }

    void Death()
    {

    }
}
