using MichaelWolfGames;
using MichaelWolfGames.DamageSystem;
using UnityEngine;

namespace MichaelWolfGames.DamageSystem
{
    public class DestroyOnDeath : SubscriberBase<HealthManager>
    {
        [SerializeField] private float _delay = 0.15f;
        [SerializeField] private GameObject _rootGameObject;
        private ParticleSystem _particle;
        protected override void Start()
        {
            base.Start();
            if (!_rootGameObject) _rootGameObject = SubscribableObject.gameObject;
            _particle = GetComponent<ParticleSystem>();
        }

        protected override void SubscribeEvents()
        {
            SubscribableObject.OnDeath += DoOnDeath;
        }

        protected override void UnsubscribeEvents()
        {
            SubscribableObject.OnDeath -= DoOnDeath;
        }

        private void DoOnDeath()
        {
            if(!_rootGameObject) _rootGameObject = SubscribableObject.gameObject;
            _particle.Play();
            Destroy(_rootGameObject, _delay);
        }
    }
}