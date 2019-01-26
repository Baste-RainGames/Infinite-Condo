using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour {
    
    public Block toSpawn;
    public Transform spawnPoint;

    private Block spawnedBlock;


    private void Update() {
        if (spawnedBlock == null) {
            spawnedBlock = Instantiate(toSpawn, spawnPoint.transform.position, Quaternion.identity);
        }
    }
}
