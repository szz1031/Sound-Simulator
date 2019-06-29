using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PostEffectManager : SingletonMono<PostEffectManager> {
    public bool B_TestMode=false;
    public static T AddPostEffect<T>() where T:PostEffectBase,new()
    {
        T effetBase = new T();
        effetBase.OnSetEffect(instance.cam_cur);
        instance.m_PostEffects.Add( effetBase);
        return effetBase;
    }
    public static T GetPostEffect<T>() where T : PostEffectBase, new()
    {
        return instance.m_PostEffects.Find(p => p.GetType() ==typeof(T)) as T;
    }
    public static void RemoveAllPostEffect()
    {
        instance.m_PostEffects.Traversal((PostEffectBase effect)=> { effect.OnDestroy(); });
        instance.m_PostEffects.Clear();
    }

    List<PostEffectBase> m_PostEffects=new List<PostEffectBase>();
    Camera cam_cur;
    protected override void Awake()
    {
        instance = this;
        cam_cur = GetComponent<Camera>();
    }
    RenderTexture tempTexture1, tempTexture2;
    protected void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        tempTexture1 = RenderTexture.GetTemporary(Screen.width, Screen.height, 0);
        Graphics.Blit(source, tempTexture1);
        for (int i = 0; i < m_PostEffects.Count; i++)
        {
            if (B_TestMode)
            {   
                m_PostEffects[i].OnRenderImage(tempTexture1, tempTexture1);
                continue;
            }

            tempTexture2 = RenderTexture.GetTemporary(Screen.width, Screen.height, 0);
            m_PostEffects[i].OnRenderImage(tempTexture1,tempTexture2);
            Graphics.Blit(tempTexture2, tempTexture1);
            RenderTexture.ReleaseTemporary(tempTexture2);
        }
        Graphics.Blit(tempTexture1,destination);
        RenderTexture.ReleaseTemporary(tempTexture1);
    }
}
