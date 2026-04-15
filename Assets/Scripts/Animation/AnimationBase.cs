using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animation
{

    public enum AnimatorParameterType
    {
        Trigger,
        Bool
    }
    public enum AnimationType
    {
        Run,
        Attack,
        Death
    }
    public class AnimationBase : MonoBehaviour
    {
        public List<AnimationSetup> animationSetups;

        public Animator animator;



        public void PlayAnimation(AnimationType type, bool boolValue = true)
        {
            var setup = animationSetups.Find(i => i.type == type);

            if (setup == null) return;

            switch (setup.parameterType)
            {
                case AnimatorParameterType.Trigger:
                    animator.SetTrigger(setup.parameterName);
                    break;

                case AnimatorParameterType.Bool:
                    animator.SetBool(setup.parameterName, boolValue);
                    break;
            }
        }
    }

    [System.Serializable]
    public class AnimationSetup
    {
        public AnimationType type;
        public string parameterName;
        public AnimatorParameterType parameterType;
    }

}
