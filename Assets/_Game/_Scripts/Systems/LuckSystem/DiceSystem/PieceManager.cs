using System.Collections.Generic;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
    public static PieceManager Instance { get; private set; }

    public List<PieceData> allPieces;
    public List<int> existingPieces = new List<int>();
    public List<PieceData> missingPieces = new List<PieceData>();

    public NPCRelationship npcRelationship;
    public DiceMergerManager diceMergerManager;
    public DropChanceManager dropChanceManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        npcRelationship = GameObject.FindGameObjectWithTag("NPC").GetComponent<NPCRelationship>();
    }
    private void Start()
    {
        SetInitialDropChances();
        GetMissingPieces();
    }
    public void SetInitialDropChances()
    {
        float initialChance = 1f / allPieces.Count * 100; // 1/6 = %16.67
        foreach (PieceData piece in allPieces)
        {
            piece.dropChance = initialChance;
        }
    }

    private List<PieceData> GetMissingPieces()
    {
        foreach (PieceData piece in allPieces)
        {
            if (!existingPieces.Contains(piece.pieceID))
            {
                missingPieces.Add(piece);
            }
        }
        PieceDropChanceOnConsole();
        return missingPieces;
    }
    private void AddExistingPieces(PieceData piece)
    {
        Debug.Log("Dropped Piece: " + piece.pieceID);
        if (!existingPieces.Contains(piece.pieceID))
        {
            existingPieces.Add(piece.pieceID);
            missingPieces.Remove(piece);

            if (npcRelationship.GetNPCRelationshipAmount() != 50) // npc ile iletisim kurmadan parca dusurdugumde sans degisimi olmasin diye kontrol
            {
                dropChanceManager.UpdateDropChancesBasedOnRelationship();
            }
        }
        diceMergerManager.AddCollectedPiece(piece); // her toplanan parcasi saymasi gerek 
    }
    public void PieceDropChanceOnConsole()
    {
        foreach (PieceData piece in allPieces)
        {
            Debug.Log("Piece ID: " + piece.pieceID + ", New Drop Chance: " + piece.dropChance + "%");
        }
    }

    private PieceData CalculatePieceDrop()
    {
        // Toplam �ans hesaplan�r
        float totalChance = 0f;
        foreach (PieceData piece in allPieces)
        {
            totalChance += piece.dropChance;
        }

        // 0 ile toplam �ans aras�nda rastgele bir de�er se�ilir
        float randomValue = Random.Range(0f, totalChance);
        float cumulativeChance = 0f;

        // Par�alar�n d��me �ans�na g�re rastgele bir par�a se�ilir
        foreach (PieceData piece in allPieces)
        {
            cumulativeChance += piece.dropChance;

            if (randomValue <= cumulativeChance)
            {
                return piece;
            }
        }

        // E�er hi�bir par�a se�ilemezse null d�nd�r�l�r
        return null;
    }

    /// <summary>
    /// Rastgele bir par�a dusmesini saglar
    /// </summary>
    public PieceData DropPiecesFromTheEnemy()
    {
        PieceData Droppiece = CalculatePieceDrop();
        AddExistingPieces(Droppiece);
        return Droppiece;
    }

    public void ResetPiecesAfterDice(PieceData piece)
    {
        if (existingPieces.Contains(piece.pieceID))
        {
            existingPieces.Remove(piece.pieceID);
            missingPieces.Add(piece);
            Debug.Log($"Piece {piece.pieceID} has been reset and moved back to missing pieces.");
        }
    }
}
