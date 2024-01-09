using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class CameraRot : PlayerData
{
    [SerializeField] private float mouseSpeed = 8f; //회전속도
    private float mouseX = 0f, mouseY = 0f;

    private void Start() =>
        Observable.EveryUpdate()
            .Where(_ => !Pause.GameIsPaused)
            .Subscribe(_ => Rotate())
            .AddTo(gameObject);

    void Rotate()
    {
        mouseY += Input.GetAxis("Mouse Y") * mouseSpeed;
        mouseX += Input.GetAxis("Mouse X") * mouseSpeed;

        mouseY = Mathf.Clamp(mouseY, -50f, 30f);

        Com.root.localEulerAngles = new Vector3(0, mouseX, 0);
        Com.cam.localEulerAngles = new Vector3(-mouseY, 0, 0);
    }
}