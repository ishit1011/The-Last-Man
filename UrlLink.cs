using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrlLink : MonoBehaviour
{
    public void OpenURL()
    {
        Application.OpenURL("https://green-cybill-88.tiiny.site");
        Debug.Log("URL Working..");
    }
}
