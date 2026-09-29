using UnityEngine;

public class MagLoadingHandler_V01 : MonoBehaviour
{

    // SETUP FIERST ------------------------------------------------------------------------------------------------------------------------------------------

    public GameObject loadObject; // set in inspector which object we want to use to check for loadage (maybe replace with ID system later)
    public GameObject BulletSpriteObject; // set in inspector which object to use to show when bullets have entered the mag
    public GameObject FullSpriteObject; // set in inspector which object to use to show the full tag when the mag is full
    public Sprite BulletSprite; // sets the cosmetic bullet sprite in inspector
    public Sprite EmptySprite; // sets empty sprite in inspector
    public Sprite FullSprite; // sets full sprite in inspector
    public int howManyToLoad = 1; // default of one for safety reasons, and also you should always need to load at least one
    public GameObject FullMagObject; // set object in inspector to allow this object to turn on colliders to be ready for the mag to enter the gun

    private int howManyLoaded = 0; // zero defualt becuase it should start unlaoded
    private bool isLoaded = false; // controls the state of loading completion, false by default until turned on by loading completion
    private string collidedObject; // this should hopefully store which object the parent object is colliding with

    private GameObject grippers; // this is to store the GRIPPAHS object because Im going to try to use it to determine whether or not to load
    private Camera mainCamera; // making a variable to store main camera. Imma try to store it out of grippahs this time

    // VVVV METHODS VVVV -------------------------------------------------------------------------------------------------------------------------------------

    public void setLoaded(bool yippie) // sets whether or not the parent object is concidered loaded
    {

        isLoaded = yippie; // sets isLoaded to passed arguments to allow further loading or changing states from being loaded to loading the gun

    }

    public bool getLoaded() // gets the loaded state of the parent object, used externally
    {

        return isLoaded; // returns is loaded

    }
    
    // VVVV COLLISION CHECKS VVVV ----------------------------------------------------------------------------------------------------------------------------
   
    private void OnCollisionEnter2D(Collision2D collision) // this triggers when entering this collision to find the name of the thing that enters
    {

        collidedObject = collision.gameObject.name; // this should hopefully update collidedObject's name to reflect anything colliding with it
        // it works now, I forgot to add a rigid body

    }

    // STEART ------------------------------------------------------------------------------------------------------------------------------------------------
    void Start()
    {

        // settup main camera
        mainCamera = GameObject.Find("GRIPPAH").GetComponent<sticktomouse>().mainCamera; // sets this main camera to use the same as the grippers
        grippers = GameObject.Find("GRIPPAH"); // stores GRIPPAH as the local grippers variable to make it easier to use

    }

    // Im only going a tiny bit insane its fine --------------------------------------------------------------------------------------------------------------
    void Update()
    {

        if (loadObject != null) // makes sure this object is set and only runs when safe
        {

            if (collidedObject == loadObject.name & Input.GetMouseButtonUp(0)) // this should hopefully check to see if the load object is touching and if the mouse has just been let go
            {

                GameObject.Find("box of bullets").GetComponent<BulletSpawnHandler_V1>().allowParticle(false); // dissables the despawn particle when loading
                howManyLoaded += 1;
                BulletSpriteObject.GetComponent<SpriteRenderer>().sprite = BulletSprite; // changes cosmetic bullet sprite from empty to bullet to show when loading starts

            }

            if (loadObject.GetComponent<draggableobject>().getGrabbed() == false) // checks to see when loaded Object is let go off, then resets collision name
            {

                GameObject.Find("box of bullets").GetComponent<BulletSpawnHandler_V1>().allowParticle(true); // re-enables despawn particle when not loading
                collidedObject = "none";

            }

            if (howManyLoaded >= howManyToLoad) // checks to see if the amount of bullets loaded increases enough to trigger full load
            {

                FullSpriteObject.GetComponent<SpriteRenderer>().sprite = FullSprite; // shows the full sprite as a tag to let the player now the mag is full
                this.gameObject.GetComponent<BoxCollider2D>().enabled = false; // turns off the collider to dissable any more loading
                FullMagObject.GetComponent<BoxCollider2D>().enabled = true; // turns on the new collider so we can load the mag into the gun
                isLoaded = true; // sets loaded to true to allow for the next step loading into the gun

            }

        }

        // VVVV DEBUG VVVV ------------------------------------------------------------------------------------------------------------------------------------

        //Debug.Log(collidedObject); // testing to see if the collision is working
        // its not
        // it is now
        //Debug.Log(howManyLoaded);
        // wonderful, it succesfully only incriments by one each time

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