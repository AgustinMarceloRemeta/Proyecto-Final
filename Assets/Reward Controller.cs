using UnityEngine;
using UnityEngine.UI;

public class RewardController : MonoBehaviour
{
    [SerializeField] Image[] rewardImages;
    Rewards rewards;

    public void SetRewards()
    {
        if(rewards.appOpen) rewardImages[0].gameObject.SetActive(false);
        if(rewards.firtsCourse) rewardImages[1].gameObject.SetActive(false);
        if(rewards.firtsStudent) rewardImages[2].gameObject.SetActive(false);
        if(rewards.firtsNote) rewardImages[3].gameObject.SetActive(false);
        if(rewards.firtsAssistance) rewardImages[4].gameObject.SetActive(false);
        if(rewards.firtsEdit) rewardImages[5].gameObject.SetActive(false);
    }
    
}

public class Rewards
{
    public bool appOpen = false;
    public bool firtsCourse = false;
    public bool firtsStudent = false;
    public bool firtsNote = false;
    public bool firtsAssistance = false;
    public bool firtsEdit = false;
}
