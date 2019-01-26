using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour {
    
    public Block[] possibleBlocks;

    public Block[] possibleBlocksMed;

    public Block[] possibleBlocksHard;

    private Block spawnedBlock;

    private float difficulty;

    private Block previousblock;
    private Block previousblock2; 
    private Block previousblock3;

    

    private void Update()
    {
        var spawnPoint = new Vector3(Mathf.FloorToInt(Tweaks.Instance.GridX / 2f), Tweaks.Instance.GridY, 0f);
        
        if (spawnedBlock == null)
        {
            if (difficulty <= 9999)
            {
                Block selection = possibleBlocks[Random.Range(0, possibleBlocks.Length)];
                 
                if ((selection == previousblock) || (selection == previousblock2) || (selection == previousblock3))
                    {

                    }
                else
                {
                    spawnedBlock = Instantiate(selection, spawnPoint, Quaternion.identity);

                    previousblock3 = previousblock2;

                    previousblock2 = previousblock;

                    previousblock = selection;
                }


    
            }

            if (difficulty > 99999)
            {
                Block selection = possibleBlocks[Random.Range(0, possibleBlocks.Length)];

                if ((selection == previousblock) || (selection == previousblock2) || (selection == previousblock3))
                {

                }
                else
                {
                    spawnedBlock = Instantiate(selection, spawnPoint, Quaternion.identity);

                    previousblock3 = previousblock2;

                    previousblock2 = previousblock;

                    previousblock = selection;
                }
            }

            if (difficulty > 9999999)
            {
                Block selection = possibleBlocks[Random.Range(0, possibleBlocks.Length)];

                if ((selection == previousblock) || (selection == previousblock2) || (selection == previousblock3))
                {

                }
                else
                {
                    spawnedBlock = Instantiate(selection, spawnPoint, Quaternion.identity);

                    previousblock3 = previousblock2;

                    previousblock2 = previousblock;

                    previousblock = selection;
                }
            }
            
        }

        difficulty = Time.time;

    }
}
