using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileModeHandler : MonoBehaviour
{

    public static MissileModeHandler Instance = null;
    [SerializeField] private GameObject missile;
    [SerializeField] private float missilePhase3PositionY;
    [SerializeField] private GameObject player;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(Instance.gameObject);
            Instance = this;
        }
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddMissilePhase(GameObject spawnPoint , int phaseNumber)
    {
        Debug.Log("Add Missile" + phaseNumber);
        if (phaseNumber == 0 || phaseNumber == 1)
        {
            GameObject tmpMissile = Instantiate(missile, spawnPoint.transform.position, missile.transform.rotation);
            if(phaseNumber == 0)
            {
                tmpMissile.name = "MissilePhase0";
            }
            else
            {
                tmpMissile.name = "MissilePhase1";
            }
            tmpMissile.SetActive(true);
            tmpMissile.GetComponent<MissileForMissileMode>().SetToMissile(phaseNumber);
        }
        else
        {
            Debug.Log("Phase2");
            GameObject tmpMissile = Instantiate(missile, new Vector3(player.transform.position.x , missilePhase3PositionY , 0)
                , new Quaternion(0 , 0 , 180 , 1));
            Debug.Log(missile.transform.rotation);
            tmpMissile.name = "MissilePhase2";
            tmpMissile.SetActive(true);
            tmpMissile.GetComponent<MissileForMissileMode>().SetToMissile(phaseNumber);
        }
    }
    
}
