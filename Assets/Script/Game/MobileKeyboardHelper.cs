using UnityEngine;
using TMPro;

public class MobileKeyboardHelper : MonoBehaviour
{
    private TouchScreenKeyboard mobileKeyboard;
    public TMP_InputField targetInputField;

    public void OpenKeyboard()
    {
        if (targetInputField != null)
        {
            mobileKeyboard = TouchScreenKeyboard.Open(targetInputField.text, TouchScreenKeyboardType.Default);
        }
    }

    void Update()
    {
        if (mobileKeyboard != null && targetInputField != null)
        {
            targetInputField.text = mobileKeyboard.text;

            if (mobileKeyboard.status != TouchScreenKeyboard.Status.Visible)
            {
                mobileKeyboard = null;
            }
        }
    }
}