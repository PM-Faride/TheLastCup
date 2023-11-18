using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthCounter : MonoBehaviour
{
    [SerializeField] private bool remoteDamage;
    private string colorProperty = "_Color";
    private string blackTintProperty = "_Black";
    [SerializeField] private int counterHealthPhase = 0;
    MaterialPropertyBlock block;
    private bool canDamageEffect;
    [SerializeField] private string gameobjectTag;
    [SerializeField] private string damagerTag = "PlayerBullet";
    [SerializeField] public int healthNumber;
    [SerializeField] private int healthLeft;
    [SerializeField] private int[] healthPhase;
    [SerializeField] private bool death = false;
    [SerializeField] private UnityEvent DeathEvent;
    [SerializeField] private UnityEvent ExplodeEvent;
    [SerializeField] private UnityEvent[] HealthPhaseEvent;
    [SerializeField] private MeshRenderer mesh;
    [SerializeField] private Color freezeColor;
    [SerializeField] private Color freezeBlackColor;
    private float damageTime = 0.3f;
    [SerializeField] private bool defaultOmidColor;
    [SerializeField] private UnityEvent RemoteDamageEvent;
    // Start is called before the first frame update
    void Start()
    {
        if (defaultOmidColor)
        {
            freezeColor = new Color32(255, 255, 255, 255);
            freezeBlackColor = new Color32(55, 51, 51, 0);
        }
        canDamageEffect = true;
        block = new MaterialPropertyBlock();
        
        if (gameobjectTag != null)
        {
            gameObject.tag = gameobjectTag;
        }
        healthLeft = healthNumber;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!death)
        {
            if (collision.gameObject.tag == damagerTag && healthLeft > 0)
            {
                if(canDamageEffect && gameObject.activeSelf)
                {
                    StartCoroutine(DamageEffect());
                }
                healthLeft--;
                if(remoteDamage)
                {
                    RemoteDamageEvent.Invoke();
                }
            }
            if (healthLeft <= 0)
            {
                death = true;
                gameObject.tag = "DeathEnemy";
                if (Variables.isMissileMode)
                {
                    Debug.Log("Missile");
                    MissileModeHandler.Instance.AddMissilePhase(gameObject, 0);
                }
                DeathEvent.Invoke();
                ExplodeEvent.Invoke();
            }
            else
            {
                if (healthPhase.Length > 0)
                {
                    for (int i = 0; i < healthPhase.Length; i++)
                    {
                        if (healthLeft <= healthPhase[i] && counterHealthPhase == i)
                        {
                            counterHealthPhase++;
                            HealthPhaseEvent[i].Invoke();

                        }
                    }
                }
            }
        }
    }
    public void Damage()
    {
        if (!death)
        {
            if (healthLeft > 0)
            {
                healthLeft--;
                if (healthLeft <= 0)
                {
                    death = true;
                    DeathEvent.Invoke();
                    ExplodeEvent.Invoke();
                }
            }
        }
    }
    void ScaleZero()
    {
        gameObject.transform.localScale = Vector3.zero;
    }
    public void ForceDeath()
    {
        death = true;
        DeathEvent.Invoke();
        ExplodeEvent.Invoke();
    }
    IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(2.5f);
        Destroy(gameObject);
    }
    IEnumerator DamageEffect()
    {

        canDamageEffect = false;
        block.SetColor(colorProperty, freezeColor);
        block.SetColor(blackTintProperty, freezeBlackColor);
        mesh.SetPropertyBlock(block);
        yield return new WaitForSeconds(damageTime);
        block.SetColor(colorProperty, Color.white);
        block.SetColor(blackTintProperty, Color.black);
        mesh.SetPropertyBlock(block);
        canDamageEffect = true;
    }
}
 