using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class UIButtonPress : MonoBehaviour
{

    public enum ButtonType
    {
        Key,
        Button
    }

    public enum CommandType
    {
        LightAttack,
        HeavyAttack,
        Dodge,
        Block,
        LockOn
    }

    private Player player;
    private Image img;
    private Color defaultColor;
    private float defaultScale;
    private Graphic[] graphicElements;
    public ButtonType type;
    public CommandType commandType;
    public bool IsKey {
        get {
            return type == ButtonType.Key;
        }
    }
    [ShowIf("IsKey")]
    public KeyCode code;
    [HideIf("IsKey")]
    public string buttonName = string.Empty;
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
                g.color = new Color(g.color.r, g.color.b, g.color.b, a);
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
            bool pressed = (type == ButtonType.Key) ? Input.GetKey(code) : Input.GetButton(buttonName);
            Color targetColor = (pressed) ? pressColor : defaultColor;
            float targetScale = (pressed) ? 1.2f * defaultScale : defaultScale;
            img.color = Color.Lerp(img.color, targetColor, Time.deltaTime * lerpSpeed);
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * targetScale, Time.deltaTime * lerpSpeed);
        }
    }

    private PlayerCommandBase GetCommand()
    {
        switch(commandType)
        {
            case CommandType.LightAttack:
                return player.Combat.attack;
            case CommandType.HeavyAttack:
                return player.Combat.attack;
            case CommandType.Block:
                return player.Combat.block;
            case CommandType.LockOn:
                return player.Combat.lockOn;
        }
        return null;
    }
}
