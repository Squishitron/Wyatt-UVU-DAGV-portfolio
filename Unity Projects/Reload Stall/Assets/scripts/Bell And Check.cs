using UnityEngine;

public class BellAndCheck : MonoBehaviour
{

    // settup ------------------------------------------------------------------------------------------------------------------------------------------------

    public GameObject BoltObject; // sets bolt object in inspector (THIS NEEDS TO CHANGE FOR REPEATABILITY IN THE FUTURE, WE JUST GETTING WORKING FOR NOW TIME IS RUNNING OUT)
    public ParticleSystem particles1;
    public ParticleSystem particles2;
    public ParticleSystem particles3;
    private string collidedObject; // stores the name of the collided object

    // methods -----------------------------------------------------------------------------------------------------------------------------------------------



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
                particles1.Play(true);
                particles2.Play(true);
                particles3.Play(true);

            }
            
        }
        
        if (Input.GetMouseButtonUp(0))
        {

            collidedObject = "nothing";

        }

    }

    // DEBUG -------------------------------------------------------------------------------------------------------------------------------------------------

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