using System;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.UI;
using UnityEngine;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private EntityData m_data;

        public Action<Collider2D> OnTriggerEnter;
        public Action<Collider2D> OnTriggerExit;

        private Entity m_controllingEntity;

        public IMovable Movement { get; private set; }
        public Entity ControllingEntity => m_controllingEntity;
        public Health Health => ControllingEntity.Health;
        public Energy Energy => ControllingEntity.Energy;
        public EntityStats Stats => ControllingEntity.Stats;

        private IPlayerBehaviour m_currentBehaviour;
        private ActionPlayerBehaviour m_actionBehaviour;
        private TurnBasedPlayerBehaviour m_turnBasedBehaviour;

        public TurnBasedPlayerBehaviour TurnBasedPlayerBehaviour => m_turnBasedBehaviour;

        public void Initialize(UserInput userInput, DungeonGrid grid, EnergyHUD energyHUD)
        {
            AnimationPlayer animationPlayer = GetComponent<AnimationPlayer>();
            m_controllingEntity = new Entity(m_data, transform, animationPlayer);

            Movement = GetComponent<IMovable>();
            m_actionBehaviour = new ActionPlayerBehaviour(userInput, Movement, this);

            MouseControl mouseControl = new MouseControl(userInput, grid);
            TurnBasedUserInput turnBasedUserInput = new TurnBasedUserInput(userInput, mouseControl);

            m_turnBasedBehaviour = new TurnBasedPlayerBehaviour(this, turnBasedUserInput, energyHUD);
            m_currentBehaviour = m_actionBehaviour;
        }

        public void Enable()
        {
            m_currentBehaviour.Enable();
            ControllingEntity.Enable();
        }

        public void Disable()
        {
            m_currentBehaviour.Disable();
            ControllingEntity.Disable();
        }

        public void EnterCombat() => SwitchState(m_turnBasedBehaviour);
        public void ExitCombat() => SwitchState(m_actionBehaviour);

        private void SwitchState(IPlayerBehaviour newState)
        {
            m_currentBehaviour.Disable();
            m_currentBehaviour = newState;
            m_currentBehaviour.Enable();
        }

        private void Update() => ControllingEntity.RunAnimations();

        private void FixedUpdate() => m_currentBehaviour.Update();

        private void OnTriggerEnter2D(Collider2D collision) => OnTriggerEnter?.Invoke(collision);

        private void OnTriggerExit2D(Collider2D collision) => OnTriggerExit?.Invoke(collision);
    }
}