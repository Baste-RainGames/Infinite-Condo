using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour {
    
    public Block[] possibleBlocks;
    private Block spawnedBlock;


    private void Update() {
        var spawnPoint = new Vector3(Mathf.FloorToInt(Tweaks.Instance.GridX / 2f), Tweaks.Instance.GridY, 0);
        
        if (spawnedBlock == null) {
            var selection = possibleBlocks[Random.Range(0, possibleBlocks.Length)];
            spawnedBlock = Instantiate(selection, spawnPoint, Quaternion.identity);
        }
    }
}
