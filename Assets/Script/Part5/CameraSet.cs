using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSet : MonoBehaviour
{
    public GameObject MainScence;
    public GameObject CameraPos1;
    public GameObject MainCamera;
    public GameObject CameraPos2;
    private Vector3 center=new Vector3(0,0,0);
    public void SeletModeCamera()
    {
        MainCamera.transform.position = CameraPos1.transform.position;
        MainCamera.transform.rotation = CameraPos1.transform.rotation;
    }
    public void ChangeButtonCamera()
    {
        MainCamera.transform.position = CameraPos2.transform.position;
        MainCamera.transform.rotation = CameraPos2.transform.rotation;
    }
    private void Update()
    {
       if(Input.GetMouseButtonDown(2))
        {
            foreach (var item in PanelControl.vector3s)
            {
                center += item;
            }
            center /= PanelControl.vector3s.Count;
            center.z -= 5;
            Debug.Log(center);
        }
       if(Input.GetMouseButton(2))
        {
            MainScence.transform.RotateAround(center, MainCamera.transform.up*360, Input.GetAxis("Mouse X"));
            MainScence.transform.RotateAround(center, -MainCamera.transform.right*360, Input.GetAxis("Mouse Y"));
        }
       if(Input.GetMouseButtonUp(2))
        {
            center = Vector3.zero;
        }
    }
}
