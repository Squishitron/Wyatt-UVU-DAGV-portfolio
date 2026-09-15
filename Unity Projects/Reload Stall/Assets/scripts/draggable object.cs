using System;
using UnityEditor.UI;
using UnityEngine;

public class draggableobject : MonoBehaviour

{

    //setting up to find screen and world positions
    Vector3 mousePos;
    RaycastHit2D raycastHit2D;
    Transform clickObject;
    //public to be set in inspector
    public Camera mainCamera;
    //setting variable to track if this object is grabbable or not, public idealy with default false
    //if that public could be a dropdown menu would be super ideal
    public Boolean isGrabbable = false;
    //referencing the gripper object so we can access its script to check against its hover variable
    //damn, not allowed to use find
    //so thisVVVV is returning null, maybe if we just directly find the object and its variable and just store the variable we want right away?

    //private GameObject grippers = GameObject.Find("GRIPPAH");

    //so apparently .Find has to be called in start or update and I've been running in circles for no fucking reason and now Im disgruntled
    //new plan, declare private gameobject here, leave it empty, in start find the other fucking object (which actually makes sense now Im thinking about it)
    //because none of the stuff we're sniffing for actually happens until AFTER the game starts running, so makes sense in hindsight
    //anyway, we then check against it to see if grabby stuff happens
    
    //ok its defined, now fill it during start VVV
    GameObject grippers;




    //so anyway this was all garbo in the end VVV
    //it pointed me towards trying some "object sterilization or something" we'll try that
    //ok I think I found the syntax, its serialize / serialized
    //[SerializeField] private sticktomouse stickymouse;
    //ok, in theory this should allow us to use variables from the stick to mouse script
    //it might have except we need the object specific version and .find just works actually like actually cause lying internet ai




    //testing a random variable to see if we can pull it from stick to mouse (we can absolutely just via variable name, and also just object name)
    /*that means we can freely use a system like this to check if the object we are hovering is interactable
     or not, which in theory is to be local, but keep the variable public so that we can set it per object via the inspector so that we can use this
    same script for all hoverable objects regardless of if we want them to be grabbable or not
    maybe this should also be changable by other collisions so that we can turn off the grabbability of object as they are used in the game world
    so maybe a getter and setter might be appropriate afterall, or at least a setter for when bullets will eventually collide with the magazine*/
    
    //so it worked
    //public string sticktoMousetest = "default";



    //is this yitch grabbed rn? cause clickObject doesnt constantly update
    bool isGrabbed = false;

    // not sure what mouseDown does cause I forgot to document when I wrote it, and Im afraid to delete it
    bool mouseDown = false;



    /*
    string GetMouseTest()
    {

        return sticktoMousetest;

    }
    so turns out we dont actually need a get command because protection level BS but also because we can
    just directly call for the variable from stick to mouse directly, so we should be good to go as long as we can
    check name and tag the same way for parent object of this script to check against the mouse version
    to allow or dissalow the dragging function so it will not fire to everything all at once like it did before */


    void Start()
    {

        //this should hopefully actually work when done down here, and actually fill our temp variable with the proper gameobject
        // YES YES YES YES YES YES YES YES YES YES YES IT FINALLY FUCKING WORKED
        //just had to think and research for a tad, but it makes sense why in the end, even if it burned almost 2 hours
        grippers = GameObject.Find("GRIPPAH");

    }


    void Update()
    {

        //finding mouse screen position
        mousePos = Input.mousePosition;
        mousePos.z = 5f;

        //normalising mouse to world position
        Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(mousePos);

        //raycast from screen space to check for clickable objects
        //Ray mouseRay = Camera.main.ScreenPointToRay(mousePos);

        //when clicking
        if (Input.GetMouseButtonDown(0))
        {

            //this should check to see if the parent object is the one the mouse is actually hovering over
            //we have to use stickymouse instead of sticktomouse because we have to use our serialized name instead of the actual
            //class name

            //FINALLY YES YES YES YES YES YES YES YES WE CAN GRAB INDIVIDUAL OBJECTS NOW!!!!
            if (this.gameObject.name == grippers.GetComponent<sticktomouse>().hoveredOver)
            {

                isGrabbed = true;

            }    

        }
        //testing to see if this ^ is working
        Debug.Log(isGrabbed);
        //so apparently .find does just fucking work afterall, and google AI is a lying son of a bitch like always
        //so Imma go back and remove the serialization BS
        //Debug.Log(GameObject.Find("GRIPPAH"));
        //lets see if this works
        //hrmm, grippers appears to be returning NULL
        //that would explain why it didnt work
        Debug.Log(grippers.GetComponent<sticktomouse>().hoveredOver);
        //so now its filling with a game object, but it wont fucking let me do gameobject things and its saying something about "method groups"
        //so turns out I was just missing a pair of parentheses AFTER the fucking triangle ones for some reason even tho I did it before and should
        //have known better and now Im kinda mad at myself
        //congrats at costing almost 2 hours parentheses
        //it works now, Imma leave it uncommented for now so I can remember it for a little while



        //when letting go
        //letting go should be globally applicable, regardless of if its being hovered or not
        if (Input.GetMouseButtonUp(0))
        {

            isGrabbed = false;

        }


        //if this yitch is grabbed, move the object to mouse world position
        if (isGrabbed == true)
        {

            transform.position = mouseWorld;

        }




        /*ok so its testing time
        lets see if we can find whatever parrent object this script is attached to's name and print it to console
        there should be a tad bit of noise in the console because multiple objects already have this script attached
        hopefully in the long run this wont cause too much of a performance impact, but we if we needed to out of desperation
        we could just in theory bring back a copy of the mouse ray script so that we could just enable dissable this on the fly as needed
        depending on whether or not this thing is getting hovered over by the mouse*/


        //there is absolutley no didly freaking way its as simple as just fumfing "THIS"
        //whatever I guess this works, so we can make a quick block to check if whatever is being hovered by the mouse just matches our own name to drag
        //brb gonna draw a quick stupid ball so I can just mass produce grabbable objects real quick for testing
        
        //Debug.Log(this.gameObject.name);


    }



}