using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrightSouls.Audio
{
    [CreateAssetMenu(fileName = "SFXMap_Entity", menuName = "Sound/SFXMap", order = 1)]
    public class SFXMap : ScriptableObject
    {
        public List<SoundEffect> mappings = new List<SoundEffect>(1);

        private Dictionary<SFXName, SoundEffect> _sfxMap;

        public SoundEffect GetSFX(SFXName effect)
        {
            if (_sfxMap == null)
                this.InitializeDictionary();

            SoundEffect sfx;
            if (_sfxMap.TryGetValue(effect, out sfx))
                return sfx;
            else
            {
                Debug.LogError("Sound Effect not found for effect type " + effect);
                return null;
            }
        }

        private void InitializeDictionary()
        {
            _sfxMap = new Dictionary<SFXName, SoundEffect>();
            foreach (var m in mappings)
                _sfxMap.Add(m.effect, m);
        }
    }
}
