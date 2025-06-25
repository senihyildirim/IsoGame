using UnityEngine;

[CreateAssetMenu(menuName = "Luck System/Piece Data")]
public class PieceData : ScriptableObject
{
    public int pieceID; // Par�a kimli�i (1-6 aras�nda zar numaras�)
    public GameObject pieceModel; 
    public bool isCollected; 
    public float dropChance; 
}
