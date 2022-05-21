using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightMovement : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        updateLightRotation();
    }
    void updateLightRotation()
    {
        Vector3 cameraPos = Camera.main.transform.position;
        Vector2 angle = new Vector2(cameraPos.x, cameraPos.z);
        float inverseVal = (angle.y) / (angle.x);
        float angle2 = Mathf.Rad2Deg*Mathf.Atan(inverseVal);
        this.transform.eulerAngles = new Vector3(0, angle2, 0);
    }
}
