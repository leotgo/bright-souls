using UnityEngine;

namespace BrightSouls.Gameplay
{
    public sealed class PlayerAttributeContainer : MonoBehaviour, IAttributesContainerOwner
    {
        /* ------------------------------- Properties ------------------------------- */

        public PoiseAttribute Poise
        {
            get => attributes.GetAttribute<PoiseAttribute>();
        }

        public MaxPoiseAttribute MaxPoise
        {
            get => attributes.GetAttribute<MaxPoiseAttribute>();
        }

        public StaminaAttribute Stamina
        {
            get => attributes.GetAttribute<StaminaAttribute>();
        }

        public MaxStaminaAttribute MaxStamina
        {
            get => attributes.GetAttribute<MaxStaminaAttribute>();
        }

        public HealthAttribute Health
        {
            get => attributes.GetAttribute<HealthAttribute>();
        }

        public MaxHealthAttribute MaxHealth
        {
            get => attributes.GetAttribute<MaxHealthAttribute>();
        }

        public FactionAttribute Faction
        {
            get => attributes.GetAttribute<FactionAttribute>();
        }

        public StatusAttribute Status
        {
            get => attributes.GetAttribute<StatusAttribute>();
        }

        public AttributesContainer Attributes
        {
            get => attributes;
        }

        /* ------------------------ Inspector-Assigned Fields ----------------------- */

        [SerializeField] private PlayerAttributeData data;

        /* ----------------------------- Runtime Fields ----------------------------- */

        private AttributesContainer attributes;

        /* ------------------------------ Unity Events ------------------------------ */

        private void Start()
        {
            InitializeAttributes();
        }

        /* ----------------------------- Initialization ----------------------------- */

        private void InitializeAttributes()
        {
            var health = new HealthAttribute(data.Health);
            var maxHealth = new MaxHealthAttribute(data.MaxHealth);
            var stamina = new StaminaAttribute(data.Stamina);
            var maxStamina = new MaxStaminaAttribute(data.MaxStamina);
            var poise = new PoiseAttribute(data.Poise);
            var maxPoise = new MaxPoiseAttribute(data.MaxPoise);
            var faction = new FactionAttribute();
            var status = new StatusAttribute();

            attributes = new AttributesContainer();
            attributes.AddAttribute<HealthAttribute>(health);
            attributes.AddAttribute<MaxHealthAttribute>(maxHealth);
            attributes.AddAttribute<StaminaAttribute>(stamina);
            attributes.AddAttribute<MaxStaminaAttribute>(maxStamina);
            attributes.AddAttribute<PoiseAttribute>(poise);
            attributes.AddAttribute<MaxPoiseAttribute>(maxPoise);
            attributes.AddAttribute<FactionAttribute>(faction);
            attributes.AddAttribute<StatusAttribute>(status);
        }

        /* -------------------------------------------------------------------------- */
    }
}