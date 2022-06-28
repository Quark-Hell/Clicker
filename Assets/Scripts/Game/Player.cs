using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Range(0, 10)]
    public float SpeedOfMoving;
    [Range(0, 10)]
    public float SpeedOfRotate;

    public GameObject Gun;

    [Range(0, 10)]
    public float MaxDistanceFromCenter;
    private float StartGameObjectZ; //Start pos of Z coordinate


    void Start()
    {

    }


    void Update()
    {
        AimGun();
    }

    void Shoot()
    {
        //float WeaponRecoilStrong = 5;
    }

    Plane plane = new Plane(Vector3.up, 0);
     void AimGun()
    {
        Ray ray;
        float distance = 1000;

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out distance))
        {
            Vector3 forward = ray.GetPoint(distance) - Gun.transform.position;
            Gun.transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
        }
    }

    //Calling in MovingPlayerButton
    public void Moving(float Side)
    {
        float increasePos = gameObject.transform.position.z + SpeedOfMoving * Side * Time.deltaTime;
        if (increasePos < StartGameObjectZ + MaxDistanceFromCenter && increasePos > StartGameObjectZ - MaxDistanceFromCenter)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x,
                                         gameObject.transform.position.y,
                                         increasePos);

            RotateCamera(Side);
        }
    }

    //Rotate doing only if player moving
    void RotateCamera(float Side)
    {
        transform.eulerAngles = new Vector3(16, transform.eulerAngles.y + SpeedOfRotate * Side * Time.deltaTime, 0);
    }
}
