using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostEffectBase  {
    const string S_ParentPath = "Hidden/PostEffect/";
    Camera cam_Cur;
    public Material mat_Cur { get; private set; }
    Shader sd_Cur;
    bool b_supported;
    public PostEffectBase()
    {
        b_supported = true;
        sd_Cur = Shader.Find(S_ParentPath+this.GetType().ToString());
        if (sd_Cur == null)
        {
            Debug.LogError("Shader:" + S_ParentPath + this.GetType().ToString()+" Not Found");
            b_supported = false;
        }
        else if (!sd_Cur.isSupported)
        {
            Debug.LogError("Shader:" + S_ParentPath + this.GetType().ToString() + " Is Not Supported");
            b_supported = false;
        }
        mat_Cur = new Material(sd_Cur)
        {
            hideFlags = HideFlags.DontSave
        };
    }
    protected bool RenderDefaultImage(RenderTexture source, RenderTexture destination)
    {
        if (!B_Supported)
        {
            Graphics.Blit(source, destination);
            return true;
        }
        return false;
    }
    public virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (RenderDefaultImage(source, destination)) 
            return;
        Graphics.Blit(source, destination, Mat_Cur);
    }
    public virtual void OnSetEffect(Camera cam)
    {
        cam_Cur = cam;
    }
    public virtual void OnDestroy()
    {
        GameObject.Destroy(mat_Cur);
    }
    #region Get
    protected Material Mat_Cur
    {
        get
        {
            return mat_Cur;
        }
    }
    protected Shader Sd_Cur
    {
        get
        {
            return sd_Cur;
        }
    }
    public bool B_Supported
    {
        get
        {
            return b_supported;
        }
    }
    public Camera Cam_Cur
    {
        get
        {
            return cam_Cur;
        }
    }
    #endregion
}
public class PE_ViewNormal : PostEffectBase
{
    public override void OnSetEffect(Camera cam)
    {
        base.OnSetEffect(cam);
        cam.depthTextureMode |= DepthTextureMode.DepthNormals;
    }
}
public class PE_ViewDepth : PostEffectBase
{
    public override void OnSetEffect(Camera cam)
    {
        base.OnSetEffect(cam);
        cam.depthTextureMode |= DepthTextureMode.Depth;
    }
}
public class PE_BSC : PostEffectBase {      //Brightness Saturation Contrast
    public override void OnSetEffect(Camera _cam)
    {
        base.OnSetEffect(_cam);
        SetEffect();
    }
    public void SetEffect(float _brightness = 1f, float _saturation = 1f, float _contrast = 1f)
    {
        Mat_Cur.SetFloat("_Brightness", _brightness);
        Mat_Cur.SetFloat("_Saturation", _saturation);
        Mat_Cur.SetFloat("_Contrast", _contrast);
    }
}
public class PE_EdgeDetection : PostEffectBase  //Edge Detection Easiest
{
    public void SetCamera(Camera _cam)
    {
        base.OnSetEffect(_cam);
        SetEffect(Color.green);
    }
    public void SetEffect(Color _edgeColor,float _showEdge = 1f )
    {
        Mat_Cur.SetColor("_EdgeColor", _edgeColor);
        Mat_Cur.SetFloat("_ShowEdge", _showEdge);
    }
}
public class PE_GaussianBlur : PostEffectBase       //Gassuain Blur
{
    float F_BlurSpread = 2f;
    int I_Iterations = 5;
    int I_DownSample = 4;
    public override void OnSetEffect(Camera cam)
    {
        base.OnSetEffect(cam);
        SetEffect();
    }
    public void SetEffect(float _blurSpread=2f, int _iterations=5, int _downSample = 4)
    {
        F_BlurSpread = _blurSpread;
        I_Iterations = _iterations;
        I_DownSample = _downSample;
    }
    public override void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (RenderDefaultImage(source,destination))
            return;


        if (I_DownSample == 0)
            I_DownSample = 1;

