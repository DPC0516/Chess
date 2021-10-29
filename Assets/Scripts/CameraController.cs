using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Public;
#pragma warning disable 0649
public class CameraController : MonoBehaviour
{
    [SerializeField]
    private new Camera camera;

    //카메라의 최대 위/아래 각도
    private const int max_payer_rotation_up = -90;
    private const int max_camera_rotation_down = 0;

    private const float max_camera_fov = 100f;
    private const float min_camera_fov = 10f;

    private const float mouse_sensitivity = 200f;
    private const float fov_sensitivity = 10f;

    private float camera_fov = 60f;

    //현재 카메라 각도
    private float camera_rotation_x = 0f;
    private float camera_rotation_y = 0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(Key.move_camera))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            //마우스 이동 감지
            float is_x = Input.GetAxisRaw("Mouse X");
            float is_y = Input.GetAxisRaw("Mouse Y");
            float to_rotate_x = is_x * mouse_sensitivity * Time.deltaTime;
            float to_rotate_y = is_y * mouse_sensitivity * Time.deltaTime;

            //X각도 변환
            camera_rotation_x += to_rotate_x;

            camera_rotation_y -= to_rotate_y;
            camera_rotation_y = Mathf.Clamp(camera_rotation_y, max_payer_rotation_up, max_camera_rotation_down);

            //각도 변환
            transform.localRotation = Quaternion.Euler(camera_rotation_y, camera_rotation_x, 0f);
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        camera_fov += Input.GetAxisRaw("Mouse ScrollWheel") * fov_sensitivity * -1f;
        camera_fov = Mathf.Clamp(camera_fov, min_camera_fov, max_camera_fov);
        camera.fieldOfView = camera_fov;
    }
}
