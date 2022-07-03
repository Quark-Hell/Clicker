using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TipsAnim : MonoBehaviour
{
    [SerializeField]
    private List<string> TipsList;

    [Range(0, 1)]
    [SerializeField] private float MinSize;
    [Range(1, 2)]
    [SerializeField] private float MaxSize;

    [SerializeField]
    [Range(0, 1)]
    private float Speed;

    private RectTransform TipRect;
    private Text TipText;
    private Text OutlineTipText;
    private float sizeBuff;

    void Start()
    {
        TipRect = gameObject.GetComponent<RectTransform>();
        TipText = gameObject.GetComponent<Text>();
        OutlineTipText = gameObject.transform.GetChild(0).GetComponent<Text>();

        sizeBuff = TipRect.localScale.x;

        int randTips = Random.Range(0, TipsList.Count);

        TipText.text = TipsList[randTips];
        OutlineTipText.text = TipsList[randTips];
    }


    void Update()
    {
        ChangingSize();
    }

    void ChangingSize()
    {
        sizeBuff += Speed * Time.deltaTime;

        //Invert
        if (sizeBuff > MaxSize)
        {
            sizeBuff = MaxSize;
            Speed = -Speed;
        }
        else if (sizeBuff < MinSize)
        {
            sizeBuff = MinSize;
            Speed = -Speed;
        }

        TipRect.localScale = new Vector2(sizeBuff, sizeBuff);
    }
}
