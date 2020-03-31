using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSet : MonoBehaviour
{
    public GameObject CameraPos1;
    public GameObject MainCamera;
    public GameObject CameraPos2;
    private Vector3 inityCameraPosition;
    private Quaternion inityCameraQuatertion;
    public float MouceSpeed = 5f;
    public float MouceTraslateSpeed = 1f;
    private void Start()
    {
        MainCamera.transform.position = CameraPos2.transform.position;
        MainCamera.transform.rotation = CameraPos2.transform.rotation;
        inityCameraPosition = CameraPos2.transform.position;
        inityCameraQuatertion = CameraPos2.transform.rotation;
    }
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
       if(Input.GetMouseButton(2))
        {
            MainCamera.transform.Translate(new Vector3(-Input.GetAxis("Mouse X") *MouceTraslateSpeed,0,0),Space.Self);
            MainCamera.transform.Translate(new Vector3(0,-Input.GetAxis("Mouse Y") * MouceTraslateSpeed,0),Space.Self);
        }
        Camera_Move();
        Camera_Rotate();
    }
    public void Position_Reset()
    {
        MainCamera.transform.position = inityCameraPosition;
        MainCamera.transform.rotation = inityCameraQuatertion;
    }
    public void Camera_Move()
    {
        if(Input.GetAxis("Mouse ScrollWheel")!=0)
        {
            MainCamera.transform.Translate(new Vector3(0,0,Input.GetAxis("Mouse ScrollWheel")* MouceSpeed),Space.Self);
        }
    }
    public void Camera_Rotate()
    {
        if(Input.GetMouseButton(1))
        {

            MainCamera.transform.RotateAround(MainCamera.transform.position, Vector3.up, Input.GetAxis("Mouse X"));
            MainCamera.transform.RotateAround(MainCamera.transform.position, MainCamera.transform.right, -Input.GetAxis("Mouse Y"));
        }
    }
}
