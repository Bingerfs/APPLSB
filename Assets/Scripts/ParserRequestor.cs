using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using Microsoft.MixedReality.Toolkit.UI;

namespace LSB
{
    public class ParserRequestor : MonoBehaviour
    {
        private static readonly string API_URL = "https://lsbapi.herokuapp.com"; 
        [Serializable] public class ResultHandler : UnityEvent<UnityWebRequest, string> { }
        public ResultHandler OnResult;
        [SerializeField]
        public GameObject mainTextToolTip;
        public GameObject progressIndicator;


        private ToolTip toolTip;

        private IProgressIndicator progressIndicatorRotatingOrbs;

        //public Text mainText;
        private void OnEnable()
        {
            progressIndicatorRotatingOrbs = progressIndicator.GetComponent<IProgressIndicator>();
            toolTip = mainTextToolTip.GetComponent<ToolTip>();
        }

        private void Start()
        {
            //OnRequest("mi casa");
        }

        public void OnRequest(string word)
        {
            word = word.Trim().TrimEnd('.');
            HandleProgressIndicator(progressIndicatorRotatingOrbs);
            StartCoroutine(Request(word));
        }

        private IEnumerator Request(string word)
        {
            setConnectionStatusImage();
            UnityWebRequest request = new UnityWebRequest(API_URL, "POST");
            request.timeout = 10;
            byte[] bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(createRequest(word)));

            request.uploadHandler = new UploadHandlerRaw(bodyRaw);

            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            HandleProgressIndicator(progressIndicatorRotatingOrbs);
            while (progressIndicatorRotatingOrbs.State != ProgressIndicatorState.Closed)
            {
                yield return null;
            }
            if (OnResult != null)
            {
                OnResult.Invoke(request, word);
            }
        }

        private void setConnectionStatusImage()
        {
            if (HasConnectionProblems())
            {
                mainTextToolTip.SetActive(false);
            }
            else
            {
                toolTip.ToolTipText = "";
                mainTextToolTip.SetActive(false);
            }
        }

        private LSBRequest createRequest(string word)
        {
            LSBRequest request = new LSBRequest();
            request.word = word;
            return request;
        }

        private bool HasConnectionProblems()
        {
            return Application.internetReachability == NetworkReachability.NotReachable;
        }

        private async void HandleProgressIndicator(IProgressIndicator indicator)
        {
            await indicator.AwaitTransitionAsync();

            switch (indicator.State)
            {
                case ProgressIndicatorState.Closed:
                    OpenProgressIndicator(indicator);
                    break;
                case ProgressIndicatorState.Open:
                    await indicator.CloseAsync();
                    break;
            }
        }

        private async void OpenProgressIndicator(IProgressIndicator indicator)
        {
            await indicator.OpenAsync();
        }
    }
}
