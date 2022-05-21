using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class buttonClick : MonoBehaviour
{
    public TMP_Text clickMessage;
    void Start()
    {
        
    }


    void Update()
    {
        
    }
    public void updateTextOnClick(GameObject go)
    {
        StartCoroutine(changeText(go.name + " is clicked"));
    }
    IEnumerator changeText(string mesg)
    {
        string oldText = clickMessage.text;
        clickMessage.text = mesg;
        yield return new WaitForSeconds(3);
        clickMessage.text = oldText;
    }
}
