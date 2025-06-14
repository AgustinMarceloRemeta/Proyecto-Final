using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RewardAlert : MonoBehaviour
{
    [SerializeField] int repeatingCouroutine, velocityReward;
    [SerializeField] Sprite[] rewardImages;
    [SerializeField] Image imageUi;
    public void StartReward(string reward)
    {
        StopAllCoroutines();
        switch (reward)
        {
            case "appOpen": StartCoroutine(RewardCoroutine(repeatingCouroutine, rewardImages[0]));
                break; 
            case "firtsCourse": StartCoroutine(RewardCoroutine(repeatingCouroutine, rewardImages[0]));
                break; 
            case "firtsStudent": StartCoroutine(RewardCoroutine(repeatingCouroutine, rewardImages[0]));
                break; 
            case "firtsNote": StartCoroutine(RewardCoroutine(repeatingCouroutine, rewardImages[0]));
                break; 
            case "firtsAssistance": StartCoroutine(RewardCoroutine(repeatingCouroutine, rewardImages[0]));
                break; 
            case "firtsEdit": StartCoroutine(RewardCoroutine(repeatingCouroutine, rewardImages[0]));
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
            gameObject.SetActive(true);
            yield return new WaitForSeconds(velocityReward);
            gameObject.SetActive(false);
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
