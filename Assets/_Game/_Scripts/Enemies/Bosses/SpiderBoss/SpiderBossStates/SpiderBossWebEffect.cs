using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpiderBossWebEffect : MonoBehaviour
{
    public float stunTime = 3f;
    
    private void OnTriggerEnter(Collider other)
    {
        // E�er �arp��an nesne Player tag'ine sahipse
        if (other.CompareTag("Player"))
        {
            // Player'� ge�ici olarak durdur
            StartCoroutine(DisablePlayerMovement(other));
        }
    }

    // Player'�n hareketini ge�ici olarak devre d��� b�rakma ve geri a�ma
    private IEnumerator DisablePlayerMovement(Collider player)
    {
        // Player'�n Movement veya CharacterController script'ini bul ve devre d��� b�rak
        PlayerInput playerInput = player.GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            playerInput.currentActionMap.Disable();
        }

        // 3 saniye boyunca hareketsiz kalmas�n� sa�la
        yield return new WaitForSeconds(stunTime);

        // 3 saniye sonra Player'�n hareketini tekrar aktif hale getir
        if (playerInput != null)
        {
            playerInput.currentActionMap.Enable();
        }

        // Web prefab nesnesini de aktif de�il hale getirerek havuza geri ekle (opsiyonel)
        gameObject.SetActive(false);
    }
}