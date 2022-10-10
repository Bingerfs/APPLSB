using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace LSB
{
    public class AnimatorControllerStates : MonoBehaviour
    {
        private List<string> AnimationList = new List<string>();

        private Animator _animator;

        public Animator Animator
        {
            get { return _animator; }

            set { _animator = value; }
        }

        public AnimatorControllerStates(Animator animator)
        {
            Animator = animator;
            TextAsset codes = (TextAsset)Resources.Load("Codes");

            char[] delimiters = new char[] { '\r', '\n' };
            foreach (string s in codes.text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries))
            {
                AnimationList.Add(s);
            }
        }

        void Start()
        { 
            /*TextAsset codes = (TextAsset) Resources.Load("Codes");
            
            char[] delimiters = new char[] { '\r', '\n' };
            foreach(string s in codes.text.Split(delimiters,StringSplitOptions.RemoveEmptyEntries))
            {               
                AnimationList.Add(s);
            }*/
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
            var animations = Animator.runtimeAnimatorController.animationClips.AsEnumerable();
            filteredAnimations = animations.Where(animation => expression.Codes.Any(code => animation.name.Contains(code.WholeCode.Substring(1))));
            return filteredAnimations.Any();
        }

        public List<string> GetAnimationList()
        {
            return AnimationList;
        }
    }
}