        int rtW = source.width >> I_DownSample;
        int rtH = source.height >> I_DownSample;

        RenderTexture buffer0 = RenderTexture.GetTemporary(rtW, rtH, 0);
        buffer0.filterMode = FilterMode.Bilinear;

        Graphics.Blit(source, buffer0);
        for (int i = 0; i < I_Iterations; i++)
        {
            Mat_Cur.SetFloat("_BlurSpread", 1 + i * F_BlurSpread);

            RenderTexture buffer1;

            buffer1 = RenderTexture.GetTemporary(rtW, rtH, 0);
            Graphics.Blit(buffer0, buffer1, Mat_Cur, 0);
            RenderTexture.ReleaseTemporary(buffer0);
            buffer0 = buffer1;

            buffer1 = RenderTexture.GetTemporary(rtW,rtH,0);
            Graphics.Blit(buffer0, buffer1, Mat_Cur, 1);
            RenderTexture.ReleaseTemporary(buffer0);
            buffer0 = buffer1;
        }
        Graphics.Blit(buffer0, destination);
        RenderTexture.ReleaseTemporary(buffer0);
    }
}
public class PE_Bloom : PostEffectBase
{
    float F_BlurSpread = 2f;
    int I_Iterations = 3;
    int I_DownSample = 3;
    float F_LuminanceThreshold = .85f;
    float F_LuminanceMultiple = 10;
    public override void OnSetEffect(Camera cam)
    {
        base.OnSetEffect(cam);
        SetEffect();
    }
    public void SetEffect(float _blurSpread = 2f, int _iterations = 5, int _downSample = 4,float _luminanceThreshold=.85f,float _luminanceMultiple=10)
    {
        F_BlurSpread = _blurSpread;
        I_Iterations = _iterations;
        I_DownSample = _downSample;
        F_LuminanceThreshold = _luminanceThreshold;
        F_LuminanceMultiple = _luminanceMultiple;
    }
    public override void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (RenderDefaultImage(source, destination))
            return;

        if (I_DownSample == 0)
            I_DownSample = 1;
        Mat_Cur.SetFloat("_LuminanceThreshold", F_LuminanceThreshold);
        Mat_Cur.SetFloat("_LuminanceMultiple", F_LuminanceMultiple);
        int rtW = source.width >> I_DownSample;
        int rtH = source.height >> I_DownSample;

        RenderTexture buffer0 = RenderTexture.GetTemporary(rtW, rtH, 0);
        buffer0.filterMode = FilterMode.Bilinear;
        Graphics.Blit(source, buffer0,Mat_Cur,0);
        for (int i = 0; i < I_Iterations; i++)
        {
            Mat_Cur.SetFloat("BlurSize", 1f+i*F_BlurSpread);
            RenderTexture buffer1;
            buffer1 = RenderTexture.GetTemporary(rtW, rtH, 0);
            Graphics.Blit(buffer0, buffer1, Mat_Cur, 1);
            RenderTexture.ReleaseTemporary(buffer0);
            buffer0 = buffer1;

            buffer1 = RenderTexture.GetTemporary(rtW, rtH, 0);
            Graphics.Blit(buffer0, buffer1, Mat_Cur, 2);
            RenderTexture.ReleaseTemporary(buffer0);
            buffer0 = buffer1;
        }

        Mat_Cur.SetTexture("_Bloom", buffer0);
        RenderTexture.ReleaseTemporary(buffer0);
        Graphics.Blit(source, destination, Mat_Cur, 3);
    }
}
public class PE_MotionBlur : PostEffectBase     //Camera Motion Blur ,Easiest
{
    private RenderTexture rt_Accumulation;
    public override void OnSetEffect(Camera cam)
    {
        base.OnSetEffect(cam);
        SetEffect();
    }
    public void SetEffect(float _BlurSize=1f)
    {
        Mat_Cur.SetFloat("_BlurAmount", 1 - _BlurSize);
    }
    public override void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (rt_Accumulation == null || rt_Accumulation.width != source.width || rt_Accumulation.height != source.height)
        {
            GameObject.Destroy(rt_Accumulation);
            rt_Accumulation = new RenderTexture(source.width,source.height,0);
            rt_Accumulation.hideFlags = HideFlags.DontSave;
            Graphics.Blit(source, rt_Accumulation);
       }

