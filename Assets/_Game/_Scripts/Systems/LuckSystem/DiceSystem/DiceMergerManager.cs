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
    /// Par�a topland���nda �a�r�l�r ve zar olu�turma kontrol� yap�l�r.
    /// </summary>
    public void AddCollectedPiece(PieceData piece)
    {
        if (collectedPiecesCount.ContainsKey(piece.pieceID))
        {
            collectedPiecesCount[piece.pieceID]++;
            Debug.Log($"Piece {piece.pieceID} collected. Total: {collectedPiecesCount[piece.pieceID]}");

            // E�er t�m par�alar topland�ysa zar olu�tur
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
            // E�er par�an�n say�s� 0'a ula�t�ysa existingPieces'ten ��kar ve missingPieces'e ekle
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

    // Zar say�s�n� saklamak istersek
    public void SaveDiceCount()
    {
        PlayerPrefs.SetInt("TotalDiceCreated", diceCount);
        PlayerPrefs.Save();
    }

    // Oyunu a�t���n�zda zar say�s�n� geri y�klemek i�in
    public void LoadDiceCount()
    {
        diceCount = PlayerPrefs.GetInt("TotalDiceCreated", 0);  // 0 default de�erdir.
    }
}
