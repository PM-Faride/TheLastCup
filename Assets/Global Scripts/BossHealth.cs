using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossHealth : MonoBehaviour
{
    private string colorProperty = "_Color";
    private string blackTintProperty = "_Black";
    [SerializeField] private bool isFirstPhase;
    [SerializeField] private int allBossHealth;
    [SerializeField] private int counterHealthPhase = 0;
    MaterialPropertyBlock block;
    private bool canDamageEffect;
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
    // Start is called before the first frame update
    void Start()
    {
        if(isFirstPhase)
        {
            Variables.bossAllPhaseHealth = allBossHealth;
            Variables.bossFirstHealth = allBossHealth;
        }
        if (defaultOmidColor)
        {
            freezeColor = new Color32(255, 255, 255, 255);
            freezeBlackColor = new Color32(55, 51, 51, 0);
        }
        canDamageEffect = true;
        block = new MaterialPropertyBlock();
        healthLeft = healthNumber;

    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!death)
        {
            if (collision.gameObject.tag == damagerTag && healthLeft > 0 && !Variables.lose)
            {
                if (canDamageEffect && gameObject.activeSelf)
                {
                    StartCoroutine(DamageEffect());
                }
                healthLeft--;
                Variables.bossAllPhaseHealth--;
            }
            if (healthLeft <= 0)
            {
                death = true;
                gameObject.tag = "DeathEnemy";
                DeathEvent.Invoke();
                ExplodeEvent.Invoke();
                //Debug.Log("death");
            }
            else if(healthPhase.Length > 0)
            {
                for (int i = 0; i < healthPhase.Length; i++)
                {
                    if (healthLeft <= healthPhase[i] && counterHealthPhase == i)
                    {
                        Debug.Log("BUUUUUUUUUUUUUG");
                        counterHealthPhase++;
                        HealthPhaseEvent[i].Invoke();
                    }
                }
            }
        }
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
    public void RemoteDamage()
    {
        if(!death && healthLeft > 0)
        {
            healthLeft--;
            Variables.bossAllPhaseHealth--;
        }
        if (healthLeft <= 0)
        {
            death = true;
            DeathEvent.Invoke();
        }
    }
}
