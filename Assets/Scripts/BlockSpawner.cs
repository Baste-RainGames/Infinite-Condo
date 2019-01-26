using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{

    public Block[] possibleBlocks;

    public Block[] additionalBlocksMed;

    public Block[] additionalBlocksHard;

    public Transform previewspawn;

    public Block previewblock;

    public Block spawnedBlock;

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
                previewblock = Instantiate(selection, previewspawn.transform.position, Quaternion.identity);
                previewblock.enabled = false;

                previousblock3 = previousblock2;

                previousblock2 = previousblock;

                previousblock = selection;

                preview = selection;

                newpreview = false;
            }
        }
        if (spawnedBlock == null || !spawnedBlock.enabled)
        {
            var spawnPoint = new Vector3(Mathf.Floor(Tweaks.Instance.GridX / 2f) + 0.5f, Mathf.Floor(Tweaks.Instance.GridY) - 0.5f);
            
            spawnedBlock = Instantiate(preview, spawnPoint, Quaternion.identity);

            newpreview = true;
            if(previewblock != null)
                Destroy(previewblock.gameObject);
            else
                Debug.LogError("Help no preview block!");
        }
    }
}