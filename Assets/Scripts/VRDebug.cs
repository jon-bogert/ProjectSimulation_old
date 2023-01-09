using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//using UnityEditor.ShaderGraph.Drawing;


public class VRDebug : MonoBehaviour
{
    [SerializeField] float messageTime = 3f;
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

    float timer1 = 0f;
    float timer2 = 0f;
    float timer3 = 0f;
    float timer4 = 0f;
    float timer5 = 0f;

    //bool isFirstMsg = true;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFPS();
        UpdateOpacity();
        UpdateTimers();
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

        timer5 = timer4;
        timer4 = timer3;
        timer3 = timer2;
        timer2 = timer1;
        timer1 = messageTime;
    }

    float ToOpacity(float timer)
    {
        return timer / messageTime;
    }

    void UpdateOpacity()
    {
        {
            Color tmp = new Color(Message5.color.r, Message5.color.g, Message5.color.b, ToOpacity(timer5));
            Message5.color = tmp;
        }
        {
            Color tmp = new Color(Message4.color.r, Message4.color.g, Message4.color.b, ToOpacity(timer4));
            Message4.color = tmp;
        }
        {
            Color tmp = new Color(Message3.color.r, Message3.color.g, Message3.color.b, ToOpacity(timer3));
            Message3.color = tmp;
        }
        {
            Color tmp = new Color(Message2.color.r, Message2.color.g, Message2.color.b, ToOpacity(timer2));
            Message2.color = tmp;
        }
        {
            Color tmp = new Color(Message1.color.r, Message1.color.g, Message1.color.b, ToOpacity(timer1));
            Message1.color = tmp;
        }
    }

    void UpdateTimers()
    {
        timer5 = (timer5 <= 0) ? 0 : timer5 - Time.deltaTime;
        timer4 = (timer4 <= 0) ? 0 : timer4 - Time.deltaTime;
        timer3 = (timer3 <= 0) ? 0 : timer3 - Time.deltaTime;
        timer2 = (timer2 <= 0) ? 0 : timer2 - Time.deltaTime;
        timer1 = (timer1 <= 0) ? 0 : timer1 - Time.deltaTime;
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
