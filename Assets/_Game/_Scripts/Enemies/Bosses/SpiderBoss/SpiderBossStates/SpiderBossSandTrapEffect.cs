using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpiderBossSandTrapEffect : MonoBehaviour
{
    public float stunDuration = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerHealth>(out var playerHealth))
        {
            StartCoroutine(StunPlayer(playerHealth));
        }
    }

    private IEnumerator StunPlayer(PlayerHealth playerHealth)
    {
        if (playerHealth.TryGetComponent<PlayerInput>(out var playerInput))
        {
            playerInput.currentActionMap.Disable();
        }

        yield return new WaitForSeconds(stunDuration);

        if (playerInput != null)
        {
            playerInput.currentActionMap.Enable();
        }

        gameObject.SetActive(false);
    }
}
