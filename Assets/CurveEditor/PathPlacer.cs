using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPlacer : MonoBehaviour
{
    private bool stop = false;
    private GameObject tmpPoint;
    private int numberOfPoint;
    private int counterPosition = 0;
    private Vector3 destination;
    private Vector3[] pointPosition;
    private float originalSpeed;
    [SerializeField] private GameObject mortaz;
    [SerializeField] public GameObject point;
    [SerializeField] private float speed;
    [SerializeField] public float spacing = .1f;
    [SerializeField] public float resolution = 1;
    [SerializeField] private float threshold;

    void Start()
    {
        originalSpeed = speed;
        Vector2[] points = FindObjectOfType<PathCreator>().path.CalculateEvenlySpacedPoints(spacing, resolution);
        numberOfPoint = points.Length;
        pointPosition = new Vector3[points.Length];
        foreach (Vector2 p in points)
        {
            tmpPoint = Instantiate(point, p, point.transform.rotation, gameObject.transform);
            pointPosition[counterPosition] = tmpPoint.transform.position;
            counterPosition++;
        }
        counterPosition = 80;
        ChangeDestination();
    }
    private void Update()
    {
        if (!stop)
        {
            mortaz.transform.position =  Vector2.Lerp(mortaz.transform.position, destination, speed);
            if (Vector3.Distance(mortaz.transform.position, destination) <= threshold)
            {
                ChangeDestination();
            }
        }
    }

    void ChangeDestination()
    {
        destination = pointPosition[counterPosition];
        counterPosition++;
        if(counterPosition == numberOfPoint)
        {
            counterPosition = 0;
        }
    }
    public void Stop(bool dir)
    {
        stop = dir;
    }

    public void PauseMovement()
    {
        speed = 0;
    }

    public void ContinueMoving()
    {
        speed = originalSpeed; 
    }

}
