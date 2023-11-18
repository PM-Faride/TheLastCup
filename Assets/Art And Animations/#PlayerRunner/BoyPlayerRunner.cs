using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using UnityEngine.Events;

public class BoyPlayerRunner : MonoBehaviour
{
    private SkeletonAnimation boyPlayer;
    private MeshRenderer mesh;
    private bool invicible = false;
    private float counterForTimeNoDamage;
    private int counterForMeshEnabled = 0;
    private bool isShooting = false;
    private bool isJumping = false;
    private bool isDashing = false;
    private bool isSitting = false;
    private bool isDashCooldown = false;
    private bool death = false;
    private bool canShoot = false;
    private bool canTarget = false;
    private bool canJump = false;
    private int mainTrack = 0;
    private int moveTrack = 1;
    private int shootTrack = 2;
    private string stop = "Stop";
    private string currentMainTrackAnimation;
    private bool forwarding = false;
    private bool backwarding = false;
    private bool isGrounded;
    private bool actvieBullet = false;
    private Rigidbody2D rb;
    private float realPositionX;
    private GameObject tmpDashDust;
    private float distanceOfLines;
    private GameObject tmpBullet;
    private string bulletLineOne = "BulletLine1";
    private string bulletLineTwo = "BulletLine2";
    private float currentSize;
    private bool noDamage = false;
    private bool startPlaying = false;
    [SerializeField] private int playerHealth;
    [SerializeField] private bool isLineOne;
    [SerializeField] private float lineOneY;
    [SerializeField] private float lineTwoY;
    [SerializeField] private int sortingOrderLine1;
    [SerializeField] private int sortingOrderLine2;
    [SerializeField] private float gravity;
    [SerializeField] private float mass;
    [SerializeField] private float minSize ;
    [SerializeField] private float maxSize ;
    private bool[] shootAnimePlaying = new bool[12];
    [SerializeField] private float horizontal;
    [SerializeField] private float vertical;
    [SerializeField] private float timeForNoDamage;
    [SpineAnimation] public string runAnime;
    [SpineAnimation] public string sitAnime;
    [SpineAnimation] public string startAnime;
    [SpineAnimation] public string deathAnime;
    [SpineAnimation] public string backwardAnime;
    [SpineAnimation] public string forwardAnime;
    [SpineAnimation] public string jumpAnime;
    [SpineAnimation] public string damageAnime;
    [SpineAnimation] public string shootBehindAnime;
    [SpineAnimation] public string shootBehindTopAnime;
    [SpineAnimation] public string shootFrontAnime;
    [SpineAnimation] public string shootFrontTopAnime;
    [SpineAnimation] public string shootTopAnime;
    [SpineAnimation] public string shootSittingAnime;
    [SpineAnimation] public string targetSittingAnime;
    [SpineAnimation] public string targetBehindAnime;
    [SpineAnimation] public string targetBehindTopAnime;
    [SpineAnimation] public string targetFrontAnime;
    [SpineAnimation] public string targetFrontTopAnime;
    [SpineAnimation] public string targetTopAnime;
    [SpineAnimation] public string takeOffAnime;
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float backwardSpeed;
    [SerializeField] private float jumpVelocity;
    [SerializeField] private float shootRate;
    [SerializeField] private float dashCoolDownTime;
    [SerializeField] private float dashLenghtTime;
    [SerializeField] private GameObject dashDust;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform shootTransformBone;
    [SerializeField] private UnityEvent CallSitColliderEvent;
    [SerializeField] private UnityEvent CallRunColliderEvent;
    [SerializeField] private UnityEvent NotAllowGroundCheckEvent;
    [SerializeField] private UnityEvent ChangeLayerEvent;
    [SerializeField] private UnityEvent ChangeColliderToSitEvent;
    [SerializeField] private UnityEvent ChangeColliderToRunEvent;
    [SerializeField] private UnityEvent ChangeDamgerHandlerLayerEvent;
    [SerializeField] private UnityEvent FirstLayerSetToOneEvent;
    [SerializeField] private UnityEvent FirstLayerSetToTwoEvent;
    [SerializeField] private UnityEvent CallDamageHandlerNoDamageEvent;
    // Start is called before the first frame update
    private void Awake()
    {
        mesh = GetComponent<MeshRenderer>();
        counterForTimeNoDamage = timeForNoDamage;
    }
    void Start()
    {
        currentMainTrackAnimation = stop;
        distanceOfLines = lineTwoY - lineOneY;
        if (isLineOne)
        {
            FirstLayerSetToOneEvent.Invoke();
            transform.localScale = new Vector3(maxSize, maxSize, transform.localScale.z);
            transform.position = new Vector3(transform.position.x, lineOneY, transform.position.z);
            GetComponent<MeshRenderer>().sortingOrder = 100;
        }
        else
        {
            FirstLayerSetToTwoEvent.Invoke();
            transform.localScale = new Vector3(minSize, minSize, transform.localScale.z);
            transform.position = new Vector3(transform.position.x, lineTwoY, transform.position.z);
            GetComponent<MeshRenderer>().sortingOrder = -100;
        }
        boyPlayer = GetComponent<SkeletonAnimation>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravity;
        rb.mass = mass;
        StartCoroutine(StartAnime());
    }
    private void Update()
    {
        if (!noDamage && startPlaying)
        {
            if (!actvieBullet && isShooting)
            {
                actvieBullet = true;
                StartCoroutine(ShootBullet());
            }
            ShootBullet();
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (startPlaying)
        {
            if (!noDamage)
            {
                if (currentMainTrackAnimation != stop)
                {
                    boyPlayer.AnimationName = currentMainTrackAnimation;
                }
                SetMovementParameter();
                //ShootAndCancelIt();
                JumpAnimationController();
            }
            if (invicible)
            {
                counterForMeshEnabled++;
                counterForTimeNoDamage -= Time.deltaTime;
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
                    CallDamageHandlerNoDamageEvent.Invoke();
                    counterForMeshEnabled = 0;
                    invicible = false;
                }
            }
        }
    }
    void ActiveMainTrackAnimation(string name)
    {
        currentMainTrackAnimation = name;
    }
    IEnumerator StartAnime()
    {
        var anime = boyPlayer.state.SetAnimation(0, startAnime, true);
        yield return new WaitForSpineAnimationComplete(anime);
        //ActiveMainTrackAnimation(runAnime);
    }
    public void PlayeTakeOff()
    {
        StopCoroutine(StartAnime());
        StartCoroutine(TakeOffAnime());
    }
    IEnumerator TakeOffAnime()
    {

        var anime = boyPlayer.state.SetAnimation(0, takeOffAnime, false);
        yield return new WaitForSpineAnimationComplete(anime);
        startPlaying = true;
        ActiveMainTrackAnimation(runAnime);
    }
    IEnumerator SetOtherTrackAnimation(int trackName , string animeName , bool loop)
    {
        if(isGrounded)
        {
            var anime = boyPlayer.state.SetAnimation(trackName, animeName, loop);
            yield return new WaitForSpineAnimationComplete(anime);
        }
    }
    IEnumerator SetJumpAnimation()
    {
        Debug.Log("Jump Anime");
        var anime = boyPlayer.state.SetAnimation(moveTrack, jumpAnime, true);
        yield return new WaitForSpineAnimationComplete(anime);
    }
    void DeleteOtherTrackAnimation(int trackAnime , float mixTime)
    {
        boyPlayer.AnimationState.SetEmptyAnimation(trackAnime , mixTime);
    }
    void SetMovementParameter()
    {
        horizontal = PlayerController.moveControl.x;
        vertical = PlayerController.moveControl.y;
        if (vertical >= 0)
        {
            if(isSitting)
            {
                rb.velocity = Vector2.zero;
                isSitting = false;
                ChangeColliderToRunEvent.Invoke();
                CallRunColliderEvent.Invoke();
                ActiveMainTrackAnimation(runAnime);
            }
            else if (horizontal > 0)
            {
                if (!canTarget)
                {
                    rb.velocity = new Vector2(forwardSpeed * Time.fixedDeltaTime, rb.velocity.y);
                }
                if (!forwarding && !canTarget)
                {
                    forwarding = true;
                    backwarding = false;

                    StartCoroutine(SetOtherTrackAnimation(moveTrack, forwardAnime, true));
                }
                if(isShooting)
                {
                    if(vertical == 0)
                    {
                        if (!GetShootBoolean(shootFrontAnime))
                        {
                            SetShootBoolean(shootFrontAnime);
                            StartCoroutine(SetOtherTrackAnimation(shootTrack, shootFrontAnime, true));
                        }
                    }
                    else if(vertical > 0)
                    {
                        if (!GetShootBoolean(shootFrontTopAnime))
                        {
                            SetShootBoolean(shootFrontTopAnime);
                            StartCoroutine(SetOtherTrackAnimation(shootTrack, shootFrontTopAnime, true));
                        }
                    }
                }
                else if(canTarget)
                {
                    if (vertical == 0)
                    {
                        if (!GetShootBoolean(targetFrontAnime))
                        {
                            SetShootBoolean(targetFrontAnime);
                            StartCoroutine(SetOtherTrackAnimation(shootTrack, targetFrontAnime, true));
                        }
                    }
                    else if (vertical > 0)
                    {
                        if (!GetShootBoolean(targetFrontTopAnime))
                        {
                            SetShootBoolean(targetFrontTopAnime);
                            StartCoroutine(SetOtherTrackAnimation(shootTrack, targetFrontTopAnime, true));
                        }
                    }
                }
            }
            else if (horizontal == 0)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                if(!isJumping)
                {
                    DeleteOtherTrackAnimation(moveTrack, 0.08f);
                }
                forwarding = false;
                backwarding = false;
                if (isShooting)
                {
                    if (vertical == 1)
                    {
                        if (!GetShootBoolean(shootTopAnime))
                        {
                            SetShootBoolean(shootTopAnime);
                            DeleteOtherTrackAnimation(moveTrack, 0.08f);
                            StartCoroutine(SetOtherTrackAnimation(shootTrack, shootTopAnime, true));
                        }

                    }
                    else if (vertical == 0)
                    {
                        if (!GetShootBoolean(shootFrontAnime))
                        {
                            SetShootBoolean(shootFrontAnime);
                            StartCoroutine(SetOtherTrackAnimation(shootTrack, shootFrontAnime, true));
                        }
                    }
                }
                else if(canTarget)
                {
                    if (vertical == 1)
                    {
                        
                        if (!GetShootBoolean(targetTopAnime))
                        {
                            Debug.Log(targetTopAnime);
                            SetShootBoolean(targetTopAnime);
                            DeleteOtherTrackAnimation(moveTrack, 0.08f);
                            StartCoroutine(SetOtherTrackAnimation(shootTrack, targetTopAnime, true));
                        }

                    }
                    else if (vertical == 0)
                    {
                        if (!GetShootBoolean(targetFrontAnime))
                        {
                            SetShootBoolean(targetFrontAnime);
                            StartCoroutine(SetOtherTrackAnimation(shootTrack, targetFrontAnime, true));
                        }
                    }
                }
                else
                {
                    ////DeleteOtherTrackAnimation(moveTrack, 0.08f);
                }
                //else if(vertical == 1)
                //{
                //    if (!GetShootBoolean(targetTopAnime))
                //    {
                //        Debug.Log(targetTopAnime);
                //        SetShootBoolean(targetTopAnime);
                //        DeleteOtherTrackAnimation(moveTrack, 0.08f);
                //        StartCoroutine(SetOtherTrackAnimation(shootTrack, targetTopAnime, true));
                //    }
                //}
                //else if (vertical == 0)
                //{
                //    DeleteOtherTrackAnimation(moveTrack, 0.08f);
                //}
            }
            else if (horizontal < 0)
            {
                if (!canTarget)
                {
                    rb.velocity = new Vector2(-backwardSpeed * Time.fixedDeltaTime, rb.velocity.y);
                }
                if (!backwarding && !canTarget)
                {
                    forwarding = false;
                    backwarding = true;
                    StartCoroutine(SetOtherTrackAnimation(moveTrack, backwardAnime, true));
                }
                if (isShooting)
                {
                    if (vertical == 0)
                    {
                        if (!GetShootBoolean(shootBehindAnime))
                        {
                            SetShootBoolean(shootBehindAnime);
                            StartCoroutine(SetOtherTrackAnimation(shootTrack, shootBehindAnime, true));
                        }
                    }
                    else if (vertical > 0)
                    {
                        if (!GetShootBoolean(shootBehindTopAnime))
                        {
                            SetShootBoolean(shootBehindTopAnime);
                            StartCoroutine(SetOtherTrackAnimation(shootTrack, shootBehindTopAnime, true));
                        }
                    }
                }
                else if(canTarget)
                {
                    if (vertical == 0)
                    {
                        if (!GetShootBoolean(targetBehindAnime))
                        {
                            SetShootBoolean(targetBehindAnime);
                            StartCoroutine(SetOtherTrackAnimation(shootTrack, targetBehindAnime, true));
                        }
                    }
                    else if (vertical > 0)
                    {
                        if (!GetShootBoolean(targetBehindTopAnime))
                        {
                            SetShootBoolean(targetBehindTopAnime);
                            StartCoroutine(SetOtherTrackAnimation(shootTrack, targetBehindTopAnime, true));
                        }
                    }
                }
            }
        }
        else if(vertical < 0)
        {
            if (!isSitting && !isJumping)
            {
                rb.velocity = Vector2.zero;
                CallSitColliderEvent.Invoke();
                isSitting = true;
                ChangeColliderToSitEvent.Invoke();
                ActiveMainTrackAnimation(sitAnime);
            }
            if(isShooting)
            {
                if (!GetShootBoolean(shootSittingAnime))
                {
                    SetShootBoolean(shootSittingAnime);
                    DeleteOtherTrackAnimation(moveTrack, 0.08f);
                    StartCoroutine(SetOtherTrackAnimation(shootTrack, shootSittingAnime, true));
                }
            }
            else if(canTarget)
            {
                if (!GetShootBoolean(targetSittingAnime))
                {
                    SetShootBoolean(targetSittingAnime);
                    DeleteOtherTrackAnimation(moveTrack, 0.08f);
                    StartCoroutine(SetOtherTrackAnimation(shootTrack, targetSittingAnime, true));
                }
            }
        }
        
    }
    void SetShootBoolean(string name)
    {
        for (int i = 0; i < shootAnimePlaying.Length; i++)
        {
            if (shootAnimePlaying[i])
            {
                shootAnimePlaying[i] = false;
                break;
            }
        }
        if (name == shootBehindAnime)
        {
            shootAnimePlaying[0] = true;
        }
        else if (name == shootBehindTopAnime)
        {
            shootAnimePlaying[1] = true;
        }
        else if (name == shootFrontAnime)
        {
            shootAnimePlaying[2] = true;
        }
        else if (name == shootFrontTopAnime)
        {
            shootAnimePlaying[3] = true;
        }
        else if (name == shootTopAnime)
        {
            shootAnimePlaying[4] = true;
        }
        else if (name == shootSittingAnime)
        {
            shootAnimePlaying[5] = true;
        }
        else if (name == targetSittingAnime)
        {
            shootAnimePlaying[6] = true;
        }
        else if (name == targetBehindAnime)
        {
            shootAnimePlaying[7] = true;
        }
        else if (name == targetBehindTopAnime)
        {
            shootAnimePlaying[8] = true;
        }
        else if (name == targetFrontAnime)
        {
            shootAnimePlaying[9] = true;
        }
        else if (name == targetFrontTopAnime)
        {
            shootAnimePlaying[10] = true;
        }
        else if (name == targetTopAnime)
        {
            
            shootAnimePlaying[11] = true;
            if(shootAnimePlaying[11])
            {
                
            }
        }
        else if(name == stop)
        {
            for (int i = 0; i < shootAnimePlaying.Length; i++)
            {
                shootAnimePlaying[i] = false;
            }
        }
    }
    bool GetShootBoolean(string name)
    {

        bool tmpBoolean;
        if (name == shootBehindAnime)
        {
            tmpBoolean = shootAnimePlaying[0];
        }
        else if (name == shootBehindTopAnime)
        {
            tmpBoolean = shootAnimePlaying[1];
        }
        else if (name == shootFrontAnime)
        {
            tmpBoolean = shootAnimePlaying[2];
        }
        else if (name == shootFrontTopAnime)
        {
            tmpBoolean = shootAnimePlaying[3];
        }
        else if (name == shootTopAnime)
        {
            tmpBoolean = shootAnimePlaying[4];
        }
        else if (name == shootSittingAnime)
        {
            tmpBoolean = shootAnimePlaying[5];
        }
        else if (name == targetSittingAnime)
        {
            tmpBoolean = shootAnimePlaying[6];
        }
        else if (name == targetBehindAnime)
        {
            tmpBoolean = shootAnimePlaying[7];
        }
        else if (name == targetBehindTopAnime)
        {
            tmpBoolean = shootAnimePlaying[8];
        }
        else if (name == targetFrontAnime)
        {
            tmpBoolean = shootAnimePlaying[9];
        }
        else if (name == targetFrontTopAnime)
        {
            tmpBoolean = shootAnimePlaying[10];
        }
        else if (name == targetTopAnime)
        {
            tmpBoolean = shootAnimePlaying[11];
        }
        else
        {
            tmpBoolean = true;
        }
        return tmpBoolean;
    }
    void ShootAndCancelIt()
    {
        if (canShoot)
        {
            if (isDashing)
            {
                isShooting = false;
            }
            else
            {
                isShooting = true;
            }
            Variables.isShooting = isShooting;
        } // shoot
        if (!canShoot)
        {
            if(isShooting)
            {
                SetShootBoolean(stop);
            }
            DeleteOtherTrackAnimation(shootTrack, 0.08f);
            isShooting = false;
            Variables.isShooting = isShooting;
            
        } // cancel shoot
    }
    IEnumerator ShootBullet()
    {
        yield return new WaitForSeconds(shootRate);
        //BulletDictionary.Instance.SpawnFromPool("Bullet", shootTransformBone.position, shootTransformBone.rotation, false);
        //GetComponent<BulletDictionary>().SpawnFromPool("Bullet", shootTransformBone.position, shootTransformBone.rotation, false);
        tmpBullet = Instantiate(bullet, shootTransformBone.position, shootTransformBone.rotation);
        if(isLineOne)
        {
            tmpBullet.transform.localScale = new Vector3(maxSize, maxSize, transform.localScale.z);
            tmpBullet.GetComponent<MeshRenderer>().sortingLayerName = bulletLineOne;
            tmpBullet.layer = LayerMask.NameToLayer("RunnerLine1");
            tmpBullet.GetComponent<MeshRenderer>().sortingOrder = sortingOrderLine1 - 1;
        }
        else
        {
            tmpBullet.transform.localScale = new Vector3(minSize, minSize, transform.localScale.z);
            tmpBullet.GetComponent<MeshRenderer>().sortingLayerName = bulletLineTwo;
            tmpBullet.layer = LayerMask.NameToLayer("RunnerLine2");
            tmpBullet.GetComponent<MeshRenderer>().sortingOrder = sortingOrderLine2 - 1;
        }
        tmpBullet.SetActive(true);
        actvieBullet = false;
    }
    void JumpAnimationController()
    {
        if(isJumping)
        {
            if(isGrounded)
            {
                DeleteOtherTrackAnimation(moveTrack, 0.08f);
                isJumping = false;
                forwarding = false;
                backwarding = false;
            }
        }
    }
    public void ShootController(bool dir)
    {
        canShoot = dir;
        isShooting = dir;
        if(!dir && !canTarget)
        {
            SetShootBoolean(stop);
            DeleteOtherTrackAnimation(shootTrack, 0.08f);
        }

    }
    public void JumpController()
    {
        if (isGrounded && !isSitting)
        {
            isJumping = true;
            isGrounded = false;
            
            StartCoroutine(SetJumpAnimation());
            NotAllowGroundCheckEvent.Invoke();
            //rb.velocity = Vector2.up * jumpVelocity;
            rb.AddForce(new Vector2(0, jumpVelocity));
        }
    }
    public void DashController()
    {
        Debug.Log("First");
        if (!isSitting)// && !isJumping && isGrounded)
        {
            if (!isDashCooldown)
            {
                Debug.Log("Second");
                isDashing = true;
                isDashCooldown = true;
                tmpDashDust = Instantiate(dashDust, transform.position + new Vector3(0, 0.6f, 0), transform.rotation);
                tmpDashDust.SetActive(true);
                realPositionX = transform.position.x;
                //transform.position = new Vector3(1000, transform.position.y, transform.position.z);
                GetComponent<MeshRenderer>().enabled = false;
                StartCoroutine(DashCooldown());
                
            }
        }
    }
    public void TargetController(bool dir)
    {
        Debug.Log("Target");
        if(dir)
        {
            DeleteOtherTrackAnimation(moveTrack, 0.08f);
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        if (!dir && !isShooting)
        {
            SetShootBoolean(stop);
            DeleteOtherTrackAnimation(shootTrack, 0.08f);
        }
        canTarget = dir;
    }
    IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(dashLenghtTime);
        GetComponent<MeshRenderer>().enabled = true;
        ChangeLayerEvent.Invoke();
        ChangeDamgerHandlerLayerEvent.Invoke();
        isLineOne = !isLineOne;
        if (isLineOne)
        {
            GetComponent<MeshRenderer>().sortingOrder = sortingOrderLine1;
            transform.localScale = new Vector3(maxSize, maxSize, transform.localScale.z);
            tmpDashDust = Instantiate(dashDust, new Vector3(realPositionX, transform.position.y - distanceOfLines + 0.6f, transform.position.z),transform.rotation);
            tmpDashDust.SetActive(true);
            transform.position = new Vector3(realPositionX, transform.position.y - distanceOfLines, transform.position.z);
        }
        else
        {
            GetComponent<MeshRenderer>().sortingOrder = sortingOrderLine2;
            transform.localScale = new Vector3(minSize, minSize, transform.localScale.z);
            tmpDashDust = Instantiate(dashDust, new Vector3(realPositionX, distanceOfLines + transform.position.y + 0.6f, transform.position.z), transform.rotation);
            tmpDashDust.SetActive(true);
            transform.position = new Vector3(realPositionX, distanceOfLines + transform.position.y, transform.position.z);
        }
        
        yield return new WaitForSeconds(dashCoolDownTime);
        isDashCooldown = false;
    }
    public void GroundCheck(bool dir)
    {
        isGrounded = dir;
    }
    public void DamageHandler()
    {
        noDamage = true;
        invicible = true;
        if (playerHealth >= 1)
        {
            playerHealth--;
            Variables.playerHealth[0] = playerHealth;
            Variables.playerDamage[0] = true;

            if (playerHealth == 0)
            {
                StartCoroutine(DamageAnime());

            }
            else
            {
                
                StartCoroutine(DamageAnime());
            }
        }
    }
    IEnumerator DamageAnime()
    {

        
        isDashing = false;
        isJumping = false;
        //isShooting = false;
        Variables.isShooting = isShooting;
        rb.velocity = Vector2.zero;
        var anime = boyPlayer.state.SetAnimation(0, damageAnime, false);
        yield return new WaitForSpineAnimationComplete(anime);
        noDamage = false;
        //rb.gravityScale = playerGravity;
        //if (playerHealth == 0)
        //{
        //    StartCoroutine(LoseAnime());
        //}
    }
    //IEnumerator LoseAnime()
    //{
    //    playerDamageDetector.SetActive(false);
    //    groundDetector.SetActive(false);
    //    activeUpdate = false;
    //    var anime = player.state.SetAnimation(0, loseAnime, true);
    //    rb.velocity = Vector2.zero;
    //    rb.gravityScale = 0;
    //    rb.velocity = new Vector2(0, 5);
    //    yield return new WaitForSeconds(1.0f);

    //}
}
