using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
// ooo interesting, visual studio seems to auto import modules you pull from, thats cool
//problem now is that it doesnt unpull those modules when you stop using them.... ah well


public class sticktobounds : MonoBehaviour
{

    private bool OkToRun = false; // this varaible will save whether or not this script is ok to run, false by default until passing checks

    public GameObject grabbedObject = null; // this stores which object should be clamped to these bounds, null by default to avoid errors
    private GameObject me; // this variable is to store the game object this script is attached to : set in start

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        if (grabbedObject != null) // check to see if the assigned gameobject exists
        {

            if (grabbedObject.GetComponent<draggableobject>() != null) // this should then check to see if the assigned object is draggable
            {

                OkToRun = true; // this should set the flag to allow the script to run through given it exists and is draggable

            }

        }

        me = this.gameObject; // this should set "me" to the parent gameobject this script is attached to

    }

    // Update is called once per frame
    void Update()
    {
        
        if (OkToRun == true) // everything should live within this check to only run when the conditions are met
        {

            

        }

        // trying safety check stuff
        Debug.Log(grabbedObject); // just testing somethings: so gameobject does return null when unassigned, this is a good sign
        Debug.Log(grabbedObject.GetComponent<draggableobject>()); // now lets test to see if this return null when component is missing
        // good news, it does return null! so our safetey checks should work as intended!

        // quick test to make sure setting me works as intended
        Debug.Log(me); // ok it worked, setting "me" in the start step functioned as intended

        Debug.Log(me.transform.localScale); // oop, lets see what this does real quick, if local scale does what I think it might we could have a lead
        // so this gets what our scaling is currently set to in the inspector
        // problem is I cant tell how wide or tall something is from this, because I dont know the objects original size pre scale
        Debug.Log(me.transform.lossyScale); // ok so what does lossyScale do?
        // it appears to do the same as localScale
        Debug.Log(me.transform.root); // what does root do? I assume this probably returns the game object again
        // it appears to return "UnityEngine.Transform" but with the objects name
        Debug.Log("hrmm");

        // ^^^^ DEBUGGING ^^^^ -----------------------------------------------------------------------------------------------------------------------------

    }

}

// VVVV ORIGNIAL THOUGHTS AND PLANNING VVVV ----------------------------------------------------------------------------------------------------------------

// ya fucking hoo, I swear if this replaces clamp Im gonna be furrious
// idea for this script is going to essentially be a script we can attach to a "bounds" object
// this we accept a game object as a perameter (aka which object we want clamped to these bounds)
// we do some quick mafs to figure out if the clamped object is leaving the bounds its assigned to, then if it is, keep it in those bounds
// I want this to work properly *WITH* clamp if possible, because I dont want a lot of pulling room on the object

// allthough now that Im thinking about it, this may just replace clamp anyway, because it should only fire when
// the clamped object perameter leaves, and when its parented to another object it shouldnt until grabbed individualy anyway

// for the sake of freedom maybe we could remove the clamping all together from draggable object, and instead do it here
// clamping would be ON by defualt, since if we didnt want any clamping at all we would just exlude this script
// but we could have an option to "unbound" either the x or the y, allowing to roam infinitely in either of those directions
// which of course will be off by default as I originaly saw this as an error with the original clamping options
// I was originally happy with the older clamping option because I made it, and it worked (mostly) but further and deeper testing revieled
// a lot of little errors that would have made moving forward harder

// this script should clamp X and Y individually, so it doesnt pull to center
// I want this script to keep an object from leaving after all, no to root it totally in place

// I also want a "springy" feature (primarily for the weapon bolt) that will return it to a sort of "home position" when let go
// I'm not presently sure just how I would do that, but it does need to be interpolated (for game feel I suppose, but EH)
// (allhtough it would technically work just snapping, that doesnt sit nice in my brain, but then again I am REALLY REALLY pressed)
// (for time rn, and I need to move on from this to program the loading mechanics over the weekend)


// okay lets get to work
// I guess first things first is our safety checks for the script
// first of all since the gameobject default is null, we should first check to see if an object is assigned
// then we should then check the assigned game object to see if it has draggable object, and if so then proceed with the script

// ok so safety checks are working now
// I guess next thing to do would be to find a way to calculate attached objects bounds, either by sprite size or hitBox2D
// hitBox2D sounds better at first, but after thinking about it, that may interfere with the dragging functions
// maybe sprite size could work? just a blank color and crank the transparancy?
// ok so sprite size could work, find a way to calculate the game object size since it changes with sprite size
// question is how do we find its *size* and not just its position
// if I can find its size, its max bounds are simply its position +- half of its size value
// or if theres a way to find its max bounds automatically thats pretty sick too

// Ok new problem, Im struggling to find a way to find our size
// I found a way to find local scale, but thats not of much use if we cant automatically determine original sprite size
// unlessssssssssssssssss, we start from a VERY SPECIFICALLY 1m by 1m square then scale can tell us how big it is percentile wise
// but that solution doesnt sit right with me, because it feels finicky and unrobust
// Im going to do some internet research before I continue

// there is no freaking way its just as simple as "size.x" or "size.y"
// if it is, that shows the power of just doing a quick search for some documentation
// ok of to test it
// ok problem, size doesnt appear to be able to be called during Update(), or at all
// it doesnt appear to be real, thats the last time I ever trust google AI, its a lying SOB
// I WILL ONLY FIND NEW CODE OR SEARCH FOR SOLUTIONS ON FORUMS FROM NOW ON
// looks like our options (actually) are GetComponent<Collider>().bounds.size, or GetComponent<Renderer>().bounds.size
// I guess that makes sense, because an empty game object doesnt have a size without on of these components
// I will try the renderer method, because I dont want to use a collider for aformentioned reasons of mouse ID and draggable interfierance

// UUUUUUUUUUUCGH THIS TEN POINT TESTING THING IS KICKING MY ASS
// I COULD JUST HARDCODE IT IF I WASNT TRYING TO BE BETTER AT THIS
// RAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAGH