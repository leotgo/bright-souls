using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using BrightSouls.Player;

namespace BrightSouls.UI
{
    public class UIButtonPress : MonoBehaviour
    {
        /* ------------------------------- Definitions ------------------------------ */

        public enum MouseKeyboardDisplay
        {
            Mouse,
            Keyboard
        }

        /* ------------------------------- Properties ------------------------------- */

        public bool Disabled
        {
            get
            {
                return disabled;
            }
            set
            {
                disabled = value;
                foreach (var g in graphicElements)
                {
                    float a = (disabled) ? disabledAlpha : 1f;
                    //g.DOFade(a, 0.2f);
                }
            }
        }

        /* ------------------------ Inspector-Assigned Fields ----------------------- */

        [SerializeField] private PlayerComponentIndex player;
        [SerializeField] private Image img;
        [SerializeField] private Graphic[] graphicElements;
        [SerializeField] private MouseKeyboardDisplay mkDisplay = MouseKeyboardDisplay.Keyboard;
        [SerializeField] private Color pressColor = Color.yellow;
        //[SerializeField] private PlayerActionType action;
        [SerializeField] [Range(1f, 20f)] private float lerpSpeed = 10f;
        [SerializeField] [Range(0f, 1f)] private float disabledAlpha = 0.3f;

        /* ----------------------------- Runtime Fields ----------------------------- */

        private Color defaultColor;
        private float defaultScale;
        private bool disabled;

        /* ------------------------------ Unity Events ------------------------------ */

        private void Start()
        {
            defaultColor = img.color;
            defaultScale = transform.localScale.x;
        }

        private void Update()
        {
            PlayerCommandBase cmd = GetCommand();
            if (cmd != null)
            {
                if (disabled == cmd.CanExecute())
                    Disabled = !cmd.CanExecute();
            }
            else
                disabled = false;
            if (!disabled)
            {

                bool pressed = CheckPressed();
                Color targetColor = (pressed) ? pressColor : defaultColor;
                float targetScale = (pressed) ? 1.2f * defaultScale : defaultScale;
                img.color = Color.Lerp(img.color, targetColor, Time.deltaTime * lerpSpeed);
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * targetScale, Time.deltaTime * lerpSpeed);
            }
        }

        /* --------------------------------- Helpers -------------------------------- */

        private PlayerCommandBase GetCommand()
        {
            /*switch (action)
            {
                case PlayerActionType.Attack:
                    return player.Combat.attack;
                case PlayerActionType.Dodge:
                    return player.Combat.dodge;
                case PlayerActionType.Block:
                    return player.Combat.block;
                case PlayerActionType.LockOn:
                    return player.Camera.lockOn;
                case PlayerActionType.Interact:
                    return player.Interactor.interact;
            }*/
            return null;
        }

        private bool CheckPressed()
        {
            return false;//return player.Input.IsPressingAction(action);
        }

        /* -------------------------------------------------------------------------- */
    }
}