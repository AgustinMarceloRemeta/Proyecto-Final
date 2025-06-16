using System;
using UnityEngine;
using UnityEngine.UI;

public class RewardController : MonoBehaviour
{
    [SerializeField] Image[] rewardImages;
    [SerializeField]Rewards rewards;
    public static RewardController instance;
    RewardAlert rewardAlert;

    private void Awake()
    {
        instance = this;
        rewardAlert = FindFirstObjectByType<RewardAlert>();
    }

    private void Start()
    {
        AddReward(Reward.appOpen);
    }
    public void SetRewards()
    {
        if(rewards.appOpen) rewardImages[0].gameObject.SetActive(false);
        else if(rewards.firtsCourse) rewardImages[1].gameObject.SetActive(false);
        else if(rewards.firtsStudent) rewardImages[2].gameObject.SetActive(false);
        else if(rewards.firtsNote) rewardImages[3].gameObject.SetActive(false);
        else if(rewards.firtsAssistance) rewardImages[4].gameObject.SetActive(false);
        else if(rewards.firtsEdit) rewardImages[5].gameObject.SetActive(false);
    }

    public void SetSpecifyReward(Reward reward)
    {
        switch (reward)
        {
            case Reward.appOpen:
                rewardImages[0].gameObject.SetActive(false);
                rewards.appOpen = true;
                break;
            case Reward.firtsCourse:
                rewardImages[1].gameObject.SetActive(false);
                rewards.firtsCourse= true;
                break;
            case Reward.firtsStudent:
                rewardImages[2].gameObject.SetActive(false);
                rewards.firtsStudent = true;
                break;
            case Reward.firtsNote:
                rewardImages[3].gameObject.SetActive(false);
                rewards.firtsNote= true;
                break;
            case Reward.firtsAssistance:
                rewardImages[4].gameObject.SetActive(false);
                rewards.firtsAssistance= true;
                break;
            case Reward.firtsEdit:
                rewardImages[5].gameObject.SetActive(false);
                rewards.firtsEdit = true;
                break;
        }
    }

        public void AddReward(Reward reward)
    {
        if (isBlocked(reward))
        {
            rewardAlert.StartReward(reward);
            SetSpecifyReward(reward);
        }
    }



    private bool isBlocked(Reward reward)
    {
        switch (reward)
        {
            case Reward.appOpen: return !rewards.appOpen;
            case Reward.firtsCourse: return !rewards.firtsCourse;
            case Reward.firtsStudent: return !rewards.firtsStudent;
            case Reward.firtsNote: return !rewards.firtsNote;
            case Reward.firtsAssistance: return !rewards.firtsAssistance;
            case Reward.firtsEdit: return !rewards.firtsEdit;

            default: return false;
        }
    }
}
[Serializable]
public class Rewards
{
    public bool appOpen = false;
    public bool firtsCourse = false;
    public bool firtsStudent = false;
    public bool firtsNote = false;
    public bool firtsAssistance = false;
    public bool firtsEdit = false;
}

public enum Reward
{
    appOpen,
    firtsCourse,
    firtsStudent,
    firtsNote,
    firtsAssistance,
    firtsEdit
}
