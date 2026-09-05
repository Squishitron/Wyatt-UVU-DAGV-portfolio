using UnityEngine;
using UnityEngine.InputSystem;

public class sticktomouse : MonoBehaviour
{

    Vector3 myPos;
    Vector3 mousePos;
    public Camera mainCamera;

    // Update is called once per frame
    void Update()
    {

        //finding current mouse position
        mousePos = Input.mousePosition;
        //setting default depth
        mousePos.z = 5f;

        //converting mouse position from screen to world space
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);


        //myPos = mousePos;
        //unused

        //updating myPos to match worldPos of the mouse cursor
        myPos = worldPos;

        //moves object to new myPos
        transform.position = myPos;


        //Debug.Log(mousePos);
        //Debug.Log(myPos);
        Debug.Log(worldPos);

    }
}
