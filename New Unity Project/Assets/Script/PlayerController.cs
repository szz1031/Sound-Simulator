using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSetting;
public class PlayerController : MonoBehaviour {
    Transform tf_Head;
    CharacterController m_Controller;
    public float F_CameraSensitivity=.5f;
    public float F_MovementSpeed=3;
    private void Awake()
    {
        tf_Head = transform.Find("Head");
        m_Controller = GetComponent<CharacterController>();
    }
    private void Start()
    {
        CameraController.Instance.Attach(tf_Head);
        PCInputManager.Instance.AddMovementDelta(OnMove);
        PCInputManager.Instance.AddMouseRotateDelta(OnRotate);
        PCInputManager.Instance.AddBinding<PlayerController>(enum_BindingsName.Interact,TryInterach);
    }
    RaycastHit hit = new RaycastHit();
    void TryInterach()
    {
        if (CameraController.Instance.ForwardRayCheck( enum_HitCheckType.Dynamic.ToCastLayer(),GameConst.I_PlayerInteractDistance, ref hit))
        {
            InteractorBase interact = hit.collider.GetComponent<InteractorBase>();
            if (interact != null)
                interact.TryInteract();
        }
    }
    void OnRotate(Vector2 delta)
    {
        CameraController.Instance.RotateCamera(delta*F_CameraSensitivity);
    }
    void OnMove(Vector2 delta)
    {
        delta.Normalize();
        m_Controller.Move(((CameraController.Instance.CameraXZForward*delta.y+CameraController.Instance.CameraXZRightward*delta.x)*F_MovementSpeed+Vector3.down*9.8f)*Time.deltaTime);
    }
}
