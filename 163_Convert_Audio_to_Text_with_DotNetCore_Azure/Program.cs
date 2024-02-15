using Microsoft.CognitiveServices.Speech;

namespace SpeechConversion
{
    internal class Program
    {
        public static void Main()
        {
            RecognizeSpeechAsync().Wait();
        }

        private static async Task RecognizeSpeechAsync()
        {
            // Replace with your own subscription key and service region.
            const string speechKey = "--your subscription key--";
            const string serviceRegion = "centralus";

            // Create an instance of a speech config with specified subscription key and service region.
            var config = SpeechConfig.FromSubscription(speechKey, serviceRegion);

            // Create a speech recognizer.
            using var recognizer = new SpeechRecognizer(config);
            Console.WriteLine("Say something...");

            // Start speech recognition and await a result.
            var result = await recognizer.RecognizeOnceAsync();

            switch (result.Reason)
            {
                // Check the result
                case ResultReason.RecognizedSpeech:
                    Console.WriteLine($"Recognized: {result.Text}");
                    break;

                case ResultReason.NoMatch:
                    Console.WriteLine("No speech could be recognized.");
                    break;

                case ResultReason.Canceled:
                {
                    var cancellation = CancellationDetails.FromResult(result);
                    Console.WriteLine($"Speech Recognition canceled. Reason: {cancellation.Reason}");

                    if (cancellation.Reason == CancellationReason.Error)
                    {
                        Console.WriteLine($"Error details: {cancellation.ErrorDetails}");
                    }

                    break;
                }
            }
        }
    }
}