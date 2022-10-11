using Assets;
using LSB;
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
    static string UNITY_RESOURCES_FOLDER = Path.Combine("Assets", "Resources");
    static string CATEGORY_PATH = Path.Combine(UNITY_RESOURCES_FOLDER, "DataBase", "config", "categories.json");
    static string CONFIG_PATH = Path.Combine(UNITY_RESOURCES_FOLDER, "DataBase", "config", "data.json");
    static string NAME_ANIMATOR_CONTROLLER = "main.controller";
    static string CODE_INDICATOR = "#";
    static string ANIMATIONS_FOLDER_PATH = @"Animations";
    static string CODE_SEPARATOR_CLIP = "_";
    static string NAME_CODES_FILE = "Codes.txt";
    public int SelectedModule { get; set; } = 1;

    public int NumberOfSigns { get; set; } = 5;

    [Serializable] public class ResultHandler : UnityEvent<IEnumerable<Expression>> { }
    public ResultHandler OnResult;

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



    void Start()
    {
        TextAsset readCodes = (TextAsset)Resources.Load("Codes");
        char[] delimiters = new char[] { '\r', '\n' };
        var listOfCodes = readCodes.text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
        foreach (string code in listOfCodes)
        {
            _signCodes = _signCodes.Append(code);
        }

        using (StreamReader reader = new StreamReader(CONFIG_PATH))
        {
            var readData = reader.ReadToEnd();
            var listOfModules = JsonConvert.DeserializeObject<List<Module>>(readData);
            foreach (var module in listOfModules)
            {
                _modulesDictionary.Add(module.Name, GetAllModuleCategories(module));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (HasEvaluationBeenActivated)
        {
            HasEvaluationBeenActivated = false;
            var randomisedExpressionsList = RandomisedSignCodes.Select(code => 
            {
                var splitCode = code.Split('#');
                var auxExpression = new Expression($"#{splitCode[1]}", splitCode[0]);
                return auxExpression;
            }).ToList();
            EvaluationResponses = randomisedExpressionsList.Select(expression => new EvaluationResponse(expression)).ToList();
            StartCoroutine(Evaluate());
        }

        if (HasSetResponseBeenActivated)
        {
            OnEvaluationResponse("aah");
        }
    }

    private IEnumerator Evaluate()
    {
        if (EvaluationResponses.Any())
        {
            while (!(EvaluationResponses.All(response => response.IsAlreadyResponded)))
            {
                ExpressionList expressionList = new ExpressionList();
                CurrentSignEvaluated = EvaluationResponses.FirstOrDefault(evaluationResponse => !evaluationResponse.IsAlreadyResponded);
                OnResult.Invoke(GetCurrentEvaluatedExpressionLoop(CurrentSignEvaluated));
                yield return new WaitUntil(() => CurrentSignEvaluated.IsAlreadyResponded);
            }

            EvaluationResponses = null;
        }
        else
        {
            yield break;
        }
    }

    private IEnumerable<string> GetAllModuleCategories(Module module)
    {
        IEnumerable<string> categories = new List<string>();
        using (StreamReader reader = new StreamReader(CATEGORY_PATH))
        {
            var readData = reader.ReadToEnd();
            var listOfCategories = JsonConvert.DeserializeObject<IEnumerable<Category>>(readData);
            categories = listOfCategories.Where(category => module.Categories.Any(category2 => category.Name.Equals(category2.Name))).Select(category => $"{category.Code}{1}");
            var size = categories.Count();
        }

        return categories;
    }

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
    }
}
