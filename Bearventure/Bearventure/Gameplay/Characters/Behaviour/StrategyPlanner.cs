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
        #endregion
        /// <summary>
        /// Initialization method for pre-determined actions.
        /// </summary>
        private void InitActions()
        {
            chase = new Action(Constants.ActionType.Chase);
            chase.AddCondition(new Condition(Constants.ConditionType.DistanceToPlayerLowerThan, subject.Vision));
            chase.AddCondition(new Condition(Constants.ConditionType.DistanceToPlayerGreaterThan, subject.AttackRange));

            preDeterminedActions.Add(chase);

            stop = new Action(Constants.ActionType.Stop);
            stop.AddCondition(new Condition(Constants.ConditionType.DistanceToPlayerLowerThan, subject.AttackRange));
            stop.AddCondition(new Condition(Constants.ConditionType.VelocityOtherThan, 0f));

            preDeterminedActions.Add(stop);

            defaultAction = new Action(Constants.ActionType.Default);
            defaultAction.AddCondition(new Condition(Constants.ConditionType.DistanceToPlayerGreaterThan, subject.Vision));

            preDeterminedActions.Add(defaultAction);

            attack = new Action(Constants.ActionType.Attack);
            attack.AddCondition(new Condition(Constants.ConditionType.DistanceToPlayerLowerThan, subject.AttackRange));

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
