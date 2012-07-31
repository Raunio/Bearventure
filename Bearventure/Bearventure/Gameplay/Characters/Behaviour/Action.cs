using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bearventure
{
    public class Action
    {
        /// <summary>
        /// Actions are defined by their ActionType. Actions can have multiple conditions under which they are executed. 
        /// </summary>
        public enum ActionType
        {
            Default,
            Chase,
            Attack,
            Flee,
            Stop,
        };

        private ActionType type;
        private List<Condition> conditions;
        private bool primary;

        public Action(ActionType type)
        {
            this.type = type;
            primary = false;

            conditions = new List<Condition>();
        }
        public Action(ActionType type, bool primary)
        {
            this.type = type;
            this.primary = primary;
        }

        public ActionType Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }
        public bool Primary
        {
            get
            {
                return primary;
            }

            set
            {
                primary = value;
            }
        }

        public void AddCondition(Condition condition)
        {
            conditions.Add(condition);
        }

        public bool ConditionsFulfilled(Enemy subject, Character player)
        {
            int conditionsMet = 0;

            foreach (Condition cnd in conditions)
            {
                if (cnd.Fulfilled(subject, player) == true)
                {
                    conditionsMet++;
                }
            }

            if (conditionsMet == conditions.Count)
            {
                return true;
            }
            else
                return false;
        }
    }
}
