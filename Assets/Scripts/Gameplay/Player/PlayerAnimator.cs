using UnityEngine;

namespace BrightSouls.Gameplay
{
    public class PlayerAnimator : MonoBehaviour
    {
        /* --------------------------------- Fields --------------------------------- */

        [SerializeField] private Player player;
        [SerializeField] private Animator anim;

        /* ------------------------- MonoBehaviour Callbacks ------------------------ */

        private void Start()
        {
            player.Combat.Events.onStagger += OnStagger;
            player.Combat.Events.onDodge += OnDodge;
            player.Combat.Events.onBlockHit += OnBlockHit;
        }

        /* ----------------------------- Event Handlers ----------------------------- */

        private void OnStagger()
        {
            anim.SetTrigger("stagger");
        }

        private void OnDodge(Vector3 dir)
        {
            anim.SetFloat("dodge_x", dir.x);
            anim.SetFloat("dodge_y", dir.z);
            anim.SetTrigger("dodge");
        }

        private void OnBlockHit()
        {
            anim.SetTrigger("block_hit");
        }

        /* -------------------------------------------------------------------------- */
    }
}