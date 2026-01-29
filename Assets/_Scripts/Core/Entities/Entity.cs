using System;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities.Models;
using MagmaHeart.Core.Presentation;
using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    [RequireComponent(typeof(TileBasedMovement))]
    [RequireComponent(typeof(Facing))]
    [RequireComponent(typeof(EntityAnimation))]
    [RequireComponent(typeof(Outline))]
    public class Entity : MonoBehaviour
    {
        public EntityModel Model { get; private set; }
        public HealthModel Health => Model.Health;
        public EnergyModel Energy => Model.Energy;
        public ITurnController TurnController { get; private set; }
        public TileBasedMovement TileBasedMovement { get; private set; }
        public Facing Facing { get; private set; }
        public EntityAnimation Animation { get; private set; }
        public Outline Outline { get; private set; }
        
        private Func<Vector3Int> m_getCurrentTilePosition;

        public virtual void Initialize(EntityData data, RoomGrid grid, bool isPlayer, ITurnController turnController, int id)
        {
            m_getCurrentTilePosition = () => grid.WorldToTilePosition(transform.position);
            Model = new EntityModel(data, m_getCurrentTilePosition(), isPlayer, id);
            TurnController = turnController;

            TileBasedMovement = GetComponent<TileBasedMovement>();
            Facing = GetComponent<Facing>();
            Outline = GetComponent<Outline>();

            Animation = GetComponent<EntityAnimation>();
            Animation.Initialize(data.AnimatorController);
        }

        private void Update()
        {
            Model.TilePosition = m_getCurrentTilePosition();
            Animation.PlayAnimations();
        }
    }
}
