using System;
using Unity.VisualScripting;
//using UnityEditor.Experimental.GraphView;
//using UnityEditor.UI;
using UnityEngine;

// ^^^^ import and stuff ^^^^ ------------------------------------------------------------------------------------------------------------------------


// VVVV everything else VVVV -------------------------------------------------------------------------------------------------------------------------

public class draggableobject : MonoBehaviour

{

    // setting up to find screen and world positions
    Vector3 mousePos;
    RaycastHit2D raycastHit2D;
    Transform clickObject;
    // public to be set in inspector
    public Camera mainCamera;


    // setting variable to track if this object is grabbable or not, public idealy with default false
    // if that public could be a dropdown menu would be super ideal
    public Boolean isGrabbable = false;


    // setting booleans to controll clamping from inspector, x and y respectively
    public Boolean ClampX = false; // default false
    public Boolean ClampY = false;// defualt false
    // setting boolean for contextual clamping (clamping only applies when grabbing this object)
    public Boolean ClampContextual = false; // defualt false

    // public Boolean testClamp = false;// this is just temporary to test clamping object by object

    float clampedX; // this is to store the result of LockMyX
    float clampedY; // this is to store the result of LockMyY

    private bool priorityX = false; // stores whether clamp to bounds X has priority, false by default because priority should not kick in until moving out of bounds
    bool priorityY = false; // stores whether clamp to bounds Y has priority
    Vector3 grabbedPos; // this is to be used to store piece by piece the vector3 positions of the parent objects position

    // referencing the gripper object so we can access its script to check against its hover variable
    // ok its defined, now fill it during start VVV
    GameObject grippers;


    
    // testing a random variable to see if we can pull it from stick to mouse (we can absolutely just via variable name, and also just object name)
    /* that means we can freely use a system like this to check if the object we are hovering is interactable
     or not, which in theory is to be local, but keep the variable public so that we can set it per object via the inspector so that we can use this
    same script for all hoverable objects regardless of if we want them to be grabbable or not
    maybe this should also be changable by other collisions so that we can turn off the grabbability of object as they are used in the game world
    so maybe a getter and setter might be appropriate afterall, or at least a setter for when bullets will eventually collide with the magazine */
    
    // so it worked
    // public string sticktoMousetest = "default";



    // is this yitch grabbed rn? cause clickObject doesnt constantly update
    bool isGrabbed = false;

    // not sure what mouseDown does cause I forgot to document when I wrote it, and Im afraid to delete it
    // it doesnt seem to be currently used at all
    bool mouseDown = false;


    // ^^^^ script variable definitions ^^^^ ---------------------------------------------------------------------------------------------------------





    // VVVV method definition for clamp programs VVVV -------------------------------------------------------------------------------------------------
    static float LockMyX(bool OnOff, float simplex, Vector3 myPos) // this method is used for locking the X of the parent object, maybe needs a better name
    {
        //iterates once to lock x position
        if (OnOff == false)
        {
            
            simplex = myPos.x; // sets the middle variable
            OnOff = true; // dissables repeat setting
            //return simplex; // return the set value from method to use outside

        }

        return simplex; // does this work? it does, we return simple x here now to avoid errors

    }   

    static float LockMyY(bool OnOff, float simpley, Vector3 myPos)
    {

        // iterates once to lock y position
        if (OnOff == false)
        {

            simpley = myPos.y; //  sets the middle variable
            OnOff = true; // dissables repeat setting
            // these can be set / reset later when needed

        }

        return simpley; // returns simple y for use in clamping

    }

    public void setPriority(bool setter, string XorY) // this should funciton to be used outside this script to set priority of parent object
    {

        if (XorY == "x")
        {

            priorityX = setter;

        }

        if (XorY == "y")
        {

            priorityY = setter;

        }

    }


