using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Text))]
public class UIT_Localization : MonoBehaviour
{
    public string KEY;
    private void Awake()
    {
        TLocalization.OnLocaleChanged += OnLocaleChanged;
    }
    private void Start()
    {
        OnLocaleChanged();
    }
    private void OnDestroy()
    {
        TLocalization.OnLocaleChanged -= OnLocaleChanged;
    }
    void OnLocaleChanged()
    {
        GetComponent<Text>().text = KEY.Localize(); 
    }
}