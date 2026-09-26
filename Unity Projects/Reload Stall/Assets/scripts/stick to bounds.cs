using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UIElements;
// ooo interesting, visual studio seems to auto import modules you pull from, thats cool
//problem now is that it doesnt unpull those modules when you stop using them.... ah well


public class sticktobounds : MonoBehaviour
{

    public Camera mainCamera; // just set the fucking camera in inspector like the other scripts
    Vector3 mouseWorld; // use this to store the mouses world position after normalization

    private bool OkToRun = false; // this varaible will save whether or not this script is ok to run, false by default until passing checks

    public GameObject grabbedObject = null; // this stores which object should be clamped to these bounds, null by default to avoid errors
    private GameObject me; // this variable is to store the game object this script is attached to : set in start

    private float meSizeX; // this variable is to store me's pixel size in the x direction
    private float meSizeY; // this varaible is to store me's pixel size in the y direction


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
        //meSizeX = me.GetComponent<SpriteRenderer>().sprite.rect.width / 2; // setting and storing half of width in pixels
        //meSizeY = me.GetComponent<SpriteRenderer>().sprite.rect.height / 2; // setting and storing half of height in pixels
        // let me try local scale
        meSizeX = me.transform.lossyScale.x / 2; // hmm, we do seem to be able acess the x specifically, now if we half it
        meSizeY = me.transform.lossyScale.y / 2; // half of lossy scale on the Y direction

    }

    // Update is called once per frame
    void Update()
    {

        mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition); // normalizing mouse position to world position for like the third fucking time

        if (OkToRun == true) // everything should live within this check to only run when the conditions are met
        {

            if (grabbedObject.transform.position.x >= me.transform.position.x + meSizeX) // checks bounds in the positive X direction
            {

                grabbedObject.transform.position = new Vector3(me.transform.position.x + meSizeX, grabbedObject.transform.position.y, grabbedObject.transform.position.z); // sets grabbed objects position to max bounds +X
                grabbedObject.GetComponent<draggableobject>().setPriority(true, "x"); // sets priority when leaving x bounds

                // ooo, wait uh, uh oh... those are backwards. We are moving grabbed object after all
                // ok that should be right, if it leaves the bounds on the right, it should be kept in
                // using local scale to get it to work, check the notes at the bottom for more information

            }

            if (grabbedObject.transform.position.x <= me.transform.position.x - meSizeX) // checks bounds in the negitive X direction
            {

                grabbedObject.transform.position = new Vector3(me.transform.position.x - meSizeX + 0.01f, grabbedObject.transform.position.y, grabbedObject.transform.position.z); // sets grabbed objects position to max bounds -X
                grabbedObject.GetComponent<draggableobject>().setPriority(true, "x"); // sets this script to have priority when leaving x bounds

            }

            if (grabbedObject.transform.position.y >= me.transform.position.y + meSizeY) // checks bounds in the positibe Y direction
            {

                grabbedObject.transform.position = new Vector3(grabbedObject.transform.position.x, me.transform.position.y + meSizeY, grabbedObject.transform.position.z); // sets grabbed objects position to max bounds +Y
                grabbedObject.GetComponent<draggableobject>().setPriority(true, "y"); // sets this script to have priority when leaving y bounds

            }

            if (grabbedObject.transform.position.y <= me.transform.position.y - meSizeY) // checks bounds in the negative Y direction
            {

                grabbedObject.transform.position = new Vector3(grabbedObject.transform.position.x, me.transform.position.y - meSizeY, grabbedObject.transform.position.z); // sets grabbed objects position to max bounds -Y
                grabbedObject.GetComponent<draggableobject>().setPriority(true, "y"); // sets this script to have priority when leaving y bounds

            }


            /*if (grabbedObject.transform.position.x > me.transform.position.x - meSizeX & grabbedObject.transform.position.x < me.transform.position.x + meSizeX) // this should essentially check to see if the obejct is in the allowed bounds, and if so reset priority
            {

                //grabbedObject.GetComponent<draggableobject>().setPriority(false, "x"); //  reseting priority
                // it appears to be forever resseting priority because of our little bump back, this is a problem
                // maybe if we reset priority in the other script?
                // I changed it to be OR instead of AND, realized it would never be true to be out of bounds in both directions at the same time
                // waiiiiiit, or wont work, one or the other will always be true and we have an infinite reset loop

            }*/ // having some issues with this perminantly reseting

            if (mouseWorld.x < me.transform.position.x + meSizeX & mouseWorld.x > me.transform.position.x - meSizeX) // checks to see if mouse is pulled in
            {

                grabbedObject.GetComponent<draggableobject>().setPriority(false, "x"); // resets priority to draggable when returning to x bounds

            } // IT FUCKING WOOOOOOOOOOOORKS

            if (mouseWorld.y < me.transform.position.y + meSizeY & mouseWorld.y > me.transform.position.y - meSizeY) // checks to see if mouse is within Y bounds
            {

                grabbedObject.GetComponent<draggableobject>().setPriority(false, "y"); // resets priority to draggable when returning to y bounds

            }

        }

        // trying safety check stuff
        //Debug.Log(grabbedObject); // just testing somethings: so gameobject does return null when unassigned, this is a good sign
        //Debug.Log(grabbedObject.GetComponent<draggableobject>()); // now lets test to see if this return null when component is missing
        // good news, it does return null! so our safetey checks should work as intended!

        // quick test to make sure setting me works as intended
        //Debug.Log(me); // ok it worked, setting "me" in the start step functioned as intended

        //Debug.Log(me.transform.localScale); // oop, lets see what this does real quick, if local scale does what I think it might we could have a lead
        // so this gets what our scaling is currently set to in the inspector
        // problem is I cant tell how wide or tall something is from this, because I dont know the objects original size pre scale
        //Debug.Log(me.transform.lossyScale); // ok so what does lossyScale do?
        // it appears to do the same as localScale
        //Debug.Log(me.transform.root); // what does root do? I assume this probably returns the game object again
        // it appears to return "UnityEngine.Transform" but with the objects name
        //Debug.Log(me.GetComponent<SpriteRenderer>().sprite.rect.width); // this should allow me to find the sprite width in pixles
        // in theory half of this value from origin is the edge of the sprite

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

