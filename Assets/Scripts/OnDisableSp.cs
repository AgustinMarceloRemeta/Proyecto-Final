using UnityEngine;
using UnityEngine.Events;

public class OnDisableSp : MonoBehaviour
{
    public UnityEvent disableEvent;
    private void OnDisable()
    {
        disableEvent?.Invoke();
    }
}
