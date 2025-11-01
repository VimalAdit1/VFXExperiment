using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]public struct TentacleDetails
{
    [SerializeField] public TentacleTest tentacle;
    [HideInInspector]public float startLength;
    [HideInInspector]public float startSpeed;
    [SerializeField] public float endLength;
}
public class Pillar : MonoBehaviour
{
    [SerializeField] private List<TentacleDetails> tentacles;

    [SerializeField] private bool isSolved;

    private bool isShrunk;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeTentacles();
    }

    private void InitializeTentacles()
    {
        for(int i=0; i< tentacles.Count; i++)
        {
            TentacleDetails tentacleDetails = tentacles[i];
            if (tentacleDetails.tentacle == null)
            {
                continue;
            }
            tentacleDetails.startLength = tentacleDetails.tentacle.GetLength();
            tentacleDetails.startSpeed = tentacleDetails.tentacle.GetTrailSpeed();
            tentacles[i] = tentacleDetails;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isSolved)
        {
            ShrinkTentacle();
        }
        else
        {
            GrowTentacles();
        }
    }

    private void GrowTentacles()
    {
        if (!isShrunk) return;
        foreach (TentacleDetails tentacleDetails in tentacles)
        {
            tentacleDetails.tentacle.SetTrailSpeed(tentacleDetails.startSpeed);
            tentacleDetails.tentacle.SetLength(tentacleDetails.startLength);
        }
        isShrunk = false;
    }

    private void ShrinkTentacle()
    {
        if (isShrunk) return;
        foreach (TentacleDetails tentacleDetails in tentacles)
        {
            tentacleDetails.tentacle.SetTrailSpeed(1500f);
            tentacleDetails.tentacle.SetLength(tentacleDetails.endLength);
        }
        isShrunk = true;
    }
}
