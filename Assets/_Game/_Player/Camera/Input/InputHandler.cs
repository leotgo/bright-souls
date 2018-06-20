using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Patterns.Command;

namespace System.Input
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour target;
        private IInputTarget inputTarget;

        private void Awake()
        {
            Assert.IsNotNull(target, "Target is null on " + ToString());
            
        }

        [ExecuteInEditMode]
        private void OnValidate()
        {
            if (target != null)
            {
                Assert.IsTrue(target is IInputTarget, string.Format("Target {0} is not {1}", target, typeof(IInputTarget)));
                target = null;
            }
        }

        private void Update()
        {
            foreach (var c in inputTarget.Commands)
            {
                c.Execute(target);
            }
            /*if (Input.GetButtonDown("QuickSave"))
                target.Notify(Message.System_SaveData);
            if (Input.GetButtonDown("QuickLoad"))
                target.Notify(Message.System_LoadData);
            if (Input.GetKeyDown(KeyCode.Escape))
                target.Notify(Message.System_TogglePause);
            if (Input.GetKeyDown(KeyCode.R))
                target.Notify(Message.System_ReloadScene);*/
        }

    }

    public interface IInputTarget
    {
        Command[] Commands { get; }

    }
}