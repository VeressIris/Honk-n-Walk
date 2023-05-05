using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin perlinNoise;
    [SerializeField] private GameObject mainCam;

    void Start()
    {
        if (virtualCamera != null)
        {
            perlinNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if (perlinNoise != null)
            {
                ResetCamera();
            }
            else
            {
                Debug.LogWarning("CinemachineBasicMultiChannelPerlin component not found on virtual camera.");
            }
        }
        else
        {
            Debug.LogWarning("CinemachineVirtualCamera component not found on CameraShake script GameObject.");
        }
        //^stolen from chatGPT bc it couldn't find the multichannelperlin for some goddamn reason, i still don't understand why THIS works byt hey, i'm not complaining
    }


    public void ShakeCamera(float intensity, float time)
    {
        perlinNoise.m_AmplitudeGain = intensity;
        StartCoroutine(Wait(time));
    }

    private IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        ResetCamera();
    }

    private void ResetCamera()
    {
        perlinNoise.m_AmplitudeGain = 0f;
        mainCam.transform.position = new Vector3(mainCam.transform.position.x, 0, mainCam.transform.position.z); //need to reset the y position to 0 or else all the objects will be offsetted every time the camera shakes
    }
}
