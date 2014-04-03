using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bearventure.Gameplay.Triggers
{
    public interface ITriggerable
    {
        event EventHandler Triggered;
    }

    public class Trigger
    {

    }
}
