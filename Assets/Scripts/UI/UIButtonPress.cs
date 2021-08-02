using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace BrightSouls.UI
{
    public class UIButtonPress : MonoBehaviour
    {

        public enum MouseKeyboardDisplay
        {
            Mouse,
            Keyboard
        }

        private Player player;
        private Image img;
        private Color defaultColor;
        private float defaultScale;
        private Graphic[] graphicElements;
        //public PlayerActionType action;
        public MouseKeyboardDisplay mkDisplay = MouseKeyboardDisplay.Keyboard;
        public Color pressColor = Color.yellow;
        [Range(1f, 20f)]
        public float lerpSpeed = 10f;
        [Range(0f, 1f)]
        public float disabledAlpha = 0.3f;
        private bool disabled;
        public bool Disabled {
            get {
                return disabled;
            }
            set {
                disabled = value;
                foreach (var g in graphicElements)
                {
                    float a = (disabled) ? disabledAlpha : 1f;
                    //g.DOFade(a, 0.2f);
                }
            }
        }

        private void Start()
        {
            player = GetComponentInParent<Player>();
            img = GetComponent<Image>();
            graphicElements = GetComponentsInChildren<Graphic>();
            defaultColor = img.color;
            defaultScale = transform.localScale.x;
        }

        private void Update()
        {
            PlayerCommandBase cmd = GetCommand();
            if (cmd != null)
            {
                if (disabled == cmd.IsValid())
                    Disabled = !cmd.IsValid();
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
    }
}