        rt_Accumulation.MarkRestoreExpected();

        Graphics.Blit(source, rt_Accumulation, Mat_Cur);
        Graphics.Blit(rt_Accumulation, destination);
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
        if(rt_Accumulation!=null)
        GameObject.Destroy(rt_Accumulation);
    }
}
public class PE_MotionBlurDepth:PE_MotionBlur
{
    private Matrix4x4 mt_CurVP;
    public override void OnSetEffect(Camera cam)
    {
        base.OnSetEffect(cam);
        cam.depthTextureMode |= DepthTextureMode.Depth;
        mt_CurVP = Cam_Cur.projectionMatrix * Cam_Cur.worldToCameraMatrix;
    }
    public override void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        base.OnRenderImage(source, destination);
        Mat_Cur.SetMatrix("_PreviousVPMatrix", mt_CurVP);
        mt_CurVP = Cam_Cur.projectionMatrix * Cam_Cur.worldToCameraMatrix;
        Mat_Cur.SetMatrix("_CurrentVPMatrixInverse", mt_CurVP.inverse);
        Graphics.Blit(source,destination,Mat_Cur);
    }
}
public class PE_FogDepth : PostEffectBase
{
    Transform tra_Cam;
    public override void OnSetEffect(Camera cam)
    {
        base.OnSetEffect(cam);
        cam.depthTextureMode |= DepthTextureMode.Depth;
        tra_Cam = Cam_Cur.transform;
        SetEffect(TCommon.ColorAlpha( Color.white,.5f));
    }
    public PE_FogDepth SetEffect(Color _fogColor,  float _fogDensity = .5f, float _fogYStart = -1f, float _fogYEnd = 5f)
    {
        Mat_Cur.SetFloat("_FogDensity", _fogDensity);
        Mat_Cur.SetColor("_FogColor", _fogColor);
        Mat_Cur.SetFloat("_FogStart", _fogYStart);
        Mat_Cur.SetFloat("_FogEnd", _fogYEnd);
        return this;
    }
    public void SetTexture(Texture noise)
    {
        Mat_Cur.SetTexture("_NoiseTex", noise);
    }
    public override void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (RenderDefaultImage(source,destination))
            return;

        float fov = Cam_Cur.fieldOfView;
        float near = Cam_Cur.nearClipPlane;
        float far = Cam_Cur.farClipPlane;
        float aspect = Cam_Cur.aspect;

        float halfHeight = near * Mathf.Tan(fov * .5f * Mathf.Deg2Rad) ;
        Vector3 toRight = tra_Cam.right * halfHeight * aspect;
        Vector3 toTop = tra_Cam.up * halfHeight ;

        Vector3 topLeft = tra_Cam.forward * near + toTop - toRight;
        float scale = topLeft.magnitude / near;
        topLeft.Normalize();
        topLeft *= scale;

        Vector3 topRight = tra_Cam.forward * near + toTop + toRight;
        topRight.Normalize();
        topRight *= scale;

        Vector3 bottomLeft = tra_Cam.forward * near - toTop - toRight;
        bottomLeft.Normalize();
        bottomLeft *= scale;
        Vector3 bottomRight = tra_Cam.forward * near - toTop + toRight;
        bottomRight.Normalize();
        bottomRight *= scale;

        Matrix4x4 frustumCornersRay = Matrix4x4.identity;
        frustumCornersRay.SetRow(0, bottomLeft);
        frustumCornersRay.SetRow(1, bottomRight);
        frustumCornersRay.SetRow(2, topLeft);
        frustumCornersRay.SetRow(3, topRight);

