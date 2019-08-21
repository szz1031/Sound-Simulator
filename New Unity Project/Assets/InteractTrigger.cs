using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTrigger : InteractorBase {
    public GameObject TriggerObject;
    public string EventName;
    public string Subtitle;
    public float TriggerDuration;
    public int TriggerTimes;
    int i_count = 0;
    float triggerCheck = 0f;
    private void OnTriggerEnter(Collider other)
    {
        if (i_count>=TriggerTimes||triggerCheck > 0)
            return;

        triggerCheck = TriggerDuration;
        i_count++;
        AudioManager.PostEvent(EventName ,TriggerObject);
        if (Subtitle != "")
            UIManager.Instance.AddSubtitle(Subtitle);
    }
    private void Update()
    {
        if (triggerCheck > 0)
            triggerCheck -= Time.deltaTime;
    }
}
