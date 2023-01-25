using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Microsoft.MixedReality.Toolkit.UI;
using Assets.Util;
using System.Collections.Generic;
using UnityEngine.Events;
using System;

namespace LSB
{
    public class AnimatorCommander : MonoBehaviour
    {
        public Animator anim;

        private IAnimationController _controller;

        [SerializeField]
        public GameObject mainTextToolTip;

        [Serializable] public class ResultHandler : UnityEvent<float> { }
        public ResultHandler OnInterpretationExperience;

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
            //OldRange = (OldMax - OldMin)
            //NewRange = (NewMax - NewMin)
            //NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin
            var oldRange = 1.0f - 0f;
            var newRange = 3.0f - 1.0f;
            var newValue = (((eventData.NewValue - 0f) * newRange) / oldRange) + 1f;
            animationSpeed = newValue;
            anim.speed = newValue;
        }

        public void OnCommand(IEnumerable<Expression> expressions)
        {
            if (_currentModule == LSBModule.INTERPRETATION)
            {
                OnInterpretationExperience.Invoke(0.5f);
            }
            
            StartCoroutine(_controller.Scene(expressions));
        }

        public void OnError(string word)
        {
            if (_currentModule == LSBModule.INTERPRETATION)
            {
                OnInterpretationExperience.Invoke(0.5f);
            }

            ExpressionList expressions = LocalParser.ParseExpressionList(word);
            StartCoroutine(_controller.Scene(expressions.tokens));
        }

        public void OnSwapToEvaluationModule()
        {
            _currentModule = LSBModule.EVALUATION;
        }

        public void OnSwapToInterpretationMode()
        {
            _currentModule = LSBModule.INTERPRETATION;
        }
    }
}
