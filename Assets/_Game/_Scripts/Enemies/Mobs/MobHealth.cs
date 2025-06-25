using UnityEngine;
using DG.Tweening;
using System.Collections;

public class MobHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100;
    private float currentHealth;

    [Header("Drop Piece Animation")]
    public float spawnJumpHeight = 1f;
    public int jumpCount = 1;
    public float jumpDuration = 1f;

    [Header("Damage Flash")]
    [SerializeField] private Material flashMaterial;
    [SerializeField] private float flashDuration = 0.1f;
    private MaterialPropertyBlock propertyBlock;
    private Renderer[] renderers;
    private Coroutine flashCoroutine;

    private PieceData droppedPiece;

    private void Start()
    {
        currentHealth = maxHealth;
        // Initialize damage flash components
        propertyBlock = new MaterialPropertyBlock();
        renderers = GetComponentsInChildren<Renderer>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        PlayDamageFlash();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator FlashEffect()
    {
        // Store original materials
        Material[] originalMaterials = new Material[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].material;
            renderers[i].material = flashMaterial;
        }

        yield return new WaitForSeconds(flashDuration);

        // Restore original materials
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = originalMaterials[i];
        }
    }

    private void PlayDamageFlash()
    {
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }
        flashCoroutine = StartCoroutine(FlashEffect());
    }

    private void Die()
    {
        // DropPiece();
        Destroy(gameObject);
    }

    private void DropPiece()
    {
        if (PieceManager.Instance == null) return;

        try
        {
            droppedPiece = PieceManager.Instance.DropPiecesFromTheEnemy();

            if (droppedPiece != null && droppedPiece.pieceModel != null)
            {
                InstantiatePiece(droppedPiece);
            }
            else
            {
                Debug.Log("No valid piece to drop.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"Error dropping piece: {e.Message}");
        }
    }

    private void InstantiatePiece(PieceData piece)
    {
        if (piece == null || piece.pieceModel == null) return;

        try
        {
            var obj = Instantiate(piece.pieceModel, transform.position, Quaternion.identity);
            if (obj != null)
            {
                obj.transform.DOJump(GetRandomPosition(), spawnJumpHeight, jumpCount, jumpDuration);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error instantiating piece: {e.Message}");
        }
    }

    private Vector3 GetRandomPosition()
    {
        return new Vector3(
            transform.position.x + Random.Range(-7f, 7f),
            transform.position.y + Random.Range(1f, 3f),
            transform.position.z + Random.Range(-7f, 7f)
        );
    }
}
