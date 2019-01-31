using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Patterns.Observer;
using InControl;
using DG.Tweening;

public class PlayerCamera : MonoBehaviour, IObserver {

	public enum CameraMode {
		ThirdPerson,
		LockOn
	}
	private CameraMode _mode;
	public CameraMode Mode {
		get => _mode;
		set {
			_mode = value;
			switch(_mode)
			{
				case CameraMode.LockOn:
					tPCam.Priority = 0;
					lockOnCam.Priority = lockOnCam.Priority == 0 ? 1 : 0;
					lockOnCam2.Priority = lockOnCam.Priority == 0 ? 1 : 0;
					lockOnCam.LookAt = lockOnCam.Priority == 1 ? _lockOnTarget.transform : lockOnCam.LookAt;
					lockOnCam2.LookAt = lockOnCam2.Priority == 1 ? _lockOnTarget.transform : lockOnCam2.LookAt;
					break;
				case CameraMode.ThirdPerson:
					tPCam.Priority = 1;
					lockOnCam.Priority = 0;
					lockOnCam2.Priority = 0;
					break;
			}
		}
	}

    private LockOnDetector _detector;
	private Character _lockOnTarget;
	public Character LockOnTarget {
		get => _lockOnTarget;
		set {
			if(value != _lockOnTarget)
			{
				_lockOnTarget = value;
				Mode = (_lockOnTarget != null) ? CameraMode.LockOn : CameraMode.ThirdPerson;
				this.Notify(Message.Combat_LockOnTransformChange, value != null ? value.transform : null);
			}
		}
	}
    public LockOnCommand lockOn;
    public LockOnChangeCommand lockOnChange;
	private bool lockOnChanged = false;
	private float lockOnDelay = 0f;

	[SerializeField] private CinemachineBrain camBrain;
	[SerializeField] private CinemachineVirtualCamera lockOnCam;
	[SerializeField] private CinemachineVirtualCamera lockOnCam2;
	[SerializeField] private CinemachineFreeLook tPCam;

	private void Awake()
	{
		lockOnCam.gameObject.SetActive(false);
		tPCam.gameObject.SetActive(false);
	}

	private void Start()
	{
		var player = Player.instance;
        lockOn = new LockOnCommand(player);
        lockOnChange = new LockOnChangeCommand(player);
        _detector = player.GetComponentInChildren<LockOnDetector>();
		lockOnCam.Follow = player.transform;

		camBrain.transform.parent = transform.root;
		lockOnCam.transform.parent = transform.root;
		tPCam.transform.parent = transform.root;

		lockOnCam.gameObject.SetActive(true);
		tPCam.gameObject.SetActive(true);

		this.Observe(Message.Combat_Death);
        this.Observe(Message.Combat_HitEnemy);
	}

	public void RotateUpdate(float x, float y)
	{
		if(Mode == CameraMode.ThirdPerson)
		{
			tPCam.m_XAxis.m_InputAxisValue = x;
			tPCam.m_YAxis.m_InputAxisValue = y;
		}
		else if (Mode == CameraMode.LockOn)
		{
			if(lockOnChanged)
			{
				lockOnDelay += Time.deltaTime;
				if(lockOnDelay > 0.5f)
				{
					lockOnDelay = 0f;
					lockOnChanged = false;
				}
			}
			if(Mathf.Abs(x) > 0.5)
			{
				if(lockOnChange.IsValid() && !lockOnChanged)
				{
					lockOnChange.Execute(Mathf.Sign(x));
					lockOnChanged = true;
				}
			}
		}
	}

	public void SetMaxSpeed(float x, float y)
	{
        tPCam.m_XAxis.m_MaxSpeed = x;
        tPCam.m_YAxis.m_MaxSpeed = y;
    }

	public void OnNotification(object sender, Message message, params object[] args)
	{
		switch(message)
		{
			case Message.Combat_Death:
				if((UnityEngine.Object)sender == LockOnTarget || (UnityEngine.Object)sender == Player.instance)
					LockOnTarget = null;
				break;
            case Message.Combat_HitEnemy:
                break;
		}
	}

	public class LockOnCommand : PlayerCommand
	{
		public LockOnCommand(Player player) : base(player) { }

		public override bool IsValid()
		{
			return player.Camera._detector.PossibleTargets.Count > 0 || player.Camera.LockOnTarget != null;
		}

		public override void Execute()
		{
			if (!player.Camera.LockOnTarget)
				player.Camera.LockOnTarget = player.Camera.GetLockOnTarget(0f);
			else
				player.Camera.LockOnTarget = null;
		}
	}

	public class LockOnChangeCommand : PlayerCommand<float>
	{
		public LockOnChangeCommand(Player player) : base (player) { }

		public override bool IsValid()
		{
			return player.Camera._detector.PossibleTargets.Count > 1;
		}

		public override void Execute(float dir)
		{
			player.Camera.LockOnTarget = player.Camera.GetLockOnTarget(dir);
		}
	}

    public Character GetLockOnTarget(float dir)
    {
        Character closestInDir = LockOnTarget;
        float diffToCenter = 1.0f;
        _detector.RefreshTargets();
        foreach (var target in _detector.PossibleTargets)
        {
            if (target != LockOnTarget)
            {
                var viewPos = Camera.main.WorldToViewportPoint(target.transform.position);
                float diff = Mathf.Abs(viewPos.x - 0.5f);
                if (diff < diffToCenter)
                {
                    bool possibleTarget = false;
                    if ((dir == 0f) ||
                        (dir > 0f && viewPos.x > 0.5f) ||
                        (dir < 0f && viewPos.x < 0.5f))
                        possibleTarget = true;
                    closestInDir = possibleTarget ? target : closestInDir;
                    diffToCenter = possibleTarget ? diff : diffToCenter;
                }
            }

        }
        return closestInDir;
    }

}

