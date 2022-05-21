using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class GameManager : MonoBehaviour
{
    public Transform revealingLight;
    public GameObject _origin;
    public GameObject spaceShip;
    public GameObject canvas;
    public List<float> canvasPrev;
    public List<float> canvasFinal;
    public List<float> camSpeedInitial;
    public List<float> camSpeedFinal;
    public List<Vector3> offsetInAndOutSpaceship;
    public GameObject sun;
    public GameObject CirclesOverPlanet;
    public GameObject planetDetails;
    public GameObject lineRenderersParent;
    public GameObject[] planets;
    public TMP_Text currentPlanetName;
    string activeSceneName;
    public static GameManager instance;
    public List<float> solarSystemScalefactor;
    public List<float> solarSystemPosfactor;
    public float solarSystemScaleVal;
    public float solarSystemPosVal;
    public float distanceToLookPlanet;
    bool walkingAroundPlanet;
    GameObject lastWalkAroundPlanet;
    public GameObject welcomeNote;
    public List<GameObject> listOfObjectToTurnOffToShowQuotes;
    Transform initialTransform;
    public int activePlanetIndex;
    public GameObject spacesuit;
    public GameObject exitWalkAroundButton;

    void Start()
    {
        OnAppStartThings();
        initialTransform = this.gameObject.transform;
        adjustSizeAndDistanceOfPlanets();
        //adjusting the size of planets in the starting
        //calculateDist(go[0], go[1]);
    }
    void OnAppStartThings()
    {
        hideAllObjectsHiddenToShowQuote();
        StartCoroutine(showWelcomeNote());
        _origin.GetComponent<CameraFollow2>().enabled = false;
    }
    void playVoiceWelcomeNote()
    {
        SoundManager.instance.playClipAtOrigin(SoundManager.instance.welcomeNote, 3);
    }
    IEnumerator showWelcomeNote()
    {
        yield return new WaitForSeconds(0.5f);
        welcomeNote.SetActive(true);
        playVoiceWelcomeNote();
        yield return new WaitForSeconds(3f);
        welcomeNote.SetActive(false);
        _origin.GetComponent<CameraFollow2>().enabled = true;
        Vector3 offset = offsetInAndOutSpaceship[0];
        CameraFollow2.instance.setTargetTransfrom(initialTransform, offset);
        showAllObjectsHiddenToShowQuote();
        canvas.SetActive(false);
        StartCoroutine(onAppStartAnimation(14));
        yield return new WaitForSeconds(14);
        SoundManager.instance.playClipAtOrigin(SoundManager.instance.followInstructions, 3);
        canvas.SetActive(true);
    }
    IEnumerator onAppStartAnimation(int animTime)
    {
        _origin.GetComponent<Animator>().enabled = true;
        Animator animator = _origin.GetComponent<Animator>();
        animator.SetBool("startAnim", true);
        yield return new WaitForSeconds(animTime);
        //_origin.GetComponent<Animator>().enabled = false;
        animator.SetBool("startAnim", false);
    }
    void showAllObjectsHiddenToShowQuote()
    {
        for(int i = 0; i < listOfObjectToTurnOffToShowQuotes.Count;i++)
        {
            listOfObjectToTurnOffToShowQuotes[i].SetActive(true);
        }
    }
    void hideAllObjectsHiddenToShowQuote()
    {
        for (int i = 0; i < listOfObjectToTurnOffToShowQuotes.Count; i++)
        {
            listOfObjectToTurnOffToShowQuotes[i].SetActive(false);
        }
    }
    void adjustSizeAndDistanceOfPlanets()
    {
        for(int i = 0 ; i < planets.Length; i++)
        {
            float scaleVal = solarSystemScaleVal * solarSystemScalefactor[i];
            planets[i].transform.localScale = new Vector3(scaleVal, scaleVal, scaleVal);
            //float posVal = solarSystemPosVal * solarSystemPosfactor[i];
            //planets[i].transform.position = new Vector3(posVal,0,0);
        }
    }
    void calculateDist(GameObject go1, GameObject go2)
    {
        Vector3 pos1 = go1.transform.position;
        Vector3 pos2 = go2.transform.position;
        Vector3 dist = pos2 - pos1;
        Debug.Log("distance bw given objects is " + dist.magnitude);
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    
    public void openActiveScene()
    {
        SceneManager.LoadScene(activeSceneName);
    }
    public void openGameobject(GameObject go) {go.SetActive(true);}
    public void closeGameobject(GameObject go) {go.SetActive(false);}
    public void openScene(int index) {SceneManager.LoadScene(index);}
    void changeCanvasSize(int index)
    {
        if(index == 0)
        {
            canvas.GetComponent<RectTransform>().localPosition = new Vector3(0,0,canvasPrev[0]);
            canvas.GetComponent<RectTransform>().localScale = new Vector3(canvasPrev[1], canvasPrev[1], canvasPrev[1]);
        }
        else if(index == 1)
        {
            canvas.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, canvasFinal[0]);
            canvas.GetComponent<RectTransform>().localScale = new Vector3(canvasFinal[1], canvasFinal[1], canvasFinal[1]);
        }
    }
    public void leaveWalkAround()
    {
        if(!spacesuit.activeInHierarchy)
        {
            lastWalkAroundPlanet.GetComponent<rotateAroundSun>().enabled = true;
            walkingAroundPlanet = false;
            changeCamPointPos(new Vector3(0, 1, 0));
            _origin.GetComponent<CameraFollow2>().enabled = true;
            allignToplanet.instance.exploringPlanet = false;
            Invoke("activateLineRenderes", 0.2f);
            for (int i = 0; i < CirclesOverPlanet.transform.childCount; i++)
            {
                CirclesOverPlanet.transform.GetChild(i).gameObject.SetActive(true);
            }
            CameraFollow2.instance.translationSpeed = camSpeedInitial[0];
            CameraFollow2.instance.rotationSpeed = camSpeedInitial[1];
            Vector3 offset = offsetInAndOutSpaceship[0];
            CameraFollow2.instance.setTargetTransfrom(initialTransform, offset);
            currentPlanetName.text = "";
            planetDetails.SetActive(false);
            OnWalkEndAnimation();
            exitWalkAroundButton.SetActive(false);
        }
        else
        {
            showMessage("Please get inside spaceship first...");
        }
        
        //for (int i = 1; i < planets.Length; i++)
        //{
        //    planets[i].GetComponent<rotateAroundSun>().sunAroundRotationSpeed = planets[i].GetComponent<rotateAroundSun>().planetSpeed;
        //}
        //changeCanvasSize(0);
    }
    IEnumerator changeCameraRot(Transform t)
    {
        Camera.main.transform.LookAt(t);
        yield return new WaitForEndOfFrame();
        if(walkingAroundPlanet)
        {
            StartCoroutine(changeCameraRot(t));
        }
    }
    void OnWalkStartAnimation()
    {

    }
    void OnWalkEndAnimation()
    {

    }
    public void walkAroundPlanet(int index)
    {
        if(!spacesuit.activeInHierarchy)
        {
            OnWalkStartAnimation();
            activePlanetIndex = index;
            SoundManager.instance.playClipAtOrigin(SoundManager.instance.desiredPlanet, 3);
            walkingAroundPlanet = true;
            Transform t = planets[index].transform;
            Transform t_cam = planets[index].transform.GetChild(planets[index].transform.childCount - 1).transform;
            if (revealingLight != null)
            {
                revealingLight.transform.LookAt(t);
            }
            for (int i = 0; i < lineRenderersParent.transform.childCount; i++)
            {
                lineRenderersParent.transform.GetChild(i).gameObject.SetActive(false);
            }
            lastWalkAroundPlanet = t.gameObject;
            changeCamPointPos(new Vector3(0, 0.5f, 0));
            PlayerPrefs.SetFloat(t.gameObject.name + "speed", t.gameObject.GetComponent<rotateAroundSun>().sunAroundRotationSpeed);
            lastWalkAroundPlanet.GetComponent<rotateAroundSun>().enabled = false;
            _origin.GetComponent<CameraFollow2>().enabled = true;
            for (int i = 0; i < CirclesOverPlanet.transform.childCount; i++)
            {
                CirclesOverPlanet.transform.GetChild(i).gameObject.SetActive(false);
            }
            _origin.GetComponent<CameraFollow2>().enabled = true;
            Vector3 offset = offsetInAndOutSpaceship[2];
            //r⃗ = (mb⃗ + na⃗) / (m + n) --> first formula internally division
            //r⃗   = (mb⃗ –na⃗)/ (m–n) --> second formula externally division
            Vector3 myPos = _origin.transform.position;
            Vector3 targetPos = t.transform.position;
            float distanceBetween = Mathf.Abs((targetPos - myPos).magnitude);
            float m = distanceBetween - distanceToLookPlanet; // m ratio value for formula
            float n = distanceToLookPlanet * planets[index].transform.localScale.x;
            Vector3 targetOrbitPos = (m * targetPos + n * myPos) / (m + n);
            //SphereCollider collider = planets[index].GetComponent<SphereCollider>();
            //targetOrbitPos = collider.ClosestPoint(myPos);
            GameObject targetOrbit = new GameObject();
            targetOrbit.AddComponent<Rigidbody>();
            targetOrbit.transform.rotation = planets[index].transform.rotation;
            targetOrbit.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            targetOrbit.transform.position = planets[index].GetComponent<SphereCollider>().ClosestPoint(_origin.transform.position);
            targetOrbit.transform.position = new Vector3(targetOrbit.transform.position.x, planets[index].transform.Find("camPoint").transform.position.y * 2, targetOrbit.transform.position.z);
            Transform targetOrbitTransform = targetOrbit.transform;
            CameraFollow2.instance.setTargetTransfrom(targetOrbitTransform, offset);
            currentPlanetName.text = t.gameObject.name;
            activeSceneName = t.gameObject.name;
            planetDetails.SetActive(true);
            //allignToplanet.instance.exploringPlanet = true; ->> text decalration off
            List<string> values = getPlanetDetails(currentPlanetName.text.ToLower());
            planetDetails.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = values[0];
            planetDetails.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = values[1];
            planetDetails.transform.GetChild(2).GetChild(0).GetComponent<TMP_Text>().text = values[2];
            planetDetails.transform.GetChild(3).GetChild(0).GetComponent<TMP_Text>().text = values[3];
        }
        else
        {
            showMessage("Please get inside spaceship first...");
        }
        
        //CameraFollow2.instance.setTargetTransfrom(t_cam,offset);
        //RaycastHit hitInfo;
        //Physics.Raycast(myPos, targetPos, out hitInfo, Mathf.Infinity);
        //if(hitInfo.collider)
        //{
        //    if(hitInfo.collider.tag == "planets")
        //    {
        //        targetOrbitPos = hitInfo.collider.ClosestPoint(myPos);
        //        Debug.Log("hit");
        //    }
        //    else
        //    {
        //        Debug.Log("we hitted " + hitInfo.collider.gameObject.name + " at position "+ hitInfo.collider.gameObject.transform.position);
        //        Debug.DrawRay(myPos, hitInfo.collider.gameObject.transform.position * 10000, Color.white);
        //    }
        //}
        //else
        //{
        //    Debug.Log("not hit");
        //}
        //lastWalkAroundPlanet.GetComponent<rotateAroundSun>().axisRotationSpeed = 0f;
        //lastWalkAroundPlanet.GetComponent<rotateAroundSun>().sunAroundRotationSpeed = 0;
        //for(int i =1; i <planets.Length; i++)
        //{
        //    planets[i].GetComponent<rotateAroundSun>().sunAroundRotationSpeed = planets[i].GetComponent<rotateAroundSun>().onPlanetWalkSpeed;
        //}
        //if(lastWalkAroundPlanet)
        //{
        //    lastWalkAroundPlanet.GetComponent<rotateAroundSun>().sunAroundRotationSpeed = PlayerPrefs.GetFloat(lastWalkAroundPlanet.name + "speed");
        //    changeCamPointPos(new Vector3(0, 1, 0));
        //}
        //Invoke("disableCameraFollow", 3f);
        //changeCanvasSize(1);
        //StartCoroutine(changeCameraRot(t_cam));
        //CameraFollow2.instance.translationSpeed = Mathf.Lerp(10,1000, 2f);
        //CameraFollow2.instance.rotationSpeed = 12;
        //_origin.GetComponent<FollowGyro>().enabled = false;
        //Invoke("returnToNormalCameraSpeed", 2);
    }
    void changeCamPointPos(Vector3 pos)
    {
        lastWalkAroundPlanet.transform.Find("camPoint").transform.localPosition = pos;
    }
    public void takeViewFromOutsideTheSpaceship()
    {
        spaceShip.SetActive(false);
        changeCamPointPos(new Vector3(0, 1, 0));
        Vector3 offset = offsetInAndOutSpaceship[1];
        CameraFollow2.instance.setOffset(offset);
    }
    public void takeViewFromInsideTheSpaceship()
    {
        Invoke("setSpaceShipActive", 1);
        changeCamPointPos(new Vector3(0, 0.5f, 0));
        Vector3 offset = offsetInAndOutSpaceship[2];
        CameraFollow2.instance.setOffset(offset);
    }
    void setSpaceShipActive()
    {
        spaceShip.SetActive(true);
    }
    void disableCameraFollow()
    {
        _origin.GetComponent<CameraFollow2>().enabled = false;
    }
    void increaseTranslationalSpeed()
    {
        CameraFollow2.instance.translationSpeed = 1000;
    }
    void returnToNormalCameraSpeed()
    {
        CameraFollow2.instance.translationSpeed = 100;
        CameraFollow2.instance.rotationSpeed = 100;
    }
    void activateLineRenderes()
    {
        for (int i = 0; i < lineRenderersParent.transform.childCount; i++)
        {
            lineRenderersParent.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
    public void enableGyro()
    {
        _origin.GetComponent<CameraFollow2>().enabled = false;
        _origin.GetComponent<FollowGyro>().enabled = true;
    }
    public void showMessage(string mesg)
    {
        StartCoroutine(showMessageOnScreen(mesg));
    }
    public IEnumerator showMessageOnScreen(string mesg)
    {
        string oldMessage = currentPlanetName.text;
        currentPlanetName.fontSize = currentPlanetName.fontSize + 10;
        currentPlanetName.text = mesg;
        yield return new WaitForSeconds(3);
        currentPlanetName.fontSize = currentPlanetName.fontSize - 10;
        currentPlanetName.text = oldMessage;

    }
    List<string> getPlanetDetails(string planetName)
    {
        List<string> value = new List<string>();
        //distance from sun
        //number of moons
        //light from sun one way time
        //year length
        if (planetName == "mercury")
        {
            value.Add("0.4 AU");
            value.Add("0");
            value.Add("2.588 Mins");
            value.Add("88 EARTH DAYS");
        }
        else if (planetName == "venus")
        {
            value.Add("0.7 AU");
            value.Add("0");
            value.Add("6.044 Mins");
            value.Add("255 EARTH DAYS");
        }
        else if (planetName == "earth")
        {
            value.Add("1 AU");
            value.Add("1");
            value.Add("8.347 Mins");
            value.Add("365.25 EARTH DAYS");
        }
        else if (planetName == "mars")
        {
            value.Add("1.5 AU");
            value.Add("2");
            value.Add("11.731 Mins");
            value.Add("1.88 EARTH YEARS");
        }
        else if (planetName == "jupiter")
        {
            value.Add("5.2 AU");
            value.Add("79");
            value.Add("41.360 Mins");
            value.Add("11.86 EARTH YEARS");
        }
        else if (planetName == "saturn")
        {
            value.Add("9.5 AU");
            value.Add("62");
            value.Add("82.309 Mins");
            value.Add("29.45 EARTH YEARS");
        }
        else if (planetName == "uranus")
        {
            value.Add("19.8 AU");
            value.Add("27");
            value.Add("163.903 Mins");
            value.Add("84 EARTH YEARS");
        }
        else if (planetName == "neptune")
        {
            value.Add("30.1 AU");
            value.Add("14");
            value.Add("248.821 Mins");
            value.Add("164.81 EARTH YEARS");
        }
        return value;

    }
}
