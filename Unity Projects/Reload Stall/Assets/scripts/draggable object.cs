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


    //setting booleans to controll clamping from inspector, x and y respectively
    public Boolean ClampX = false; //default false
    public Boolean ClampY = false;//defualt false
    //setting boolean for contextual clamping (clamping only applies when grabbing this object)
    public Boolean ClampContextual = false; //defualt false

    //public Boolean testClamp = false;// this is just temporary to test clamping object by object

    float clampedX;// this is hopefully to get our clamped x to be usable throughout scope without publicing it


    //referencing the gripper object so we can access its script to check against its hover variable
    //ok its defined, now fill it during start VVV
    GameObject grippers;



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


    //^^^^ script variable definitions ^^^^





    //VVVV method definition for clamp programs VVVV
    static float LockMyX(bool OnOff, float simplex, Vector3 myPos)//this method is used for locking the X of the parent object, maybe needs a better name
    {
        //iterates once to lock position
        if (OnOff == false)
        {
            //sets the middle variable
            simplex = myPos.x;
            OnOff = true;//dissables repeat setting
            //return simplex;//return the set value from method to use outside

        }

        return simplex; // does this work? it does, we return simple x here now to avoid errors

    }   



    //begin loop
    void Start()
    {

        //this should hopefully actually work when done down here, and actually fill our temp variable with the proper gameobject
        // YES YES YES YES YES YES YES YES YES YES YES IT FINALLY FUCKING WORKED
        //just had to think and research for a tad, but it makes sense why in the end, even if it burned almost 2 hours
        grippers = GameObject.Find("GRIPPAH");

        clampedX = LockMyX(false, clampedX, transform.position);//false OnOff to get x to clamp and remain clamped, clampedX for use in clamping, and transform.position to set clamp x based off local position
        //called in start for now for testing
        //remember to store the result

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

        //Debug.Log(GameObject.Find("GRIPPAH"));
        
        //lets see if this works
        //hrmm, grippers appears to be returning NULL
        //that would explain why it didnt work
        Debug.Log(grippers.GetComponent<sticktomouse>().hoveredOver);
        
        //so turns out I was just missing a pair of parentheses AFTER the triangle ones for some reason even tho I did it before and should
        //know better
        


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



        //VVV CLAMPING BLOCKS VVV
        if (ClampX == true)
        {

            transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);//when true, forever set x to the clamped value, but allow the others to change

            //scope issues stand by, we're not preserving the simplex value
            //block of code has been moved to a method to encourage reusability and to fix scope problems


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

        //if (testClamp == true)//THE METHOD WORKKSSS LETS GOOOO!
        {
         
            //Debug.Log(clampedX);// I hope this works, the method is called in start, but if it survives as should be to update then we can use it
        
        }
    }



}