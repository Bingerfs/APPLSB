using Assets.Util;
using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace LSB
{
    public class AnimatorControllerStates : IAnimationController
    {
        private List<string> AnimationList = new List<string>();

        private Animator _animator;

        public Animator Animator
        {
            get { return _animator; }

            set { _animator = value; }
        }

        private string CONDITIONAL_EVENT_PARAMETER;

        private ToolTip _toolTip;

        public GameObject _mainTextToolTip;

        public AnimatorControllerStates(Animator animator, GameObject mainTextToolTip, string conditionalParameter)
        {
            Animator = animator;
            _mainTextToolTip = mainTextToolTip;
            _toolTip = _mainTextToolTip.GetComponent<ToolTip>();
            CONDITIONAL_EVENT_PARAMETER = conditionalParameter;
            TextAsset codes = (TextAsset)Resources.Load("Codes");

            char[] delimiters = new char[] { '\r', '\n' };
            foreach (string s in codes.text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries))
            {
                AnimationList.Add(s);
            }
        }

        public bool ExpressionHasAllStates(Expression expression)
        {
            return expression.IsPartOf(AnimationList);
            /*foreach(string stateSelected in AnimationList)
            {
                if (stateSelected.Contains(stateName))
                {
                    return true;
                }
            }

            return false;  */
        }

        /*public bool HasAllStateNames(List<Expression> expressions)
        {
            foreach(string stateName in stateNames)
            {
                if(!HasStateName(stateName))
                {
                    return false;
                }
            }
            return true;
        }*/

        public bool ExpressionHasAnimationClips(Expression expression)
        {
            try
            {
                return TryGetAnimationClips(out _, expression);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /*public bool StateHasAnimationClip(Animator animator, Expression expression)
        {
            try
            { 
                string expressionCode = expression.GetStringList().Substring(1);
                return HasAllStateNames(expression.code) && ExistAnimationOfExpression(animator, expressionCode);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool ExistAnimationOfExpression(Expression expression)
        {
            if (GetAnimationClip(animator, expressionCode))
            {
                return true;
            }
            return false;
        }*/

        public IEnumerable<AnimationClip> GetAnimationClip(Expression expression)
        {
            var animations = Animator.runtimeAnimatorController.animationClips.AsEnumerable();
            return animations.Where(animation => expression.Codes.Any(code => animation.name.Contains(code.WholeCode.Substring(1))));            
        }

        public bool TryGetAnimationClips(out IEnumerable<AnimationClip> filteredAnimations, Expression expression)
        {
            var listCodes = expression.Codes.ToList();
            var animations = Animator.runtimeAnimatorController.animationClips.AsEnumerable();
            filteredAnimations = animations.Where(animation => listCodes.Any(code => animation.name.Contains(code.WholeCode.Substring(1))));
            filteredAnimations = filteredAnimations.OrderBy(fm => listCodes.FindIndex(c => fm.name.Contains(c.WholeCode.Substring(1)))).ToList();
            var visualizelist = filteredAnimations.ToList();
            return filteredAnimations.Any();
        }

        public List<string> GetAnimationList()
        {
            return AnimationList;
        }

        public IEnumerator Scene(IEnumerable<Expression> expressions)
        {
            //Animator.speed = animationSpeed;

            foreach (Expression expression in expressions)
            {
                if (expression.Type != ExpressionType.TENSE)
                {
                    _mainTextToolTip.SetActive(true);
                    _toolTip.ToolTipText = expression.Word;
                }

                Expression selected = expression;
                if (!ExpressionHasAllStates(expression) || !ExpressionHasAnimationClips(expression))
                {
                    selected = LocalParser.ParseExpression(expression);
                }

                IEnumerable<AnimationClip> animationsToPlay = new List<AnimationClip>();
                if (TryGetAnimationClips(out animationsToPlay, expression))
                {
                    foreach (var animationToPlay in animationsToPlay)
                    {
                        var splitAnimatioName = animationToPlay.name.Split('_');
                        var integerCode = int.Parse(splitAnimatioName[1]);
                        var animationDuration = animationToPlay.length;
                        Animator.SetInteger(CONDITIONAL_EVENT_PARAMETER, integerCode);
                        yield return new WaitForSeconds(animationDuration);
                    }
                }
                else
                {
                    yield break;
                }
            }

            Animator.SetInteger(CONDITIONAL_EVENT_PARAMETER, 0);
            _toolTip.ToolTipText = "";
            _mainTextToolTip.SetActive(false);
        }
    }
}
