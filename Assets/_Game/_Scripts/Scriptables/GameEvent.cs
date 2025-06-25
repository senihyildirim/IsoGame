using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameEvent : ScriptableObject
{
    private List<GameEventListener> listeners = new List<GameEventListener>();

    public void Raise()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
            listeners[i].OnEventRaised();
    }

    public void RegisterListener(GameEventListener listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(GameEventListener listener)
    {
        listeners.Remove(listener);
    }

#if UNITY_EDITOR
    [Header("Editor Testing")]
    [SerializeField] private bool testRaise;

    public void OnValidate()
    {
        if (testRaise)
        {
            Raise();
            testRaise = false;
        }
    }
#endif
}
