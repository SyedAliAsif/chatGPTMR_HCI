# ChatGPT_MR  
An MR implementation of the HCI Lab @ UD's Medical Examination through Chat GPT project

# Contact Info
For any and all questions, please contact me at asifrabi@udel.edu

# Demo Video
[![Video Title](https://img.youtube.com/vi/saMoOezbxRw/0.jpg)](https://www.youtube.com/watch?v=saMoOezbxRw)

# Demo Video with Emotionally Intelligent Virtual Buddy
[![Video Title](https://img.youtube.com/vi/_-bvOt2c2CA/0.jpg)](https://www.youtube.com/watch?v=_-bvOt2c2CA)

# Installation Video
[![Video Title](https://img.youtube.com/vi/CR11_s1qHyg/0.jpg)](https://www.youtube.com/watch?v=CR11_s1qHyg)

# How to use Chat GPT API
The following instructions are credited to user **srcnalt**

Saving Your Credentials
To make requests to the OpenAI API, you need to use your API key and organization name (if applicable). To avoid exposing your API key in your Unity project, you can save it in your device's local storage.

To do this, follow these steps:

* Create a folder called .openai in your home directory (e.g. C:User\UserName\ for Windows or ~\ for Linux or Mac)
* Create a file called auth.json in the .openai folder
* Add an api_key field and a organization field (if applicable) to the auth.json file and save it  
Here is an example of what your auth.json file should look like:
```
{
    "api_key": "sk-...W6yi",
    "organization": "org-...L7W"
}
```

# for text to speech
For making the text to speech to work you need to create your own Azure subscription and use your key [![Video Title](https://img.youtube.com/vi/YWfGZvstlWw/0.jpg)] (https://www.youtube.com/watch?v=YWfGZvstlWw)

# For Performing Sentiment Analysis using Google Cloud Language API
You can go to -> \Assets\Samples\OpenAI Unity\0.1.14\Whisper.cs file to check for the API key and user your own to perform the sentiment analysis
To resolve any dependency issue in runtime, you can use NuGet Manager in Unity from here - (https://github.com/GlitchEnzo/NuGetForUnity.git?path=/src/NuGetForUnity)

# Known Issues
Please visit the issues tab to add/review known issues

# Other
Special credit for the intitial codebase: Gavin Caulfield

#Team Information
Syed Ali Asif - asifrabi@udel.edu
Mohammad Samer Abdul Munim Al Ratrout - mratrout@udel.edu
Behdokht Kiafar - kiafar@udel.edu
Emma Cao - caoemma@udel.edu
Zolfa Saadat - zsaadat@udel.edu
