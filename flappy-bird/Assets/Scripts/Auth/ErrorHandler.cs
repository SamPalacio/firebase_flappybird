using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ErrorHandler : MonoBehaviour
{
    public static ErrorHandler Instance;
    public TMP_Text errorText;
    private void Awake()
    {
        Instance = this;
    }
    public void SetError(string m)
    {
        errorText.text = m; 
    }


}
