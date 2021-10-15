using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrightSouls.Player
{
    public abstract class PlayerCommandBase
    {
        protected PlayerComponentIndex player;

        public PlayerCommandBase(PlayerComponentIndex owner)
        {
            this.player = owner;
        }

        public abstract bool CanExecute();
    }

    public abstract class PlayerCommand : PlayerCommandBase
    {
        public PlayerCommand(PlayerComponentIndex player) : base(player) { }
        public abstract void Execute();
    }

    public abstract class PlayerCommand<T> : PlayerCommandBase {

        public PlayerCommand(PlayerComponentIndex owner) : base(owner) { }
        public abstract void Execute(T arg);

    }

    public abstract class PlayerCommand<T1,T2> : PlayerCommandBase
    {
        public PlayerCommand(PlayerComponentIndex owner) : base(owner) { }
        public abstract void Execute(T1 arg1, T2 arg2);
    }
}