using UnityEngine;

[CreateAssetMenu]
public class FloatVariable : ScriptableObject
{
    [SerializeField] private float currentValue;
    [SerializeField] private float defaultValue;

    public float CurrentValue { get { return currentValue; } set { currentValue = value; } }
    public float DefaultValue { get { return defaultValue; } set { defaultValue = value; } }

    public void Reset()
    {
        currentValue = defaultValue;
    }
}
