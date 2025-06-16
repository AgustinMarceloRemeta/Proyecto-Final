using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RewardAlert : MonoBehaviour
{
    [SerializeField] int repeatingCouroutine;
    [SerializeField] float velocityReward;
    [SerializeField] Sprite[] rewardImages;
    [SerializeField] Image imageUi;
    public void StartReward(Reward reward)
    {
        StopAllCoroutines();
        switch (reward)
        {
            case Reward.appOpen: StartCoroutine(RewardCoroutine(repeatingCouroutine, rewardImages[0]));
                break; 
            case Reward.firtsCourse: StartCoroutine(RewardCoroutine(repeatingCouroutine, rewardImages[1]));
                break; 
            case Reward.firtsStudent: StartCoroutine(RewardCoroutine(repeatingCouroutine, rewardImages[2]));
                break; 
            case Reward.firtsNote: StartCoroutine(RewardCoroutine(repeatingCouroutine, rewardImages[3]));
                break; 
            case Reward.firtsAssistance: StartCoroutine(RewardCoroutine(repeatingCouroutine, rewardImages[4]));
                break; 
            case Reward.firtsEdit: StartCoroutine(RewardCoroutine(repeatingCouroutine, rewardImages[5]));
                break; 
            default:
                break;
        }
    }

    public IEnumerator RewardCoroutine(int countBlinks, Sprite image)
    {
        if (countBlinks > 0)
        {
            imageUi.sprite = image;
            transform.GetChild(0).gameObject.SetActive(true);
            yield return new WaitForSeconds(velocityReward);
            transform.GetChild(0).gameObject.SetActive(false);
            yield return new WaitForSeconds(velocityReward);
            StartCoroutine(RewardCoroutine(countBlinks - 1,image));
        }
        else
        {
            imageUi.sprite =null;
            yield return null;
        }
    }
}
