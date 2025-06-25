using UnityEngine;

public class PttecController : MonoBehaviour
{
    public WeaponBase currentWeapon; // Aktif silah referansı

    private bool isFiring = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            isFiring = !isFiring;
        }

        if (isFiring && currentWeapon != null)
        {
            currentWeapon.Fire();
        }

        if (Input.GetKeyDown(KeyCode.U) && currentWeapon != null) // Ultimate için örnek
        {
            currentWeapon.UseUltimate();
        }
    }
}
