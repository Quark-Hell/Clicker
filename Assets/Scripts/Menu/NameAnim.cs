using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class NameAnim : MonoBehaviour
{
    [Range(0, 1)]
    public float MinSize;
    [Range(1, 2)]
    public float MaxSize;
    [Range(0, 1)]
    public float Speed;

    private RectTransform Name;
    private float sizeBuff;

    void Start()
    {
        print("work");
        Name = gameObject.GetComponent<RectTransform>();

        sizeBuff = Name.localScale.x;
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

        Name.localScale = new Vector2(sizeBuff, sizeBuff);
    }
}
