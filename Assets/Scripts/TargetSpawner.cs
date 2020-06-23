using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    public float minMaxXSpawnRange;
    public float minMaxZSpawnRange;

    public List<GameObject> targetPrefabs;

    public float spawnTime = 5.0f;
    private float spawnTimer = 0.0f;

    public bool isSpawning = false;

    // public Room myRoom;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSpawning) return;

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnTime)
        {
            spawnTimer = 0.0f;
            Spawn();
        }
    }

    void Spawn()
    {
        int index = Random.Range(0, targetPrefabs.Count);
        float randX = Random.Range(-minMaxXSpawnRange, minMaxXSpawnRange);
        float randZ = Random.Range(-minMaxZSpawnRange, minMaxZSpawnRange);

        Vector3 spawnPos = new Vector3(randX, 5.0f, randZ);
        spawnPos.x += transform.position.x;
        spawnPos.z += transform.position.z;

        float randY = Random.Range(0.0f, 360.0f);
        Quaternion spawnRot = Quaternion.Euler(0.0f, randY, 0.0f);

        GameObject clone = Instantiate(targetPrefabs[index], spawnPos, spawnRot);
        /* Hazard hazard = clone.GetComponent<Hazard>();
        hazard.myRoom = myRoom; */
    }
}
