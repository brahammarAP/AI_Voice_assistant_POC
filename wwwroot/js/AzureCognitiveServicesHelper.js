
let player = null;

window.Interop = {
    TextToSpeechSDK: async function (textToSpeechString, language, key, region) {
        "use strict";

        try {
            if (typeof SpeechSDK !== 'undefined') {
                const speechConfig = SpeechSDK.SpeechConfig.fromSubscription(key, region);
                speechConfig.speechSynthesisVoiceName = language;

                player = new SpeechSDK.SpeakerAudioDestination();
                const audioConfig = SpeechSDK.AudioConfig.fromSpeakerOutput(player);
                const synthesizer = new SpeechSDK.SpeechSynthesizer(speechConfig, audioConfig);

                synthesizer.speakTextAsync(textToSpeechString,
                    function (result) {
                        if (result.reason === SpeechSDK.ResultReason.SynthesizingAudioCompleted) {
                            console.log("Synthesis completed for: " + textToSpeechString);
                        } else if (result.reason === SpeechSDK.ResultReason.Canceled) {
                            console.log("Synthesis failed. Error details:" + result.errorDetails);
                        }
                        synthesizer.close();
                    },
                    function (err) {
                        console.error("Error: " + err);
                        synthesizer.close();
                    });
            }

            if (!!window.SpeechSDK) {
                SpeechSDK = window.SpeechSDK;
                console.log("TextToSpeechSDK is loaded");
            }
        } catch (error) {
            console.error("Error creating Speech SDK object:", error);
        }
    },

    StopPlay: function () {
        if (player !== null) {
            try {
                player.mute();

            } catch (eroor) {
                console.error("Error in stopping speech:", error);
            }
        }
    },

    SpeechToTextSDK: async function (key, region) {
        "use strict";

        console.log("createSpeechSDK function invoked");
        try {
            if (typeof SpeechSDK !== 'undefined') {
                const speechConfig = SpeechSDK.SpeechConfig.fromSubscription(key, region);
                speechConfig.speechRecognitionLanguage = "sv-SE";
                const audioConfig = SpeechSDK.AudioConfig.fromDefaultMicrophoneInput();
                const recognizer = new SpeechSDK.SpeechRecognizer(speechConfig, audioConfig);

                const recognitionPromise = new Promise((resolve, reject) => {
                    recognizer.recognizeOnceAsync(

                        function (result) {
                            resolve(result.text);
                            return result.text;
                            recognizer.close();
                        },
                        function (err) {
                            console.error("Error during recognition:", err);
                            reject(err);
                            recognizer.close();
                        }
                    );
                });

                const recognitionResult = await recognitionPromise;

                return recognitionResult;
            } else {
                console.error('SpeechToTextSDK is not defined. Make sure the script is loaded.');
                return null;
            }
            if (!!window.SpeechSDK) {
                SpeechSDK = window.SpeechSDK;
                console.log("SpeechToTextSDK is loaded");
            }
        } catch (error) {
            console.error("Error creating Speech SDK object from javascript:", error);
            return null;
        }
    }
};