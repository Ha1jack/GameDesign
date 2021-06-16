using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof( SpriteRenderer))] 



public class Tiling : MonoBehaviour
{
    public int offsetX = 2;


    public bool hasARightBuddy = false;
    public bool hasALeftBuddy = false;

    public bool reverseScale = false;

    private float spriteWidth = 0f;
    private Camera cam;
    private Transform myTransform;

    private void Awake()
    {
        cam = Camera.main;
        myTransform = transform;
    }


    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
        spriteWidth = sRenderer.sprite.bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        // does it still need buddies? If not do nothing
        if (hasALeftBuddy == false || hasARightBuddy == false) {
            // calculate the cameras extend (half the width) of twhat the camera can see in the world coordinates 
            float camHorizontalExtend = cam.orthographicSize * Screen.width / Screen.height;

            // calculate the x position where the camera can see the edge of the sprite
            float edgeVisiblePositionRight = (myTransform.position.x + spriteWidth/2) - camHorizontalExtend;
            float edgeVisiblePositionLeft = (myTransform.position.x - spriteWidth/2) + camHorizontalExtend;


            // checking is we can see the edge of the element and then calling MakeNewBuddy if we can
            if (cam.transform.position.x >= edgeVisiblePositionRight - offsetX && hasARightBuddy == false)
            {
                MakeNewBuddy(1);
                hasARightBuddy = true;
            }
            else if (cam.transform.position.x <= edgeVisiblePositionLeft + offsetX && hasALeftBuddy == false)
            {
                MakeNewBuddy(-1);
                hasALeftBuddy = true;
            }
        }
    }

    
    // a function the creates a buddy on the side required 
    void MakeNewBuddy (int rightOrLeft) {
        // calculating the new position for our new buddy
        Vector3 newPosition = new Vector3(myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z);
        // instantiating our new bussy and storing him in a variable
        Transform newBuddy = Instantiate(myTransform, newPosition, myTransform.rotation) as Transform;

        // if not tilable let's reverse the x size of our object to get rid of ugly seems
        if (reverseScale == true) {
            newBuddy.localScale = new Vector3(newBuddy.localScale.x * -1, newBuddy.localScale.y, newBuddy.localScale.z);
        }

        newBuddy.parent = myTransform.parent; 
        if (rightOrLeft > 0) {
            newBuddy.GetComponent<Tiling>().hasALeftBuddy = true;
        }
        else  {
            newBuddy.GetComponent<Tiling>().hasARightBuddy = true;
        }
    }

}
