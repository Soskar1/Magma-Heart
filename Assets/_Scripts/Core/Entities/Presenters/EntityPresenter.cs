using System;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities.CombatSystem;
using MagmaHeart.Core.Entities.Models;
using UnityEngine;

namespace MagmaHeart.Core.Entities.Presenters
{
    [RequireComponent(typeof(TurnBasedMovement))]
    [RequireComponent(typeof(Facing))]
    [RequireComponent(typeof(EntityAnimation))]
    public class EntityPresenter : MonoBehaviour
    {
        [SerializeField] private EntityData m_data;
        private DungeonGrid m_grid;

        public EntityModel Model { get; private set; }
        public HealthModel Health => Model.Health;
        public EnergyModel Energy => Model.Energy;
        public CombatController CombatController { get; protected set; }
        public TurnBasedMovement TurnBasedMovement { get; private set; }
        public Facing Facing { get; private set; }
        public EntityAnimation Animation { get; private set; }

        public virtual void Initialize(DungeonGrid grid, bool isPlayer)
        {
            m_grid = grid;

            Func<Vector3Int> getCurrentTilePosition = () => m_grid.WorldToTilePosition(transform.position);
            Model = new EntityModel(m_data.Stats, getCurrentTilePosition, isPlayer);

            TurnBasedMovement = GetComponent<TurnBasedMovement>();
            Facing = GetComponent<Facing>();
            Animation = GetComponent<EntityAnimation>();
        }
    }
}
