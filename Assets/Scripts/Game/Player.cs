using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Range(0, 10)]
    public float SpeedOfMoving;
    [Range(0, 10)]
    public float SpeedOfRotate;

    public GameObject FieldOfFixationShoot;

    [SerializeField] private SpawnManager spawnManager;

    public GameObject Gun;
    public GameObject Pistol;
    public GameObject Rifle;
    public AudioSource GunSound;

    private bool HavePistol = true;

    private GameObject PistolFire;
    private GameObject RifleFire;

    [SerializeField] private AudioClip PistolCockSound;
    [SerializeField] private AudioClip PistolShootSound;

    public Text ScoreText;
    public Text ScoreTextOnLoseMenu;
    public int Score;

    [Range(0, 1)]
    public float SpeedOfLookingGun;
    [Range(0, 100)]
    public float WeaponRecoilStrong = 5;
    [Range(0, 100)]
    public float Damage;

    [Range(0, 10)]
    public float MaxDistanceFromCenter;

    public bool IsLose;
    public GameObject LoseMenu;

    private float StartGameObjectZ; //Start pos of Z coordinate

    void Start()
    {
        FieldPos = FieldOfFixationShoot.transform.localPosition;
        FieldWidth = FieldOfFixationShoot.GetComponent<RectTransform>().rect.width;
        FieldHeight = FieldOfFixationShoot.GetComponent<RectTransform>().rect.height;

        ScoreText.text = "Score: " + Score;

        StartGameObjectZ = transform.position.z;

        Timer = spawnManager.StartSpawnDelay;
        DelayText.text = "" + Timer;

        PistolFire = Pistol.transform.GetChild(0).gameObject;
        RifleFire = Rifle.transform.GetChild(0).gameObject;
    }

    void Update()
    {
        if (GameNotStarted)
        {
            StartGameEffect();
        }
        if (AboveField() && IsLose == false)
        {
            AimGun();
            Shoot();
            //Just effect of fire
            FireFading();
        }
    }

    [SerializeField] private int MaxTextSize;
    [SerializeField] private int MinTextSize;
    [SerializeField] private Text DelayText;
    [SerializeField] private AudioSource MusicSource;
    private float Elapsed = 1;
    private float Timer;
    [HideInInspector] public bool GameNotStarted = true;
    void StartGameEffect()
    {
        //Timer
        Elapsed -= Time.deltaTime;
        //Restart timer
        if (Elapsed <= 0)
        {
            //Text changing
            if (Timer - 1 == 0)
            {
                Timer--;
                MusicSource.Play();
                DelayText.fontSize = MaxTextSize;
                DelayText.text = "Fire!!!";
            }
            else if (Timer == 0)
            {
                GameNotStarted = false;
                DelayText.text = "";
            }
            else
            {
                Timer--;
                DelayText.fontSize = MaxTextSize;
                DelayText.text = "" + Timer;
            }
            Elapsed = 1;
        }

        //FontSize changing
        DelayText.fontSize = (int)Mathf.Lerp(MinTextSize, MaxTextSize , Elapsed);
    }

    private float FireElapsed = 0.1f;
    private bool IsFire;
    void FireFading()
    {
        if (IsFire)
        {
            FireElapsed -= Time.deltaTime;
            print(FireElapsed);
            if (FireElapsed <= 0)
            {
                FireElapsed = 0.1f;
                IsFire = false;
                //Disable gun fire
                if (HavePistol)
                {
                    PistolFire.SetActive(false);
                }
                else
                {
                    RifleFire.SetActive(false);
                }
            }
        }
    }

    void Shoot()
    {
        RaycastHit hit;
        Ray ray;
        float distance = 1000;
        int layerMask = (1 << 6) | (1 << 8);

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            //Recoil
            Gun.transform.eulerAngles = new Vector3(
                            Gun.transform.eulerAngles.x - WeaponRecoilStrong,
                            Gun.transform.eulerAngles.y,
                            Gun.transform.eulerAngles.z);

            //Hit
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //Gun sound
            GunSound.PlayOneShot(PistolShootSound);

            //Fire effect
            IsFire = true;
            if (HavePistol)
            {
                PistolFire.SetActive(true);
            }
            else
            {
                RifleFire.SetActive(true);
            }

            if (Physics.Raycast(ray, out hit, distance, layerMask))
            {
                if (hit.transform.gameObject.layer == 6)
                {
                    hit.transform.parent.parent.GetComponent<EnemyProfile>().BeHit(Damage);
                }
                else if (hit.transform.gameObject.layer == 8)
                {
                    hit.transform.parent.parent.GetComponent<Boosters>().BeHit();
                }
            }
        }
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            //Recoil
            Gun.transform.eulerAngles = new Vector3(
                Gun.transform.eulerAngles.x - WeaponRecoilStrong,
                Gun.transform.eulerAngles.y,
                Gun.transform.eulerAngles.z);

            //Hit
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //Gun sound
            GunSound.PlayOneShot(PistolShootSound);

            //Fire effect
            IsFire = true;
            if (HavePistol)
            {
                PistolFire.SetActive(true);
            }
            else
            {
                RifleFire.SetActive(true);
            }

            if (Physics.Raycast(ray, out hit, distance, layerMask))
            {
                if (hit.transform.gameObject.layer == 6)
                {
                    hit.transform.parent.parent.GetComponent<EnemyProfile>().BeHit(Damage);
                }
                else if (hit.transform.gameObject.layer == 8)
                {
                    hit.transform.GetComponent<Boosters>().ReturnSize();
                    hit.transform.GetComponent<Boosters>().BeHit();
                }
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

    public void Lose()
    {
        ScoreTextOnLoseMenu.text = "Your score:" + Score;
        LoseMenu.SetActive(true);
    }
}
