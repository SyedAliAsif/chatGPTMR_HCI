using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI;
using TMPro;
using System;
using System.Threading;
using System.Linq;
using System.Globalization;

public class MakeRequest : MonoBehaviour
{
    public TextMeshProUGUI input;
    public TextMeshProUGUI output;
    public GameObject speak;
    public GameObject avatar;
    public Tracking tracker;
    private bool requestCompleted;
   [HideInInspector] public string emotionalState;

    /*
     * Makes the request to chatGPT, grabs ChatGPT's reponse, and plays it (if necessary)
     * 
     * @param: None
     * 
     * @return: None
     * 
     */
    
     /*
     public class CreateChatCompletionResponse
    {
        [JsonProperty("system_fingerprint")]
        public string? SystemFingerprint { get; set; }

    }
    */
    public void Request()
    {
        if (input.text == "")
        {
            output.text = "Sorry, but I didn't seem to get an input. Please try again";
            if (avatar.activeSelf) { speak.GetComponent<UIManager>().SpeechPlayback(); }
        }
        else { 
            var openai = new OpenAIApi();
            try
            {
                requestCompleted = false;
                var req = new CreateChatCompletionRequest
                {
                    //Ideal model, but can be tweaked with for future models
                    Model = "gpt-3.5-turbo",
                    //MaxTokens = 64,
                    MaxTokens = 2048,
                    Messages = new List<ChatMessage>
                        {
                            new ChatMessage()
                            {
                                Role = "user", //user
                                //input.text = User Question
                               // Content = $"{input.text}; use only 2048 tokens for api",
                                //Content = $"{input.text}; You are a supportive therapist. After each user input, respond in a caring manner, and then classify their response into one of these three categories: sad, happy, or angry. Put your classification in  at the end of your response, like this: sad", //your goal is to provide empathetic responses.Start the conversation by asking the user how they are feeling or what's on theor mind. after each user input respond in a caring manner.",
                                // Content = $"{input.text}; use only 2048 tokens for api",
                                Content = $"{input.text}; You are a supportive therapist. After each user input, respond in a caring manner.", //your goal is to provide empathetic responses.Start the conversation by asking the user how they are feeling or what's on theor mind. after each user input respond in a caring manner.",

                            }
                        },
                    Temperature = 0.2f,
                };

                //sending the 2nd request to classify the response
                /*
                var second_req = new CreateChatCompletionRequest
                {
                    //Ideal model, but can be tweaked with for future models
                    Model = "gpt-3.5-turbo",
                    //MaxTokens = 64,
                    MaxTokens = 64,
                    Messages = new List<ChatMessage>
                        {
                            new ChatMessage()
                            {
                                Role = "user",
                                //input.text = User Question
                               // Content = $"{input.text}; use only 64 tokens for api",
                                Content = $"{output.text}; what category does your answer fall on: sad, angry or happy? only use one word ",


                            }
                        },
                    Temperature = 0.2f,
                };

                */

                openai.CreateChatCompletionAsync(req,
                    (responses) =>
                    {
                        var result = string.Join("", responses.Select(response => response.Choices[0].Delta.Content));
                        output.text = result;
                       // print("The result of the chatgpt response " +result);
                        // print(result.GetTypeCode);
                        //Console.WriteLine(result.typeOf());
                        //emotionalState = ClassifyEmotion(result);
                        //emotionalState = "sad";
                        //print("the type for emotional state: " + emotionalState);
                    },
                    () =>
                    {
                        if (!requestCompleted) // Check if the request has already been completed, prevents double API requests
                            {
                            requestCompleted = true;
                                // Increment interactions and set the flag
                                tracker.numInteractions += 1;

                                // Add to GPT responses and questions
                                tracker.GPTResponses.Add(output.text);
                            tracker.GPTQuestions.Add(input.text);
                           // string result2 = output.text;
                           // emotionalState = ClassifyEmotion(result2);
                            //emotionalState = "sad";
                            //print("the type for emotional state: " + emotionalState);
                            // If avatar is active, play speech
                            if (avatar.activeSelf) { speak.GetComponent<UIManager>().SpeechPlayback(); }
                        }
                    },
                    new CancellationTokenSource()


                );



            }
            catch (Exception ex)
            {
                Debug.LogError("An error occurred during the API request: " + ex.Message);
            }
        }
    }
    public string ClassifyEmotion(string text)
    {
        if (text.Contains("happy"))
        {
            return "happy";
        }
        else if (text.Contains("sorry"))
        {
            return "sad";
        }
        else
        {
            print("it comes here all the time");
            return "neutral";
        }

    }


}
