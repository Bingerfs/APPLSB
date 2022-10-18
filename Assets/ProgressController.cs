using Assets;
using Assets.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressController : MonoBehaviour, IDataPersistence
{
    private SerializableDictionary<string, int> _dictionaryProgressBySign;

    public void LoadUserData(UserData userData)
    {
        _dictionaryProgressBySign = userData.progress;
    }

    public void SaveUserData(ref UserData userData)
    {
        userData.progress = _dictionaryProgressBySign;
    }

    public void OnSaveProgressBySign(IEnumerable<EvaluationResponse> evaluationResponses)
    {
        foreach (var evaluationResponse in evaluationResponses)
        {
            int auxInteger = 0;
            foreach (var code in evaluationResponse.Expression.Codes)
            {
                if (_dictionaryProgressBySign.TryGetValue(code.WholeCode, out auxInteger))
                {
                    _dictionaryProgressBySign[code.WholeCode] = evaluationResponse.IsCorrect ? auxInteger - 1 : auxInteger + 1;
                }
                else
                {
                    _dictionaryProgressBySign.Add(code.WholeCode, evaluationResponse.IsCorrect? 1: 0);
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
