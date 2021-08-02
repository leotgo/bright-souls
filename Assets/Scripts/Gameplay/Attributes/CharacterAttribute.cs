using UnityEngine;

namespace BrightSouls
{
    public interface ICharacterAttribute { }

    public abstract class CharacterAttribute<T> : ICharacterAttribute
    {
        public delegate void OnAttributeChanged(T oldValue, T newValue);
        public event OnAttributeChanged onAttributeChanged;

        public CharacterAttribute()
        {
            this.value = default(T);
        }

        public CharacterAttribute(T value)
        {
            this.value = value;
        }

        public T Value
        {
            get { return value; }
            set
            {
                onAttributeChanged.Invoke(this.value, value);
                this.value = value;
            }
        }
        [SerializeField] private T value = default(T);
    }
}