// hang on, Imma add some extra variables to store things from the grabbed sprite as well, cause I do not want to rewrite this shit over and over and over
// eh on second thought

// hrm shit, for some reason OK to run isnt turning on
// ok it is turning on, turns out the world space calculations run on scale size, pixel size is blowing our numbers WAY out of proportions
// still not sure what I would do with local scale, even 2.01 seems too big, unless I just need half of it
// it appears to be returned as a Vector3, I may be able to try something with that

// local scale is MUCH MUCH MUCH closer, I'm seeing progress finally!!!!
// its still just a tad larger than it needs to be, so I need somemore done, but its closer
// maybe it works better on a perfectly square object?
// it does stick to the square perfectly, but it doesnt stay with it until its let go for some reason now

// ok, so we've got a little bit of a quirk to work out, but the bounds work now, we just needed to start with a perfect square.
// so note to self, use this method with 2D square game objects, then scale those sqaures to the bounds you want, then parent the bounds object

// for now I'm going to set up the rest of the bounds, then we will search a way to fix the bounds of the draggable thing

// hrmmmmmmmm, so it works as long as the sqaure *ISNT* parented to something else, which is a problem, because it needs to be parented to something else
// what happens if we use lossyScale instead of Local scale
// it works unparented, now we are going test it parented to see if it still works and prevents the error that localScale had
// holy shit it worked! glad I went on that whim
// next up we need to fix the leaving bounds thing before you let go
// and then we need to add the springy deal

// I could do a sort of priority system that works between the two?
// split the mouse position into two parts in the draggable object so we can turn them on or off based on priority
// but then that might cause problems if the object gets let go of when going past bounds with the mouse
// priority might work off of grabbed status and mouse position?
// but then again we are planning on breaking grabbed status eventually, and this might be the block to do it
// not yet because me need minimum complete viable, but you know what I mean

// maybe we move setting mouse position things to a method, then we can call those from both sides and manage priority that way?
// maybe reverse the clamp script to function *IN* draggable object?
// that way we could just hold and manage the execution order there

// we need a setter and getter method for priority on the other side
// ok we should have a priority setter, no to test it

// ok priority setters are working, changes that need to be made: small adjustment of 0.01 in oposite direction of setting mode
// : reset needed in stick to bounds

// ok we've got some really good and solid progress towards getting this to work, but I'm having issues with priority reset only taking effect when
// either letting go, or reseting infinitely based on bounds and reseting
// maybe if we instead handle priority between the paired objects based on the mouses position? so reset only has priority when the mouse is at the bounds
// and it turns off letting the normal update proceed when the mouse comes back in

// IT WOOOOOOOOOORKS HAHAHAHAHAHAHAHAHAHAHAHAHHAHA
// IT EVEN SURVIVIES FLICKING AND SPAMMING!!!!!!!!!!!! HAHAHAHAHAHAHHAHAHAHAHAH!!!!!!!!!!

// I elect to leave the spring alone for now, because I have other homework to finish
// spring mode can come tomorrow