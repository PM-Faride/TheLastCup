using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UnityEngine.InputSystem;
using UnityEngine.Events;
public class AirplaneController : MonoBehaviour
{
    public InputActionAsset controller;
    private InputActionMap player;
    private InputAction fireAction;
    private InputAction moveAction;
    private InputAction rageAction;
    public static Vector2 move;
    private SkeletonAnimation airplane;
    private Rigidbody2D rb;
    private bool allowDownToCenter;
    private bool allowUpToCenter;
    private bool allowCenterToUp;
    private bool allowCenterToDown;
    private float counterForTimeNoDamage;
    private int counterForMeshEnabled = 0;
    private MeshRenderer mesh;
    private AudioSource audio;
    private string stop = "Stop";
    private bool activeUpdate = false;
    private float previousHorizontal;
    private float previousVertical;
    public static bool airplaneMove = false;


    [SerializeField] private string currentAnime;
    [SerializeField] private int playerHealth;
    [SerializeField] private bool noDamage = false;
    [SerializeField] private float timeForNoDamage = 2.0f;
    [SerializeField] private Vector2 velocity;
    [SerializeField] private bool fire;
    [SerializeField] private bool allowFire = true;
    [SerializeField] private float horizontal;
    [SerializeField] private float vertical;
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float upSpeed;
    [SerializeField] private float degree;
    [SerializeField] private float fireRate;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform firePoint;
    [SerializeField] private bool rage = false;
    [SerializeField] private GameObject smoke;
    [SerializeField] private GameObject smoke2;

    [Header("Animation Name")]
    [SpineAnimation] public string centerAnime;
    [SpineAnimation] public string downAnime;
    [SpineAnimation] public string upAnime;
    [SpineAnimation] public string centerToDownAnime;
    [SpineAnimation] public string centerToUpAnime;
    [SpineAnimation] public string downToCenterAnime;
    [SpineAnimation] public string upToCenterAnime;
    [SpineAnimation] public string startAnime;
    [SpineAnimation] public string hurtAnime;
    [SpineAnimation] public string loseAnime;
    [SerializeField] private AudioClip clip;
    [SerializeField] private UnityEvent ShootVfxOnEvent;
    [SerializeField] private UnityEvent ShootVfxOffEvent;
    [SerializeField] private UnityEvent DamageSFXEvent;

    private void Awake()
    {
        Variables.bossDead = false;
        Time.timeScale = 1;
        mesh = GetComponent<MeshRenderer>();
        counterForTimeNoDamage = timeForNoDamage;
        player = controller.FindActionMap("Player", true);
        fireAction = player.FindAction("Fire");
        moveAction = player.FindAction("Move");
        rageAction = player.FindAction("Rage");
        fireAction.performed += Shoot;
        fireAction.canceled += CancelShoot;
        moveAction.performed += ctx => move = ctx.ReadValue<Vector2>();
        moveAction.canceled += ctx => move = Vector2.zero;
    }
    void Start()
    {
        previousHorizontal = 0;
        previousVertical = 0;
        airplane = GetComponent<SkeletonAnimation>();
        audio = GetComponent<AudioSource>();
        audio.clip = clip;
        rb = GetComponent<Rigidbody2D>();
        currentAnime = centerAnime;
        allowCenterToDown = true;
        allowCenterToUp = true;
        StartCoroutine(StartAnime());
    }

