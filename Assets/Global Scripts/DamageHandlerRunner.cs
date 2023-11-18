using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageHandlerRunner : MonoBehaviour
{
    [SerializeField] private bool noDamage = false;
    [SerializeField] private PolygonCollider2D sitCollider;
    [SerializeField] private BoxCollider2D runCollider;
    [SerializeField] private int[] myLayerMask = new int[2];
    [SerializeField] private UnityEvent CallPlayerDamageEvent;

    public void SetFirstLayerMask(int dir)
    {
        sitCollider.enabled = false;
        runCollider.enabled = true;
        if(dir == 1)
        {
            gameObject.layer = myLayerMask[0];
        }
        else if(dir == 2)
        {
            gameObject.layer = myLayerMask[1];
        }

    }
    public void ChangeLayerMask()
    {
        
        if (gameObject.layer == myLayerMask[1])
        {
            gameObject.layer = myLayerMask[0];
        }
        else
        {
            gameObject.layer = myLayerMask[1];
        }
    }
    public void ChangeCollider(bool dir)
    {
        Debug.Log("ChangeCollider");
        if (dir) // set to sit
        {
            sitCollider.enabled = true;
            runCollider.enabled = false;
        }
        else
        {
            sitCollider.enabled = false;
            runCollider.enabled = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && !noDamage)
        {
            noDamage = true;
            CallPlayerDamageEvent.Invoke();
            Debug.Log("DamageEnter");
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && !noDamage)
        {
            noDamage = true;
            CallPlayerDamageEvent.Invoke();
            Debug.Log("DamageStay");
        }
    }
    public void SetNoDamage()
    {
        Debug.Log("HiAgain");
        noDamage = false;
    }
}
