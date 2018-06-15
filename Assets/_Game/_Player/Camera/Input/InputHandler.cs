using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {

    [SerializeField] private GameObject target;
    public GameObject Target {
        get {
            return target;
        }
        set {
            target.SendMessage("OnInputEnd");
            target = value;
            target.SendMessage("OnInputStart");
        }
    }

    private void Start()
    {
        if (target.GetComponent<IInputReceiver>() == null)
        {
            Debug.LogError("Target does not contain an Input Receiver!");
            enabled = false;
        }
        target.SendMessage("OnInputStart");
    }

    private void Update()
    {
        target.SendMessage("OnInput");
    }

}

public interface IInputReceiver
{
    void OnInputStart();
    void OnInput();
    void OnInputEnd();
}
