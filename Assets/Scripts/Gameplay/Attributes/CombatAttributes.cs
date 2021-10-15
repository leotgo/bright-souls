namespace BrightSouls
{
    [System.Serializable]
    public class HealthAttribute : CharacterAttribute<float>
    {
        public HealthAttribute(float defaultValue) : base(defaultValue) { }
    }
    [System.Serializable]
    public class MaxHealthAttribute : CharacterAttribute<float>
    {
        public MaxHealthAttribute(float defaultValue) : base(defaultValue) { }
    }
    [System.Serializable]
    public class StaminaAttribute : CharacterAttribute<float>
    {
        public StaminaAttribute(float defaultValue) : base(defaultValue) { }
    }
    [System.Serializable]
    public class MaxStaminaAttribute : CharacterAttribute<float>
    {
        public MaxStaminaAttribute(float defaultValue) : base(defaultValue) { }
    }
    [System.Serializable]
    public class PoiseAttribute : CharacterAttribute<float>
    {
        public PoiseAttribute(float defaultValue) : base(defaultValue) { }
    }
    [System.Serializable]
    public class MaxPoiseAttribute : CharacterAttribute<float>
    {
        public MaxPoiseAttribute(float defaultValue) : base(defaultValue) { }
    }
}