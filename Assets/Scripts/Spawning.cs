using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawning : MonoBehaviour
{
    public GameObject[] spawnObjects;
    public Transform[] spawnLocations;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < spawnLocations.Length; i++)
        {
            Instantiate(spawnObjects[Random.Range(0, spawnObjects.Length)], spawnLocations[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
