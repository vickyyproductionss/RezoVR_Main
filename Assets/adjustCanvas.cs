using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class adjustCanvas : MonoBehaviour
{
    public Transform canvas;
    public Transform canvasPos;
    private void LateUpdate()
    {
        Vector3 updatedPos = canvasPos.position;
        canvas.transform.position = updatedPos;
        canvas.transform.rotation = this.transform.rotation;
    }

}
