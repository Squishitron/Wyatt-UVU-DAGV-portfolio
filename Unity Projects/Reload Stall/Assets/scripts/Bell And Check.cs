using System.Collections;
using UnityEngine;

public class BellAndCheck : MonoBehaviour
{

    // settup ------------------------------------------------------------------------------------------------------------------------------------------------

    public GameObject BoltObject; // sets bolt object in inspector (THIS NEEDS TO CHANGE FOR REPEATABILITY IN THE FUTURE, WE JUST GETTING WORKING FOR NOW TIME IS RUNNING OUT)
    public GameObject weaponSpawnHandler; // we storing the weapons spawn handler in here so we can call the resets and spawns from here
    public GameObject moneyHandler; // this should store the canvas so we can access the money values to update score
    public ParticleSystem particles1;
    public ParticleSystem particles2;
    public ParticleSystem particles3;
    private string collidedObject; // stores the name of the collided object
    private bool weWating = false; // this should work in tandem with the timers, to track whether or not we are currently waiting without expressly stopping the updates
    private bool doneWating = false; // this tracks whether or not the resapwn stuff should happen when the timer is done being run, false by default, should practically always be false unless firing

    // methods -----------------------------------------------------------------------------------------------------------------------------------------------

    IEnumerator WaitForASec(float x) // setting up coruotine to get the timer to work (please dont fail me reddit)
    {

        weWating = true; // lets the script know that we are waiting

        yield return new WaitForSeconds(x);

        doneWating = true; // lets the script know that we are done waiting now
        weWating = false; // resets the weWaiting to let the script know it is no longer in a waiting system

    }

    // On Collision ------------------------------------------------------------------------------------------------------------------------------------------
    private void OnCollisionEnter2D(Collision2D collision) // this triggers when entering this collision to find the name of the thing that enters
    {

        collidedObject = collision.gameObject.name; // stores the collided objects name : REMEMBER RIGID BODY
        //Debug.Log("is this working? collission");
        //Debug.Log(collidedObject);

    }

    // Start -------------------------------------------------------------------------------------------------------------------------------------------------
    void Start()
    {

        Debug.Log("bing bong");

    }

    // Update ------------------------------------------------------------------------------------------------------------------------------------------------
    void Update()
    {


        if (collidedObject == BoltObject.name & Input.GetMouseButtonUp(0))
        {

            if(BoltObject.GetComponent<boltPrimer>().GetReady() == true)
            {

                Debug.Log("are we getting in here?");
                //Destroy(BoltObject.transform.root.gameObject); // destroys the bolt objects root (weapon part collection, so it should despawn everything)
                weaponSpawnHandler.GetComponent<WeaponSpawnHandler>().despawnReset(); // despawn the pew pew, and reset weapon state
                particles1.Play(true);
                particles2.Play(true);
                particles3.Play(true);
                // this honestly pretty much works as it, now just add score and a timer for reset
                // mayyyyyyyyyyyyybe make the particle timer a *tad* shorter so the celebration isnt weirdly long
                // yeah shorter timer is good
                //weaponSpawnHandler.GetComponent<WeaponSpawnHandler>().randomizeSpawn(weaponSpawnHandler.GetComponent<WeaponSpawnHandler>().clipLocations, weaponSpawnHandler.GetComponent<WeaponSpawnHandler>().rifleLocations, weaponSpawnHandler.GetComponent<WeaponSpawnHandler>().clip, weaponSpawnHandler.GetComponent<WeaponSpawnHandler>().rifle);
                // uuuuuuuuhhhhhhh, dont mind this, it got LONG calling it this way, I didnt think about calling it outside and the arguments
                StartCoroutine(WaitForASec(1.5f)); // this should allow us to wait and update without breaking the loops, running the wait for a sec as a coroutine, then updating out for other bs
                moneyHandler.GetComponent<MoneyManager>().changeMoney(25);

            }
            
        }

        if (weWating == false & doneWating == true) // this fires off after the waiting coroutine is called and runs its course, to respawn and continue the game
        {

            weaponSpawnHandler.GetComponent<WeaponSpawnHandler>().randomizeSpawn(weaponSpawnHandler.GetComponent<WeaponSpawnHandler>().clipLocations, weaponSpawnHandler.GetComponent<WeaponSpawnHandler>().rifleLocations, weaponSpawnHandler.GetComponent<WeaponSpawnHandler>().clip, weaponSpawnHandler.GetComponent<WeaponSpawnHandler>().rifle);
            // uuuuuuuuhhhhhhh, dont mind this, it got LONG calling it this way, I didnt think about calling it outside and the arguments
            doneWating = false; // this should dissable this and clear any waiting status until it needs to be called again

        }

        if (Input.GetMouseButtonUp(0))
        {

            collidedObject = "nothing";

        }

        // DEBUG -------------------------------------------------------------------------------------------------------------------------------------------------
        //Debug.Log("Imma sniff sniff, does this forever run during the wait?");
    
    }

}
// so good news, this works for now, but problem, it only accepts the original clone of the gun
// idealy we could chane the last few scripts to use a tag system, and you would set the tag for every obejct or something?
// draggable object could use a get tag method so every draggable object could have a tag discoverable from the mouse
// might want to do that anyway for the different mouse pointers / grabbing states
// tag system for the loading systems? so we could merge the two load handlers together? bullet > mag, cosmetic prop, destroy loading object / despawn loading object:
// mag > gun, cosmetic prop, destroy loading object / despawn loading object.

// open drawer, grab canister for flamer, when grabbed cabinet slam shut to open and spawn a new one


// gamefeel wise since we talked about it in class today, could be fun to have the RGGGH gripper sprite to shake slightly and turn a little bit red when gripping hard
// pointer finger could become default until hovering over something grippable
// pinch grip for small things like the bullets or the bolt on a gun
// ding ding noise for money up with a quick money particle system

// gun gravity system?
// when you grab and lift parts they could behave as intended and right themselves upwards
// but when you drop the object they could fall straight downwards until the "land" on the table, where they will skid to a stop
// this could allow you to leave them anywhere up and down technically on the table, so you can still get under them to load mags and stuff, but they skid further down onto the tabel
// the higher up you drop them from

// teacher also recomended you lose money (score) when you drop a bullet off screen

// trying to load mag when not full have "uh uh" noise and push it away slightly



// OKAY, we're back to this again 10/10/2026
// its time to finally make the bell ring and enable / dissable the check zone, and to handle score and despawn resapwn
// ok, so the first despawn and celebration works
// now there is simply the matter of figuring out how timers work, so we can wait a good half second or two, then respawn, simple as
// the celebration state is already there, just incrament score there
// the particel allowance system is already there, just decrease score there : maybe make an anti celbration particle for feedback

// then after that, we need a BIG timer to stop everything when the game is done
// and then a main menu possibly if we've got time
// I know thats technically the assignment, but its honestly not as important as the functioning loop and feedback UI

// Ok I tried the coroutine thing that reddit was telling me to do, and it works a treat
// I dont fully understand how it works, but it works and that is plenty good enough for me

// Ok, next up I guess is to spaghetti together a ding dong script that enables and dissables the checkout script