using System.Collections.Generic;

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

        private List<Condition> conditions;

        public Action(ActionType type)
        {
            this.Type = type;
            this.Primary = false;

            conditions = new List<Condition>();
        }
        public Action(ActionType type, bool primary)
        {
            this.Type = type;
            this.Primary = primary;
        }

        public ActionType Type
        {
            get;
            set;
        }
        public bool Primary
        {
            get;
            set;
        }

        public void AddCondition(Condition condition)
        {
            conditions.Add(condition);
        }

        public bool ConditionsFulfilled(Enemy subject, Character player)
        {
            int conditionsMet = 0;

            foreach (Condition cnd in conditions)
                if (cnd.Fulfilled(subject, player))
                    conditionsMet++;

            return conditionsMet == conditions.Count ? true : false;
        }
    }
}
