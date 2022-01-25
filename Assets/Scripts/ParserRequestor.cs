using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

namespace LSB
{
    public class ParserRequestor : MonoBehaviour
    {
        private static readonly string API_URL = "https://lsbapi.herokuapp.com"; 
        [Serializable] public class ResultHandler : UnityEvent<UnityWebRequest, string> { }
        public ResultHandler OnResult;

        public InputField inputField;

        public GameObject connectionStatusImage;
        public GameObject mainTextToolTip;
        public GameObject progressIndicator;

        [SerializeField]
        public TextMeshPro mainText;

        //public Text mainText;

        private void Start()
        {
            OnRequest("Hola, como estas vamos a comer");
        }

        public void OnRequest(string word)
        {
            inputField.text = "";
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
            if (OnResult != null)
            {

                //connectionStatusImage.SetActive(false);
                progressIndicator.SetActive(false);
                mainTextToolTip.SetActive(true);
                mainText.text = "";
                OnResult.Invoke(request, word);
            }
        }

        private void setConnectionStatusImage()
        {
            if (hasConnectionProblems())
            {
                connectionStatusImage.SetActive(true);
                mainTextToolTip.SetActive(false);
                progressIndicator.SetActive(false);
            }
            else
            {
                connectionStatusImage.SetActive(false);
                mainTextToolTip.SetActive(true);
                progressIndicator.SetActive(true);
            }
        }

        private LSBRequest createRequest(string word)
        {
            LSBRequest request = new LSBRequest();
            request.word = word;
            return request;
        }

        private bool hasConnectionProblems()
        {
            return Application.internetReachability == NetworkReachability.NotReachable;
        }
    }
}
