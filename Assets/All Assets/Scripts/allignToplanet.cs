using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class allignToplanet : MonoBehaviour
{
    public GameObject planet;
    public float initialXPos;
    public bool exploringPlanet;
    public static allignToplanet instance;
    Vector3 myPos;
    //float xFactor;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        initialXPos = planet.transform.position.x;
        LineRenderer lr = this.gameObject.GetComponent<LineRenderer>();
        Vector3 from = planet.transform.position;
        Vector3 to = Camera.main.transform.position;
        //allignOutlineToPlanet();
        //xFactor = 960 / 400;
        //this.gameObject.transform.position = new Vector3((planet.transform.position.x * xFactor) / 1000, 0 , Camera.main.transform.position.z + 10);
        //lr.material = default;
        //lr.SetPosition(0, from);
        //lr.SetPosition(1, to);
    }
    private void Update()
    {
        //allignOutlineToPlanet();
    }
    void allignOutlineToPlanet()
    {
        Vector3 from = Camera.main.transform.position;
        Vector3 to = planet.transform.position - from;
        RaycastHit hitInfo;
        var ray = new Ray(from, to);
        if (Physics.Raycast(ray, out hitInfo,Mathf.Infinity))
        {
            if(hitInfo.transform.gameObject.tag == "canvas")
            {
                Vector3 _point = hitInfo.point;
                this.gameObject.transform.position = _point;
                this.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().color = Color.white;
                this.gameObject.GetComponent<Image>().color = Color.clear;
            }
        }
        //1920000
        //this.gameObject.transform.position = new Vector3((planet.transform.position.x * xFactor) / 1000, 0 , Camera.main.transform.position.z + 10);
    }
}