    // begin loop ------------------------------------------------------------------------------------------------------------------------------------
    void Start()
    {

        // this should hopefully actually work when done down here, and actually fill our temp variable with the proper gameobject
        // YES YES YES YES YES YES YES YES YES YES YES IT FINALLY FUCKING WORKED
        // just had to think and research for a tad, but it makes sense why in the end, even if it burned almost 2 hours
        grippers = GameObject.Find("GRIPPAH");

        clampedX = LockMyX(false, clampedX, transform.position); // false OnOff to get x to clamp and remain clamped, clampedX for use in clamping, and transform.position to set clamp x based off local position
        // called in start for now for testing
        // remember to store the result

        clampedY = LockMyY(false, clampedY, transform.position); //  false OnOff to get y to clamp and remain clamped, clampedY for use in clamping, and tranform.position to set clamp y based off local position
        // sort of helps when you actually call the method ey?

        // may need a clean way to call these outside of start
        // we will see as I continue testing and itterating
        // nah this works, no to focus on contextual

    }


    void Update()
    {

        // finding mouse screen position
        mousePos = Input.mousePosition;
        mousePos.z = 5f;

        // normalising mouse to world position
        Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(mousePos);

        // raycast from screen space to check for clickable objects
        // Ray mouseRay = Camera.main.ScreenPointToRay(mousePos);

        // when clicking
        if (Input.GetMouseButtonDown(0))
        {

            // this should check to see if the parent object is the one the mouse is actually hovering over
            // we have to use stickymouse instead of sticktomouse because we have to use our serialized name instead of the actual
            // class name

            // FINALLY YES YES YES YES YES YES YES YES WE CAN GRAB INDIVIDUAL OBJECTS NOW!!!!
            if (this.gameObject.name == grippers.GetComponent<sticktomouse>().hoveredOver & isGrabbable == true) // query if object name mathes what the moues hovers over, then if grabbable then proceed
            {

                isGrabbed = true;
                // this works for now, and I'll probably just leave it as is for a bit. But a problem with toggling is the clamped position remains where it was
                // not where it currently is, so it teleports back to where it was first clamped. It also only correclty clamped if it *starts* clamped
                // so this works as proof of concept, but it needs some changes if contextual is going to work. maybe a different block
                // maybe if we just perminantly call lockmyX/Y perminantly as false when clampX or clampY is false, so it correctly updates position while off
                // as well as preventing errors when starting unclamped

                // update 2, I forgot to save the return value back to clampedY and clamped X, I will try again using my own method correctly
                // update 3, it functions completely as intended now! even removes no set errors at the start!

                // when mouse flicking it is possible to grab two objects at the same time, so we need some way to stop this so contextual doesnt
                // break. Especially since I want a different "bounds" script now that I've thought things over.
                // since with some objects we want the clamping to not be able to leave the parent object
                // idealy this should work along side contextual

            }    

        }


        // testing to see if this ^ is working
        //Debug.Log(isGrabbed);

        //Debug.Log(GameObject.Find("GRIPPAH"));
        
        // lets see if this works
        // hrmm, grippers appears to be returning NULL
        // that would explain why it didnt work
        //Debug.Log(grippers.GetComponent<sticktomouse>().hoveredOver);
        
        // so turns out I was just missing a pair of parentheses AFTER the triangle ones for some reason even tho I did it before and should
        // know better
        // move these to actual debug brick later (DO NOT FORGET DO NOT FORGET DO NOT FORGET DO NOT FORGET)


        // when letting go
        // letting go should be globally applicable, regardless of if its being hovered or not
        if (Input.GetMouseButtonUp(0))
        {

            isGrabbed = false; // this is okay, since everything should get ungrabbed when mouse is let go regardless
            setPriority(false, "x");
            setPriority(false, "y"); // maybe if we reset when letting go it might work?
            // problem would then be youd have to let go to go the other way
            // thats progress

        }


        // if this yitch is grabbed, move the object to mouse world position
        if (isGrabbed == true)
        {

            if (priorityX == false) // allow sticking to mouse when this script has priority
            {

                grabbedPos.x = mouseWorld.x;

            }

            if (priorityY == false) // allow sticking to mouse when this script has priority
            {

                grabbedPos.y = mouseWorld.y;

            }
            
            grabbedPos.z = 5f;
            transform.position = grabbedPos;

        }
        // grabbing really needs an update to break when gettig too far away from the clamped object
        // we also need something to reset the clamped value to "home position" just in case when its contextual
        // that might mean its easier to change contextual all together, I need to think

        // making some change ups to is grabbed to work in conjunction to the *NEW* clamping funcitons


        // VVVV CLAMPING BLOCKS VVVV -----------------------------------------------------------------------------------------------------------------------

        /*if (ClampX == true & ClampContextual == false) // clamp parent objects x value via method when clamp x is set to true in inspector and contextual is off
        {

            transform.position = new Vector3(clampedX, transform.position.y, transform.position.z); // when true, forever set x to the clamped value, but allow the others to change

            // scope issues stand by, we're not preserving the simplex value
            // block of code has been moved to a method to encourage reusability and to fix scope problems

        } else if (ClampX == false) {

            //LockMyX(false, clampedX, transform.position); // in theory if we perminantly call this as false when clamping is false it should fix teleport and unassigned errors
            // nope this doesnt work
            // something else needs to be done

            clampedX = LockMyX(false, clampedX, transform.position); // with correct return saving lets see if constantly calling this as false while clamp is off works
            // ok it works now that I actually save the return (silly me)

        }

        if (ClampY == true & ClampContextual == false) //  clamp parent objects y value via method when clamp y is set to true in inspector, and contextual is off
        {

            transform.position = new Vector3(transform.position.x, clampedY, transform.position.z); // forever set this objects y value to the clamped y value when true

        } else if (ClampY == false) {

            clampedY = LockMyY(false, clampedY, transform.position); // while false, unclamp and continuously update y position until clamped again

        }

        // contextual clamping

        if (ClampX == true & ClampContextual == true) // contextually clamp X when localy grabbed, so object is free to move with parent
        {

            if (isGrabbed == true)
            {

                transform.position = new Vector3(clampedX, transform.position.y, transform.position.z); // lock the X when grabbed and clamp X is true

            } else {

                clampedX = LockMyX(false, clampedX, transform.position); // when not currently grabbed, unlock the X, only clamp when this is grabbed and not its parent

            }

        }

        if (ClampY == true & ClampContextual == true) // contextually clamp Y when localy grabbed, so object is free to move with parent
        {

            if (isGrabbed == true)
            {

                transform.position = new Vector3(transform.position.x, clampedY, transform.position.z);// lock the y when grabbed and clamp Y is true

            } else {

                clampedY = LockMyY(false, clampedY, transform.position); // when not currently grabbed, unlock the Y, only clamp when this is grabbed and not its parent

            }

        }*/ // we in theory dont need the old clamping feature anymore

        // ^^^^ scripting ^^^^ -------------------------------------------------------------------------------------------------------------------------



        // VVVV debug VVV ------------------------------------------------------------------------------------------------------------------------------

        /* ok so its testing time
        lets see if we can find whatever parrent object this script is attached to's name and print it to console
        there should be a tad bit of noise in the console because multiple objects already have this script attached
        hopefully in the long run this wont cause too much of a performance impact, but if we needed to out of desperation
        we could just in theory bring back a copy of the mouse ray script so that we could just enable dissable this on the fly as needed
        depending on whether or not this thing is getting hovered over by the mouse */


        // there is absolutley no didly freaking way its as simple as just fumfing "THIS"
        // whatever I guess this works, so we can make a quick block to check if whatever is being hovered by the mouse just matches our own name to drag
        // brb gonna draw a quick stupid ball so I can just mass produce grabbable objects real quick for testing

        //Debug.Log(this.gameObject.name);

        Debug.Log(priorityY);
        Debug.Log(priorityX);

        //if (testClamp == true)//THE METHOD WORKKSSS LETS GOOOO!
        {

            //Debug.Log(clampedX); // I hope this works, the method is called in start, but if it survives as should be to update then we can use it

        }
    }



}

// Im going to try to rewrite the move to mouse part of this function so we can enable dissable them based purely on priority without breaking the grabbed condition