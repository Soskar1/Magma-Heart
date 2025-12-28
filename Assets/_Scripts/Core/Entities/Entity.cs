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
        [SerializeField] private EntityData m_data;

        public EntityModel Model { get; private set; }
        public HealthModel Health => Model.Health;
        public EnergyModel Energy => Model.Energy;
        public EntityTurnContext TurnContext { get; protected set; }
        public TileBasedMovement TileBasedMovement { get; private set; }
        public Facing Facing { get; private set; }
        public EntityAnimation Animation { get; private set; }
        public Outline Outline { get; private set; }

        public virtual void Initialize(RoomGrid grid, bool isPlayer)
        {
            Func<Vector3Int> getCurrentTilePosition = () => grid.WorldToTilePosition(transform.position);
            Model = new EntityModel(m_data, getCurrentTilePosition, isPlayer);

            TileBasedMovement = GetComponent<TileBasedMovement>();
            Facing = GetComponent<Facing>();
            Animation = GetComponent<EntityAnimation>();
            Outline = GetComponent<Outline>();
        }
    }
}
