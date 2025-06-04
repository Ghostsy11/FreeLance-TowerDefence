using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class EconomyManager : MonoBehaviour
{

    public static EconomyManager Instance;

    public int Gold { get; set; }
    private TextMeshProUGUI goldText;
    public int Stone { get; set; }
    private TextMeshProUGUI stoneText;
    public int Reward { get; private set; }


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {


        goldText = GameObject.FindGameObjectWithTag("GoldText").gameObject.GetComponent<TextMeshProUGUI>();
        stoneText = GameObject.FindGameObjectWithTag("StoneText").gameObject.GetComponent<TextMeshProUGUI>();


        Gold = 200;
        Stone = 200;

        UpdateGoldStoneValue();

        Debug.Log(GetGoldAmount());
        Debug.Log(GetStoneAmount());

    }

    private void Update()
    {
        UpdateGoldStoneValue();
    }

    public int GetGoldAmount()
    {
        return Gold;
    }

    public int GetStoneAmount()
    {
        return Stone;
    }

    public void ResetGoldAmount()
    {
        Gold = 0;
    }

    public void ResetStoneAmound()
    {
        Stone = 0;
    }


    public void KillReward(int goldReward, int stoneReward)
    {
        Gold += goldReward;
        Stone += stoneReward;
    }

    public void GetSomeGoldCM()
    {
        Reward = 200;
        Gold += Reward;
    }

    public void GetSomeStoneCM()
    {
        Reward = 200;
        Stone += Reward;
    }

    public void NormalAchievement()
    {
        Reward = 200;
        Gold += Reward;
        Stone += Reward;
    }

    public void GoodAchievement()
    {
        Reward = 400;
        Gold += Reward;
        Stone += Reward;
    }

    public void EpicAchievement()
    {
        Reward = 1000;
        Gold += Reward;
        Stone += Reward;
    }

    public void RareAchievement()
    {
        Reward = 3000;
        Gold += Reward;
        Stone += Reward;
    }


    public void UpdateGoldStoneValue()
    {
        goldText.text = Gold.ToString();
        stoneText.text = Stone.ToString();
    }
}
