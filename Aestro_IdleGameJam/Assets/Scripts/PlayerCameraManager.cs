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
    }

    public void SwitchToMainCam()
    {
        MainCamera.Priority = 10;
        DieCamera.Priority = 0;
        CaveCamera.Priority = 0;
    }

    public void SwitchToDieCam()
    {
        DieCamera.Priority = 10;
        MainCamera.Priority = 0;    
        CaveCamera.Priority = 0;
    }

    public void SwitchToCaveCam()
    {
        CaveCamera.Priority = 10;
        DieCamera.Priority = 0;
        MainCamera.Priority = 0;
    }
}


  
  
      

     

