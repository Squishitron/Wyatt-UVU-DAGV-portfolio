using UnityEngine;

public class RingTheBell : MonoBehaviour
{

    // setup -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public GameObject GRIPPERS; // finding the grippers object because we need the hover and parent ID's to check for clicking on the ding dong
    public GameObject checkOutObject; // this should store the object the checkout script is attached to, aka Bell and Chekcs

    private bool dingleDongle = false; // this boolean is to control whether or not the checkout hitbox is active or not, maybe also play a sound once we get there

    // Methods -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void HitBoxEnables(bool x) // this probably doesnt need to be a method, but eh, its gonna be one now, this will controll whether or not the chekcout hitbox is on, basically meant to
    { // mirror the state of the passed bool

        checkOutObject.GetComponent<BoxCollider2D>().enabled = x; // mirroring enabled state to the bool

    }

    // Start -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    void Start()
    {
        


    }

    // Update ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    void Update()
    {
        
        if (GRIPPERS.GetComponent<sticktomouse>().hoveredOver == this.gameObject.name & GRIPPERS.GetComponent<sticktomouse>().hoveredOverParent == this.gameObject.transform.root.gameObject.name & Input.GetMouseButtonDown(0)) // inital check to see if the hover ID matches this object 
        {

            dingleDongle = true; // sets the enabler bool to true when hovered and clicked
            Debug.Log("we getting in here?");

        }
        
        if (Input.GetMouseButtonUp(0)) // maybe if we reset when un-clicking it could work better
        {

            dingleDongle = false; // resets the enabler bool to false when not clicked or not hovered

        }

        HitBoxEnables(dingleDongle); // always runs to force the hitbox on or off depending on clicking

        // VVVV DEBUG VVVV ------------------------------------------------------------------------------------------------------------------------------------------------------------------------


    }

}

// we are almost there
// but something seems incorrect, it only seems to work getting clicked on.... sometimes
// I need to figure out how to get it clicked on *ALL* the time
// HRMMMMM suspicious... it gets into the loop every click, but the thing doesnt enable every click...

// OK! updating it to reset when letting go instead it works nice now
// now we need score and an overall timer