using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ellipsePathRenderer : MonoBehaviour
{
    public GameObject planet;
    public int segments;
    public int planetCode;
    public float angle = 0f;
    float xradius;
    float yradius;
    float a;
    float b;
    LineRenderer line;
    [System.Obsolete]
    void Start()
    {
        xradius = GameManager.instance.solarSystemPosVal * GameManager.instance.solarSystemPosfactor[planetCode];
        a = xradius;
        yradius = xradius * (1- planet.GetComponent<rotateAroundSun>().eccentricity * planet.GetComponent<rotateAroundSun>().eccentricity);
        b = yradius;
        line = gameObject.GetComponent<LineRenderer>();
        line.SetVertexCount(segments + 1);
        line.useWorldSpace = true;
        CreatePoints();
        //Debug.Log(Mathf.Sin(Mathf.Deg2Rad * 180) + " value");
    }

    private void Update()
    {
        updatePoints();
    }
    void updatePoints()
    {
        #region mycode
        //float x = ((a * a) * (b * b)) / ((b * b) + (a * a)*(1 / (Mathf.Cos(Mathf.Deg2Rad * angle) * Mathf.Cos(Mathf.Deg2Rad * angle)) - 1));
        //x = Mathf.Sqrt(x);
        //float z = x * (Mathf.Tan(Mathf.Deg2Rad * angle));
        #endregion
        if(planet.transform.position.z > 0)
        {
            angle = Mathf.Rad2Deg * Mathf.Asin((planet.transform.position.x / xradius));
        }
        else if(planet.transform.position.z < 0)
        {
            angle = Mathf.Rad2Deg * Mathf.Asin(-(planet.transform.position.x / xradius));
            angle += 180;
        }
        
        CreatePoints();
    }

    void CreatePoints()
    {
        float x = 0;
        float y = 0f;
        float z = 0;

        int multiplyFactor = +1;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * xradius;
            z = Mathf.Cos(Mathf.Deg2Rad * angle) * yradius;
            line.SetPosition(i, new Vector3(x, y, z*multiplyFactor));
            angle = angle+(360f / segments);
            
        }
        //if (angle <= 90 && angle > -60)
        //{
        //    multiplyFactor = 1;
        //}
        //if (planet.gameObject.name == "mercury")
        //{
        //    Debug.Log(x + " x pos");
        //    Debug.Log(z + " z pos");
        //}
        
    }
}
