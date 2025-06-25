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
            {(0, 25), 19},   // 0-25 aras� ili�ki i�in maks �ans 22
            {(25, 50), 18},  // 25-50 aras� ili�ki i�in maks �ans 18
            {(50, 75), 14},  // 50-75 aras� ili�ki i�in minimum �ans 14
            {(75, 100), 10}  // 75-100 aras� ili�ki i�in minimum �ans 10
        };
    }

    // �li�kiye g�re minChanceValue'yu ayarlar
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
    /// �ans oranlar�n� ili�kiye g�re g�nceller.
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

        float minChanceValue = GetMinChanceValue(currentRelationship); // �ans� ili�ki bar�na g�re ayarla
        Debug.Log("chance value" + minChanceValue);

        // Toplanan par�alar�n �ans�n� minimum �ansa d���r
        float extraChance = 0f;
        foreach (PieceData piece in pieceManager.allPieces)
        {
            if (pieceManager.existingPieces.Contains(piece.pieceID))
            {
                extraChance += piece.dropChance - minChanceValue;
                piece.dropChance = minChanceValue;
            }
        }

        // Geri kalan (eksik) par�alara e�it miktarda ekstra �ans ekleniyor
        float increaseAmount = extraChance / pieceManager.missingPieces.Count;

        foreach (PieceData piece in pieceManager.missingPieces)
        {
            piece.dropChance += increaseAmount;
        }

        pieceManager.PieceDropChanceOnConsole();
    }
}
