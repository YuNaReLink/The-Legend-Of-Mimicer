using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class MouseSpeedSetting : MonoBehaviour
{
    private enum InputFieldNumber
    {
        MouseX,
        MouseY
    }
    [SerializeField]
    private List<InputField> inputFieldList = new List<InputField>();

    public void AwakeInitilaize()
    {
        GameObject g = null;
        InputField field = null;
        for(int i = 0; i < transform.childCount; i++)
        {
            g = transform.GetChild(i).gameObject;
            if(g != null)
            {
                field = g.GetComponent<InputField>();
                if(field != null)
                {
                    inputFieldList.Add(field);
                }
            }
        }
    }

    public void StartInitilaize()
    {
        inputFieldList[(int)InputFieldNumber.MouseX].onEndEdit.AddListener(SetMouseXSpeed);
        inputFieldList[(int)InputFieldNumber.MouseY].onEndEdit.AddListener(SetMouseYSpeed);
    }

    private void SetMouseXSpeed(string _text)
    {
        string text = _text;
        if (!IsNumeric(text)) { return; }
        float xSpeed = 0;
        if(float.TryParse(text, out xSpeed))
        {
            if(MouseSensitivityManager.Instance.GetMouseXSpeed == xSpeed) { return; }
            if (xSpeed > MouseSensitivityManager.Instance.MaxMouseSpeed)
            {
                xSpeed = MouseSensitivityManager.Instance.MaxMouseSpeed;
            }
            MouseSensitivityManager.Instance.SetMouseXSpeed(xSpeed);
            inputFieldList[(int)InputFieldNumber.MouseX].text = xSpeed.ToString();
        }
        else
        {
            inputFieldList[(int)InputFieldNumber.MouseX].text = MouseSensitivityManager.Instance.GetMouseXSpeed.ToString();
        }
    }
    private void SetMouseYSpeed(string _text)
    {
        string text = _text;
        if (!IsNumeric(text)) { return; }
        float ySpeed = 0;
        if (float.TryParse(text, out ySpeed))
        {
            if (MouseSensitivityManager.Instance.GetMouseYSpeed == ySpeed) { return; }
            if (ySpeed > MouseSensitivityManager.Instance.MaxMouseSpeed)
            {
                ySpeed = MouseSensitivityManager.Instance.MaxMouseSpeed;
            }
            MouseSensitivityManager.Instance.SetMouseYSpeed(ySpeed);
            inputFieldList[(int)InputFieldNumber.MouseY].text = ySpeed.ToString();
        }
        else
        {
            inputFieldList[(int)InputFieldNumber.MouseY].text = MouseSensitivityManager.Instance.GetMouseYSpeed.ToString();
        }
    }

    // テキストが数字のみかを判定するメソッド
    bool IsNumeric(string text)
    {
        return Regex.IsMatch(text, @"^\d+$");
    }
}
