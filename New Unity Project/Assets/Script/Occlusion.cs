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
    public float OcclusionPercentage;
    
 //   public float FinalOcclu;
 //   float obs;

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

            if (HitInfo2.distance < 1 && DisToListener - HitInfo1.distance >= 1)
            {
                BlockedDistance = DisToListener - HitInfo1.distance - 1;
            }
            else
            {
                BlockedDistance = DisToListener - HitInfo1.distance - HitInfo2.distance;
            }

            HightDistance = Mathf.Abs(transform.position.y - Camera.position.y);

            if (BlockedDistance <= 0.5)
            {
                OcclusionPercentage = BlockedDistance / DisToListener;
                Debug.DrawRay(SourcePosition, RayDirection1, Color.green);
            }

            else if (BlockedDistance <=2)
            {
                OcclusionPercentage = (BlockedDistance - 0.5f)/DisToListener;
                Debug.DrawRay(SourcePosition, RayDirection1, Color.yellow);
            }
            else if (BlockedDistance <= 4)
            {
                OcclusionPercentage = (1.25f * BlockedDistance - 1.0f)/DisToListener;
                Debug.DrawRay(SourcePosition, RayDirection1, Color.cyan);
            } 
            else 
            {
                OcclusionPercentage = BlockedDistance / DisToListener ;
                Debug.DrawRay(SourcePosition, RayDirection1, Color.red);
            }
        
            if (HightDistance >=1.5 && HightDistance <= 3.5)
            {
                OcclusionPercentage = OcclusionPercentage + 0.2f * HightDistance - 0.3f;              
            }

            if (HightDistance > 3.5)
            {
                OcclusionPercentage = OcclusionPercentage + 0.4f;                
            }


            if (OcclusionPercentage > 1) { OcclusionPercentage = 1; }
            if (OcclusionPercentage < 0) { OcclusionPercentage = 0; }


            AkSoundEngine.SetObjectObstructionAndOcclusion(this.gameObject, Camera.gameObject, 0.0f, OcclusionPercentage);
//            AkSoundEngine.GetObjectObstructionAndOcclusion(this.gameObject, Camera.gameObject, out obs, out FinalOcclu);
        }


    }
}
