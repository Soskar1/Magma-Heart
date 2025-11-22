using System;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities.CombatSystem;
using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    [RequireComponent(typeof(TurnBasedMovement))]
    [RequireComponent(typeof(Facing))]
    [RequireComponent(typeof(EntityAnimation))]
    public class Entity : MonoBehaviour
    {
        [SerializeField] private EntityData m_data;
        private DungeonGrid m_grid;

        public EntityModel Model { get; private set; }
        public Health Health => Model.Health;
        public Energy Energy => Model.Energy;
        public EntityStats Stats => Model.Stats;
        public CombatController CombatController { get; protected set; }
        public TurnBasedMovement TurnBasedMovement { get; private set; }
        public Facing Facing { get; private set; }
        public EntityAnimation Animation { get; private set; }

        public virtual void Initialize(DungeonGrid grid, bool isPlayer)
        {
            m_grid = grid;

            Func<Vector3Int> getCurrentTilePosition = () => m_grid.WorldToTilePosition(transform.position);
            Model = new EntityModel(this, m_data, getCurrentTilePosition, isPlayer);

            TurnBasedMovement = GetComponent<TurnBasedMovement>();
            Facing = GetComponent<Facing>();
            Animation = GetComponent<EntityAnimation>();
        }
    }
}
