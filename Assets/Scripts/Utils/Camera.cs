using UnityEngine;
using Cinemachine;

public class Camera : MonoBehaviour
{
    public CinemachineFreeLook freeLook;

    [Header("Settings")]
    public float sensitivityX = 300f;
    public float sensitivityY = 2f;

    void Update()
    {
        HandleRotation();
    }

    void HandleRotation()
    {
        if (Input.GetMouseButton(0)) 
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            freeLook.m_XAxis.Value += mouseX * sensitivityX * Time.deltaTime;
            freeLook.m_YAxis.Value -= mouseY * sensitivityY * Time.deltaTime;

            freeLook.m_YAxis.Value = Mathf.Clamp01(freeLook.m_YAxis.Value);
        }
    }
}