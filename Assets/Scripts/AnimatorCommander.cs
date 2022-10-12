using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Microsoft.MixedReality.Toolkit.UI;
using Assets.Util;
using System.Collections.Generic;

namespace LSB
{
    public class AnimatorCommander : MonoBehaviour
    {
        public Animator anim;

        private IAnimationController _controller;

        [SerializeField]
        public GameObject mainTextToolTip; 
        //public Text mainText;

        public float animationDuration;

        public float animationSpeed;

        private static float DEFAULT_SPEED = 1.5f;

        private string OMITTED_CATEGORY = "#99";

        private string CONDITIONAL_EVENT_PARAMETER = "currentSign";

        private LSBModule _currentModule = LSBModule.INTERPRETATION;

        private LSBModule _previousModule;

        public void Start()
        {
            _previousModule = _currentModule;
            _controller = new AnimatorControllerStates(anim, mainTextToolTip, CONDITIONAL_EVENT_PARAMETER);
            animationSpeed = 1.5f;
        }

        public void Update()
        {
            if (_currentModule != _previousModule)
            {
                _previousModule = _currentModule;
                switch (_currentModule)
                {
                    case LSBModule.INTERPRETATION:
                        _controller = new AnimatorControllerStates(anim, mainTextToolTip, CONDITIONAL_EVENT_PARAMETER);
                        break;
                    case LSBModule.EVALUATION:
                        _controller = new AnimatorControllerEvaluation(anim, mainTextToolTip, CONDITIONAL_EVENT_PARAMETER);
                        break;
                    default:
                        break;
                }
            }
        }

        public void OnEnable()
        {
            
        }

        public void SetSlowSpeed()
        {
            //animationDuration = 1.5f;
            animationSpeed = 1.0f;
        }

        public void SetMediumSpeed()
        {
            //animationDuration = 1.0f;
            animationSpeed = 1.5f;
        }

        public void SetFastSpeed()
        {
            //animationDuration = 0.5f;
            anim.speed = 3.0f;
        }

        public void SetSpeed(SliderEventData eventData)
        {
            animationSpeed = eventData.NewValue;
        }

        public void OnCommand(IEnumerable<Expression> expressions)
        { 
            StartCoroutine(_controller.Scene(expressions));
        }

        public void OnError(string word)
        {
            ExpressionList expressions = LocalParser.ParseExpressionList(word);
            Debug.Log(expressions.tokens.Count);
            StartCoroutine(_controller.Scene(expressions.tokens));
        }

        public void OnSwapToEvaluationModule()
        {
            _currentModule = LSBModule.EVALUATION;
        }
         
    }
}
