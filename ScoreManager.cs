using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    [Header("Score Manager")]
    public int kills;
    public int enemyKills;
    public Text playerKillCounter;
    public Text enemyKillCounter;
    public Text Maintext;


    private void Awake()
    {
        if(PlayerPrefs.HasKey("kills"))
        {
            kills = PlayerPrefs.GetInt("0");
        }
        else if(PlayerPrefs.HasKey("enemyKills"))
        {
            enemyKills = PlayerPrefs.GetInt("0");
        }
    }


    private void Update()
    {
        StartCoroutine(WinOrLose());
    }


    IEnumerator WinOrLose()
    {
        playerKillCounter.text = "" + kills;
        enemyKillCounter.text = "" + enemyKills;
        
        if(kills >= 10)
        {
            Maintext.text = "Blue Team Victory";
            PlayerPrefs.SetInt("kills", kills);
            Time.timeScale = 0f;
            yield return new WaitForSeconds(5f);
            SceneManager.LoadScene("TDMRoom");
            // AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("TDMRoom");
            // while (!asyncLoad.isDone)
            // {
            //     yield return null;
            // }
        }
        
        else if(enemyKills >= 10)
        {
            Maintext.text = "Red Team Victory";
            PlayerPrefs.SetInt("enemyKills", enemyKills);
            Time.timeScale = 0f;
            yield return new WaitForSeconds(5f);
            SceneManager.LoadScene("TDMRoom");
            // AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("TDMRoom");
            // while (!asyncLoad.isDone)
            // {
            //     yield return null;
            // }
        }
    }
}
