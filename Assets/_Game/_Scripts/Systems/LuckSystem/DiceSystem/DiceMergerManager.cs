using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceMergerManager : MonoBehaviour
{
    private List<PieceData> allPieces;
    public Dictionary<int, int> collectedPiecesCount = new Dictionary<int, int>();
    private int diceCount = 0;

    PieceManager pieceManager;

    private void Start()
    {
        pieceManager = PieceManager.Instance;
        allPieces = pieceManager.allPieces;

        InitializeCollectedPieces();
    }

    private void InitializeCollectedPieces()
    {
        foreach (PieceData piece in allPieces)
        {
            collectedPiecesCount[piece.pieceID] = 0;
        }
    }

    /// <summary>
    /// Parça toplandýðýnda çaðrýlýr ve zar oluþturma kontrolü yapýlýr.
    /// </summary>
    public void AddCollectedPiece(PieceData piece)
    {
        if (collectedPiecesCount.ContainsKey(piece.pieceID))
        {
            collectedPiecesCount[piece.pieceID]++;
            Debug.Log($"Piece {piece.pieceID} collected. Total: {collectedPiecesCount[piece.pieceID]}");

            // Eðer tüm parçalar toplandýysa zar oluþtur
            if (AreAllPiecesCollected())
            {
                CreateDice();
            }
        }
    }

    private bool AreAllPiecesCollected()
    {
        foreach (PieceData piece in allPieces)
        {
            if (collectedPiecesCount[piece.pieceID] == 0)
            {
                return false;
            }
        }
        return true;
    }

    private void CreateDice()
    {
        Debug.Log("All pieces collected! Creating a dice...");

        diceCount++;
        Debug.Log($"Total dice created: {diceCount}");

        foreach (PieceData piece in allPieces)
        {
            if (collectedPiecesCount[piece.pieceID] > 0)
            {
                collectedPiecesCount[piece.pieceID]--;
                Debug.Log($"Piece {piece.pieceID} collected. Total: {collectedPiecesCount[piece.pieceID]}");
            }
            // Eðer parçanýn sayýsý 0'a ulaþtýysa existingPieces'ten çýkar ve missingPieces'e ekle
            if (collectedPiecesCount[piece.pieceID] == 0)
            {
                pieceManager.ResetPiecesAfterDice(piece);
            }
        }
        pieceManager.SetInitialDropChances();
        pieceManager.dropChanceManager.UpdateDropChancesBasedOnRelationship();
    }

   
    public int GetDiceCount()
    {
        return diceCount;
    }

    // Zar sayýsýný saklamak istersek
    public void SaveDiceCount()
    {
        PlayerPrefs.SetInt("TotalDiceCreated", diceCount);
        PlayerPrefs.Save();
    }

    // Oyunu açtýðýnýzda zar sayýsýný geri yüklemek için
    public void LoadDiceCount()
    {
        diceCount = PlayerPrefs.GetInt("TotalDiceCreated", 0);  // 0 default deðerdir.
    }
}
