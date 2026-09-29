using UnityEngine;

public class BulletSpawnHandler_V1 : MonoBehaviour
{

    // ok boss, variable settup time ------------------------------------------------------------------------------------------------------------------------
    
    public GameObject spawnObject; // object we want to be handled by this spawn behavior... aka for now the bullet and maybe the flame canister
    public ParticleSystem ObjectParticle; // partical system to be used by this behavior to mask despawning (Please for love of fuh rember to set this)
    public GameObject despawnTarget; // object used to store Vector 3 location for spawnObject when despawning : set in inspector

    private GameObject ME; // gameobject variable to make using this object easier \/\/ REMEMBER TO SET THIS IN START
    private GameObject grippers; // gameobject variable to make using the grippers easier because like all other clicks we need it
    private bool spawned = false; // this is to track if the bullet has been spawned in, and is to controll (for now) if particles spawn and despawn the bullet)
    private bool particleAllowed = true; // use with the clip to set particles to be allowed or dissallowed, true by default becuase its the most common case

    // settup done -------------------------------------------------------------------------------------------------------------------------------------------
    // VVVV methods settup VVVV ------------------------------------------------------------------------------------------------------------------------------
    private void spawnParticle()
    {

        ParticleSystem yahoo = Instantiate(ObjectParticle, spawnObject.transform.position, spawnObject.transform.rotation); // spawn a copy of the object particle to the spawn objects position
        yahoo.Play(); // this should play the spawned particle right after it spawns
        // this instantaite thing is cool, I might have to make more use of it in the future
        // for now tho, Imma just follow my suedo code cause Im in a fucking rush

    }

    public void allowParticle(bool wheyey) // use externally to set particle allowance. True by default unless otherwise told for obvious reasons
    {

        particleAllowed = wheyey; // sets particle allowed to passed argument

    }

    // we know what start does, cmon -------------------------------------------------------------------------------------------------------------------------
    void Start()
    {

        // start settup
        ME = this.gameObject; // should set ME to the parent game object
        grippers = GameObject.Find("GRIPPAH"); // setting grippers to GRIPPAH (dont ever delete that btw or this HAWHOLE thing gets fucked

    }

    // per FRAMO, it is I! EVIL DR FRAMO!!!! ------------------------------------------------------------------------------------------------------------------
    void Update()
    {
        
        if (grippers.GetComponent<sticktomouse>().hoveredOver == ME.name & Input.GetMouseButtonDown(0)) // if this object is hovered and the mouse is clicked
        {

            // just grabbing only for now to test initial settup
            spawnObject.GetComponent<draggableobject>().setGrabbed(true);
            // ok time to test
            // ok setter is working as intended, spawning works, now we need it to work along side despawn
            // we will use particals no matter what to start, then we will remove particles when the clip works
            spawned = true;

        }

        if (spawned == true & Input.GetMouseButtonUp(0)) // if the bullet is in a spawned state and the mouse is let go
        {
            if (particleAllowed == true) // spawn particles only when allowed
            {

                spawnParticle(); // moving this up here so it happens right before the spawn object despawns
                // should allow us to spawn this in the correct position right as the spawn object itself despawns

            }

            spawnObject.transform.position = despawnTarget.transform.position; // move the spawn object to the despawn target when despawning
            spawned = false; // resets spawned status to match despawning
            // now to research how to clone the particle system so we can mask despawn with it, but testing first
            // ok this is working, now lets play with particles
            // perfect, partiles work as intended while despawning
            // when the clip gets its changes, we just need a nice easy single if

        }

    }

}

// oop, forgot this scripts safety check layer
// ah well, it works without it for now, so we just gonna ball since Im low on time