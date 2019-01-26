using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{

    public Block[] possibleBlocks;

    public Block[] possibleBlocksMed;

    public Block[] possibleBlocksHard;

    public Transform spawnPoint;

    public Transform previewspawn;

    private Block previewblock;

    private Block spawnedBlock;

    private float difficulty;

    private bool newpreview = true;

    private Block preview;
    private Block previousblock;
    private Block previousblock2;
    private Block previousblock3;



    private void Update()


    {
        if (difficulty <= 9999)
        {
            if (newpreview == true)
            {
                Block selection = possibleBlocks[Random.Range(0, possibleBlocks.Length)];
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
            if (spawnedBlock == null)
            {
                spawnedBlock = Instantiate(preview, spawnPoint.transform.position, Quaternion.identity);

                newpreview = true;
                Destroy(previewblock.gameObject);
            }




        }
        if (difficulty > 99999)
        {
            if (newpreview == true)
            {
                Block selection = possibleBlocks[Random.Range(0, possibleBlocks.Length)];
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
            if (spawnedBlock == null)
            {
                spawnedBlock = Instantiate(preview, spawnPoint.transform.position, Quaternion.identity);

                newpreview = true;
                Destroy(previewblock.gameObject);
            }
        }

        if (difficulty > 9999999)
        {
            if (newpreview == true)
            {
                Block selection = possibleBlocks[Random.Range(0, possibleBlocks.Length)];
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
            if (spawnedBlock == null)
            {
                spawnedBlock = Instantiate(preview, spawnPoint.transform.position, Quaternion.identity);

                newpreview = true;
                Destroy(previewblock.gameObject);
            }
        }
        difficulty = Time.time;
    }
}