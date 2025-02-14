using DotNetEnv;
using Microsoft.CognitiveServices.Speech;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech.Audio;

void OutputSpeechSynthesisResult(SpeechSynthesisResult speechSynthesisResult, string text)
{
    switch (speechSynthesisResult.Reason)
    {
        case ResultReason.SynthesizingAudioCompleted:
            Console.WriteLine($"Speech synthesized for text: [{text}]");
            break;
        case ResultReason.Canceled:
            var cancellation = SpeechSynthesisCancellationDetails.FromResult(speechSynthesisResult);
            Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

            if (cancellation.Reason == CancellationReason.Error)
            {
                Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                Console.WriteLine($"CANCELED: ErrorDetails=[{cancellation.ErrorDetails}]");
                Console.WriteLine($"CANCELED: Did you set the speech resource key and region values?");
            }
            break;
        default:
            break;
    }
}

// This example requires environment variables named "SPEECH_KEY" and "SPEECH_REGION"
// Please put your .env path here
Env.Load(@"C:\Users\MSI\source\repos\GaleriaVoces\.env");

string? key = Environment.GetEnvironmentVariable("SPEECH_KEY");
string? region = Environment.GetEnvironmentVariable("SPEECH_REGION");
var speechConfig = SpeechConfig.FromSubscription(key, region);

// The neural multilingual voice can speak different languages based on the input text.
// You can find other voices going to Speech Studio Portal --> Voice Gallery and...
// when you have found the correct voice, go to "Sample Code" and see the variable "config.SpeechSynthesisVoiceName"
speechConfig.SpeechSynthesisVoiceName = "en-US-AvaMultilingualNeural";

using (var speechSynthesizer = new SpeechSynthesizer(speechConfig))
{
    // Get text from the console and synthesize to the default speaker.
    Console.WriteLine("Enter some text that you want to speak >");
    string? text = Console.ReadLine();
    if (text != null)
    {
        var speechSynthesisResult = await speechSynthesizer.SpeakTextAsync(text);
        OutputSpeechSynthesisResult(speechSynthesisResult, text);
    }
    else
    {
        Console.WriteLine("No input was provided.");
    }
}

Console.WriteLine("Press any key to exit...");
Console.ReadKey();







