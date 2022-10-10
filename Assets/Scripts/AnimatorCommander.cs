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

        public AnimatorControllerStates controller;

        [SerializeField]
        public GameObject mainTextToolTip; 
        //public Text mainText;

        public float animationDuration;

        public float animationSpeed;

        private static float DEFAULT_SPEED = 1.5f;

        private string OMITTED_CATEGORY = "#99";

        private string CONDITIONAL_EVENT_PARAMETER = "currentSign";

        private ToolTip toolTip;

        public void Start()
        {
            controller = new AnimatorControllerStates(anim);
            animationSpeed = 1.5f;
        }

        public void OnEnable()
        {
            toolTip = mainTextToolTip.GetComponent<ToolTip>();
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
            animationSpeed = 3.0f;
        }

        public void SetSpeed(SliderEventData eventData)
        {
            animationSpeed = eventData.NewValue;
        }

        public void OnCommand(IEnumerable<Expression> expressions)
        { 
            StartCoroutine(scene(expressions));
        }

        public void OnError(string word)
        {
            ExpressionList expressions = LocalParser.ParseExpressionList(word);
            Debug.Log(expressions.tokens.Count);
            StartCoroutine(scene(expressions.tokens));
        }

        public IEnumerator scene(IEnumerable<Expression> expressions)
        {
            anim.speed = animationSpeed;
             
            foreach (Expression expression in expressions)
            {
                if (expression.Type != ExpressionType.TENSE)
                {
                    mainTextToolTip.SetActive(true);
                    toolTip.ToolTipText = expression.Word;
                }
                
		        Expression selected = expression;
                if(!controller.ExpressionHasAllStates(expression) || !controller.ExpressionHasAnimationClips(expression))
                {
                    selected = LocalParser.ParseExpression(expression);
                }

                IEnumerable<AnimationClip> animationsToPlay = new List<AnimationClip>();
                if (controller.TryGetAnimationClips(out animationsToPlay, expression))
                {
                    foreach (var animationToPlay in animationsToPlay)
                    {
                        var splitAnimatioName = animationToPlay.name.Split('_');
                        var integerCode = int.Parse(splitAnimatioName[1]);
                        animationDuration = animationToPlay.length;
                        anim.SetInteger(CONDITIONAL_EVENT_PARAMETER, integerCode);
                        yield return new WaitForSeconds(animationDuration);
                    }
                }
                else
                {
                    yield break;
                }
            }

            anim.SetInteger(CONDITIONAL_EVENT_PARAMETER, 0);
            toolTip.ToolTipText = "";
            mainTextToolTip.SetActive(false);
        }
         
    }
}
