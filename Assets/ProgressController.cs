using Assets;
using Assets.DataPersistence;
using Assets.Util;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProgressController : MonoBehaviour, IDataPersistence
{
    private SerializableDictionary<string, UserProgressData> _dictionaryProgressBySign;

    [SerializeField]
    private UserPreferences _userPreferences;

    [SerializeField]
    private GameObject slatePrefab;

    private GameObject _previosInstantiated;

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
            UserProgressData auxProgressData = null;
            foreach (var code in evaluationResponse.Expression.Codes)
            {
                if (_dictionaryProgressBySign.TryGetValue(code.WholeCode, out auxProgressData))
                {
                    auxProgressData.totalTries = auxProgressData.totalTries + 1;
                    auxProgressData.correctResponses = evaluationResponse.IsCorrect ? auxProgressData.correctResponses + 1 : auxProgressData.correctResponses;
                    _dictionaryProgressBySign[code.WholeCode] = auxProgressData;
                }
                else
                {
                    auxProgressData = new UserProgressData();
                    auxProgressData.totalTries = 1;
                    auxProgressData.correctResponses = evaluationResponse.IsCorrect ? 1 : 0;
                    auxProgressData.signCode = code.WholeCode;
                    _dictionaryProgressBySign.Add(code.WholeCode, auxProgressData);
                }
            }
        }
    }

    public void OnProgressSlateSpawned(int moduleNumber)
    {
         var expressionsByModule = ModuleDataManager.Instance.GetExpressionsByModule(moduleNumber);
        var signProgressByModule = _dictionaryProgressBySign.Values.Where(ps => expressionsByModule.Any(e => e.WholeCode.Equals(ps.signCode))).ToList();
        expressionsByModule = expressionsByModule.Where(e => signProgressByModule.Any(sp => sp.signCode.Equals(e.WholeCode))).ToList();
        var expressionGroupedByCategory = expressionsByModule.GroupBy(e => e.CategoryCode).ToList();
        var categoriesProgress = expressionGroupedByCategory.Select(c => {
            var categoryProgress = new UserCategoryProgress();
            categoryProgress.categoryName = c.Key;
            categoryProgress.expressionsProgress = c.Select(exp => {
                var expressionProgress = signProgressByModule.FirstOrDefault(sp => sp.signCode.Equals(exp.WholeCode));
                return new UserExpresssionProgress { word = exp.Expression, totalCorrectResponses = expressionProgress.correctResponses, totalResponses = expressionProgress.totalTries };
            }).ToList();
            return categoryProgress;
        }).ToList();

        var instantiatedPrefab = Instantiate(slatePrefab);
        instantiatedPrefab.SetActive(false);
        var progressScript = instantiatedPrefab.GetComponentInChildren<CategoryProgressCollection>();
        if (progressScript != null)
        {
            progressScript.CategoriesProgress = categoriesProgress.ToList();
        }

        instantiatedPrefab.SetActive(true);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
