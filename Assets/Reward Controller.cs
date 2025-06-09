using UnityEngine;
using UnityEngine.UI;

public class RewardController : MonoBehaviour
{
    [SerializeField] Image[] rewardImages;
    Rewards rewards;

    public void SetRewards()
    {
        if(rewards.appOpen) rewardImages[0].color = Color.grey;
        if(rewards.firtsCourse) rewardImages[1].color = Color.grey;
        if(rewards.firtsStudent) rewardImages[2].color = Color.grey;
        if(rewards.firtsNote) rewardImages[3].color = Color.grey;
        if(rewards.firtsAssistance) rewardImages[4].color = Color.grey;
        if(rewards.firtsEdit) rewardImages[5].color = Color.grey;
    }
    
}

public class Rewards
{
    public bool appOpen ;
    public bool firtsCourse;
    public bool firtsStudent;
    public bool firtsNote;
    public bool firtsAssistance;
    public bool firtsEdit;
}