        Mat_Cur.SetMatrix("_FrustumCornersRay", frustumCornersRay);
        Mat_Cur.SetMatrix("_VPMatrixInverse", (Cam_Cur.projectionMatrix * Cam_Cur.worldToCameraMatrix).inverse);
        Graphics.Blit(source,destination,Mat_Cur);
    }
}
public class PE_EdgeDetectionDepth:PE_EdgeDetection
{
    public override void OnSetEffect(Camera cam)
    {
        base.OnSetEffect(cam);
        cam.depthTextureMode |= DepthTextureMode.DepthNormals;
        SetEffect();
    }
    public void SetEffect(float _sampleDistance = 1f, float _sensitivityDepth = 1f, float _sensitivityNormal = 1f)
    {
        Mat_Cur.SetFloat("_SampleDistance", _sampleDistance);
        Mat_Cur.SetFloat("_SensitivityDepth", _sensitivityDepth);
        Mat_Cur.SetFloat("_SensitivityNormals", _sensitivityNormal);
    }
}
public class PE_FogDepthNoise : PE_FogDepth
{
    public Texture TX_Noise;
    public float F_FogSpeedX=.02f;
    public float F_FogSpeedY=.02f;
    public float F_NoiseAmount=.8f;
    public override void OnSetEffect(Camera cam)
    {
        base.OnSetEffect(cam);
        Mat_Cur.SetTexture("_NoiseTex", TX_Noise);
        Mat_Cur.SetFloat("_FogSpeedX", F_FogSpeedX);
        Mat_Cur.SetFloat("_FogSpeedY", F_FogSpeedY);
        Mat_Cur.SetFloat("_NoiseAmount", F_NoiseAmount);
    }
    public override void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        base.OnRenderImage(source, destination);
    }
}
public class PE_BloomSpecific : PostEffectBase //Need To Bind Shader To Specific Items
{
    Camera m_RenderCamera;
    RenderTexture m_RenderTexture;
    Shader m_RenderShader;
    public PE_GaussianBlur m_GaussianBlur { get; private set; }
    public override void OnSetEffect(Camera cam)
    {
        base.OnSetEffect(cam);
        m_RenderShader = Shader.Find("Hidden/PostEffect/PE_BloomSpecific_Render");
        if (m_RenderShader == null)
            Debug.LogError("Null Shader Found!");
        m_GaussianBlur = new PE_GaussianBlur();
        GameObject temp = new GameObject("Render Camera");
        temp.transform.SetParent(Cam_Cur.transform);
        temp.transform.localPosition = Vector3.zero;
        m_RenderCamera = temp.AddComponent<Camera>();
        m_RenderCamera.clearFlags = CameraClearFlags.SolidColor;
        m_RenderCamera.backgroundColor = Color.black;
        m_RenderCamera.orthographic = Cam_Cur.orthographic;
        m_RenderCamera.orthographicSize = Cam_Cur.orthographicSize;
        m_RenderCamera.nearClipPlane = Cam_Cur.nearClipPlane;
        m_RenderCamera.farClipPlane = Cam_Cur.farClipPlane;
        m_RenderCamera.fieldOfView = Cam_Cur.fieldOfView;
        m_RenderCamera.enabled = false;
        m_RenderTexture = new RenderTexture(Screen.width, Screen.height, 1);
        m_RenderCamera.targetTexture = m_RenderTexture;
        SetEffect();
    }
    public void SetEffect(float _blueSpread=1f, int _iterations = 10, int _downSample=2)
    {
        m_GaussianBlur.SetEffect(_blueSpread, _iterations, _downSample);
    }
    public override void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (RenderDefaultImage(source, destination))
            return;
        m_RenderCamera.RenderWithShader(m_RenderShader, "RenderType");
        m_GaussianBlur.OnRenderImage(m_RenderTexture, m_RenderTexture);     //Blur
        Mat_Cur.SetTexture("_RenderTex", m_RenderTexture);
        Graphics.Blit(source, destination, Mat_Cur, 1);        //Mix
    }
}