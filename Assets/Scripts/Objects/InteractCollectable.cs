using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractCollectable : InteractWithPlayer
{
    protected override void Interact()
    {
        base.Interact();
        GetComponent<Collectable>().PlayerHasCollected();

    }


}
