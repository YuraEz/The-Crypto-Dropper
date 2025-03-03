using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Txt;
    [SerializeField] private Button btn;

    private bool fwe;
    private void Start()
    {
        Txt.text = "OFF";
        btn.onClick.AddListener(() =>
        {
     
            if (fwe)
            {
                Txt.text = "OFF";
                fwe = false;
                return;
            }
            Txt.text = "ON";
            fwe = true;
            return;
        });
    }
}
