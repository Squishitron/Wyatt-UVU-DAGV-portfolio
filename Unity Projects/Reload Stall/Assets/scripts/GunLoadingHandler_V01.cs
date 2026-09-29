using UnityEngine;

public class GunLoadingHandler_V01 : MonoBehaviour
{

    // SETUP FIERST ------------------------------------------------------------------------------------------------------------------------------------------

    public GameObject loadObject; // set in inspector which object we want to use to check for loadage (maybe replace with ID system later)
    public GameObject ChildObject; // set in insepctor to parent this object to new parent post loading
    public GameObject SpriteObject; // set in inspector which object to use to show the full tag when the mag is full
    public Sprite EmptySprite; // sets empty sprite in inspector
    public Sprite LoadedSprite; // sets mid sprite in inspector
    public Sprite ReadySprite; // sets the ready to sell sprite in inspector
    
    private int howManyToLoad = 1; // changed to private and set to 1 so we can recicle the grabbiby stuff to get this to work to speed things up
    private int howManyLoaded = 0; // zero defualt becuase it should start unlaoded
    private bool isLoaded = false; // controls the state of loading completion, false by default until turned on by loading completion
    private string collidedObject; // this should hopefully store which object the parent object is colliding with
    private GameObject ME; // variable to store this game object as a sprite for parenting reasons later

    // VVVV METHODS VVVV -------------------------------------------------------------------------------------------------------------------------------------

    public void setLoaded(bool yippie) // sets whether or not the parent object is concidered loaded
    {

        isLoaded = yippie; // sets isLoaded to passed arguments to allow further loading or changing states from being loaded to loading the gun

    }

    public bool getLoaded() // gets the loaded state of the parent object, used externally
    {

        return isLoaded; // returns is loaded

    }

    public void setSprite(Sprite x)
    {

        ME.GetComponent<SpriteRenderer>().sprite = x; // sets parent objects sprite to the passed argument

    }

    // not sure we need these methods for this part, but I'll keep them for a second so I can have them if needed
    
    // VVVV COLLISION CHECKS VVVV ----------------------------------------------------------------------------------------------------------------------------
   
    private void OnCollisionEnter2D(Collision2D collision) // this triggers when entering this collision to find the name of the thing that enters
    {

        collidedObject = collision.gameObject.name; // this should hopefully update collidedObject's name to reflect anything colliding with it
        // it works now, I forgot to add a rigid body

    }

    // STEART ------------------------------------------------------------------------------------------------------------------------------------------------
    void Start()
    {

        ME = this.gameObject; // sets ME to the parent gameobject of the script

    }

    // Im only going a tiny bit insane its fine --------------------------------------------------------------------------------------------------------------
    void Update()
    {

        if (loadObject != null) // makes sure this object is set and only runs when safe
        {

            if (collidedObject == loadObject.name & Input.GetMouseButtonUp(0)) // this should hopefully check to see if the load object is touching and if the mouse has just been let go
            {

                howManyLoaded += 1; // removed particle systems, since they are not needed for this step
                SpriteObject.GetComponent<SpriteRenderer>().sprite = LoadedSprite; // shows the loaded sprite to denote the mag being accepted into the weapon

            }

            if (ChildObject.GetComponent<draggableobject>().getGrabbed() == false) // checks to see when loaded Object is let go off, then resets collision name
            { // changed to child object because it should be the one to have the draggable component, and all triggers should be outside objects

                collidedObject = "none"; // removed particle systems, since they are not needed for this step

            }

            if (howManyLoaded >= howManyToLoad) // checks to see if the amount of bullets loaded increases enough to trigger full load
            {

                loadObject.gameObject.GetComponent<BoxCollider2D>().enabled = false; // turns off the collider to dissable any more loading
                ChildObject.transform.position = new Vector3 (ME.transform.position.x, ME.transform.position.y); // sets the load object position to this objects position
                // in this case this should be mag, but Im just trying not to hard code (to clarify this should absolutely work with anything you want to parent. Its just a tad spaghetti
                ChildObject.GetComponent<BoxCollider2D>().enabled = false; //dissables the box collider on the child object (the one with the draggable) so that it cant be dragged anymore
                setLoaded(true); // sets loaded to true for next step of the process

            }

        }

        // VVVV DEBUG VVVV ------------------------------------------------------------------------------------------------------------------------------------

        //Debug.Log(collidedObject); // testing to see if the collision is working
        //Debug.Log(howManyLoaded);

    }

}

// oh carp, ray isnt working either


// ray stuff that didnt work VVVVVVVV ------------------------------------------------------------------------------------------------------------------------

//RaycastHit2D raycastHit2D; // screw it, collisions are hard and I dont have time to learn, so we doing raycasts again since we already know
                           // how raycasts work. So we just gonna do rays again for now
//Transform hitObject; // used with raycast to track which object we are hitting

/*
Vector3 temp = this.gameObject.transform.position; // setting x and y for the ray
temp.z = 5f; // setting dept for the ray
Ray handlerRay = mainCamera.ScreenPointToRay(temp); // ray settup from temp position
raycastHit2D = Physics2D.Raycast(handlerRay.origin, handlerRay.direction); // raycast from handlerRay position
hitObject = raycastHit2D ? raycastHit2D.collider.transform : null; // check for collision and if not null return object to colliding object

Debug.Log(hitObject); // checking to see if ray is working properly
                      // its not, this is a huge problem

//if (Collision.GameObject.name == ) */