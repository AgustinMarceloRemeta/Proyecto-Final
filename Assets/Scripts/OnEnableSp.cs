using UnityEngine;
using UnityEngine.Events;

public class OnEnableSp : MonoBehaviour
{
    public UnityEvent eventOnEnable;
    private void OnEnable()
    {
        eventOnEnable?.Invoke();
    }
}
