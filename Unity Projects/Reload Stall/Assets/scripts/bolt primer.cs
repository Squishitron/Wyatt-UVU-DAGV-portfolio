using UnityEngine;

public class boltPrimer : MonoBehaviour
{

    // settup ------------------------------------------------------------------------------------------------------------------------------------------------
    public GameObject StatusSpriteObject; // this is to store the object from Gun load handler so we can fiddle with its sprite
    public Sprite ReadySprite; // sets ready sprite in inspector
    public GameObject BoltObject; // sets the bolt we want to check for in inspector, in an effort to make this usable across multiple rifle types
    public GameObject GunObject; // sets the gun object in the inspector so that we can check for its ready status before allowing bolts to progress
    public GameObject PullBackCheck; // sets gameobject in inspector to check for the bolt being pulled back
    public GameObject PullForwardCheck; // sets gameobject in inspector to check for the bolt returning forward (this is mainly for sniper bolts, since they wont use springy)

    private bool StatusReady = false; // this is so we can check if the weapon is ready when its time to turn it in
    private int ReadySteps = 0; // this is so we can incrament when the bolt goes through the loading stages so we can prepare to get this weapon ready for submission
    private GameObject ME; // variable to store this object to make using it easier : REMEMBER TO SET AT START

    private string collidedObject; // this variable stores which object this is colliding with so we can check for triggers

    // METHODS -----------------------------------------------------------------------------------------------------------------------------------------------
    public bool GetReady() // this method will return the weapons overall ready status (Im starting to regret all these variables getting seperated, but the spaghetti is strong tonight (9/28/2026)
    {

        return StatusReady; // is grace ready question


    }

    // On Collision ------------------------------------------------------------------------------------------------------------------------------------------
    private void OnCollisionEnter2D(Collision2D collision) // this triggers when entering this collision to find the name of the thing that enters
    {

        collidedObject = collision.gameObject.name; // stores the collided objects name : REMEMBER RIGID BODY

    }

    // Start -------------------------------------------------------------------------------------------------------------------------------------------------
    void Start()
    {

        ME = this.gameObject; // setting ME to this gameobject
        PullForwardCheck.GetComponent<BoxCollider2D>().enabled = false; // setting second step to false until its called for
        PullBackCheck.GetComponent<BoxCollider2D>().enabled = true; // setting first step to true until its completed

    }

    // Update ------------------------------------------------------------------------------------------------------------------------------------------------
    void Update()
    {

        if (GunObject.GetComponent<GunLoadingHandler_V01>().getLoaded() == true) // checks to see if a mag is loaded before allowing bolt progression
        {
            if (collidedObject == PullBackCheck.name & Input.GetMouseButtonUp(0)) // this should hopefully check to see if the load object is touching and if the mouse has just been let go
            {

                ReadySteps += 1; // steps the ready chain forward by one
                PullBackCheck.GetComponent<BoxCollider2D>().enabled = false; // dissables the pull back hitbox to avoid extra or repeat progress
                PullForwardCheck.GetComponent<BoxCollider2D>().enabled = true; // enables the pull forward check to progress the check states

            }

            if (collidedObject == PullForwardCheck.name) // this checks for the pull forward check to progress the stage
            {

                ReadySteps += 1; // progress the ready chain forward by one again
                PullForwardCheck.GetComponent<BoxCollider2D>().enabled = false; // turns off the pull forward to finish the chain and prevent repeat progress
                BoltObject.GetComponent<draggableobject>().enabled = false; // turns off the grabbable of the bolt so you cant keep racking it after its ready

            }

        }

        if (ReadySteps >= 2) // this checks for far enough ready progress (aka the bolt being pulled back and let forward again
        {

            StatusReady = true; // flags the gun as being ready for submission for the next step
            StatusSpriteObject.GetComponent<SpriteRenderer>().sprite = ReadySprite; // sets the status object to show the ready status for the player feedback

        }

        if (ME.GetComponent<draggableobject>().getGrabbed() == false) // checks to see when Object is let go off, then resets collision name
        { // changed to ME object because it should be the one to have the draggable component, and in this case it is the bolt itself

            collidedObject = "none"; // removed particle systems, since they are not needed for this step

        }

        // DEBUG -------------------------------------------------------------------------------------------------------------------------------------------------
        //Debug.Log(ReadySteps);
        //Debug.Log(StatusReady);

    }

}
