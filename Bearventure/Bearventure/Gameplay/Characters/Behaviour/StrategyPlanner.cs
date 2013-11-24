using System.Collections.Generic;
using System.Linq;
using Bearventure.Gameplay.Characters;
using Microsoft.Xna.Framework;

namespace Bearventure
{
    /// <summary>
    /// The StrategyPlanner checks a list of pre-determined actions for their conditions. An action with fulfilled conditions
    /// is added to actionQueue. The structure of the system will be altered to allow actions that only need to be executed once to be added.
    /// </summary>
    public class StrategyPlanner
    {
        public List<Action> actionQueue;
        private Enemy subject;
        private Character player;
        private List<Action> preDeterminedActions;
        private float reactTimer;

        #region testing
        Action chase;
        Action stop;
        Action defaultAction;
        Action attack;
        Action flee;
        Action cower;
        #endregion
        /// <summary>
        /// Initialization method for pre-determined actions.
        /// </summary>
        private void InitActions()
        {
            if (subject.combatBehaviour == Constants.CombatBehaviour.Default)
                InitDefaultCombat();
            else if (subject.combatBehaviour == Constants.CombatBehaviour.AttackAndFlee)
                InitAttackAndFlee();
        }
        private void InitDefaultCombat()
        {
            defaultAction = new Action(Constants.ActionType.Default);
            defaultAction.AddCondition(new Condition(Constants.ConditionType.DistanceToPlayerGreaterThan, subject.Vision));
            preDeterminedActions.Add(defaultAction);

            chase = new Action(Constants.ActionType.Chase);
            chase.AddCondition(new Condition(Constants.ConditionType.DistanceToPlayerLowerThan, subject.Vision));
            chase.AddCondition(new Condition(Constants.ConditionType.DistanceToPlayerGreaterThan, subject.AttackRange));
            chase.AddCondition(new Condition(Constants.ConditionType.Blocked, false));
            chase.AddCondition(new Condition(Constants.ConditionType.HealthHigherThan, subject.maxHealth / 4));

            preDeterminedActions.Add(chase);

            stop = new Action(Constants.ActionType.Stop);
            stop.AddCondition(new Condition(Constants.ConditionType.DistanceToPlayerLowerThan, subject.AttackRange));
            stop.AddCondition(new Condition(Constants.ConditionType.VelocityOtherThan, 0f));

            preDeterminedActions.Add(stop);

            flee = new Action(Constants.ActionType.Flee, true);
            flee.AddCondition(new Condition(Constants.ConditionType.HealthLowerThan, subject.maxHealth / 4));

            preDeterminedActions.Add(flee);

            cower = new Action(Constants.ActionType.Stop, true);
            cower.AddCondition(new Condition(Constants.ConditionType.HealthLowerThan, subject.maxHealth / 4));
            cower.AddCondition(new Condition(Constants.ConditionType.DistanceToPlayerGreaterThan, subject.Vision));

            preDeterminedActions.Add(cower);
        }

        private void InitAttackAndFlee()
        {
            defaultAction = new Action(Constants.ActionType.Default);
            defaultAction.AddCondition(new Condition(Constants.ConditionType.AttackReady, false));

            preDeterminedActions.Add(defaultAction);

            chase = new Action(Constants.ActionType.Chase);
            chase.AddCondition(new Condition(Constants.ConditionType.DistanceToPlayerLowerThan, subject.Vision));
            chase.AddCondition(new Condition(Constants.ConditionType.DistanceToPlayerGreaterThan, subject.AttackRange));
            chase.AddCondition(new Condition(Constants.ConditionType.AttackReady, true));

            preDeterminedActions.Add(chase);

            attack = new Action(Constants.ActionType.Attack);
            attack.AddCondition(new Condition(Constants.ConditionType.DistanceToPlayerLowerThan, subject.AttackRange));
            attack.AddCondition(new Condition(Constants.ConditionType.AttackReady, true));

            preDeterminedActions.Add(attack);
        }

        public StrategyPlanner(Enemy subject, Character player)
        {
            this.subject = subject;
            this.player = player;

            actionQueue = new List<Action>();
            preDeterminedActions = new List<Action>();

            InitActions();
        }

        public void Plan(GameTime gameTime)
        {
            reactTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (reactTimer >= subject.ReactSpeed)
            {
                foreach (Action a in preDeterminedActions)
                {
                    if (a.ConditionsFulfilled(subject, player))
                    {
                        AddActionToQueue(a);
                    }
                }

                reactTimer = 0;

                CleanActions();
            }
        }
        /// <summary>
        /// Used to add actions to queue. The method checks for duplicate actions and prevents them from being put to queue.
        /// </summary>
        private void AddActionToQueue(Action action)
        {
            if (!actionQueue.Contains(action))
                actionQueue.Add(action);
        }
        /// <summary>
        /// Goes through actionQueue and checks for actions that are marked primary.
        /// </summary>
        /// <returns>First in queue or primary action</returns>
        public Constants.ActionType CurrentAction()
        {
            foreach (Action ac in actionQueue)
                if (ac.Primary == true)
                    return ac.Type;

            return actionQueue.Any() ? actionQueue.First().Type : Constants.ActionType.Stop;
        }
        /// <summary>
        /// private method used to clean actions with fulfilled conditions from the actionQueue.
        /// </summary>
        private void CleanActions()
        {
            if (actionQueue.Any() && !actionQueue.First().ConditionsFulfilled(subject, player))
                actionQueue.RemoveAt(0);
        }
    }
}
