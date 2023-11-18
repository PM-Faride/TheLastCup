using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedForRunner : MonoBehaviour
{
    [SerializeField] private float speedForGround;
    // Start is called before the first frame update
    private void Awake()
    {
        Variables.speedFroGround = speedForGround;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
