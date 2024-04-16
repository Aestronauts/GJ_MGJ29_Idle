using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;




public class PlayerCameraManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static PlayerCameraManager instance { get; private set; }

    public CinemachineVirtualCamera MainCamera;
    //public CinemachineVirtualCamera MainCameraShake;
    public CinemachineVirtualCamera DieCamera;
    public CinemachineVirtualCamera CaveCamera;
    public CinemachineVirtualCamera FrontofPlayerCamera;
    public CinemachineVirtualCamera WideCamera;

    public GameObject hiddencave;

    private float lastCameraChange;
    public float cameraChangeTimer = 8;

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

    }

    void FixedUpdate()
    {
        if (Time.time > lastCameraChange + cameraChangeTimer)
            PickARandomCamera();


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ResetCameraTimer(16);
            SwitchToMainCam(); 
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ResetCameraTimer(16);
           SwitchToDieCam();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ResetCameraTimer(16);
            SwitchToCaveCam();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ResetCameraTimer(16);
            SwitchToWideCamera();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ResetCameraTimer(16);
            SwitchToFrontofPlayerCamera();  
        }
    }

    private void ResetCameraTimer(float _newwaittime)
    {
        lastCameraChange = Time.time;
        cameraChangeTimer = _newwaittime;
    }

    private void PickARandomCamera()
    {
        ResetCameraTimer(8);

        int randomCamID = Random.Range(0, 6);
        switch (randomCamID)
        {
            case 1:
                SwitchToDieCam();
                break;
            case 2:
                SwitchToCaveCam();
                break;
            case 3:
                SwitchToWideCamera();
                break;
            case 4:
                SwitchToWideCamera();
                break;
            case 5:
                SwitchToFrontofPlayerCamera();
                break;
            default:
                SwitchToMainCam();
                break;
        }

    }

    public void SwitchToMainCam()
    {
        hiddencave.SetActive(false);
        MainCamera.Priority = 10;
        DieCamera.Priority = 0;
        CaveCamera.Priority = 0;
        WideCamera.Priority = 0;
        FrontofPlayerCamera.Priority = 0;
    }

    public void SwitchToDieCam()
    {
        hiddencave.SetActive(false);

        DieCamera.Priority = 10;
        MainCamera.Priority = 0;    
        CaveCamera.Priority = 0;
        WideCamera.Priority = 0;
        FrontofPlayerCamera.Priority = 0;
    }

    public void SwitchToCaveCam()
    {
        hiddencave.SetActive(true);

        CaveCamera.Priority = 10;
        DieCamera.Priority = 0;
        MainCamera.Priority = 0;
        WideCamera.Priority = 0;
        FrontofPlayerCamera.Priority = 0;
    }

    public void SwitchToFrontofPlayerCamera()
    {
        hiddencave.SetActive(false);
       
        FrontofPlayerCamera.Priority = 10;
        CaveCamera.Priority = 0;
        DieCamera.Priority = 0;
        MainCamera.Priority = 0;
        WideCamera.Priority = 0;
    }
    public void SwitchToWideCamera()
    {
        hiddencave.SetActive(false);

        WideCamera.Priority = 10;
        FrontofPlayerCamera.Priority = 0;
        CaveCamera.Priority = 0;
        DieCamera.Priority = 0;
        MainCamera.Priority = 0;
    }
}


  
  
      

     

