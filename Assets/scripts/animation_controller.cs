using JetBrains.Annotations;
using OpenAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animation_controller : MonoBehaviour
{
    Animator anim;
    public RuntimeAnimatorController sadController;
    public RuntimeAnimatorController dancingController;
    public RuntimeAnimatorController talkingController;
    //[SerializeField] public Whisper whisperInstance;
    //string userEmotion = Whisper.valueofSentiment;
    //float score = OpenAI.Whisper.DocumentSentiment.
    string userEmotion;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        // requestfromchatgpt = GetComponent<MakeRequest>();
        // userEmotion = requestfromchatgpt.emotionalState;
       //string userEmotion = whisperInstance.Findobje 
        //userEmotion = OpenAI.Whisper.DocumentSentiment.;
      
      //  print("The value I get from whisper: " + userEmotion);

       // print("The value in the animation controller " + userEmotion);
        //print(userEmotion);
    }

    // Update is called once per frame
    void Update()
    {
        float score = Whisper.score;
        print("The sentiment score: " + score);
        userEmotion = Whisper.valueofSentiment;        
        print("The value of "+ userEmotion);
        if (userEmotion == "happy")
        {
            //switch to sad animation
            anim.runtimeAnimatorController = dancingController as RuntimeAnimatorController;
        }

        else if (userEmotion == "sad")
        {
            //switch to sad animation
            anim.runtimeAnimatorController = sadController as RuntimeAnimatorController;
        }
        
        else
        {
            //switch to default animation
            anim.runtimeAnimatorController = talkingController as RuntimeAnimatorController;
        }
        
    }
}
