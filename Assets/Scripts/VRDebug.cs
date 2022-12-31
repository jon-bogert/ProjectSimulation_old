using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//using UnityEditor.ShaderGraph.Drawing;


public class VRDebug : MonoBehaviour
{
    [SerializeField] TMP_Text FPSField;
    [SerializeField] TMP_Text Monitor1;
    [SerializeField] TMP_Text Monitor2;
    [SerializeField] TMP_Text Monitor3;
    [SerializeField] TMP_Text Monitor4;
    [SerializeField] TMP_Text Monitor5;
    [SerializeField] TMP_Text Monitor6;
    [SerializeField] TMP_Text Message1;
    [SerializeField] TMP_Text Message2;
    [SerializeField] TMP_Text Message3;
    [SerializeField] TMP_Text Message4;
    [SerializeField] TMP_Text Message5;

    //bool isFirstMsg = true;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFPS();
    }

    void UpdateFPS()
    {
        string newStr;
        if (Time.deltaTime != 0)
            newStr = ((int)(1f /Time.deltaTime)).ToString();
        else newStr = "0";
        FPSField.text = newStr;
    }

    public void Log(string msg)
    {
        Message5.text = Message4.text;
        Message4.text = Message3.text;
        Message3.text = Message2.text;
        Message2.text = Message1.text;
        Message1.text = msg;
    }

    public void Monitor(int num, string str)
    {
        switch (num)
        {
            case 1:
                Monitor1.text = str;
                break;
            case 2:
                Monitor2.text = str;
                break;
            case 3:
                Monitor3.text = str;
                break;
            case 4:
                Monitor4.text = str;
                break;
            case 5:
                Monitor5.text = str;
                break;
            case 6:
                Monitor6.text = str;
                break;
        }
    }
}
