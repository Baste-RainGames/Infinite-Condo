using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour {
    
    public Block[] possibleBlocks;
    public Transform spawnPoint;

    private Block spawnedBlock;


    private void Update() {
        if (spawnedBlock == null) {
            var selection = possibleBlocks[Random.Range(0, possibleBlocks.Length)];
            spawnedBlock = Instantiate(selection, spawnPoint.transform.position, Quaternion.identity);
        }
    }
}
