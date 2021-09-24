using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidGenerator : MonoBehaviour
{
    
    [SerializeField] Boid boidPrefab = null;
    [SerializeField] BoidSettings boidSettings = new BoidSettings();
    [SerializeField, Range(0, 2000)] int nbBoid = 10;
    [SerializeField, Range(0, 100)] float radiusSpawn = 20;

    bool IsValid => boidPrefab;

    private void Start()
    {
        if (!IsValid) return;
        SpawnBoids();
    }

    void SpawnBoids()
    {
        Vector3 _pos = Vector3.zero;
        for (int i = 0; i < nbBoid; i++)
        {
            _pos = UnityEngine.Random.insideUnitSphere * radiusSpawn;
            Boid _boid = Instantiate(boidPrefab, _pos, Quaternion.identity);
            _boid.gameObject.transform.SetParent(transform);
            _boid.SetSettings(boidSettings, transform);
        }
    }
}
