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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchToMainCam(); 
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
           SwitchToDieCam();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchToCaveCam();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchToWideCamera();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwitchToFrontofPlayerCamera();  
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


  
  
      

     

