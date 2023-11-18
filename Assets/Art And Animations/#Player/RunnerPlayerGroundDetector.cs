using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RunnerPlayerGroundDetector : MonoBehaviour
{
    private BoxCollider2D box2dStand;
    private CapsuleCollider2D capsul;
    private GameObject platform;
    private GameObject previousPlatform;
    private bool allowGroundCheck = true;
    private LayerMask currentLayer;
    [SerializeField] private bool isLineOne;
    [SerializeField] private float height;
    [SerializeField] private float normalHeight;
    [SerializeField] private float platformHeight;
    [SerializeField] private LayerMask[] groundLayer = new LayerMask[2];
    [SerializeField] private int[] myLayerMask = new int[2];
    [SerializeField] private bool checkDown = true;
    [SerializeField] private GameObject player;
    [SerializeField] private bool canDetect = true;
    [SerializeField] private UnityEvent DefaultGravityEvent;
    [SerializeField] private UnityEvent ZeroGravityEvent;
    [SerializeField] private bool notSetParet = false;
    [SerializeField] private float timeToAllowGroundCheckAgain;
    [SerializeField] private UnityEvent TrueGroundEvent;
    [SerializeField] private UnityEvent FalseGroundEvent;
    // Start is called before the first frame update
    void Start()
    {
        box2dStand = GetComponent<BoxCollider2D>();
        capsul = GetComponent<CapsuleCollider2D>();
        height = normalHeight;
        platform = null;
        previousPlatform = null;
        if(isLineOne)
        {
            
            gameObject.layer = myLayerMask[0];
            currentLayer = groundLayer[0];
        }
        else
        {
            gameObject.layer = myLayerMask[1];
            currentLayer = groundLayer[1];
        }


    }

    // Update is called once per frame
    void Update()
    {
        IsGrounded();
    }
    private bool IsGrounded()
    {
        #region draw ray
        
        RaycastHit2D raycastHit = Physics2D.BoxCast(box2dStand.bounds.center,
            box2dStand.bounds.size, 0f, Vector2.down, height, currentLayer);
        Color rayColor;
        if (raycastHit.collider != null)
        {

            rayColor = Color.yellow;
        }
        else
        {
            rayColor = Color.red;
        }

        Debug.DrawRay(box2dStand.bounds.center + new Vector3(box2dStand.bounds.extents.x, 0), Vector2.down * (box2dStand.bounds.extents.y + height), rayColor);
        Debug.DrawRay(box2dStand.bounds.center - new Vector3(box2dStand.bounds.extents.x, 0), Vector2.down * (box2dStand.bounds.extents.y + height), rayColor);
        Debug.DrawRay(box2dStand.bounds.center - new Vector3(box2dStand.bounds.extents.x, box2dStand.bounds.extents.y + height), Vector2.right * (box2dStand.bounds.extents.y + height), rayColor);
        #endregion
        if (raycastHit.collider != null)
        {


            if (!PlayerController.isDashing)
            {
                PlayerController.canDashAgain = true;
            }
            if (raycastHit.collider.gameObject.tag == "Ground")
            {

                if (!canDetect)
                {
                    player.transform.parent = null;
                    StartCoroutine(CanDetect());
                }
                if (PlayerController.isJumping)
                {
                    //SoundManager(landingAudio);
                }
                if (PlayerController.y_velocity <= 0)
                {
                    PlayerController.isJumping = false;
                    PlayerController.isGrounded = true;
                    PlayerController.isOnPlatform = false;
                    DefaultGravityEvent.Invoke();
                    
                }
            }
            else if (raycastHit.collider.gameObject.tag == "Platform")
            {

                platform = raycastHit.collider.gameObject;
                if (canDetect && player.transform.parent == null)
                {
                    if (!notSetParet)
                    {
                        player.transform.SetParent(raycastHit.collider.transform, true);
                    }
                    canDetect = false;

                }

                if (PlayerController.y_velocity <= 0)
                {
                    PlayerController.isJumping = false;
                    PlayerController.isGrounded = false;
                    PlayerController.isOnPlatform = true;
                    ZeroGravityEvent.Invoke();
                }

            }
        }
        else
        {
            height = normalHeight;
            player.transform.parent = null;
            if (platform != null)
            {
                platform = null;
            }
            if (!canDetect)
            {
                
                player.transform.parent = null;
                if (platform != null)
                {
                    Debug.Log("Up");
                    platform = null;
                }
                StartCoroutine(CanDetect());
            }
            PlayerController.isGrounded = false;
            PlayerController.isOnPlatform = false;
            DefaultGravityEvent.Invoke();
        }
        if (allowGroundCheck)
        {
            if (raycastHit.collider == null)
            {
                FalseGroundEvent.Invoke();
            }
            else
            {
                TrueGroundEvent.Invoke();
            }
        }
        return raycastHit.collider != null;


    } // detect player is on grounded or not
    

    IEnumerator CanDetect()
    {
        yield return new WaitForSeconds(0.1f);
        canDetect = true;
    }
    void SetCollider()
    {
        box2dStand.enabled = !PlayerController.isDashing;
    }
    public void AllowGroundCheck()
    {
        StartCoroutine(AllowGroundCheckAgain());

    }
    IEnumerator AllowGroundCheckAgain()
    {
        if (allowGroundCheck)
        {
            allowGroundCheck = false;
            yield return new WaitForSeconds(timeToAllowGroundCheckAgain);
            allowGroundCheck = true;
        }
    }
    public void ChangeLayerMask()
    {
        if(currentLayer == groundLayer[0])
        {
            currentLayer = groundLayer[1];
            gameObject.layer = myLayerMask[1];
        }
        else
        {
            currentLayer = groundLayer[0];
            gameObject.layer = myLayerMask[0];
        }
    }
}
