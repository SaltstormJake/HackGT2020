/**
* Copyright 2019 IBM Corp. All Rights Reserved.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*      http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*
*/

using IBM.Watson.ToneAnalyzer.V3;
using IBM.Watson.ToneAnalyzer.V3.Model;
using IBM.Cloud.SDK.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IBM.Cloud.SDK;
using IBM.Cloud.SDK.Authentication;
using IBM.Cloud.SDK.Authentication.Iam;

namespace WatsonIntegration
{
    public class ToneAnalyzer : MonoBehaviour
    {
        #region PLEASE SET THESE VARIABLES IN THE INSPECTOR
        [Space(10)]
        [Tooltip("The IAM apikey.")]
        [SerializeField]
        private string iamApikey;
        [Tooltip("The service URL (optional). This defaults to \"https://gateway.watsonplatform.net/assistant/api\"")]
        [SerializeField]
        private string serviceUrl;
        [Tooltip("The version date with which you would like to use the service in the form YYYY-MM-DD.")]
        [SerializeField]
        private string versionDate;
        #endregion

        private ToneAnalyzerService service;
        private string stringToTestTone = @"This is Team Rubber Ducks, with Jake, Kauri, Kevin, and Jimmy. 
        We are building a game using IBM's Watson API to help connect older and younger family members during COVID times.
        The tone analyzer will convert every sentence of recorded audio into a CSV entry with each row corresponding to a tone.
        Here are some example sentences:
        Tears are running down my cheeks as I send my last texts to my family saying I love them.
        The teacher starts the lecture, and the student turns his laptop on.
        The colored lights on his RGB Backlit keyboard flare to life like a nuclear flash, and a deep humming fills my ears and shakes my very soul.
        The entire city power grid goes dark. 
        The classroom begins to shake as the massive fans begin to spin. 
        In mere seconds my world has gone from vibrant life, to a dark, earth shattering void where my body is getting torn apart by the 150mph gale force winds and the 500 decibel groan of the cooling fans. 
        As my body finally surrenders, I weep, as my school and my city go under. 
        I fucking hate gaming laptops.
        Yay, the lemons have arrived!
        I love lemons, they are good, they bring me joy.
        Oh my god what is happening.
        Grandpa put the kettle back where it belongs.
        No grandpa, don't even think about it.";
        private bool toneTested = false;
        private bool toneChatTested = false;

        private static readonly Dictionary<string, int> toneMappings = new Dictionary<string, int>() {
            {"anger", 0},
            {"fear", 1},
            {"sadness", 2},
            {"joy", 3},
            {"analytical", 4},
            {"confident", 5},
            {"tentative", 6}
        };

        private void Start()
        {
            LogSystem.InstallDefaultReactors();
            Runnable.Run(CreateService());
        }

        private IEnumerator CreateService()
        {
            if (string.IsNullOrEmpty(iamApikey))
            {
                throw new IBMException("Plesae provide IAM ApiKey for the service.");
            }

            //  Create credential and instantiate service
            IamAuthenticator authenticator = new IamAuthenticator(apikey: iamApikey);

            //  Wait for tokendata
            while (!authenticator.CanAuthenticate())
                yield return null;

            service = new ToneAnalyzerService(versionDate, authenticator);
            if (!string.IsNullOrEmpty(serviceUrl))
            {
                service.SetServiceUrl(serviceUrl);
            }

            Runnable.Run(Examples());
        }

        private IEnumerator Examples()
        {
            ToneInput toneInput = new ToneInput()
            {
                Text = stringToTestTone
            };

            List<string> tones = new List<string>()
            {
                "emotion",
                "language"
            };
            service.Tone(callback: OnTone, toneInput: toneInput, sentences: true, tones: tones, contentLanguage: "en", acceptLanguage: "en", contentType: "application/json");

            while (!toneTested)
            {
                yield return null;
            }

            Log.Debug("ExampleToneAnalyzerV3.Examples()", "Examples complete!");
        }

        private void OnTone(DetailedResponse<IBM.Watson.ToneAnalyzer.V3.Model.ToneAnalysis> response, IBMError error)
        {
            if (error != null)
            {
                Log.Debug("ExampleToneAnalyzerV3.OnTone()", "Error: {0}: {1}", error.StatusCode, error.ErrorMessage);
            }
            else
            {
                Log.Debug("ExampleToneAnalyzerV3.OnTone()", "{0}", response.Response);
            }

            List<SentenceAnalysis> sentenceTones = response.Result.SentencesTone;

            foreach (SentenceAnalysis sentenceTone in sentenceTones) {
                sentenceToCSV(sentenceTone);
            }

            toneTested = true;
        }

        private static string sentenceToCSV(SentenceAnalysis sentence) {
            string csvResult = sentence.Text;
            double[] tones = new double[7] {
                -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0
            };

            foreach (ToneScore ts in sentence.Tones) {
                tones[toneMappings[ts.ToneId]] = (double) ts.Score;
            }

            foreach (double score in tones) {
                csvResult += string.Format(",{0}", score);
            }

            Log.Debug("CSV Result: ", "{0}", csvResult);

            return csvResult;
        }

        public void getTones(string input) {
            ToneInput toneInput = new ToneInput()
            {
                Text = input
            };
            List<string> tones = new List<string>() {
                "emotion",
                "language",
                "social"
            };

            service.Tone(callback: OnTone, toneInput: toneInput, sentences: true, tones: tones, contentLanguage: "en", acceptLanguage: "en", contentType: "application/json");
        }
    }
}
