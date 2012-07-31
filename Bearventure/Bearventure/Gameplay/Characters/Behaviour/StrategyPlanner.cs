using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

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
        private List<Action> preDetermined_actions;

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
            chase = new Action(Action.ActionType.Chase);
            chase.AddCondition(new Condition(Condition.ConditionType.DistanceToPlayerLowerThan, subject.Vision));
            chase.AddCondition(new Condition(Condition.ConditionType.DistanceToPlayerGreaterThan, subject.AttackRange));

            preDetermined_actions.Add(chase);

            stop = new Action(Action.ActionType.Stop);
            stop.AddCondition(new Condition(Condition.ConditionType.DistanceToPlayerLowerThan, subject.AttackRange));
            stop.AddCondition(new Condition(Condition.ConditionType.VelocityOtherThan, 0f));

            preDetermined_actions.Add(stop);

            defaultAction = new Action(Action.ActionType.Default);
            defaultAction.AddCondition(new Condition(Condition.ConditionType.DistanceToPlayerGreaterThan, subject.Vision));

            preDetermined_actions.Add(defaultAction);

            attack = new Action(Action.ActionType.Attack);
            attack.AddCondition(new Condition(Condition.ConditionType.DistanceToPlayerLowerThan, subject.AttackRange));

            preDetermined_actions.Add(attack);
        }

        public StrategyPlanner(Enemy subject, Character player)
        {
            this.subject = subject;
            this.player = player;

            actionQueue = new List<Action>();
            preDetermined_actions = new List<Action>();

            #region testing
            InitActions();
            #endregion
        }

        public void Plan()
        {          
            #region testing

            foreach (Action a in preDetermined_actions)
            {
                if (a.ConditionsFulfilled(subject, player))
                {
                    AddActionToQueue(a);
                }
            }

            CleanActions();

            #endregion
        }
        /// <summary>
        /// Used to add actions to queue. The method checks for duplicate actions and prevents them from being put to queue.
        /// </summary>
        private void AddActionToQueue(Action action)
        {
            foreach (Action ac in actionQueue)
            {
                if (ac == action)
                {
                    return;
                }
            }

            actionQueue.Add(action);
        }
        /// <summary>
        /// Goes through actionQueue and checks for actions that are marked primary.
        /// </summary>
        /// <returns>First in queue or primary action</returns>
        public Action.ActionType CurrentAction()
        {
            foreach (Action ac in actionQueue)
            {
                if (ac.Primary == true)
                {
                    return ac.Type;
                }
            }

            if (actionQueue.Count > 0)
            {
                return actionQueue[0].Type;
            }

            else return Action.ActionType.Stop;
        }
        /// <summary>
        /// private method used to clean actions with fulfilled conditions from the actionQueue.
        /// </summary>
        private void CleanActions()
        {
            if (actionQueue.Count >= 1)
            {
                if (!actionQueue[0].ConditionsFulfilled(subject, player))
                {
                    actionQueue.RemoveAt(0);
                }
            }

        }
    }
}
