using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateAroundSun : MonoBehaviour
{

    public float axisRotationSpeed;
    public float sunAroundRotationSpeed;
    public int planetCode;
    public float onPlanetWalkSpeed = 0;
    public float angleRotated;
    public float planetSpeed;
    public float eccentricity;
    public static rotateAroundSun instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    #region coordinates
    //x axis 
    float a = 0;
    //y axis 
    float b = 0;

    public float alpha;//angle rotated * 100
    float X = 0;
    float Y = 0;
    #endregion
    void Start()
    {
        alpha *= 100;
        planetSpeed = sunAroundRotationSpeed;
        a = GameManager.instance.solarSystemPosVal * GameManager.instance.solarSystemPosfactor[planetCode];
        b = a * (1- eccentricity * eccentricity);
        
    }

    void LateUpdate()
    {
        RotateAround();
        RotateOnOnAxis();
    }
    void RotateAround()
    {
        alpha += (sunAroundRotationSpeed * Time.deltaTime);
        X = (a * Mathf.Cos(alpha * .001f));
        Y = (b * Mathf.Sin(alpha * .001f));
        this.gameObject.transform.position = new Vector3(X, 0, Y);
    }
    void RotateOnOnAxis()
    {
        transform.Rotate(0, -axisRotationSpeed * Time.deltaTime, 0);
    }
}
