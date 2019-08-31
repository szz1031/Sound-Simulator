using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSetting;
public class PlayerController : MonoBehaviour {
    Transform tf_Head;
    CharacterController m_Controller;
    public float F_CameraSensitivity=.5f;
    public float F_MovementSpeed=3;
    bool b_crouching = false;
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
        PCInputManager.Instance.AddBinding<PlayerController>(enum_BindingsName.Switch,GameManager.Instance.SwitchSearchMode);
        PCInputManager.Instance.AddBinding<PlayerController>(enum_BindingsName.Crouch, (bool onCrouch) => { b_crouching = onCrouch; });
    }
    RaycastHit hit = new RaycastHit();
    void TryInterach()
    {
        if (GameManager.Instance.B_SearchMode)
            return;

        if (Physics.SphereCast(CameraController.Instance.MainCamera.transform.position, .1f, CameraController.Instance.MainCamera.transform.forward,out hit,GameConst.I_PlayerInteractDistance,enum_HitCheckType.Dynamic.ToCastLayer() ))
        {
            HitCheckDynamic interact = hit.collider.GetComponent<HitCheckDynamic>();
            if (interact != null)
                interact.OnTryInteract();
        }
    }
    void OnRotate(Vector2 delta)
    {
        CameraController.Instance.RotateCamera(delta*F_CameraSensitivity);
    }
    float f_stepCheck;
    bool b_stepSwap;
    void OnMove(Vector2 delta)
    {
        tf_Head.localPosition = new Vector3(0, Mathf.Lerp(tf_Head.localPosition.y,  !b_crouching?1.7f:1f,Time.deltaTime*5),0);
        delta.Normalize();
        m_Controller.Move(((CameraController.Instance.CameraXZForward*delta.y+CameraController.Instance.CameraXZRightward*delta.x)* (b_crouching?.3f:1f)*F_MovementSpeed + Vector3.down*9.8f)*Time.deltaTime);
        f_stepCheck += Time.deltaTime * Mathf.Abs(delta.magnitude);
        if (f_stepCheck > .5f)
        {
            f_stepCheck -= .5f;
            FPSCameraController.Instance.OnSprintAnimation(b_stepSwap ? 5 : -5);
            b_stepSwap = !b_stepSwap;
            AudioManager.PlayFootStep(GroundMaterial, gameObject);
        }
    }
    enum_GroundMaterialType GroundMaterial => Physics.Raycast(tf_Head.position, Vector3.down, out hit, 2f, enum_HitCheckType.Static.ToCastLayer()) ? hit.collider.GetComponent<HitCheckStatic>().E_Mateiral : enum_GroundMaterialType.Invalid;
}
