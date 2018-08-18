using System;
using UnityEngine;

namespace MichaelWolfGames.MeterSystem
{
    /// <summary>
    /// Base class for all meter objects, which read information from IMeterable objects.
    /// </summary>
    public abstract class MeterBase : MonoBehaviour
    {
        public MonoBehaviour MeterableBehaviour;

        protected bool IsSubscribed = false;
        protected IMeterable Meterable = null;

        protected virtual void OnEnable()
        {
            HandleEventSubscription(true);
            if(IsSubscribed)
                UpdateMeter(Meterable.PercentValue);
        }

        protected virtual void OnDisable()
        {
            HandleEventSubscription(false);
        }

        private void HandleEventSubscription(bool state)
        {
            if(!MeterableBehaviour && Meterable == null) return;
            if (Meterable == null)
            {
                var m = MeterableBehaviour as IMeterable;
                if(m != null)
                {
                    Meterable = m;
                }
            }

            if (Meterable != null)
            {
                if (state)
                {
                    if (!IsSubscribed)
                    {
                        Meterable.OnUpdateValue += DoOnUpdateValue;
                    }
                }
                else
                {
                    if (IsSubscribed)
                    {
                        Meterable.OnUpdateValue -= DoOnUpdateValue;
                    }
                }

                IsSubscribed = state;
            }
        }

        private void DoOnUpdateValue(float currentValue)
        {
            UpdateMeter(Meterable.PercentValue);
        }

        public virtual void SetMeterable(IMeterable meterable)
        {
            if (Meterable != null)
            {
                HandleEventSubscription(false);
            }
            Meterable = meterable;
            HandleEventSubscription(true);
        }

        protected abstract void UpdateMeter(float percentValue);


#if UNITY_EDITOR
        /// <summary>
        /// This OnValidate Method helps assure that the referenced Monobehaviour component implements IMeterable.
        /// </summary>
        private void OnValidate()
        {
            if (MeterableBehaviour)
            {
                if ((MeterableBehaviour as IMeterable) == null)
                {
                    foreach (var behaviour in MeterableBehaviour.gameObject.GetComponents<MonoBehaviour>())
                    {
                        if ((behaviour as IMeterable) != null)
                        {
                            MeterableBehaviour = behaviour;
                            break;
                        }
                    }
                }
            }

        }
#endif

    }
}