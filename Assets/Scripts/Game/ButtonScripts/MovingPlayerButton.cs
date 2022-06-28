using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class MovingPlayerButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Player player;
    public bool IsToLeft;

    private float Side;
    private bool buttonPressed;

    void Start()
    {
        if (IsToLeft)
        {
            Side = 1;
        }
        else
        {
            Side = -1;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
    }

    void Update()
    {
        if (buttonPressed)
        {
            player.Moving(Side);
        }
    }
}
