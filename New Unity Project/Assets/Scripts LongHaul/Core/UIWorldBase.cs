using UnityEngine;
using UnityEngine.UI;

public class UIWorldBase : MonoBehaviour,ISingleCoroutine
{
    public bool b_2D = true;
    protected Transform tf_Container;
    protected RectTransform rtf_Canvas;
    public Transform TF_Container
    {
        get
        {
            return tf_Container;
        }
    }

    public virtual void Init(bool useAnim)
    {
        if (rtf_Canvas)
            return;
        rtf_Canvas = transform.Find("Canvas").GetComponent<RectTransform>();
        tf_Container = rtf_Canvas.transform.Find("Container");
        if (useAnim)
            this.StartSingleCoroutine(0,TIEnumerators.ChangeValueTo((float value) => { tf_Container.localScale=Vector3.one*value; }, 0, 1, .5f));
    }
    protected void Update()
    {
        if(b_2D)
            transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane( transform.position- Camera.main.transform.position, Camera.main.transform.right), Camera.main.transform.up);
        else
            transform.LookAt(CameraController.MainCamera.transform);
    }
    protected void Hide()
    {
        Destroy(this.gameObject);
    }

    protected virtual void OnDestroy()
    {
        this.StopAllSingleCoroutines();
    }
}
