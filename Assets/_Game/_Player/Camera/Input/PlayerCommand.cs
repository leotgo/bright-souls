using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerCommandBase
{
    protected Player player;

    public PlayerCommandBase(Player owner)
    {
        this.player = owner;
    }

    public abstract bool IsValid();
}

public abstract class PlayerCommand : PlayerCommandBase
{
    public PlayerCommand(Player player) : base(player) { }
    public abstract void Execute();
}

public abstract class PlayerCommand<T> : PlayerCommandBase {

    public PlayerCommand(Player owner) : base(owner) { }
    public abstract void Execute(T arg);

}

public abstract class PlayerCommand<T1,T2> : PlayerCommandBase
{
    public PlayerCommand(Player owner) : base(owner) { }
    public abstract void Execute(T1 arg1, T2 arg2);
}

