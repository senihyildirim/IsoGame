using System.Collections.Generic;
using UnityEngine;

public class DropChanceManager : MonoBehaviour
{
    private Dictionary<(float min, float max), int> relationshipToChanceMap;

    PieceManager pieceManager;
    private void Start()
    {
        pieceManager = PieceManager.Instance;
        InitializeChanceMap();
    }

    private void InitializeChanceMap()
    {
        relationshipToChanceMap = new Dictionary<(float, float), int>
        {
            {(0, 25), 19},   // 0-25 arasý iliþki için maks þans 22
            {(25, 50), 18},  // 25-50 arasý iliþki için maks þans 18
            {(50, 75), 14},  // 50-75 arasý iliþki için minimum þans 14
            {(75, 100), 10}  // 75-100 arasý iliþki için minimum þans 10
        };
    }

    // Ýliþkiye göre minChanceValue'yu ayarlar
    private float GetMinChanceValue(float relationshipValue)
    {
        foreach (var range in relationshipToChanceMap)
        {
            if (relationshipValue >= range.Key.min && relationshipValue <= range.Key.max)
            {
                return range.Value;
            }
        }
        return 10;
    }

    /// <summary>
    /// Þans oranlarýný iliþkiye göre günceller.
    /// </summary>
    public void UpdateDropChancesBasedOnRelationship()
    {
        float currentRelationship = pieceManager.npcRelationship.GetNPCRelationshipAmount();

        if (currentRelationship == 50) // npc diyalog icerisinde bir iliski degisimi olursa diye kontrol
        {
            pieceManager.SetInitialDropChances();
            pieceManager.PieceDropChanceOnConsole();
            return;
        }

        float minChanceValue = GetMinChanceValue(currentRelationship); // Þansý iliþki barýna göre ayarla
        Debug.Log("chance value" + minChanceValue);

        // Toplanan parçalarýn þansýný minimum þansa düþür
        float extraChance = 0f;
        foreach (PieceData piece in pieceManager.allPieces)
        {
            if (pieceManager.existingPieces.Contains(piece.pieceID))
            {
                extraChance += piece.dropChance - minChanceValue;
                piece.dropChance = minChanceValue;
            }
        }

        // Geri kalan (eksik) parçalara eþit miktarda ekstra þans ekleniyor
        float increaseAmount = extraChance / pieceManager.missingPieces.Count;

        foreach (PieceData piece in pieceManager.missingPieces)
        {
            piece.dropChance += increaseAmount;
        }

        pieceManager.PieceDropChanceOnConsole();
    }
}
