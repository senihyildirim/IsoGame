using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    private float shakeTimer = -1;
    private float shakeDuration;
    private float shakeAmplitude;
    private float shakeFrequency;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        CinemachineBasicMultiChannelPerlin perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = 0.5f; // Reset the shake effect after the duration
        perlin.m_FrequencyGain = 0.5f; // Reset the frequency to avoid unintended effects
    }

    public void ShakeCamera(float duration, float amplitude, float frequency)
    {
        CinemachineBasicMultiChannelPerlin perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = amplitude;
        perlin.m_FrequencyGain = frequency; // Set the frequency of the shake
        shakeDuration = duration;
        shakeTimer = duration;
        shakeAmplitude = amplitude;
        shakeFrequency = frequency;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                CinemachineBasicMultiChannelPerlin perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                perlin.m_AmplitudeGain = 0.5f; // Reset the shake effect after the duration
                perlin.m_FrequencyGain = 0.5f; // Reset the frequency to avoid unintended effects
            }
        }
    }
}
