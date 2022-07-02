using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Speed parameters")]
    [Range(0, 10)]
    public float SpeedOfMoving;
    [Range(0, 10)]
    public float SpeedOfRotate;
    [Range(0, 1)]
    public float SpeedOfLookingGun;

    public GameObject FieldOfFixationShoot;

    [Header("Link to spawn manager")]
    [SerializeField] private SpawnManager spawnManager;

    [Header("Gun source")]
    public GameObject Gun;
    public GameObject Pistol;
    public GameObject Rifle;

    [Header("Gun sound")]
    public AudioSource GunSound;
    [SerializeField] private AudioClip CockSound;
    [SerializeField] private AudioClip ShootSound;

    private bool HavePistol = true;
    private bool GetRifleBooster = false;

    [Header("Have Rifle Timer")]
    [Range(1,10)]
    [SerializeField] private float TimeHaveRifle;
    private float ElapsedHaveRifle;

    private GameObject PistolFire;
    private GameObject RifleFire;

    [Header("Score")]
    public Text ScoreText;
    public Text ScoreTextOnLoseMenu;
    [HideInInspector] public int Score;


    [Header("Weapom parameters")]
    [Range(0, 100)]
    public float PistolRecoilStrong;
    [Range(0, 100)]
    public float RifleRecoilStrong;
    [Range(0, 100)]
    public float Damage;
    private float CurrentRecoilStrong;

    [Range(0, 10)]
    public float MaxDistanceFromCenter;

    [Header("Lose")]
    public GameObject LoseMenu;
    [HideInInspector] public bool IsLose;

    private float StartGameObjectZ; //Start pos of Z coordinate

    [SerializeField] private StatsStorage statsStorage;

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

        ElapsedHaveRifle = TimeHaveRifle;

        ElapsedBetweenShoots = DelayBetweenShoots;

        CurrentRecoilStrong = PistolRecoilStrong;

        GetRifle();
    }

    void Update()
    {
        if (GameNotStarted)
        {
            StartGameEffect();
        }
        Vector2 touchPos;
        if (AboveField(out touchPos) && IsLose == false)
        {
            AimGun(touchPos);
            TouchOnScreenForShoot(touchPos);
        }
        //Just effect of fire
        FireFading();
        if (GetRifleBooster)
        {
            HaveRifleTimer();
        }
#if UNITY_EDITOR
        TestBooster();
#endif
    }

    #region Fields for Start Game Effect
    [Header("Values for Start Game Effect")]
    [SerializeField] private int MaxTextSize;
    [SerializeField] private int MinTextSize;
    [SerializeField] private Text DelayText;
    [SerializeField] private AudioSource MusicSource;
    private float Elapsed = 1;
    private float Timer;
    [HideInInspector] public bool GameNotStarted = true;
    #endregion
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

    [Header("")]
    [Range(0, 1)]
    [SerializeField] private float DelayBetweenShoots;
    private float ElapsedBetweenShoots;
    void TouchOnScreenForShoot(Vector2 touchPos)
    {
        if (ElapsedBetweenShoots > 0 && HavePistol == false)
        {
            ElapsedBetweenShoots -= Time.deltaTime;
        }
        else
        {
            ElapsedBetweenShoots = DelayBetweenShoots;
            //If began so shoot
            for (byte i = 0; i < Input.touchCount; i ++)
            {
                if (Input.GetTouch(i).phase == TouchPhase.Began)
                {

                    Shoot(touchPos);
                    break;
                }
                else if (HavePistol == false)
                {
                    Shoot(touchPos);
                    break;
                }
            }

#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                Shoot(touchPos);
            }
            if (Input.GetMouseButton(0) && HavePistol == false)
            {
                Shoot(touchPos);
            }
#endif
        }
    }

    void Shoot(Vector2 mousePos)
    {
        RaycastHit hit;
        Ray ray;
        float distance = 1000;
        int layerMask = (1 << 6) | (1 << 8);

        //Recoil
        Gun.transform.eulerAngles = new Vector3(
            Gun.transform.eulerAngles.x - CurrentRecoilStrong,
            Gun.transform.eulerAngles.y,
            Gun.transform.eulerAngles.z);

        //Hit
        ray = Camera.main.ScreenPointToRay(mousePos);

        //Gun sound
        GunSound.PlayOneShot(ShootSound);

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

        //Check hit
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

    #region Fields for AboveField
    Plane plane = new Plane(Vector3.up, 0);
    private Vector3 FieldPos;
    private float FieldWidth;
    private float FieldHeight;
    #endregion
    bool AboveField(out Vector2 touchPos)
    {
        Vector2 ViewportTouchPos;

        for (byte i = 0; i < Input.touchCount; i++)
        {
           ViewportTouchPos = Camera.main.ScreenToViewportPoint(Input.GetTouch(i).position);

            ViewportTouchPos.x *= Screen.width;
            ViewportTouchPos.y *= Screen.height;

            if (ViewportTouchPos.x <= Screen.width - (Screen.width - FieldWidth - FieldPos.x * 2) && ViewportTouchPos.x >= Screen.width - FieldWidth)
            {
                if (ViewportTouchPos.y <= Screen.height - (Screen.height - FieldHeight - FieldPos.y * 2) && ViewportTouchPos.y >= Screen.height - FieldHeight)
                {
                    touchPos = ViewportTouchPos;
                    return true;
                }
            }
        }
#if UNITY_EDITOR
        ViewportTouchPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        ViewportTouchPos.x *= Screen.width;
        ViewportTouchPos.y *= Screen.height;

        if (ViewportTouchPos.x <= Screen.width - (Screen.width - FieldWidth - FieldPos.x * 2) && ViewportTouchPos.x >= Screen.width - FieldWidth)
        {
            if (ViewportTouchPos.y <= Screen.height - (Screen.height - FieldHeight - FieldPos.y * 2) && ViewportTouchPos.y >= Screen.height - FieldHeight)
            {
                touchPos = ViewportTouchPos;
                return true;
            }
        }
#endif
        touchPos = Vector2.zero;
        return false;
    }
    void AimGun(Vector2 mousePos)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
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

    public void GetRifle()
    {
        //Yeah.I know that i can use HavePistol/!HavePistol value in set active
        //But it's dangerous 
        //If player get one more rifle boster while have rifle
        //So it's may broken
        if (HavePistol)
        {
            Pistol.SetActive(false);
            Rifle.SetActive(true);
            CurrentRecoilStrong = RifleRecoilStrong;
            GetRifleBooster = true;
            HavePistol = false;
        }
        else
        {
            Pistol.SetActive(true);
            Rifle.SetActive(false);
            CurrentRecoilStrong = PistolRecoilStrong;
            GetRifleBooster = false;
            HavePistol = true;
        }
    }

    void HaveRifleTimer()
    {
        ElapsedHaveRifle -= Time.deltaTime;
        if (ElapsedHaveRifle <= 0)
        {
            ElapsedHaveRifle = TimeHaveRifle;
            GetRifle();
        }
    }

    public void Lose()
    {
        ScoreTextOnLoseMenu.text = "Your score:" + Score;
        LoseMenu.SetActive(true);

        statsStorage.SaveGame(Score);
        statsStorage.LoadGame();
    }

    void TestBooster()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            for (int i = spawnManager.Enemy.Count - 1; i > 0; i--)
            {
                spawnManager.Enemy[i].GetComponent<EnemyProfile>().DeathWithoutDropBooster();
            }
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            for (int i = 0; i < spawnManager.Enemy.Count; i++)
            {
                spawnManager.Enemy[i].GetComponent<EnemyProfile>().IceCube.SetActive(true);
                spawnManager.IsSpawnFreeze = true;
            }
        }
    }
}