    private void FixedUpdate()
    {
        if (activeUpdate)
        {
            if (currentAnime != stop)
            {
                airplane.AnimationName = currentAnime;
            }
            #region (H,V) region
            horizontal = move.x;
            vertical = move.y;
            velocity.x = rb.velocity.x;
            velocity.y = rb.velocity.y;
            #endregion
            if (horizontal != previousHorizontal || vertical != previousVertical)
            {
                if(horizontal == 0 && vertical == 0)
                {
                    rb.velocity = Vector3.zero;
                    
                }
                
                previousVertical = vertical;
                previousHorizontal = horizontal;
            }
            if (horizontal == 0 && vertical == 0)
            {
                if (currentAnime == upAnime)
                {
                    currentAnime = stop;
                    StartCoroutine(UpToCenter());
                }
                else if (currentAnime == downAnime)
                {
                    currentAnime = stop;
                    StartCoroutine(DownToCenter());
                }
                else if (currentAnime == upAnime)
                {
                    currentAnime = stop;
                    StartCoroutine(CenterToDown());
                }
                else if (currentAnime == downAnime)
                {
                    currentAnime = stop;
                    StartCoroutine(CenterToUp());
                }
                transform.eulerAngles = Vector3.zero;

            }
            
            else if (horizontal > 0 || horizontal < 0)
            {
                // forward and backward
                if (horizontal > 0)
                {
                    transform.eulerAngles = new Vector3(0, 0, -1 * degree);
                    if (vertical == 0)
                    {
                        rb.velocity = new Vector3(forwardSpeed * Time.fixedDeltaTime, 0, 0);
                    }
                    else if (vertical > 0)
                    {
                        if (currentAnime != stop && allowCenterToUp)
                        {
                            currentAnime = stop;
                            StartCoroutine(CenterToUp());
                        }
                        rb.velocity = new Vector3(forwardSpeed * Time.fixedDeltaTime, upSpeed * Time.fixedDeltaTime, 0);
                    }
                    else if (vertical < 0)
                    {
                        if (currentAnime != stop && allowCenterToDown)
                        {
                            currentAnime = stop;
                            StartCoroutine(CenterToDown());
                        }
                        rb.velocity = new Vector3(forwardSpeed * Time.fixedDeltaTime, -1 * upSpeed * Time.fixedDeltaTime, 0);
                    }
                }
                else
                {
                    transform.eulerAngles = new Vector3(0, 0, degree);
                    if (vertical == 0)
                    {
                        rb.velocity = new Vector3(-1 * forwardSpeed * Time.fixedDeltaTime, 0, 0);
                    }
                    else if (vertical > 0)
                    {
                        if (currentAnime != stop && allowCenterToUp)
                        {
                            currentAnime = stop;
                            StartCoroutine(CenterToUp());
                        }
                        rb.velocity = new Vector3(-1 * forwardSpeed * Time.fixedDeltaTime, upSpeed * Time.fixedDeltaTime, 0);
                    }
                    else if (vertical < 0)
                    {
                        if (currentAnime != stop && allowCenterToDown)
                        {
                            currentAnime = stop;
                            StartCoroutine(CenterToDown());
                        }
                        rb.velocity = new Vector3(-1 * forwardSpeed * Time.fixedDeltaTime, -1 * upSpeed * Time.fixedDeltaTime, 0);
                    }
                }

            }
            else if (vertical > 0 || vertical < 0)
            {
                if (vertical > 0)
                {
                    if (currentAnime != stop && allowCenterToUp)
                    {
                        currentAnime = stop;
                        StartCoroutine(CenterToUp());
                    }
                    rb.velocity = new Vector3(0, upSpeed * Time.fixedDeltaTime, 0);
                }
                else
                {
                    if (currentAnime != stop && allowCenterToDown)
                    {
                        currentAnime = stop;
                        StartCoroutine(CenterToDown());
                    }
                    rb.velocity = new Vector3(0, -1 * upSpeed * Time.fixedDeltaTime, 0);
                }
            }
            FireBullet();
        }
        if (noDamage)
        {
            counterForMeshEnabled++;
            counterForTimeNoDamage -= Time.fixedDeltaTime;
            if (counterForMeshEnabled % 15 >= 3)
            {

                mesh.enabled = true;
            }
            else
            {

                mesh.enabled = false;
            }
            if (counterForTimeNoDamage <= 0)
            {
                counterForTimeNoDamage = timeForNoDamage;
                mesh.enabled = true;
                noDamage = false;
                counterForMeshEnabled = 0;
            }
        }
    }
    IEnumerator StartAnime()
    {
        var anime = airplane.state.SetAnimation(0, startAnime, false);
        yield return new WaitForSpineAnimationComplete(anime);
        activeUpdate = true;
    }
    IEnumerator CenterToUp()
    {
        
        if(allowCenterToUp)
        {
            allowCenterToUp = false;
            var anime = airplane.AnimationState.SetAnimation(0, centerToUpAnime, false);
            yield return new WaitForSpineAnimationComplete(anime);
            allowUpToCenter = true;
            currentAnime = upAnime;
        }
        
    }
    IEnumerator CenterToDown()
    {
        if (allowCenterToDown)
        {
            allowCenterToDown = false;
            var anime = airplane.AnimationState.SetAnimation(0, centerToDownAnime, false);
            yield return new WaitForSpineAnimationComplete(anime);
            allowDownToCenter = true;
            currentAnime = downAnime;
        }
    }
    IEnumerator UpToCenter()
    {
        if (allowUpToCenter)
        {
            allowUpToCenter = false;
            var anime = airplane.AnimationState.SetAnimation(0, upToCenterAnime, false);
            yield return new WaitForSpineAnimationComplete(anime);
            allowCenterToDown = true;
            allowCenterToUp = true;
            currentAnime = centerAnime;
        }
    }
    IEnumerator DownToCenter()
    {
        if (allowDownToCenter)
        {
            allowDownToCenter = false;
            var anime = airplane.AnimationState.SetAnimation(0, downToCenterAnime, false);
            yield return new WaitForSpineAnimationComplete(anime);
            allowCenterToDown = true;
            allowCenterToUp = true;
            currentAnime = centerAnime;
        }
    }
    void FireBullet()
    {
        if (fire && allowFire)
        {
            allowFire = false;
            GameObject tmp = Instantiate(bullet, firePoint.position, transform.rotation);
            tmp.SetActive(true);
            StartCoroutine(AllowFire());
        }
    }
    IEnumerator AllowFire()
    {
        yield return new WaitForSeconds(fireRate);
        allowFire = true;
    }
    public void FireControl(bool dir)
    {
        fire = dir;
    }
    private void OnEnable()
    {
        fireAction.Enable();
        moveAction.Enable();
        rageAction.Enable();
    }
    private void OnDisable()
    {
        fireAction.Disable();
        moveAction.Disable();
        rageAction.Disable();
    }
    void Shoot(InputAction.CallbackContext ctx)
    {
        if(!audio.isPlaying)
        {
            audio.Play(0);
        }
        ShootVfxOnEvent.Invoke();
        fire = true;
    }
    void CancelShoot(InputAction.CallbackContext ctx)
    {
        if(audio.isPlaying)
        {
            audio.Stop();
        }
        ShootVfxOffEvent.Invoke();
        fire = false;
    }
    
