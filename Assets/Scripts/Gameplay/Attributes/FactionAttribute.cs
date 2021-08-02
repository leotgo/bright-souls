namespace BrightSouls
{
    public enum CharacterFaction
    {
        Player = 0,
        Enemy = 1
    }

    [System.Serializable]
    public class FactionAttribute : CharacterAttribute<CharacterFaction> { }
}