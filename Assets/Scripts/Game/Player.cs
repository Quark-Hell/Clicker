using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Range(0, 10)]
    public float SpeedOfMoving;
    [Range(0, 10)]
    public float SpeedOfRotate;

    public GameObject FieldOfFixationShoot;

    public GameObject Gun;
    public AudioSource GunSound;

    [SerializeField] private AudioClip PistolCockSound;
    [SerializeField] private AudioClip PistolShootSound;

    [Range(0, 1)]
    public float SpeedOfLookingGun;
    [Range(0, 100)]
    public float WeaponRecoilStrong = 5;
    [Range(0, 100)]
    public float Damage;

    [Range(0, 10)]
    public float MaxDistanceFromCenter;

    private float StartGameObjectZ; //Start pos of Z coordinate

    void Start()
    {
        FieldPos = FieldOfFixationShoot.transform.localPosition;
        FieldWidth = FieldOfFixationShoot.GetComponent<RectTransform>().rect.width;
        FieldHeight = FieldOfFixationShoot.GetComponent<RectTransform>().rect.height;
    }

    void Update()
    {
        if (AboveField())
        {
            AimGun();
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hit;
        Ray ray;
        float distance = 1000;
        int layerMask = 1 << 6;

  
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            //Recoil
            Gun.transform.eulerAngles = new Vector3(
                Gun.transform.eulerAngles.x - WeaponRecoilStrong,
                Gun.transform.eulerAngles.y,
                Gun.transform.eulerAngles.z);

            //Gun sound
            GunSound.PlayOneShot(PistolShootSound);

            //Hit
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, distance, layerMask))
            {
                hit.transform.parent.parent.GetComponent<EnemyProfile>().CurrentHP -= Damage;
            }
        }
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            Gun.transform.eulerAngles = new Vector3(
                Gun.transform.eulerAngles.x - WeaponRecoilStrong,
                Gun.transform.eulerAngles.y,
                Gun.transform.eulerAngles.z);

            //Hit
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //Gun sound
            GunSound.PlayOneShot(PistolShootSound);

            if (Physics.Raycast(ray, out hit, distance, layerMask))
            {
                hit.transform.parent.parent.GetComponent<EnemyProfile>().CurrentHP -= Damage;
            }
        }
#endif
    }

    Plane plane = new Plane(Vector3.up, 0);
    private Vector3 FieldPos;
    private float FieldWidth;
    private float FieldHeight;

    bool AboveField()
    {
        Vector3 ViewportMousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        ViewportMousePos.x *= Screen.width;
        ViewportMousePos.y *= Screen.height;

        if (ViewportMousePos.x <= Screen.width - (Screen.width - FieldWidth - FieldPos.x * 2) && ViewportMousePos.x >= Screen.width - FieldWidth)
        {
            if (ViewportMousePos.y <= Screen.height - (Screen.height - FieldHeight - FieldPos.y * 2) && ViewportMousePos.y >= Screen.height - FieldHeight)
            {
                return true;
            }
        }
        return false;
    }
    void AimGun()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        float distance = 1000;
        int layerMask = 1 << 7;

        if (Physics.Raycast(ray, out hit, distance, layerMask))
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