    public void DamageHandler()
    {
        if (!noDamage)
        {
            if (playerHealth >= 1)
            {
                playerHealth--;
                Variables.playerHealth[0] = playerHealth;
                Variables.playerDamage[0] = true;

                if (playerHealth == 0)
                {
                    StartCoroutine(LoseAnime());
                }
                else
                {
                    noDamage = true;
                    StartCoroutine(DamageAnime());
                }
            }

        }
        
    }
    IEnumerator LoseAnime()
    {
        activeUpdate = false;
        var anime = airplane.AnimationState.SetAnimation(0, loseAnime, true);
        yield return new WaitForSpineAnimationComplete(anime);
    }
    IEnumerator DamageAnime()
    {
        activeUpdate = false;
        var anime = airplane.AnimationState.SetAnimation(0, hurtAnime, false);
        yield return new WaitForSpineAnimationComplete(anime);
        activeUpdate = true;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Boss" ||
            collision.gameObject.tag == "EnemyBullet" || collision.gameObject.tag == "Hazard")
        {

            if (!Variables.bossDead)
            {
                DamageHandler();
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Boss" ||
            collision.gameObject.tag == "EnemyBullet" || collision.gameObject.tag == "Hazard")
        {

            if (!Variables.bossDead)
            {
                DamageHandler();
            }
        }
    }
}
