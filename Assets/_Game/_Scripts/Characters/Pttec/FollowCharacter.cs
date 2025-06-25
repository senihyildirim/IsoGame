using UnityEngine;

public class FollowCharacter : MonoBehaviour
{
    public Transform target; // Ana karakterin Transform referansı
    public float followSpeed = 1.66f; // Maksimum takip hızı
    public float acceleration = 0.66f; // İvme miktarı
    public float deceleration = 1f; // Yavaşlama miktarı
    public Vector3 offset = new Vector3(-1, 2.06f, -0.9f); // Sağ arka offset
    private Vector3 velocity = Vector3.zero; // İvme bazlı hız için

    void LateUpdate()
    {

        if (target != null)
        {
           
            // Karakterin yerel dönüşüne göre offset pozisyonu hesapla
            Vector3 desiredPosition = target.position + target.TransformDirection(offset);

            // İvme ve yavaşlamayı dikkate alarak pozisyonu güncelle
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, 1 / followSpeed, Mathf.Infinity, Time.deltaTime);

            if (!WeaponController.isFiring)
            {
                // Pttec'i karakterin baktığı yöne döndür
                transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, Time.deltaTime * followSpeed);
            }

        }
    }
}
