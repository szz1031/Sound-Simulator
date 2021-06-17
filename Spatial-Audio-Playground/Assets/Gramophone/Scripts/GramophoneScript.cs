using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GramophoneScript : MonoBehaviour {

    Animator anime;

    
    public AudioClip squeak1;
    public AudioClip squeak2;
    public AudioClip squeak3;
    public AudioClip onOffSound;
    public AudioClip needletap;
    public AudioClip vinyl;
    public AudioClip charge;
    public AudioClip mainSound;
    public AudioSource squeakSource1;
    public AudioSource squeakSource2;
    public AudioSource squeakSource3;
    public AudioSource OnOffSource;
    public AudioSource needleTapSource;
    public AudioSource vinylSource;
    public AudioSource chargeSource;
    public AudioSource mainSource;


    void Start ()
    {
        anime = GetComponent<Animator>();
    }
    private void Update()
    {
       /* if (Input.GetKeyDown(KeyCode.Return))  //Star animation by pressing "Return"/"Enter"        {
            StartGramophone();

        }
        if (Input.GetKeyDown(KeyCode.Space))    //Forced call to stop animation by pressing "Space"
        {
            StopGramophone();

        }*/

    }

    public void StartGramophone() //TO START GRAMOPHONE USE FUNCTION "StartGramophone()"
    {
        anime.SetTrigger("Start");
    }

    public void StopGramophone() //TO STOP GRAMOPHONE USE FUNCTION "StartGramophone()"
    {
        anime.SetTrigger("Stop");
        vinylSource.Stop();
        mainSource.Stop();

    }

    

    public void Squeak_1()
    {
        squeakSource1.PlayOneShot(squeak1); 
    }

    public void Squeak_2()
    {
        squeakSource2.PlayOneShot(squeak2);
    }

    public void Squeak_3()
    {
        squeakSource3.PlayOneShot(squeak3);
    }

    public void OnOffSound()
    {
        OnOffSource.PlayOneShot(onOffSound);
    }

    public void Needletap()
    {
        needleTapSource.PlayOneShot(needletap);
    }

    public void Vinyl()
    {
        vinylSource.PlayOneShot(vinyl);
    }

    public void Charge()
    {
        chargeSource.PlayOneShot(charge);
       
    }

    public void MainSound ()
    {
        mainSource.PlayOneShot(mainSound);
    }

}
