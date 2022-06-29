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
    [Range(0, 1)]
    public float SpeedOfLookingGun;
    [Range(0, 100)]
    public float WeaponRecoilStrong = 5;

    [Range(0, 10)]
    public float MaxDistanceFromCenter;
    private float StartGameObjectZ; //Start pos of Z coordinate


    void Start()
    {

    }


    void Update()
    {
        AimGun();
        Shoot();
    }

    void Shoot()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Gun.transform.eulerAngles = new Vector3(
                Gun.transform.eulerAngles.x - WeaponRecoilStrong,
                Gun.transform.eulerAngles.y,
                Gun.transform.eulerAngles.z);
        }
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            Gun.transform.eulerAngles = new Vector3(
                Gun.transform.eulerAngles.x - WeaponRecoilStrong,
                Gun.transform.eulerAngles.y,
                Gun.transform.eulerAngles.z);
        }
#endif
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
            Gun.transform.rotation = Quaternion.Lerp(Gun.transform.rotation, Quaternion.LookRotation(forward, Vector3.up), SpeedOfLookingGun);
            Gun.transform.eulerAngles = new Vector3(
                Gun.transform.eulerAngles.x,
                Gun.transform.eulerAngles.y,
                0);
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
