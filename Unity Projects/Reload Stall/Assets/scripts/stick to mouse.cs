using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class sticktomouse : MonoBehaviour
{
    //not actually sure what these do
    //I really fucking wish I knew what this did but I dont have time to research that rabbit hole rn
    //just fuffing trust the docs for a minute me
    RaycastHit2D raycastHit2D;
    Transform clickObject;

    //for use later to find world position and stick this things position to the mouse
    Vector3 myPos;
    Vector3 mousePos;

    //setting for main camera in inspector
    public Camera mainCamera;
    //public sprites to set in inspector
    public Sprite spriteOpen;
    public Sprite spriteClosed;

    //finding sprite renderer for scripts use
    private SpriteRenderer spriteRenderer;

    //setting up for the mouse hover, so we can use it to communicate whats being hovered over
    //nothing by default
    //public so it can be referenced in draggable, but hidden so nothing appears in inspector because I dont want manual control of this one
    [HideInInspector]
    public String hoveredOver = "nothing";//this is our ONLY hide in inspector public variable so far, and I would rather keep it that way if possible

    
    //called at start
    void Start()
    {
        //setting spriterender to this objects component for easier use later
        spriteRenderer = GetComponent<SpriteRenderer>();

    }


    // update per frame
    void Update()
    {

        //finding current screenspace mouse position
        mousePos = Input.mousePosition;
        //setting default depth
        mousePos.z = 5f;

        //converting mouse position from screen to world space
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);



        //setting ray pos and direciton to match mouse.... I think (not totally sure)
        Ray mouseRay = mainCamera.ScreenPointToRay(mousePos);

        //using our mouseRay to actually shoot a ray and check for object collisions with a 2D collider
        raycastHit2D = Physics2D.Raycast(mouseRay.origin, mouseRay.direction);
        clickObject = raycastHit2D ? raycastHit2D.collider.transform : null;
        
        //ok, time for the real hovering ID block
        //so we got the other side working, but now we can teleport objects, so we need to make it so the hover ID resets when you're not
        //actively hovering over something
        if (clickObject)
        {

            hoveredOver = raycastHit2D.collider.name;
            //Debug.Log(hoveredOver);

        }
        else
        {

            hoveredOver = "NOTHING!!!! HAHAHAHAHHAHA";

        }



        /*

        Im gonna keep this here for now for posterities sake, so I can tell what was happening and how to continue

        Ill make a new version of this block for the actual use case

        //diagnostic for now
        //checking to see if the ray is properly hitting 2D colliders, and if so printing to log that it is functioning
        //good news it works, now lets try testing if we can infact pull other objects components when getting hit with the ray
        //otherwise Ill have to do it the stupid way
        if (clickObject) {

            Debug.Log("this shizzz worky");
            Debug.Log(raycastHit2D.collider);
            //this works and does actually return a string
            //now we save this as a variable string to check against names to tell what mouse is hovering over
            Debug.Log(raycastHit2D.collider.name + " pp poo poo");
            
            //turns out we can just read varaibles from draggableobject by just calling the variable name of the component like this
            //so thats nice and easy
            Debug.Log(raycastHit2D.collider.GetComponent<draggableobject>().sticktoMousetest);

        } else { 
        

            Debug.Log("there freaking nothing boss");
            

        }

        */





        //myPos = mousePos;
        //unused


        //note to me, maybe dont split these functionalities apart from one another with the raycast, maybe fix that later

        //updating myPos to match worldPos of the mouse cursor
        myPos = worldPos;

        //moves the scripts attached object to new myPos
        transform.position = myPos;



        //diagnostic BS
        //Debug.Log(mousePos);
        //Debug.Log(myPos);
        //Debug.Log(worldPos);


        //these VVVV down there will likely change in the future when mouse context is added so that different objects will have different sprites
        //when open or closed depending on what grab animation those specific objects should use
        //for now tho these will work, because this prototype needs to get done before you worry about polish you dingdong

        //when left clicking close the hand
        if (Input.GetMouseButtonDown(0))
        {

            spriteRenderer.sprite = spriteClosed;

        }

        //when un left clicking open the hand
        if (Input.GetMouseButtonUp(0))
        {

            spriteRenderer.sprite = spriteOpen;

        }
        

    }
}
