using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.Observer;

/// <summary>
/// Player class:
/// - Aggregates player-related components
/// - Receives input and process action
/// - Notifies messages for every Character-related component in Observer system
/// </summary>
public class Player : Character, IObserver, IInputReceiver
{
    [SerializeField] private PlayerData saveData;

    public float sense = 1.0f;

    public Animator anim;

    private PlayerCamera currentCamera;
    private ThirdPersonCamera tpCam;
    private LockOnCamera lockOnCam;
    private PhysicsController physics;
    private StaggerBehaviour stagger;
    private CombatController  combat;
    private StaminaBehaviour stamina;

    public PlayerCamera CurrentCamera { get { return currentCamera; } }

    public ThirdPersonCamera TPCamera  { get { return tpCam; } }
    public LockOnCamera lockCamera { get { return lockOnCam; } }
    public PhysicsController Physics { get { return physics; } }
    public StaggerBehaviour Stagger { get { return stagger; } }
    public CombatController  Combat  { get { return combat; } }
    public StaminaBehaviour Stamina { get { return stamina; } }
    
    private EquipmentSlot[] slots;

    private void Start()
    {
        tpCam = GetComponentInChildren<ThirdPersonCamera>();
        lockOnCam = GetComponentInChildren<LockOnCamera>();
        currentCamera = tpCam;

        anim = GetComponentInChildren<Animator>();
        physics = GetComponent<PhysicsController>();
        combat = GetComponent<CombatController>();
        stagger = GetComponent<StaggerBehaviour>();
        stamina = GetComponent<StaminaBehaviour>();

        if(!GameData.self.saveData.ContainsKey(saveData.guid))
        {
            GameData.self.saveData.Add(saveData.guid, saveData);
        }
        else
        {
            this.saveData = (PlayerData)GameData.self.saveData[saveData.guid];
        }
        this.Observe(Message.System_LoadData);
    }

    private void OnApplicationFocus(bool focus)
    {
        Cursor.lockState = (focus) ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !focus;
    }

    public void OnInputStart()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnInput()
    {
        if (Input.GetButtonDown("QuickSave"))
            this.Notify(Message.System_SaveData);
        if (Input.GetButtonDown("QuickLoad"))
            this.Notify(Message.System_LoadData);
        if (Input.GetKeyDown(KeyCode.Escape))
            this.Notify(Message.System_TogglePause);
        if (Input.GetKeyDown(KeyCode.R))
            this.Notify(Message.System_ReloadScene);
    }

    public void OnInputEnd()
    {

    }

    public void OnNotification(object sender, Message msg, params object[] args)
    {
        if(msg == Message.System_LoadData)
        {
            Debug.Log(args[0].ToString() + " to " + args[1].ToString());
            string guid = (string) args[0];
            if(saveData.guid == guid)
            {
                this.saveData = (PlayerData)args[1];
                saveData.player = this;
                Health = saveData.health;
                Stamina.Value = saveData.stamina;
                transform.position = saveData.Position;
                transform.rotation = saveData.Rotation;
            }
        }
    }

}
