using Assets;
using Assets.DataPersistence;
using Assets.Util;
using LSB;
using Microsoft.MixedReality.Toolkit.UI;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EvaluationController : MonoBehaviour
{
    public int SelectedModule { get; set; } = 1;

    public int NumberOfSigns { get; set; } = 5;

    [SerializeField]
    private GameObject _mediumDialogPrefab;

    [Serializable] public class ResultHandler : UnityEvent<IEnumerable<Expression>> { }
    public ResultHandler OnResult;

    [Serializable] public class ExperienceHandler : UnityEvent<float> { }
    public ExperienceHandler OnEvaluationExperience;

    [Serializable] public class ProgressHandler : UnityEvent<IEnumerable<EvaluationResponse>> { }
    public ProgressHandler OnEvaluationProgress;

    [SerializeField]
    private GameObject _responseFeedbackCorrect;

    public GameObject ResponseFeedbackCorrect { get => _responseFeedbackCorrect; set => _responseFeedbackCorrect = value; }

    [SerializeField]
    private GameObject _responseFeedbackIncorrect;

    public GameObject ResponseFeedbackIncorrect { get => _responseFeedbackIncorrect; set => _responseFeedbackIncorrect = value; }

    [SerializeField]
    private GameObject _moduleSelectionMenu;

    public GameObject ModuleSelectionMenu { get => _moduleSelectionMenu; set => _moduleSelectionMenu = value; }

    [SerializeField]
    private bool _hasSetResponseBeenActivated = false;

    /// <summary>
    /// Shows white trim around edge of tooltip.
    /// </summary>
    public bool HasSetResponseBeenActivated { get => _hasSetResponseBeenActivated; set => _hasSetResponseBeenActivated = value; }

    [SerializeField]
    private bool _hasEvaluationBeenActivated = false;

    /// <summary>
    /// Shows white trim around edge of tooltip.
    /// </summary>
    public bool HasEvaluationBeenActivated { get => _hasEvaluationBeenActivated; set => _hasEvaluationBeenActivated = value; }

    private IEnumerable<string> _signCodes = new List<string>();

    private IEnumerable<string> RandomisedSignCodes
    {
        get
        {
            var moduleName = $"Módulo {SelectedModule}";
            var listOfBaseCodes = _modulesDictionary[moduleName];
            var filteredSignCodes = _signCodes.Where(code => listOfBaseCodes.Any(baseCode => code.Contains(baseCode)));
            System.Random rnd = new System.Random();
            return filteredSignCodes.OrderBy(code => rnd.Next()).Take(NumberOfSigns);
        }
    }

    public List<EvaluationResponse> EvaluationResponses { get; set; }

    private EvaluationResponse CurrentSignEvaluated { get; set; }

    private Dictionary<string, IEnumerable<string>> _modulesDictionary = new Dictionary<string, IEnumerable<string>>(); 


    [SerializeField]
    private AnimatorCommander _animatorCommander;

    [SerializeField]
    private DictationManager _dictationManager;

    void Start()
    {
        //TextAsset readCodes = Resources.Load<TextAsset>("Codes");
        //char[] delimiters = new char[] { '\r', '\n' };
        //var listOfCodes = readCodes.text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
        //foreach (string code in listOfCodes)
        //{
        //    _signCodes = _signCodes.Append(code);
        //}

        //var readData = Resources.Load<TextAsset>(Path.Combine("DataBase", "config", "data"));
        //var listOfModules = JsonConvert.DeserializeObject<List<Module>>(readData.text);
        //foreach (var module in listOfModules)
        //{
        //    _modulesDictionary.Add(module.Name, GetAllModuleCategories(module));
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (HasEvaluationBeenActivated)
        {
            HasEvaluationBeenActivated = false;
            var expressionsData = ModuleDataManager.Instance.GetExpressionsByModule(SelectedModule);
            EvaluationResponses = RandomiseExpressionDataList(expressionsData).Select(expression =>
            {
                var expressionEv = new Expression(expression.WholeCode, expression.Expression);
                return new EvaluationResponse(expressionEv);
            }).ToList();
            StartCoroutine(Evaluate());
        }

        if (HasSetResponseBeenActivated)
        {
            OnEvaluationResponse(CurrentSignEvaluated.Expression.Word);
        }
    }

    private List<ExpressionData> RandomiseExpressionDataList(List<ExpressionData> moduleExpressionsData)
    {
        System.Random rnd = new System.Random();
        return moduleExpressionsData.OrderBy(code => rnd.Next()).Take(NumberOfSigns).ToList();
    }

    private IEnumerator Evaluate()
    {
        if (null != EvaluationResponses && EvaluationResponses.Any())
        {
            while (!(EvaluationResponses.All(response => response.IsAlreadyResponded)))
            {
                if (ResponseFeedbackCorrect.activeSelf || ResponseFeedbackIncorrect.activeSelf)
                {
                    yield return new WaitForSeconds(3);
                    ResponseFeedbackIncorrect.SetActive(false);
                    ResponseFeedbackCorrect.SetActive(false);
                }
                
                ExpressionList expressionList = new ExpressionList();
                CurrentSignEvaluated = EvaluationResponses.FirstOrDefault(evaluationResponse => !evaluationResponse.IsAlreadyResponded);
                OnResult.Invoke(GetCurrentEvaluatedExpressionLoop(CurrentSignEvaluated));
                yield return new WaitUntil(() => CurrentSignEvaluated.IsAlreadyResponded);
            }

            ResponseFeedbackIncorrect.SetActive(false);
            ResponseFeedbackCorrect.SetActive(false);
            Dialog myDialog = Dialog.Open(_mediumDialogPrefab, DialogButtonType.OK, "<align=\"center\">Resultados", EvaluationResponses.Aggregate("", (current, next) => $"{current}<align=\"center\">- {next.Expression.Word} {(next.IsCorrect ? "\U00002705" : "\U0000274C")}\n"), true);
            if (myDialog != null)
            {
                myDialog.OnClosed += OnDialogClosed;
            }

            float experienceGained = EvaluationResponses.Aggregate(0f, (current, next) => next.IsCorrect ? current + 1 : current);
            if (experienceGained > 0)
            {
                OnEvaluationExperience.Invoke(experienceGained);
            }

            OnEvaluationProgress.Invoke(EvaluationResponses);
            EvaluationResponses = null;
        }
        else
        {
            yield break;
        }
    }

    private async void  OnDialogClosed(DialogResult obj)
    {
        _animatorCommander.OnSwapToInterpretationMode();
        _dictationManager.OnSwapToInterpretationMode();
    }
    //private IEnumerable<string> GetAllModuleCategories(Module module)
    //{
    //    IEnumerable<string> categories = Enumerable.Empty<string>();
    //    var readData = Resources.Load<TextAsset>(Path.Combine("DataBase", "config", "categories"));
    //    var listOfCategories = JsonConvert.DeserializeObject<IEnumerable<Category>>(readData.text);
    //    categories = listOfCategories.Where(category => module.Categories.Any(category2 => category.Name.Equals(category2.Name))).Select(category => $"{category.Code}{1}");
    //    var size = categories.Count();

    //    return categories;
    //}

    private IEnumerable<Expression> GetCurrentEvaluatedExpressionLoop(EvaluationResponse evaluationResponse)
    {
        while (!evaluationResponse.IsAlreadyResponded)
        {
            yield return evaluationResponse.Expression;
        }

        yield break;
    }

    public void OnEvaluationResponse(string response)
    {
        
        HasSetResponseBeenActivated = false;
        CurrentSignEvaluated.Response = response;
        if (CurrentSignEvaluated.IsCorrect)
        {
            ResponseFeedbackCorrect.SetActive(true);
        }
        else
        {
            ResponseFeedbackIncorrect.SetActive(true);
        }
    }

    public void OnModuleSelected(int module)
    {
        SelectedModule = module;
        ModuleSelectionMenu.SetActive(false);
        HasEvaluationBeenActivated = true;
    }
}
