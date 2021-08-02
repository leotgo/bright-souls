using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BrightSouls
{
    public class StaggerBehaviour : MonoBehaviour
    {
        private Character owner;

        public int maxStaggerHealth = 100;
        private int health = 100;
        public int StaggerHealth {
            get {
                return health;
            }
            set {
                if (owner.Attributes.GetAttribute<StatusAttribute>().HasStatus(CharacterStatus.Unstoppable))
                    return;

                if(value < health)
                    staggerRecover.Start();
                health = value;
                int comp = -1 * (int)bonusHealth;
                if (health <= comp)
                {
                    owner.GetComponent<Animator>().SetTrigger("stagger");
                    staggerCounter++;
                    if (staggerCounter >= maxConsecutiveStaggers)
                    {
                        StaggerHealth = 100;
                        staggerCounter = 0;
                        owner.Attributes.GetAttribute<StatusAttribute>().AddStatus(CharacterStatus.Unstoppable);
                        staggerInvincibilityStop.Start();
                    }
                }
            }
        }

        private uint bonusHealth = 0;
        public uint BonusHealth {
            get {
                return bonusHealth;
            }
            set {
                bonusHealth = value;
            }
        }
        public uint dodgeBonusHealth = 50;

        [SerializeField] private int maxConsecutiveStaggers = 3;
        private int staggerCounter = 0;
        private float staggerRecoverDelay = 1f;
        private TimerAction staggerRecover;
        [SerializeField] private float staggerInvincibilityTime = 2.5f;
        private TimerAction staggerInvincibilityStop;

        private void Start()
        {
            owner = GetComponent<Character>();

            staggerRecover = new TimerAction(this, this.staggerRecoverDelay, () => {
                this.StaggerHealth = 100;
                owner.Attributes.GetAttribute<StatusAttribute>().RemoveStatus(CharacterStatus.Staggered);
            });
            staggerInvincibilityStop = new TimerAction(this, this.staggerInvincibilityTime, () =>
            {
                owner.Attributes.GetAttribute<StatusAttribute>().RemoveStatus(CharacterStatus.Unstoppable);
            });

            this.StaggerHealth = maxStaggerHealth;
        }
    }
}
