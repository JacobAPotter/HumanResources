using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class checks if the player can not be seen 
//becuase a wall is blocking him. 
//if so, the material is switched to its transparent equivalent.
public class Wall : MonoBehaviour
{
    Material mat;
    Material transMaterial;
    Renderer rend;
    Player player;
    bool transMatSet;

    private void Start()
    {
        rend = GetComponent<Renderer>();

        //see if the wall is vertical or horizontal.
        //we only need to do this test if its horizontal
        float ratio = rend.bounds.size.x / rend.bounds.size.z;

        if (ratio < 1)
            enabled = false;

        WallMaterialManager man = GameManager.ActiveGameManager.WallMaterialManager;
        mat = rend.sharedMaterial;

        if (mat == man.contreteMat)
            transMaterial = man.concreteTrans;
        else if(mat == man.sciFiMat)
            transMaterial = man.sciFiTrans;
        else if (mat == man.metalMat)
            transMaterial = man.metalTrans;
        else if (mat == man.marbleMat)
            transMaterial = man.marbleTrans;
        else if (mat == man.dirtyMat)
            transMaterial = man.dirtyTrans;

        player = GameManager.ActiveGameManager.Player;
    }
    private void Update()
    {

        bool playerInXBounds = (player.transform.position.x < rend.bounds.max.x) &&
                               (player.transform.position.x > rend.bounds.min.x);

        float zPos = player.transform.position.z - transform.position.z;

        if (zPos > 0 && zPos < 5 && playerInXBounds)
        {
            if(!transMatSet)
            {
                transMatSet = true;
                rend.sharedMaterial = transMaterial;
            }
        }
        else
        {
            if(transMatSet)
            {
                transMatSet = false;
                rend.sharedMaterial = mat;
            }
        }
    }
}
