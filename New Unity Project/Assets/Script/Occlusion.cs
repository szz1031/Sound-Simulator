using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSetting;


public class Occlusion : MonoBehaviour
{
    Vector3 SourcePosition;
    public Transform Camera;
    public float DisToListener;
    public float CalculateUnder;
    public float BlockedDistance;
    public float HightDistance;
    public float BlockedPercentage;
    
    public float FinalOcclu;
    float obs;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SourcePosition = transform.position;
        RaycastHit HitInfo1, HitInfo2;
        Vector3 RayDirection1 = Camera.position - SourcePosition;
        Vector3 RayDirection2 = SourcePosition - Camera.position;
        


        DisToListener = Vector3.Distance(SourcePosition, Camera.position);

        CalculateUnder = AkSoundEngine.GetMaxRadius(this.gameObject);
        

        if (DisToListener <= CalculateUnder)
        {
            Physics.Raycast(SourcePosition, RayDirection1, out HitInfo1, DisToListener, GameSetting.GameLayer.I_SoundCastAll);
            Physics.Raycast(Camera.position, RayDirection2, out HitInfo2, DisToListener, GameSetting.GameLayer.I_SoundCastAll);

            BlockedDistance = DisToListener - HitInfo1.distance - HitInfo2.distance;

            HightDistance = Mathf.Abs(transform.position.y - Camera.position.y);

            if (BlockedDistance <= 0.5)
            {
                BlockedPercentage = BlockedDistance / DisToListener;
              //  AkSoundEngine.SetObjectObstructionAndOcclusion(this.gameObject, Camera.gameObject, 0.0f, BlockedPercentage);
                Debug.DrawRay(SourcePosition, RayDirection1, Color.blue);
            }

            else if (BlockedDistance <=2)
            {
                BlockedPercentage = (BlockedDistance - 0.5f)/DisToListener;
              //  AkSoundEngine.SetObjectObstructionAndOcclusion(this.gameObject, Camera.gameObject, 0.0f, BlockedPercentage);
                Debug.DrawRay(SourcePosition, RayDirection1, Color.red);
            }
            else if (BlockedDistance <= 4)
            {
                BlockedPercentage = (1.25f * BlockedDistance - 1.0f)/DisToListener;
             //   AkSoundEngine.SetObjectObstructionAndOcclusion(this.gameObject, Camera.gameObject, 0.0f, BlockedPercentage);
                Debug.DrawRay(SourcePosition, RayDirection1, Color.red);
            } 
            else 
            {
                BlockedPercentage = BlockedDistance / DisToListener ;
               // AkSoundEngine.SetObjectObstructionAndOcclusion(this.gameObject, Camera.gameObject, 0.0f, BlockedPercentage);
                Debug.DrawRay(SourcePosition, RayDirection1, Color.yellow);
            }
        
            if (HightDistance >=1.5 && HightDistance <= 3.5)
            {
                BlockedPercentage = BlockedPercentage + 0.2f * HightDistance - 0.3f;              
            }

            if (HightDistance > 3.5)
            {
                BlockedPercentage = BlockedPercentage + 0.4f;                
            }

            if (BlockedPercentage > 1) { BlockedPercentage = 1; }

            AkSoundEngine.SetObjectObstructionAndOcclusion(this.gameObject, Camera.gameObject, 0.0f, BlockedPercentage);
            AkSoundEngine.GetObjectObstructionAndOcclusion(this.gameObject, Camera.gameObject, out obs, out FinalOcclu);
        }


    }
}
