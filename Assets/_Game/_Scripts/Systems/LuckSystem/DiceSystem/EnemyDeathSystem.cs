using UnityEngine;
using DG.Tweening;
public class EnemyDeathSystem : MonoBehaviour
{
    private void EnemyDied()
    {
        // Þans sistemini kullanarak bir parça düþür
        PieceData droppedPiece = PieceManager.Instance.DropPiecesFromTheEnemy();
        // *****bu tek satir kodu kullanarak rastgele parcani dusurebilirsin*******

        if (droppedPiece != null)
        {
            InstantiatePiece(droppedPiece);
        }
        else
        {
            Debug.Log("No piece dropped.");
        }
    }
    private void InstantiatePiece(PieceData piece)
    {
        var obj = Instantiate(piece.pieceModel, transform);
        obj.transform.DOJump(RandomPos(), 1, 1, 1);
    }

    public Vector3 RandomPos()
    {
        Vector3 pos = new Vector3(transform.position.x + Random.Range(-7, 7), transform.position.y + Random.Range(1, 3), transform.position.z + Random.Range(-7, 7));
        return pos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EnemyDied();
        }
    }
}
