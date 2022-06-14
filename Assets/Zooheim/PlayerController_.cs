using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_ : MonoBehaviour {

    [SerializeField]
    private float walkSpeed = 25f;

    [SerializeField]
    private float lookSensitivity = 1f;

    [SerializeField]
    private float cameraRotationLimit = 60f;
    private float currentCameraRotationX = 0;

    [SerializeField]
    private Camera curCamera;

    private Rigidbody myRigid;

	void Start () {
        curCamera = Camera.main;
        curCamera.enabled = true;
        myRigid = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
	}
	

	void Update () {
        Move();
        CameraRotation();
        CharacterRotation();
	}

    void FixedUpdate() {

    }

    private void Move() {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * walkSpeed;

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
    }

    private void CharacterRotation() {
        // 좌우 캐릭터 회전
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.transform.rotation * Quaternion.Euler(_characterRotationY));
    }

    private void CameraRotation() {
        // 상하 카메라 회전
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        curCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }
}
