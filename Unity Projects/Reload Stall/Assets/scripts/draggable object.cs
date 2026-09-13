using UnityEngine;

public class draggableobject : MonoBehaviour

{

    Vector3 mousePos;
    RaycastHit2D raycastHit2D;
    Transform clickObject;
    //public to be set in inspector
    public Camera mainCamera;
    


    //is this yitch grabbed rn? cause clickObject doesnt constantly update
    bool isGrabbed = false;

    bool mouseDown = false;

    void Update()
    {

        //finding mouse screen position
        mousePos = Input.mousePosition;
        mousePos.z = 5f;

        //normalising mouse to world position
        Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(mousePos);

        //raycast from screen space to check for clickable objects
        Ray mouseRay = Camera.main.ScreenPointToRay(mousePos);

        //when clicking
        if (Input.GetMouseButtonDown(0))
        {
            //checking if cursor is over clickable object
            raycastHit2D = Physics2D.Raycast(mouseRay.origin, mouseRay.direction);
            clickObject = raycastHit2D ? raycastHit2D.collider.transform : null;

            //if clickable object clicked
            if (clickObject)
            {
                //debug change color to red
                clickObject.GetComponent<SpriteRenderer>().color = Color.red;

                //set yitch grabage to true
                isGrabbed = true;


            }

        }

        //when letting go
        if (Input.GetMouseButtonUp(0))
        {
            //if letting go of clickable object
            if (clickObject)
            {
                //debug return to defualt color
                clickObject.GetComponent<SpriteRenderer>().color = Color.white;

                //set yitch grabbed to false when letting go
                isGrabbed = false;

            }


        }

        //if this yitch is grabbed, move the object to mouse world position
        if (isGrabbed == true)
        {

            transform.position = mouseWorld;

        }

    }



}