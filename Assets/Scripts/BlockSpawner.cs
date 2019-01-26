using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{

    public Block[] possibleBlocks;

    public Block[] additionalBlocksMed;

    public Block[] additionalBlocksHard;

    public Transform spawnPoint;

    public Transform previewspawn;

    private Block previewblock;

    private Block spawnedBlock;

    private bool newpreview = true;

    private Block preview;
    private Block previousblock;
    private Block previousblock2;
    private Block previousblock3;



    private void Update() {
        List<Block> blocksToUse = new List<Block>();

        blocksToUse.AddRange(possibleBlocks);

        if (Time.time > Tweaks.Instance.mediumDifficultyTime)
            blocksToUse.AddRange(additionalBlocksMed);
        if (Time.time > Tweaks.Instance.hardDifficultyTime)
            blocksToUse.AddRange(additionalBlocksHard);
        
        if (newpreview)
        {
            Block selection = blocksToUse[Random.Range(0, blocksToUse.Count)];
            if ((selection == previousblock) || (selection == previousblock2) || (selection == previousblock3))
            {

            }
            else
            {
                Debug.Log("Creates preview");
                previewblock = Instantiate(selection, previewspawn.transform.position, Quaternion.identity);
                previewblock.enabled = false;

                previousblock3 = previousblock2;

                previousblock2 = previousblock;

                previousblock = selection;

                preview = selection;

                newpreview = false;
            }
        }
        if (spawnedBlock == null)
        {
            spawnedBlock = Instantiate(preview, spawnPoint.transform.position, Quaternion.identity);

            newpreview = true;
            Debug.Log("Destroys preview");
            Destroy(previewblock.gameObject);
        }
    }
}