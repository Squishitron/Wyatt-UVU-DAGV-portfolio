using NUnit.Framework;
using UnityEngine;

public class WeaponSpawnHandler : MonoBehaviour
{

    // definitions and settup --------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // location objects
    public GameObject[] clipLocations; // this variable will hold a list of the clip location gameobjects for spawn randomization
    public GameObject[] rifleLocations; // this variable will hold a list of the weapon loacation gameobjects for spawn randomization
    public GameObject clipResetPosition; // this varaible will hold a game object location to reset the clip to on despawn
    public GameObject rifleResetPosition; // this variable will hold a game object location to reset the rifle to on despawn

    // objects
    public GameObject rifle; // this variable holds the rifle that should be spawned or despawned by this script
    public GameObject clip; // this variable hold the clip that should be spawned or despawned by this script

    // we need these three so we can reset on desapwn
    public GameObject bolt; // this variable holds the bolt object that needs to have its primer reset on despawn
    public GameObject rifleLoadHandler; // this variable will hold the object used for loading handling on the rifle
    public GameObject clipLoadHandler; // this variable will hold the object used for loading handling on the clip

    private bool spawnedState = false; // setting to false to start, since when opening the game it should randomize to start at first
    private bool okToReset = false; // this bool flag should be used externally to controll when to despawn and respawn the weapon
    // for clarification, Im going to elect to simply use the same move the thing off screen spawning method as the bullets for now to get things working, because I want to redo the handlers
    // in order to compact them and fix some errors we have with the clones
    // but in the interest of getting the game working and playable to some degree in time to keep up with class, we'll just do it this way for now, since I know the original copy works

    private bool okToRun; // this bool should be used for safety checks

    // VVVV METHODS VVVV -------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public bool getSpawnedState() // not sure Ill need this, but just making a getter to go with setter
    {

        return spawnedState; // returns the spawned state flag

    }

    public void setSpawnedState(bool x) // setter method to set the spawned flag
    {

        spawnedState = x;

    }

    public void randomizeSpawn(GameObject[] clipOptions, GameObject[] gunOptions, GameObject clip, GameObject gun) // arguments, array for posible clip locations, array for possible gun locations, gameobject clip, gameobject gun
    {

        int clipLength = clipOptions.Length; // finds and temporarily stores the length of the clip array
        int gunLength = gunOptions.Length; // finds and temporarily stores the length of the gun array

        int clipRandom = Random.Range(0, clipLength); // finds a random position between 1 and the array length of clip options for use in chosing random spawn
        int gunRandom = Random.Range(0, gunLength); // finds a random position between 1 and the array length of gun options for use in chosing random spawn

        GameObject selectedClip = clipOptions[clipRandom]; // sets the selcted clip options object to borrow location from for spawning based on the random selection
        GameObject selectedGun = gunOptions[gunRandom]; // sets the selected gun options object to borrow location from for spawning based on the random selection

        clip.transform.position = new Vector3(selectedClip.transform.position.x, selectedClip.transform.position.y, selectedClip.transform.position.z); // sets the clips loation to the random position previously selected
        gun.transform.position = new Vector3(selectedGun.transform.position.x, selectedGun.transform.position.y, selectedGun.transform.position.z); // sets the gun location to the random position previously selected

        setSpawnedState(true); // sets the spawned state to true as the weapon should be spawned at this point
        Debug.Log("randomizeSpawn() has been run");

    }

    public void despawnReset() // needs to be public to call outside this object
    {

        // these should hopefully fully reset the guns state
        rifleLoadHandler.GetComponent<GunLoadingHandler_V01>().resetLoaded(); // reset the loaded state of the rifle
        clipLoadHandler.GetComponent<MagLoadingHandler_V01>().resetLoaded(); // reset the loaded state of the magazine
        bolt.GetComponent<boltPrimer>().resetPrimed(); // resets the weapons primed status and resets primed steps

        // and these should hopefully succsesfully seperate the guns parts and desapwn them off screen
        rifle.transform.position = new Vector3(rifleResetPosition.transform.position.x, rifleResetPosition.transform.position.y, 5f); // this should transform the rifle off screen to its despawn location
        clip.transform.position = new Vector3(clipResetPosition.transform.position.x, clipResetPosition.transform.position.y, 5f); // this should transform the clip off screen to its despawn location

    }

    // Start -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    void Start() // honestly dont know if I need start this time around, but eh Im leaving it anyway
    {

        randomizeSpawn(clipLocations, rifleLocations, clip, rifle); // sets the first random spawn of the riffle
        if (getSpawnedState() == true)
        {

            okToRun = true; // bit late for a safety check, but ah well

        }

    }

    // the frame champion ------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    void Update()
    {

        if (okToRun == true) // during update, run if safety check is passed initally
        {

            /*if (Input.GetMouseButtonDown(1)) // right click for testing purposes, but essentially this block needs to despawn the gun and reset all the loaded and cosmetic states
            {

                despawnReset();

            }*/ // this was good for testing and it works, so Ill just comment it out for now

        }

        // VVVV DEBUG VVVV -----------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //Debug.Log(spawnedState);
        /*if (Input.GetMouseButtonDown(2)) // temporary debug input, when middle clicking re randomize spawn, so we can test it without reopening the game repeatedly
        { // we are now using this to test respawning after the reset script to see if it still loads properly, if it doesnt I missed something

            randomizeSpawn(clipLocations, rifleLocations, clip, rifle);

        }*/  // again, good for testing, but well comment this out again

    }

}
// okay, this script will handle spawning and despawning of the weapons for now
// it accepts a few randomization positions for each weapon type, and this script should select randomly between each of those object locations
// it should then move the weapon components to the random positions

// despawning is also going to be handled here, just like with the bullet version, but we will get to that later after the spawning is functioning
// Im also only worrying about the rifle for now, since I want to fix some more things before we add the flamer and other weapon types

// first thing we are going to do, get the initial random spawn to happen, then we will worry about the despawn and test that manually, then we will automate
// on second thought, why do we need a bool to check for the first time spawn? we could just use the start method
// run the first random spawn on start, and then set the ok to spawn stuff to false

// ok testing the randomization spawn, it works, but it only seems to select under the last option, so math randomize +1 might be the option to use? except we would then lose the first option?
// plus one to the end of the range, so we can get the random index to be inclusive
// that causes errors about us rolling OVER the range of the array
// I guess that logically means that we are missing UNDER the array, so set the first range to zero instead of one
// ok that was it, the randomization now applies to every object in the array


// now its time to take a break and cook dinner
// next up is to test despawning and reset, once that is in and functioning, we can automate the loop
// once the loop is automated, we can add scoring
// once we add scoring, the main menu and timer can be added, then the game is technically playable begginging to end

// ok our reset methods and the despawn locations seem to work, now lets test a quick respawn on middle mouse to see if the weapon can run through being reloaded again
// almost first try, we missed turning the bolts draggability back on, but out of all the things that needed reset, thats not bad

// ok, both despawn and respawn are fully working, now lets automate

// I moved the despawn run into a method, so I can run it from the bell check, because I want the ring bell thing to work properly
// and with these being methods we can run these from the bell check, and function them on a timer there
// plus only having the check zone enabled when the bell is rung should fix at least that layering problem our mouse ID has
// (which is a fairly large problem I am kind of just sweeping under the rug for now)
// (hopefully that doesnt come back to bite me in the ass, but we'll see.... it probably will)