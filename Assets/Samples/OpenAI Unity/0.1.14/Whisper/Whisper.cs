using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;

using Google.Cloud.Language.V1;

using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

using System.Collections;
using UnityEngine.Networking;
using JetBrains.Annotations;

[Serializable]
public class Document
{
    public string type;
    public string content;
}

[Serializable]
public class SentimentRequest
{
    public Document document;
    public string encodingType;
}
namespace OpenAI
{
    public class Whisper : MonoBehaviour
    {
        public TextMeshProUGUI input;
        public MakeRequest makereq;
        public TextMeshProUGUI safe;
        public GameObject Avatar;
        public int microphone;
        public TMPro.TextMeshProUGUI emojiText; // display emoji
        public static string valueofSentiment;

        private readonly string fileName = "output.wav";
        private readonly int duration = 5;
        
        private AudioClip clip;
        private bool isRecording;
        private float time;
        private OpenAIApi openai = new OpenAIApi();

        public static float score = 0.4f;



        /*
         * Starts Recording and listening for user voice input
         * 
         * @param: None
         * 
         * @return: None
         *
         */
        public void StartRecording()
        {
            Avatar.GetComponent<AudioSource>().Stop(); //Stops for interrupt
            input.text = "Listening...";
            isRecording = true;

#if !UNITY_WEBGL

            clip = Microphone.Start(Microphone.devices[microphone], false, duration, 44100);
            #endif
        }
        // hang's code

        // Copilot call Google Cloud Language for sentiment analysis
        public class Response
        {
            public DocumentSentiment documentSentiment { get; set; }
        }

        public class DocumentSentiment
        {
            public float score { get; set; }
        }

        // Function to call sentiment analysis API
        IEnumerator AnalyzeSentiment(string text)
        {
            var apiKey = "AIz---------------------------------------8HU"; //your API key

            Debug.Log($"Analyzing sentiment for text: {text}");

            var request = new SentimentRequest
            {
                document = new Document
                {
                    type = "PLAIN_TEXT",
                    content = text
                },
                encodingType = "UTF8"
            };

            var json = JsonUtility.ToJson(request);
            Debug.Log($"The json {json}");

            var www = new UnityWebRequest($"https://language.googleapis.com/v1/documents:analyzeSentiment?key={apiKey}", "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                Debug.Log(www.downloadHandler.text); // Add this line
            }
            else
            {
                Debug.Log("Sucess Google Cloud API call.");
                var responseJson = www.downloadHandler.text;
                Debug.Log($"responseJson is {responseJson}.");
                Response responseObject = JsonConvert.DeserializeObject<Response>(responseJson);
                score = responseObject.documentSentiment.score;

                Debug.Log(score);
                MapSentimentToEmoji(score);
                // Map sentiment score to emoji
                // string emoji = MapSentimentToEmoji(score);
                // emojiText.text = emoji;
            }


        }

        void MapSentimentToEmoji(float score)
        {
            // if (score <= -0.8) return "😡";
            // else if (score <= -0.6) return "😠";
            // else if (score <= -0.4) return "😤";
            // else if (score <= -0.2) return "😒";
            // else if (score <= 0) return "😔";
            // else if (score <= 0.2) return "😕";
            // else if (score <= 0.4) return "😐";
            // else if (score <= 0.6) return "🙂";
            // else if (score <= 0.8) return "😀";
            // else return "😃";
            /*
            if (score <= -0.8) return "Extremely Angry";
            else if (score <= -0.6) return "Very Angry";
            else if (score <= -0.4) return "Angry";
            else if (score <= -0.2) return "Confused";
            else if (score <= 0) return "Disappointed";
            else if (score <= 0.2) return "Sad";
            else if (score <= 0.4) return "Speechless";
            else if (score <= 0.6) return "Happy";
            else if (score <= 0.8) return "Very Happy";
            else return "Extremely Happy";
            */
            if (score >= -1 & score < 0)
            {
                valueofSentiment = "sad";
                Debug.Log(valueofSentiment);
                print("The value in whisper: " + valueofSentiment);
            }
            else if (score > 0)
            {
                valueofSentiment = "happy";
                Debug.Log(valueofSentiment);
                print("The value in whisper: " + valueofSentiment);
            }
            else
            {
                valueofSentiment = "neutral";
                Debug.Log(valueofSentiment);
                print("The value in whisper: " + valueofSentiment);
            }
        }
        // Copilot call Google Cloud Language for sentiment analysis


        //end of the code
        /*
         * Ends recording and begins GPT translation
         * 
         * @param: None
         * 
         * @return: None
         * 
         */
        private async void EndRecording()
        {
            input.text = "Transcripting...";
            #if !UNITY_WEBGL
            Microphone.End(null);
            #endif
            
            byte[] data = SaveWav.Save(fileName, clip);
            
            var req = new CreateAudioTranscriptionsRequest
            {
                FileData = new FileData() {Data = data, Name = "audio.wav"},
                Model = "whisper-1", //OpenAI STT
                Language = "en"
            };
            var res = await openai.CreateAudioTranscription(req);

            input.text = "User: "+ res.Text;
            Debug.Log($"User: {res.Text}");
            safe.text = res.Text;
            makereq.Request();

            // Copilot - sentiment analysis
            // Analyze sentiment of res.Text
            await AnalyzeSentiment(res.Text);
            // Copilot - sentiment analysis
        }

        private void Update()
        {
            
            if (isRecording)
            {
                time += Time.deltaTime;
                
                if (time >= duration)
                {
                    time = 0;
                    isRecording = false;
                    EndRecording();
                }
            }
        }
    }
}
