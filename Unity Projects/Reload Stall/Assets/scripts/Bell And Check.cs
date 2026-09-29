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