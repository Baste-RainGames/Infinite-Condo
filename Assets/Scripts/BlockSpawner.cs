using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public Transform previewspawn;

    private Block previewblock;

    private Block spawnedBlock;

    private bool enoughUniqueBlocksToPreventPrev;

    private Block preview;
    private Block previousblock;
    private Block previousblock2;
    private Block previousblock3;

    private void Start() {
        enoughUniqueBlocksToPreventPrev = Tweaks.Instance.startingBlocks.Distinct().Count() > 3;
    }


    private void Update() {
        if (SharkAttack.SHARKATTACK)
            return;
        
        if (spawnedBlock == null || !spawnedBlock.enabled)
        {
            if (preview != null) {
                var spawnPoint = new Vector3(Mathf.Floor(Tweaks.Instance.GridX / 2f) + 0.5f, Mathf.Floor(Tweaks.Instance.GridY) - 0.5f);
                spawnedBlock = Instantiate(preview, spawnPoint, Quaternion.identity);
                Destroy(previewblock.gameObject);
            }

            SpawnPreviewOfNext();
        }
    }

    private void SpawnPreviewOfNext() {
        List<Block> blocksToUse = new List<Block>();
        blocksToUse.AddRange(Tweaks.Instance.startingBlocks);

        if (Time.time > Tweaks.Instance.mediumDifficultyTime)
            blocksToUse.AddRange(Tweaks.Instance.additionalBlocksMediumDifficulty);
        if (Time.time > Tweaks.Instance.hardDifficultyTime)
            blocksToUse.AddRange(Tweaks.Instance.additionalBlocksHardDifficulty);

        var selection = blocksToUse[Random.Range(0, blocksToUse.Count)];
        if (enoughUniqueBlocksToPreventPrev) {
            while((selection == previousblock) || (selection == previousblock2) || (selection == previousblock3))
                selection = blocksToUse[Random.Range(0, blocksToUse.Count)];
        }
            
        previewblock = Instantiate(selection, previewspawn.transform.position, Quaternion.identity);
        previewblock.enabled = false;

        previousblock3 = previousblock2;

        previousblock2 = previousblock;

        previousblock = selection;

        preview = selection;
    }
}