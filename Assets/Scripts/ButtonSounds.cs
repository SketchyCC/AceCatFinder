using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;
using Cinemachine;
using BehaviorDesigner.Runtime;
using UnityEngine.Events;



public class ButtonSounds : MonoBehaviour  //Spoiler: You can only hear sound when debugging (: AT LEAT YOU'RE NOT DEAF
{
    int fakerandom = 0;
    int yourHelplessnes;

    public AudioClip meow1;
    public AudioClip meow2;
    public AudioClip meow3;
    public AudioClip meow4;
    public AudioClip soundeffect1;
    public AudioClip soundeffect2;

    public AudioClip ambient1;
    public AudioClip ambient2;

    public AudioSource Buttonmeowsource;
    public AudioSource audioeffects;

    void Start()
    {
        fakerandom = 0;
    }
    void Update()
    {
    }

    public virtual void playMeow()
    {



        if (fakerandom == 0)
        {
            Buttonmeowsource.PlayOneShot(meow1, 0.3F);
            Invoke("Delay", 0.3f);
        }
        if (fakerandom == 1)
        {
            Buttonmeowsource.PlayOneShot(meow2, 0.5F);
            Invoke("Delay", 0.3f);
        }
        if (fakerandom == 2)
        {
            Buttonmeowsource.PlayOneShot(meow3, 0.5F);
            Invoke("Delay", 0.3f);
        }


        fakerandom++;
        if (fakerandom > 2)
        {
            fakerandom = 0;
        }

        if (Buttonmeowsource.isPlaying)
        {
            
        }

    }

    public virtual void PlaySoundEffects()
    {
        if (fakerandom == 0) 
        {
            Buttonmeowsource.clip = ambient1;
            Buttonmeowsource.volume = 0.2f;
            Buttonmeowsource.Play();
            
        }
        if (fakerandom == 1)
        {
            audioeffects.PlayOneShot(meow3, 0.5F);      //purrrrrrrrrr       
           
 
        }
        if (fakerandom == 2)
        {
            audioeffects.Stop();
            audioeffects.PlayOneShot(meow2, 0.5F);      //buwwuwwa
        }
        if (fakerandom == 3)
        {
                                                        //buwwuwwa
        }
        if (fakerandom == 4)
        {
            audioeffects.Stop();
        }
        if (fakerandom == 5)
        {
            Buttonmeowsource.PlayOneShot(meow1, 2F);    //paper ktschhrt
            
        }

        if (fakerandom == 6)                            //park ambient
        {
            Buttonmeowsource.clip = ambient2;
            Buttonmeowsource.volume = 0.2f;
            Buttonmeowsource.Play();
        }
        if (fakerandom == 7)
        {
            Buttonmeowsource.PlayOneShot(meow4, 2F);    // meow
        }
        if (fakerandom == 8)
        {
            Buttonmeowsource.PlayOneShot(meow4, 2F);
            Invoke("Delay", 1f);
            Buttonmeowsource.PlayOneShot(meow4, 2F);
        }
        if (fakerandom == 9)
        {
            Buttonmeowsource.PlayOneShot(soundeffect1, 0.5F);
                                                                  //tack
        }
        if (fakerandom == 10)
        {
            Buttonmeowsource.PlayOneShot(soundeffect2, 0.5F);
                                                               //buffff
        }


        fakerandom++;
    }



    public void Delay()
    {
        //it just doesn't behave like in the other scriot AND IT'S THE SAME LINE.
        yourHelplessnes++;
    }

    public void playjustonesound()
    {
        Buttonmeowsource.PlayOneShot(meow1, 0.3F);
        //doesn't work
    }

}
