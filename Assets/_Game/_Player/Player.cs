using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.Observer;
using Cinemachine;

/// <summary>
/// Player class:
/// - Aggregates player-related components
/// - Notifies messages for every Character-related component in Observer system
/// </summary>
public sealed class Player : Character
{

    public static Player instance;

    // Component Refs
    private PlayerInputHandler _input;
    private PlayerCamera _cam;
    private PlayerPhysics _physics;
    private StaggerBehaviour _stagger;
    private CombatController  _combat;
    private StaminaBehaviour _stamina;
    private PlayerObjectInteractor _interactor;
    [NonSerialized] public Animator anim;

    // Component Getters
    public PlayerInputHandler Input { get => _input; }
    public PlayerCamera Camera { get => _cam; }
    public PlayerPhysics Physics { get => _physics; }
    public StaggerBehaviour Stagger { get => _stagger; }
    public CombatController  Combat  { get => _combat; }
    public StaminaBehaviour Stamina { get => _stamina; }
    public PlayerObjectInteractor Interactor { get => _interactor;  }

    private void Awake()
    {
        if(instance != null)
            Destroy(instance);
        instance = this;
    }

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        _input = GetComponent<PlayerInputHandler>();
        _interactor = GetComponent<PlayerObjectInteractor>();
        _cam = FindObjectOfType<PlayerCamera>();
        _physics = GetComponent<PlayerPhysics>();
        _combat = GetComponent<CombatController>();
        _stagger = GetComponent<StaggerBehaviour>();
        _stamina = GetComponent<StaminaBehaviour>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnApplicationFocus(bool focus)
    {
        Cursor.lockState = (focus) ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !focus;
    }

}
