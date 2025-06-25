using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpiderBossWebEffect : MonoBehaviour
{
    public float stunTime = 3f;
    
    private void OnTriggerEnter(Collider other)
    {
        // Eðer çarpýþan nesne Player tag'ine sahipse
        if (other.CompareTag("Player"))
        {
            // Player'ý geçici olarak durdur
            StartCoroutine(DisablePlayerMovement(other));
        }
    }

    // Player'ýn hareketini geçici olarak devre dýþý býrakma ve geri açma
    private IEnumerator DisablePlayerMovement(Collider player)
    {
        // Player'ýn Movement veya CharacterController script'ini bul ve devre dýþý býrak
        PlayerInput playerInput = player.GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            playerInput.currentActionMap.Disable();
        }

        // 3 saniye boyunca hareketsiz kalmasýný saðla
        yield return new WaitForSeconds(stunTime);

        // 3 saniye sonra Player'ýn hareketini tekrar aktif hale getir
        if (playerInput != null)
        {
            playerInput.currentActionMap.Enable();
        }

        // Web prefab nesnesini de aktif deðil hale getirerek havuza geri ekle (opsiyonel)
        gameObject.SetActive(false);
    }
}