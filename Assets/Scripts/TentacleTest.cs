using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

[ExecuteInEditMode]
public class TentacleTest : MonoBehaviour
{
    public int segments;
    [SerializeField] private LineRenderer line;
    [SerializeField] private Transform target;
    [SerializeField] private Transform wiggleTarget;
    [SerializeField] private Transform tailTarget;
    [SerializeField] private float length;
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private float trailSpeed = 250f;
    [SerializeField] private float wiggleSpeedMin;
    [SerializeField] private float wiggleSpeedMax;
    [SerializeField] private float wiggleMagnitudeMin;
    [SerializeField] private float wiggleMagnitudeMax;
    [SerializeField] private bool visualzie;
    private float wiggleSpeed;
    private float wiggleMagnitude;
    private float targetDistance;
    private Vector3[] segmentPositions;
    private Vector3[] segmentVelocities;

    private float wiggleOffset;
     
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        
        if (Application.isPlaying)
        {
            InitializePoints();
        }
        else if (visualzie)
        {
            InitializePoints();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.isPlaying)
        {
            UpdatePositions();
        }
        else if (visualzie)
        {
            UpdatePositions();
        }
        else
        {
            line.positionCount = 0;
        }
    }

    private void UpdatePositions()
    {
        #if UNITY_EDITOR
            if (line.positionCount == 0)
            {
                InitializePoints();
            }
        #endif
        targetDistance = length/segments;
        wiggleTarget.localRotation = Quaternion.Euler(0,0,Mathf.Sin(wiggleSpeed * Time.time+wiggleOffset)*wiggleMagnitude);
        segmentPositions[0] = target.position;
        for (int i = 1; i < segments; i++)
        {
            segmentPositions[i] = Vector3.SmoothDamp(segmentPositions[i], segmentPositions[i-1]+target.right*targetDistance, ref segmentVelocities[i], smoothSpeed+i/trailSpeed);
            //segmentPositions[i] = Vector3.Lerp(segmentPositions[i], tailTarget.transform.position, ((float)i+1)/segments);
        }
        line.SetPositions(segmentPositions);
    }

    void InitializePoints()
    {
        line.positionCount = segments;
        segmentPositions = new Vector3[segments];
        segmentVelocities = new Vector3[segments];
        wiggleOffset = Random.Range(-10.0f, 10.0f);
        wiggleSpeed = Random.Range(wiggleSpeedMin, wiggleSpeedMax);
        wiggleMagnitude = Random.Range(wiggleMagnitudeMin, wiggleMagnitudeMax);
        targetDistance = length/segments;
        segmentPositions[0] = target.position;
        for (int i = 1; i < segments; i++)
        {
            segmentPositions[i] = segmentPositions[i - 1] + target.right * targetDistance;
        } 
        line.SetPositions(segmentPositions);
    }

    internal float GetLength()
    {
        return length;
    }

    internal void SetLength(float length)
    {
        this.length = length;
    }

    internal float GetTrailSpeed()
    {
        return trailSpeed;
    }
    internal void SetTrailSpeed(float speed)
    {
        trailSpeed = speed;
    }

}
