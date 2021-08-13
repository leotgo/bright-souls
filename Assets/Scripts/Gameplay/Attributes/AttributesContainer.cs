using System.Collections.Generic;
using UnityEngine;

namespace BrightSouls
{
    [System.Serializable]
    public class AttributesContainer
    {
        private bool initialized = false;
        private Dictionary<System.Type, ICharacterAttribute> attributeMap;

        [SerializeReference]
        private List<ICharacterAttribute> serializedAttributes;

        private void Initialize()
        {
            attributeMap = new Dictionary<System.Type, ICharacterAttribute>();
            foreach(var attribute in serializedAttributes)
            {
                attributeMap.Add(attribute.GetType(), attribute);
            }
            initialized = true;
        }

        public T GetAttribute<T>() where T : ICharacterAttribute
        {
            if(!initialized)
            {
                Initialize();
            }

            bool hasAttribute = attributeMap.TryGetValue(typeof(T), out var attribute);
            if(hasAttribute)
            {
                return (T)attribute;
            }
            else
            {
                Debug.LogError($"Error: Attribute of type {typeof(T)} not found in {this}. Returning default value for type {typeof(T)}.");
                return default(T);
            }
        }

        public void AddAttribute<T>(T value) where T : ICharacterAttribute
        {
            attributeMap.Add(typeof(T), value);
        }
    }
}