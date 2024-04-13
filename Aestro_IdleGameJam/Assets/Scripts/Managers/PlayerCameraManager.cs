using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;




    public class PlayerCameraManager : MonoBehaviour
    {
        // Start is called before the first frame update
        public static PlayerCameraManager instance { get; private set; }
        public CinemachineVirtualCamera VirtualCamera { get; private set; }
        public Plane CameraPlane { get; private set; }

        private float shakeTimer;
        private float shakeDuration;
        private float shakeAmplitude;
        private float shakeFrequency;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != null)
            {
                Destroy(gameObject);
            }

            VirtualCamera = GetComponent<CinemachineVirtualCamera>();
            CameraPlane = new Plane(Vector3.up, Vector3.zero);
        }

        void Update()
        {
            UpdateCameraShake();
        }

        // Method to start camera shake with specified duration, amplitude, and frequency
        public void ShakeCamera(float duration, float amplitude, float frequency)
        {
            shakeTimer = duration;
            shakeDuration = duration;
            shakeAmplitude = amplitude;
            shakeFrequency = frequency;
        }

        private void UpdateCameraShake()
        {
            if (shakeTimer > 0)
            {
                // Update the CinemachineBasicMultiChannelPerlin settings during the shake duration
                VirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain =
                    shakeAmplitude;
                VirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain =
                    shakeFrequency;

                // Reduce the shake timer over time
                shakeTimer -= Time.deltaTime;
            }
            else
            {
                // Reset the shake values once the duration is over
                VirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0f;
                VirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0f;
            }
        }
    }

