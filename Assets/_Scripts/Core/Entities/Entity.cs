using System;
using MagmaHeart.Core.Entities.Models;
using MagmaHeart.Core.Entities.Presenters;
using MagmaHeart.Core.Presentation;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace MagmaHeart.Core.Entities
{
    [RequireComponent(typeof(TileBasedMovement))]
    [RequireComponent(typeof(Facing))]
    [RequireComponent(typeof(EntityAnimation))]
    [RequireComponent(typeof(Outline))]
    [RequireComponent(typeof(StunPresenter))]
    [RequireComponent(typeof(EntityEffectsPresenter))]
    [RequireComponent(typeof(VFXPresenter))]
    [RequireComponent(typeof(EntitySfxPresenter))]
    public class Entity : MonoBehaviour
    {
        [SerializeField] private Light2D m_light;
        public EntityModel Model { get; private set; }
        public HealthModel Health => Model.Health;
        public EnergyModel Energy => Model.Energy;
        public TileBasedMovement TileBasedMovement { get; private set; }
        public Facing Facing { get; private set; }
        public EntityAnimation Animation { get; private set; }
        public Outline Outline { get; private set; }
        public EntityEffectsPresenter EffectsPresenter { get; private set; }
        public VFXPresenter VFXPresenter { get; private set; }
        public EntitySfxPresenter SfxPresenter { get; private set; }

        private Func<Vector3Int> m_getCurrentTilePosition;

        public virtual void Initialize(EntityData data, WorldGrid grid, bool isPlayer, int id)
        {
            m_getCurrentTilePosition = () => grid.WorldToTilePosition(transform.position);
            Model = new EntityModel(data, m_getCurrentTilePosition(), isPlayer, id);

            TileBasedMovement = GetComponent<TileBasedMovement>();
            Facing = GetComponent<Facing>();
            Outline = GetComponent<Outline>();
            EffectsPresenter = GetComponent<EntityEffectsPresenter>();
            VFXPresenter = GetComponent<VFXPresenter>();
            SfxPresenter = GetComponent<EntitySfxPresenter>();

            Animation = GetComponent<EntityAnimation>();
            Animation.Initialize(data.AnimatorController);

            var stunPresenter = GetComponent<StunPresenter>();
            stunPresenter.Initialize(Model);
        }

        private void OnEnable()
        {
            if (TileBasedMovement == null)
                TileBasedMovement = GetComponent<TileBasedMovement>();

            TileBasedMovement.OnChangedTarget += HandleOnChangedTarget;
        }

        private void OnDisable()
        {
            TileBasedMovement.OnChangedTarget -= HandleOnChangedTarget;
        }

        private void HandleOnChangedTarget(object _, EventArgs __)
        {
            SfxPresenter.PlayStepSound();
        }

        private void Update()
        {
            Model.TilePosition = m_getCurrentTilePosition();
        }

        public void Die()
        {
            Animation.PlayAnimation(AnimationType.Death);
            m_light.gameObject.SetActive(false);
        }
    }
}
