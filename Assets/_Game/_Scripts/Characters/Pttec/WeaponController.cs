using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WeaponController : MonoBehaviour
{
    public WeaponBase currentWeapon; // Aktif silah

    [Header("Energy Bar")]
    public Image energyBarFill; // Enerji barı (Filled Image)
    public float currentEnergy = 100f;
    public float maxEnergy = 100f;

    [Header("Ultimate Bar")]
    public Image ultimateBarFill; // Ulti dolum barı (Filled Image)
    public GameObject ultiFullIndicator; // Ulti dolu göstergesi
    private float currentUltimateCharge = 0f; // Mevcut ulti dolumu

    public static bool isFiring = false; // Ateşleme durumu
    private float nextFireTime = 0f;

    private void Start()
    {
        if (ultiFullIndicator != null)
        {
            ultiFullIndicator.SetActive(false); // Başlangıçta ulti göstergesini kapat
        }
        UpdateEnergyBar();
        UpdateUltimateBar();
    }

    private void Update()
    {
        // Ateşleme kontrolü
        if (Input.GetKeyDown(KeyCode.F))
        {
            isFiring = !isFiring; // Ateşlemeyi başlat/durdur
        }

        // Ultimate kullanımı
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (currentUltimateCharge >= currentWeapon.NeedChargeUlti)
            {
                StartCoroutine(UseUltimateAndResumeFiring());
            }
            else
            {
                Debug.Log("Not enough ultimate charge to use!");
            }
        }

        // Ateşlemeyi devam ettir
        if (isFiring && currentWeapon != null && currentEnergy > 0f)
        {
            FireWeapon();
        }
    }

    private void FireWeapon()
    {
        // Ateşleme hızı kontrolü
        if (Time.time >= nextFireTime)
        {
            if (currentEnergy >= currentWeapon.energyPerShot)
            {
                currentWeapon.Fire(); // Silah ateşler
                StartCoroutine(SmoothConsumeEnergy(currentWeapon.energyPerShot, 0.5f)); // Enerji yavaşça azalır
                ChargeUltimate(currentWeapon.ultimateChargePerShot); // Ultimate charge artırılır
                nextFireTime = Time.time + currentWeapon.attackSpeed; // Ateşleme hızına göre bekleme süresi ayarlanır
            }
            else
            {
                Debug.Log("Not enough energy to fire!");
                isFiring = false; // Enerji bittiğinde ateşleme durdurulur
            }
        }
    }

    private IEnumerator UseUltimateAndResumeFiring()
    {
        bool wasFiring = isFiring; // Ateşleme durumu kaydedilir
        isFiring = false; // Ateşleme geçici olarak durdurulur

        // Ultimate kullanımı
        currentWeapon.UseUltimate();
        currentUltimateCharge = 0f; // Ulti sıfırlanır
        UpdateUltimateBar();

        // Ultimate animasyonu veya etkisini bekleyebilirsiniz (örneğin 1 saniye)
        yield return new WaitForSeconds(1f);

        // Ultimate kullanımı sonrası ateşlemeye geri dön
        if (wasFiring)
        {
            isFiring = true; // Eğer ultimate sırasında ateş ediliyorsa tekrar başlat
        }
    }

    private void ChargeUltimate(float amount)
    {
        currentUltimateCharge += amount;
        currentUltimateCharge = Mathf.Clamp(currentUltimateCharge, 0, currentWeapon.NeedChargeUlti);
        UpdateUltimateBar();
    }

    private IEnumerator SmoothConsumeEnergy(float amount, float duration)
    {
        float startValue = currentEnergy;
        float targetValue = Mathf.Clamp(currentEnergy - amount, 0, maxEnergy);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            currentEnergy = Mathf.Lerp(startValue, targetValue, elapsedTime / duration);
            UpdateEnergyBar();
            yield return null;
        }

        currentEnergy = targetValue;
        UpdateEnergyBar();
    }

    private void UpdateEnergyBar()
    {
        if (energyBarFill != null)
        {
            energyBarFill.fillAmount = currentEnergy / maxEnergy;
        }
    }

    private void UpdateUltimateBar()
    {
        if (ultimateBarFill != null)
        {
            ultimateBarFill.fillAmount = currentUltimateCharge / currentWeapon.NeedChargeUlti;
        }

        if (ultiFullIndicator != null)
        {
            ultiFullIndicator.SetActive(currentUltimateCharge >= currentWeapon.NeedChargeUlti);
        }
    }
}
