using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;


public class RimLightSource : MonoBehaviour
{
    [ColorUsage(false, true)]
    [SerializeField] Color lightColor;
    [SerializeField] float minLightThicknes;
    [SerializeField] float maxLightThicknes;
    [SerializeField] float maxDistance;

    private CharacterMove player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            player.SetLightThickness(GetLightThickness());
        }
    }

    private float GetLightThickness()
    {
        float currentDistance = Mathf.Abs(player.transform.position.x-transform.position.x);
        if (currentDistance > maxDistance)
        {
            currentDistance = maxDistance;
        }
        return  Mathf.Lerp(maxLightThicknes,minLightThicknes ,currentDistance / maxDistance);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (player == null)
            {
                player = other.GetComponent<CharacterMove>();
            }
            if (player != null)
            {
                player.SetLightProperties(transform.position, lightColor, minLightThicknes);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (player == null)
            {
                player = other.GetComponent<CharacterMove>();
            }
            if (player != null)
            {
                player.RemoveLight();
            }
            player = null;
        }
    }
}
