using System;

namespace BrightSouls
{
    [Flags]
    public enum CharacterStatus
    {
        None = 0x0,
        Staggered = 0x1,
        IFrames = 0x2,
        Unstoppable = 0x4
    }

    [System.Serializable]
    public class StatusAttribute : CharacterAttribute<CharacterStatus>
    {
        public void AddStatus(CharacterStatus status)
        {
            Value = Value | status;
        }

        public void RemoveStatus(CharacterStatus status)
        {
            Value = Value & (~status);
        }

        public bool HasStatus(CharacterStatus status)
        {
            return (Value & status) != CharacterStatus.None;
        }
    }

}