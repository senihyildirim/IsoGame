using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [SerializeField] private GameEvent gameEvent;
    [SerializeField] private UnityEvent response;

    private void OnEnable()
    {
        gameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        gameEvent.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        response.Invoke();
    }

#if UNITY_EDITOR
    [Header("Editor Testing")]
    [SerializeField] private bool testResponse;

    public void OnValidate()
    {
        if (testResponse)
        {
            response.Invoke();
            testResponse = false;
        }
    }
#endif
}
