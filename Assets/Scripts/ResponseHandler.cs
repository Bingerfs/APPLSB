using Assets.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace LSB
{
    public class ResponseHandler : MonoBehaviour
    {

        private string REQUEST_TIME_OUT = "Request timeout";
        [Serializable] public class ResultHandler : UnityEvent<IEnumerable<Expression>> { }
        public ResultHandler OnResult;

        [Serializable] public class ErrorHandler : UnityEvent<string> { }
        public ErrorHandler OnError;

        public void OnResponse(UnityWebRequest request, string word)
        {
            try {
                ExpressionListReqResponse expressionList = JsonUtility.FromJson<ExpressionListReqResponse>(request.downloadHandler.text);
                var expressions = expressionList.ToDomainObject();
                if (request.responseCode == 200 && OnResult != null)
                    OnResult.Invoke(expressions);
                if (request.responseCode != 200 && OnError != null)
                { 
                    if(request.error== REQUEST_TIME_OUT)
                    {
                        Debug.LogError("Hubo un problema con la conexión, inténtalo más tarde");
                        //Toast.Instance.Show("Hubo un problema con la conexión, inténtalo más tarde", 3f, Toast.ToastColor.Red);
                    }
                    else
                    {
                        OnError.Invoke(word);
                    }
                }
            }
            catch (Exception)
            {
                Debug.LogError("Hubo un problema con el servidor, inténtalo más tarde.");
                //Toast.Instance.Show("Hubo un problema con el servidor, inténtalo más tarde", 3f,Toast.ToastColor.Red);
            }
        }
    }
}
