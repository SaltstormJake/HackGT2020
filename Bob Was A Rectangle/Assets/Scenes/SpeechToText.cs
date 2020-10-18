/**
* (C) Copyright IBM Corp. 2015, 2020.
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
#pragma warning disable 0649

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using IBM.Watson.SpeechToText.V1;
using IBM.Cloud.SDK;
using IBM.Cloud.SDK.Authentication;
using IBM.Cloud.SDK.Authentication.Iam;
using IBM.Cloud.SDK.Utilities;
using IBM.Cloud.SDK.DataTypes;

namespace WatsonIntegration
{
    public class SpeechToText : MonoBehaviour
    {
        #region PLEASE SET THESE VARIABLES IN THE INSPECTOR
        [Space(10)]
        [Tooltip("The service URL (optional). This defaults to \"https://stream.watsonplatform.net/speech-to-text/api\"")]
        [SerializeField]
        private string _serviceUrl;
        [Tooltip("Text field to display the results of streaming.")]
        public Text ResultsField;
        [Header("IAM Authentication")]
        [Tooltip("The IAM apikey.")]
        [SerializeField]
        private string _iamApikey;

        [SerializeField]
        ObjectRecognition objectRecognition;

        [SerializeField]
        ControlledGoalScript controlledGoal;

        [SerializeField]
        CandleController candleController;

        [Header("Parameters")]
        // https://www.ibm.com/watson/developercloud/speech-to-text/api/v1/curl.html?curl#get-model
        [Tooltip("The Model to use. This defaults to en-US_BroadbandModel")]
        [SerializeField]
        private string _recognizeModel;
        #endregion

        public delegate void ActionEvent(string actionName);
        public static event ActionEvent actionEvent;

        public delegate void GoalEvent(string goalName);
        public static event GoalEvent goalEvent;

        public delegate void LightEvent();
        public static event LightEvent lightEvent;

        public delegate void TextRecordedEvent(string text);
        public static event TextRecordedEvent textRecordedEvent;

        private int _recordingRoutine = 0;
        private string _microphoneID = null;
        private AudioClip _recording = null;
        private int _recordingBufferSize = 1;
        private int _recordingHZ = 22050;

        //private Stack<string> wordBuffer = new Stack<string>();
        private string recordedText = "";
        private int textTranscribeLengthLimit = 400;
        private string[] lastBlock = new string[0];
        private bool isProcessing = false;
        private readonly Dictionary<string, string[]> actionMappings = new Dictionary<string, string[]>()
        {
            {"LEFT" , new string[] {"LEFT", "WEST", "LEFTWARD", "LEFTWARDS", "WESTERLY", "WESTWARDS", "WESTWARD", "PORT", "PORTSIDE"}}, 
            {"RIGHT" , new string[] {"RIGHT", "EAST", "RIGHTWARDS", "RIGHTWARD", "EASTERLY", "EASTWARD", "EASTWARDS", "STARBOARD"}},
            {"UP" , new string[] {"UP", "UPWARDS", "UPWARD", "NORTH", "NORTHWARD", "NORTHWARDS", "FORWARD", "FORWARDS"}},
            {"DOWN" , new string[] {"DOWN", "DOWNWARD", "DOWNWARDS", "SOUTH", "SOUTHWARD", "SOUTHWARDS", "SOUTHERLY", "BACKWARDS", "STERN", "STERNWARD", "STERNWARDS"}},
            {"PULL" , new string[] {"PULL", "PUSH", "GRAB", "PAUL", "POLL", "YANK", "TUG", "HEAVE", "LUG"}},
            {"OPEN" , new string[] {"OPEN", "UNLOCK"}},
            {"LIGHT" , new string[] {"LIGHT", "LIGHTS", "WHITE", "BRIGHT", "CANDLES"}}
        };

        private SpeechToTextService _service;

        void Start()
        {
            LogSystem.InstallDefaultReactors();
            Runnable.Run(CreateService());

            if (objectRecognition != null) {
                actionEvent += objectRecognition.SetCommand;
            }
            if (controlledGoal != null) {
                goalEvent += controlledGoal.ProcessVoiceCommand;
            }
            if (candleController != null) {
                lightEvent += candleController.LightAll;
                DontDestroyOnLoad(candleController);
            }
        }

        private IEnumerator CreateService()
        {
            if (string.IsNullOrEmpty(_iamApikey))
            {
                throw new IBMException("Plesae provide IAM ApiKey for the service.");
            }

            IamAuthenticator authenticator = new IamAuthenticator(apikey: _iamApikey);

            //  Wait for tokendata
            while (!authenticator.CanAuthenticate())
                yield return null;

            _service = new SpeechToTextService(authenticator);
            if (!string.IsNullOrEmpty(_serviceUrl))
            {
                _service.SetServiceUrl(_serviceUrl);
            }
            _service.StreamMultipart = true;

            Active = true;
            StartRecording();
        }

        public bool Active
        {
            get { return _service.IsListening; }
            set
            {
                if (value && !_service.IsListening)
                {
                    _service.RecognizeModel = (string.IsNullOrEmpty(_recognizeModel) ? "en-US_BroadbandModel" : _recognizeModel);
                    _service.DetectSilence = true;
                    _service.EnableWordConfidence = true;
                    _service.EnableTimestamps = true;
                    _service.SilenceThreshold = 0.01f;
                    _service.MaxAlternatives = 1;
                    _service.EnableInterimResults = true;
                    _service.OnError = OnError;
                    _service.InactivityTimeout = -1;
                    _service.ProfanityFilter = false;
                    _service.SmartFormatting = true;
                    _service.SpeakerLabels = false;
                    _service.WordAlternativesThreshold = null;
                    _service.EndOfPhraseSilenceTime = null;
                    _service.StartListening(OnRecognize, OnRecognizeSpeaker);
                }
                else if (!value && _service.IsListening)
                {
                    _service.StopListening();
                }
            }
        }

        private void StartRecording()
        {
            if (_recordingRoutine == 0)
            {
                UnityObjectUtil.StartDestroyQueue();
                _recordingRoutine = Runnable.Run(RecordingHandler());
            }
        }

        private void StopRecording()
        {
            if (_recordingRoutine != 0)
            {
                Microphone.End(_microphoneID);
                Runnable.Stop(_recordingRoutine);
                _recordingRoutine = 0;
            }
        }

        private void OnError(string error)
        {
            Active = false;

            Log.Debug("ExampleStreaming.OnError()", "Error! {0}", error);
        }

        private IEnumerator RecordingHandler()
        {
            Log.Debug("ExampleStreaming.RecordingHandler()", "devices: {0}", Microphone.devices);
            _recording = Microphone.Start(_microphoneID, true, _recordingBufferSize, _recordingHZ);
            yield return null;      // let _recordingRoutine get set..

            if (_recording == null)
            {
                StopRecording();
                yield break;
            }

            bool bFirstBlock = true;
            int midPoint = _recording.samples / 2;
            float[] samples = null;

            while (_recordingRoutine != 0 && _recording != null)
            {
                int writePos = Microphone.GetPosition(_microphoneID);
                if (writePos > _recording.samples || !Microphone.IsRecording(_microphoneID))
                {
                    Log.Error("ExampleStreaming.RecordingHandler()", "Microphone disconnected.");

                    StopRecording();
                    yield break;
                }

                if ((bFirstBlock && writePos >= midPoint)
                  || (!bFirstBlock && writePos < midPoint))
                {
                    // front block is recorded, make a RecordClip and pass it onto our callback.
                    samples = new float[midPoint];
                    _recording.GetData(samples, bFirstBlock ? 0 : midPoint);

                    AudioData record = new AudioData();
                    record.MaxLevel = Mathf.Max(Mathf.Abs(Mathf.Min(samples)), Mathf.Max(samples));
                    record.Clip = AudioClip.Create("Recording", midPoint, _recording.channels, _recordingHZ, false);
                    record.Clip.SetData(samples, 0);

                    _service.OnListen(record);

                    bFirstBlock = !bFirstBlock;
                }
                else
                {
                    // calculate the number of samples remaining until we ready for a block of audio, 
                    // and wait that amount of time it will take to record.
                    int remaining = bFirstBlock ? (midPoint - writePos) : (_recording.samples - writePos);
                    float timeRemaining = (float)remaining / (float)_recordingHZ;

                    yield return new WaitForSeconds(timeRemaining);
                }
            }
            yield break;
        }

        private string lastTranscript = "";
        
        private void OnRecognize(SpeechRecognitionEvent result)
        {
            if (result != null && result.results.Length > 0)
            {
                foreach (var res in result.results)
                {
                    foreach (var alt in res.alternatives)
                    {
                        string text = string.Format("{0} ({1}, {2})", alt.transcript, res.final ? "Final" : "Interim", alt.confidence);
                        string toDisplay = "NO INSTRUC";
                        Log.Debug("ExampleStreaming.OnRecognize()", text);
                        string[] words = alt.transcript.Split(' ');
                        words = words.Take(words.Count() - 1).ToArray();

                        if (!res.final | alt.confidence < 0.4) {
                            continue;
                        }

                        recordedText += alt.transcript + ". ";

                        if (recordedText.Length > textTranscribeLengthLimit) {
                            if (textRecordedEvent != null) {
                                textRecordedEvent(recordedText);
                            }
                            recordedText = "";
                        }

                        foreach (string word in words) {
                            string wordUp = word.ToUpper();

                            if (wordUp == "BOB") {
                                isProcessing = true;
                                toDisplay = "BOB ACTIVATED";
                            }
                            else if (wordUp == "STOP") {
                                isProcessing = false;
                                toDisplay = "BOB DEACTIVATED";
                                if (actionEvent != null) {
                                    actionEvent("STOP");
                                }
                            }
                            else if (isProcessing) {
                                foreach (KeyValuePair<string, string[]> kvp in actionMappings) {
                                    if (wordUp != null && kvp.Value.Contains(wordUp)) {
                                        if (actionEvent != null && kvp.Key == "OPEN" || wordUp == "PULL") {
                                            actionEvent(kvp.Key);
                                        }
                                        if (goalEvent != null && kvp.Key == "UP" || wordUp == "LEFT" || wordUp == "DOWN" || wordUp == "RIGHT")
                                        {
                                            goalEvent(kvp.Key);
                                        }
                                        if (lightEvent != null && kvp.Key == "LIGHT")
                                        {
                                            lightEvent();
                                        }
                                        toDisplay = kvp.Key;
                                    }
                                }
                            }
                        }
                        
                        lastBlock = words;
                        ResultsField.text = toDisplay;
                    }

                    if (res.keywords_result != null && res.keywords_result.keyword != null)
                    {
                        foreach (var keyword in res.keywords_result.keyword)
                        {
                            Log.Debug("ExampleStreaming.OnRecognize()", "keyword: {0}, confidence: {1}, start time: {2}, end time: {3}", keyword.normalized_text, keyword.confidence, keyword.start_time, keyword.end_time);
                        }
                    }

                    if (res.word_alternatives != null)
                    {
                        foreach (var wordAlternative in res.word_alternatives)
                        {
                            Log.Debug("ExampleStreaming.OnRecognize()", "Word alternatives found. Start time: {0} | EndTime: {1}", wordAlternative.start_time, wordAlternative.end_time);
                            foreach (var alternative in wordAlternative.alternatives)
                                Log.Debug("ExampleStreaming.OnRecognize()", "\t word: {0} | confidence: {1}", alternative.word, alternative.confidence);
                        }
                    }
                }
            }
        }

        private void OnRecognizeSpeaker(SpeakerRecognitionEvent result)
        {
            if (result != null)
            {
                foreach (SpeakerLabelsResult labelResult in result.speaker_labels)
                {
                    Log.Debug("ExampleStreaming.OnRecognizeSpeaker()", string.Format("speaker result: {0} | confidence: {3} | from: {1} | to: {2}", labelResult.speaker, labelResult.from, labelResult.to, labelResult.confidence));
                }
            }
        }

        //credits to https://stackoverflow.com/questions/6944056/c-sharp-compare-string-similarity
        public static int GetStringDifference(string s, string t)
        {
            if (string.IsNullOrEmpty(s) & string.IsNullOrEmpty(t))
            {
                return 0;
            }
            else if (string.IsNullOrEmpty(s))
            {
                throw new System.ArgumentNullException(t, "String Cannot Be Null Or Empty");
            }
            else if (string.IsNullOrEmpty(t))
            {
                throw new System.ArgumentNullException(t, "String Cannot Be Null Or Empty");
            }

            int n = s.Length; // length of s
            int m = t.Length; // length of t

            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            int[] p = new int[n + 1]; //'previous' cost array, horizontally
            int[] d = new int[n + 1]; // cost array, horizontally

            // indexes into strings s and t
            int i; // iterates through s
            int j; // iterates through t

            for (i = 0; i <= n; i++)
            {
                p[i] = i;
            }

            for (j = 1; j <= m; j++)
            {
                char tJ = t[j - 1]; // jth character of t
                d[0] = j;

                for (i = 1; i <= n; i++)
                {
                    int cost = s[i - 1] == tJ ? 0 : 1; // cost
                    // minimum of cell to the left+1, to the top+1, diagonally left and up +cost                
                    d[i] = System.Math.Min(System.Math.Min(d[i - 1] + 1, p[i] + 1), p[i - 1] + cost);
                }

                // copy current distance counts to 'previous row' distance counts
                int[] dPlaceholder = p; //placeholder to assist in swapping p and d
                p = d;
                d = dPlaceholder;
            }

            // our last action in the above loop was to switch d and p, so p now 
            // actually has the most recent cost counts
            return p[n];
        }
    }
}
