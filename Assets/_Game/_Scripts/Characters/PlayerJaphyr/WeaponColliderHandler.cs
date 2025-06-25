using UnityEngine;

public class WeaponColliderHandler : MonoBehaviour
{
    [SerializeField] Collider weaponCollider;

    private void Start()
    {
        weaponCollider.enabled = false;
    }

    public void ActivateWeaponCollider()
    {
        weaponCollider.enabled = true;
    }

    public void DeactivateWeaponCollider()
    {
        weaponCollider.enabled = false;
    }
}
