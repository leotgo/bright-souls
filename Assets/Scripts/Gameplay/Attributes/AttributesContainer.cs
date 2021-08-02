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
            foreach(var a in serializedAttributes)
            {
                attributeMap.Add(a.GetType(), a);
            }
            initialized = true;
        }

        public T GetAttribute<T>() where T : ICharacterAttribute
        {
            if(!initialized)
                Initialize();

            bool hasAttribute = attributeMap.TryGetValue(typeof(T), out var attribute);
            if(hasAttribute)
                return (T)attribute;
            else
            {
                Debug.LogErrorFormat("Error: Attribute of type {0} not found in {1}. Returning default value for type {0}.", typeof(T), this);
                return default(T);
            }
        }

        public void AddAttribute<T>(T value) where T : ICharacterAttribute
        {
            attributeMap.Add(typeof(T), value);
        }
    }
}