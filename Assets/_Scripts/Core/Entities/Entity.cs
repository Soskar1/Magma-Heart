using System;
using MagmaHeart.Core.Entities.Models;
using MagmaHeart.Core.Entities.Presenters;
using MagmaHeart.Core.Presentation;
using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    [RequireComponent(typeof(TileBasedMovement))]
    [RequireComponent(typeof(Facing))]
    [RequireComponent(typeof(EntityAnimation))]
    [RequireComponent(typeof(Outline))]
    [RequireComponent(typeof(StunPresenter))]
    public class Entity : MonoBehaviour
    {
        public EntityModel Model { get; private set; }
        public HealthModel Health => Model.Health;
        public EnergyModel Energy => Model.Energy;
        public TileBasedMovement TileBasedMovement { get; private set; }
        public Facing Facing { get; private set; }
        public EntityAnimation Animation { get; private set; }
        public Outline Outline { get; private set; }
        
        private Func<Vector3Int> m_getCurrentTilePosition;

        public virtual void Initialize(EntityData data, WorldGrid grid, bool isPlayer, int id)
        {
            m_getCurrentTilePosition = () => grid.WorldToTilePosition(transform.position);
            Model = new EntityModel(data, m_getCurrentTilePosition(), isPlayer, id);

            TileBasedMovement = GetComponent<TileBasedMovement>();
            Facing = GetComponent<Facing>();
            Outline = GetComponent<Outline>();

            Animation = GetComponent<EntityAnimation>();
            Animation.Initialize(data.AnimatorController);

            var stunPresenter = GetComponent<StunPresenter>();
            stunPresenter.Initialize(Model);
        }

        private void Update()
        {
            Model.TilePosition = m_getCurrentTilePosition();
        }
    }
}
