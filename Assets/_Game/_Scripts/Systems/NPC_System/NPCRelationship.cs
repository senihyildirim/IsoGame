using PixelCrushers.DialogueSystem;
using UnityEngine;

public class NPCRelationship : MonoBehaviour
{
    public bool positiveRelation;
    public bool negativeRelation;
    [Range(0, 100)] public float npcRelationshipAmount = 50; // N�tr ba�lang�� de�eri

    // Ba�lang�� y�zdeleri
    public float positivePercentageIncrease = 7.5f; // �lk pozitif ili�ki art��� (% olarak)
    public float negativePercentageDecrease = -8.5f; // �lk negatif ili�ki azalmas� (% olarak)

    // Pozitif ve negatif de�erlerin her se�imde nas�l de�i�ece�i
    float positiveDecreaseStep = 1f;
    float negativeIncreaseStep = 1f;

    DropChanceManager dropChanceManager;

    private void Start()
    {
        dropChanceManager = PieceManager.Instance.dropChanceManager;
    }

    public bool IsPositive => npcRelationshipAmount > 50;
    public bool IsNegative => npcRelationshipAmount < 50;

    public float GetNPCRelationshipAmount() => npcRelationshipAmount;

    public void SetPositiveRelation()
    {
        // �li�kiye % baz�nda art�� ekler
        float increaseAmount = (npcRelationshipAmount * positivePercentageIncrease) / 100f;
        npcRelationshipAmount += increaseAmount;

        // �li�ki maksimum 100 olabilir
        if (npcRelationshipAmount > 100) npcRelationshipAmount = 100;

        Debug.Log($"Positive relationship increased by {positivePercentageIncrease}%: New value: {npcRelationshipAmount}");

        // Bir sonraki olumlu se�enekte ili�ki art��� azalacak
        if (positivePercentageIncrease > 1f)
        {
            positivePercentageIncrease -= positiveDecreaseStep;
        }

        dropChanceManager.UpdateDropChancesBasedOnRelationship();
    }

    public void SetNegativeRelation()
    {
        // �li�kiye % baz�nda azal�� ekler
        float decreaseAmount = (npcRelationshipAmount * negativePercentageDecrease) / 100f;
        npcRelationshipAmount += decreaseAmount;

        // �li�ki minimum 0 olabilir
        if (npcRelationshipAmount < 0) npcRelationshipAmount = 0;

        Debug.Log($"Negative relationship decreased by {negativePercentageDecrease}%: New value: {npcRelationshipAmount}");

        // Bir sonraki olumsuz se�enekte ili�ki d����� artacak
        if (negativePercentageDecrease > -10.5f)
        {
            negativePercentageDecrease -= negativeIncreaseStep; // Negatif bir de�er oldu�u i�in azaltmak pozitif etki yapar
        }

        dropChanceManager.UpdateDropChancesBasedOnRelationship();
    }

    private void OnEnable()
    {
        Lua.RegisterFunction("SetPositiveRelation", this, SymbolExtensions.GetMethodInfo(() => SetPositiveRelation()));
        Lua.RegisterFunction("SetNegativeRelation", this, SymbolExtensions.GetMethodInfo(() => SetNegativeRelation()));
    }

    private void OnDisable()
    {
        Lua.UnregisterFunction("SetPositiveRelation");
        Lua.UnregisterFunction("SetNegativeRelation");
    }
}
