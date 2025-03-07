using UnityEngine;
using UnityEngine.Events;

public class OnEnableSp : MonoBehaviour
{
    public UnityEvent eventOnEnable, eventOnStart;
    private void OnEnable()
    {
        eventOnEnable?.Invoke();
    }

    private void Start()
    {
        eventOnStart?.Invoke();
    }
}
