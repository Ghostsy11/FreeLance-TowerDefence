using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    #region Fields
    [Header("Panel Settings")]
    [SerializeField] GameObject SelectingPanel;
    [SerializeField] GameObject GameAchievementsPanel;
    [SerializeField] GameObject MainMenuUi;

    [Header("Animation Reward Settings")]
    [SerializeField] Image tempRewardImage;
    [SerializeField] Animator animator;
    private string boolParamterName = "Done";

    [Header("Reward Ref Normal Mode")]
    public bool normalMode;

    [SerializeField] Image normalMedal1;
    [SerializeField] Image normalMedal2;
    [SerializeField] Image normalMedal3;

    [Header("Reward Ref Creative Mode")]
    public bool hardMode;
    [SerializeField] Image hardMedel1;
    [SerializeField] Image hardMedal2;
    [SerializeField] Image hardMedal3;

    [Header("Reward Ref Hard Mode")]
    public bool creaitiveMode;
    [SerializeField] Image creaitiveMedal1;
    [SerializeField] Image creaitiveMedal2;
    [SerializeField] Image creaitiveMedal3;

    private bool normalMedal1Earned = false;
    private bool normalMedal2Earned = false;
    private bool normalMedal3Earned = false;
    private bool hardMedal1Earned = false;
    private bool hardMedal2Earned = false;
    private bool hardMedal3Earned = false;
    private bool creativeMedal1Earned = false;
    private bool creativeMedal2Earned = false;
    private bool creativeMedal3Earned = false;
    #endregion

    #region Event Functions

    private void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

        }

        SelectingPanel = GameObject.Find("SelectingGameMode");
        GameAchievementsPanel = GameObject.Find("GameAchievements");
        MainMenuUi = GameObject.Find("MianMenu");

        if (SelectingPanel != null)
        {
            SelectingPanel.SetActive(false);
        }

        if (GameAchievementsPanel != null)
        {
            GameAchievementsPanel.SetActive(false);
        }

        LoadAchievements(); // Load previously saved achievements

    }

    public void LoadScene(int sceneIndex)
    {
        Time.timeScale = 1.0f;
        if (sceneIndex == 1)
        {
            normalMode = true;
        }
        if (sceneIndex == 2)
        {
            creaitiveMode = true;
        }
        if (sceneIndex == 3)
        {
            hardMode = true;
        }
        SceneManager.LoadScene(sceneIndex);

        if (MainMenuUi != null)
        {
            MainMenuUi.SetActive(false);
        }


    }


    public void ResetGameModes()
    {
        creaitiveMode = false;
        hardMode = false;
        normalMode = false;
    }

    public void ReloadScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);

    }

    public void TurnOffSelectingPanel()
    {
        if (SelectingPanel != null)
        {
            SelectingPanel.SetActive(false);
        }
    }
    public void TurnOnSelectingPanel()
    {
        if (SelectingPanel != null)
        {
            SelectingPanel.SetActive(true);
        }
    }

    //public void SaveTheGameBeforeQuiting()
    //{
    //   if ( TryGetComponent("", GameTimer timerToSave))
    //    {

    //    }
    //}

    public void GameAchievementsTurnOn()
    {
        if (GameAchievementsPanel != null)
        {
            GameAchievementsPanel.SetActive(true);
        }
    }
    public void GameAchievementsTurnOff()
    {
        if (GameAchievementsPanel != null)
        {
            GameAchievementsPanel.SetActive(false);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
    #endregion

    #region Quests


    public void Questing()
    {
        EconomyManager manager = EconomyManager.Instance;
        Rewards rewards = Rewards.Instance;

        if (animator == null)
        {
            animator = GameObject.Find("GettingAchivement").GetComponent<Animator>();
            tempRewardImage = animator.transform.GetChild(0).GetComponent<Image>();
        }

        if (rewards.totalKills >= 100 && normalMode && !normalMedal1Earned)
        {
            normalMedal1.color = Color.white;
            tempRewardImage.sprite = normalMedal1.sprite;
            StartCoroutine(PlayRewardAnimation());
            manager.NormalAchievement();
            normalMedal1Earned = true; // Mark flag its done
            PlayerPrefs.SetInt("normalMedal1Earned", 1);  // Save progress
        }
        else if (rewards.totalKills >= 400 && normalMode && !normalMedal2Earned)
        {
            normalMedal2.color = Color.white;
            tempRewardImage.sprite = normalMedal2.sprite;
            StartCoroutine(PlayRewardAnimation());
            manager.NormalAchievement();
            normalMedal2Earned = true;
            PlayerPrefs.SetInt("normalMedal2Earned", 1);
        }
        else if (rewards.totalKills >= 1000 && normalMode && !normalMedal3Earned)
        {
            normalMedal3.color = Color.white;
            tempRewardImage.sprite = normalMedal3.sprite;
            StartCoroutine(PlayRewardAnimation());
            manager.GoodAchievement();
            normalMedal3Earned = true;
            PlayerPrefs.SetInt("normalMedal3Earned", 1);
        }
        else if (rewards.totalKills >= 1500 && hardMode && !hardMedal1Earned)
        {
            creaitiveMedal1.color = Color.white;
            tempRewardImage.sprite = hardMedel1.sprite;
            StartCoroutine(PlayRewardAnimation());
            manager.EpicAchievement();
            hardMedal1Earned = true;
            PlayerPrefs.SetInt("hardMedal1Earned", 1);
        }
        else if (rewards.totalKills >= 2000 && hardMode && !hardMedal2Earned)
        {
            hardMedal2.color = Color.white;
            tempRewardImage.sprite = hardMedal2.sprite;
            StartCoroutine(PlayRewardAnimation());
            manager.GoodAchievement();
            hardMedal2Earned = true;
            PlayerPrefs.SetInt("hardMedal2Earned", 1);
        }
        else if (rewards.totalKills >= 5000 && GameTimer.Instance.lastRecordedTime >= 360f && hardMode && !hardMedal3Earned)
        {
            hardMedal3.color = Color.white;
            tempRewardImage.sprite = hardMedal3.sprite;
            StartCoroutine(PlayRewardAnimation());
            manager.RareAchievement();
            hardMedal3Earned = true;
            PlayerPrefs.SetInt("hardMedal3Earned", 1);
        }
        else if (manager.GetGoldAmount() > 2000 && manager.GetStoneAmount() >= 2000 && creaitiveMode && !creativeMedal1Earned)
        {
            manager.NormalAchievement();
            tempRewardImage.sprite = creaitiveMedal1.sprite;
            StartCoroutine(PlayRewardAnimation());
            creaitiveMedal1.color = Color.white;
            creativeMedal1Earned = true;
            PlayerPrefs.SetInt("creativeMedal1Earned", 1);
        }
        //else if (GameObject.Find("PlayerCastle").GetComponent<ObjectHealth>().objectCurrentHealth() <= 0 && creaitiveMode && !creativeMedal2Earned)
        //{
        //    creaitiveMedal2.color = Color.white;
        //    tempRewardImage.sprite = creaitiveMedal2.sprite;
        //    StartCoroutine(PlayRewardAnimation());
        //    creativeMedal2Earned = true;
        //    PlayerPrefs.SetInt("creativeMedal2Earned", 1);
        //}
        else if (manager.GetGoldAmount() > 3000 && manager.GetStoneAmount() >= 3000 && creaitiveMode && !creativeMedal3Earned)
        {
            creaitiveMedal3.color = Color.white;
            tempRewardImage.sprite = creaitiveMedal3.sprite;
            StartCoroutine(PlayRewardAnimation());
            manager.RareAchievement();
            creativeMedal3Earned = true;
            PlayerPrefs.SetInt("creativeMedal3Earned", 1);
        }

        PlayerPrefs.Save();  // Ensure all data is saved immediately
    }

    private IEnumerator PlayRewardAnimation()
    {
        animator.SetBool(boolParamterName, true);
        yield return new WaitForSeconds(5);
        animator.SetBool(boolParamterName, false);
    }


    private void LoadAchievements()
    {
        normalMedal1Earned = PlayerPrefs.GetInt("normalMedal1Earned", 0) == 1;
        normalMedal2Earned = PlayerPrefs.GetInt("normalMedal2Earned", 0) == 1;
        normalMedal3Earned = PlayerPrefs.GetInt("normalMedal3Earned", 0) == 1;
        hardMedal1Earned = PlayerPrefs.GetInt("hardMedal1Earned", 0) == 1;
        hardMedal2Earned = PlayerPrefs.GetInt("hardMedal2Earned", 0) == 1;
        hardMedal3Earned = PlayerPrefs.GetInt("hardMedal3Earned", 0) == 1;
        creativeMedal1Earned = PlayerPrefs.GetInt("creativeMedal1Earned", 0) == 1;
        creativeMedal2Earned = PlayerPrefs.GetInt("creativeMedal2Earned", 0) == 1;
        creativeMedal3Earned = PlayerPrefs.GetInt("creativeMedal3Earned", 0) == 1;

        // Apply saved status to UI
        if (normalMedal1Earned) normalMedal1.color = Color.white;
        if (normalMedal2Earned) normalMedal2.color = Color.white;
        if (normalMedal3Earned) normalMedal3.color = Color.white;
        if (hardMedal1Earned) hardMedel1.color = Color.white;
        if (hardMedal2Earned) hardMedal2.color = Color.white;
        if (hardMedal3Earned) hardMedal3.color = Color.white;
        if (creativeMedal1Earned) creaitiveMedal1.color = Color.white;
        if (creativeMedal2Earned) creaitiveMedal2.color = Color.white;
        if (creativeMedal3Earned) creaitiveMedal3.color = Color.white;
    }

    public void ResetAchievements()
    {
        PlayerPrefs.DeleteKey("normalMedal1Earned");
        PlayerPrefs.DeleteKey("normalMedal2Earned");
        PlayerPrefs.DeleteKey("normalMedal3Earned");
        PlayerPrefs.DeleteKey("hardMedal1Earned");
        PlayerPrefs.DeleteKey("hardMedal2Earned");
        PlayerPrefs.DeleteKey("hardMedal3Earned");
        PlayerPrefs.DeleteKey("creativeMedal1Earned");
        PlayerPrefs.DeleteKey("creativeMedal2Earned");
        PlayerPrefs.DeleteKey("creativeMedal3Earned");

        PlayerPrefs.Save();
        Debug.Log("Achievements Reset!");
    }

}

#endregion

