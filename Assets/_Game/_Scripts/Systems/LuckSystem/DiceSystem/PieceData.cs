using UnityEngine;

[CreateAssetMenu(menuName = "Luck System/Piece Data")]
public class PieceData : ScriptableObject
{
    public int pieceID; // Parça kimliði (1-6 arasýnda zar numarasý)
    public GameObject pieceModel; 
    public bool isCollected; 
    public float dropChance; 
}
