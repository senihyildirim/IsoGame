using PixelCrushers.DialogueSystem;
using UnityEngine;

public class NPCRelationship : MonoBehaviour
{
    public bool positiveRelation;
    public bool negativeRelation;
    [Range(0, 100)] public float npcRelationshipAmount = 50; // Nötr baþlangýç deðeri

    // Baþlangýç yüzdeleri
    public float positivePercentageIncrease = 7.5f; // Ýlk pozitif iliþki artýþý (% olarak)
    public float negativePercentageDecrease = -8.5f; // Ýlk negatif iliþki azalmasý (% olarak)

    // Pozitif ve negatif deðerlerin her seçimde nasýl deðiþeceði
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
        // Ýliþkiye % bazýnda artýþ ekler
        float increaseAmount = (npcRelationshipAmount * positivePercentageIncrease) / 100f;
        npcRelationshipAmount += increaseAmount;

        // Ýliþki maksimum 100 olabilir
        if (npcRelationshipAmount > 100) npcRelationshipAmount = 100;

        Debug.Log($"Positive relationship increased by {positivePercentageIncrease}%: New value: {npcRelationshipAmount}");

        // Bir sonraki olumlu seçenekte iliþki artýþý azalacak
        if (positivePercentageIncrease > 1f)
        {
            positivePercentageIncrease -= positiveDecreaseStep;
        }

        dropChanceManager.UpdateDropChancesBasedOnRelationship();
    }

    public void SetNegativeRelation()
    {
        // Ýliþkiye % bazýnda azalýþ ekler
        float decreaseAmount = (npcRelationshipAmount * negativePercentageDecrease) / 100f;
        npcRelationshipAmount += decreaseAmount;

        // Ýliþki minimum 0 olabilir
        if (npcRelationshipAmount < 0) npcRelationshipAmount = 0;

        Debug.Log($"Negative relationship decreased by {negativePercentageDecrease}%: New value: {npcRelationshipAmount}");

        // Bir sonraki olumsuz seçenekte iliþki düþüþü artacak
        if (negativePercentageDecrease > -10.5f)
        {
            negativePercentageDecrease -= negativeIncreaseStep; // Negatif bir deðer olduðu için azaltmak pozitif etki yapar
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
