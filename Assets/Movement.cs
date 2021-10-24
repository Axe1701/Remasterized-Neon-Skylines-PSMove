/*
 * Written by Maxi Levi <maxilevi@live.com>, November 2017
 */


using UnityEngine;
using System.Collections;
using Assets;
using Assets.Generation;
using System.Collections.Generic;
using System;

public class Movement : MonoBehaviour {
    private GameObject moveControllerPrefab;
    public float Speed = 16;
	public float TurnSpeed = 3f;
	public TrailRenderer LeftTrail, RightTrail;
	public Color TrailColor;
	public Vector3 LeftPosition, RightPosition;
	public Material TrailMaterial;
	public AudioSource LeftSource, RightSource;
	public AudioClip SwooshClip;
	private GameObject Debris;
	private bool _lock;
	private float _leftTargetVolume = 1, _rightTargetVolume = 1;
	private float _originalVolume;
	private float _speed = 0;
    private IntPtr tracker;

    private List<UniMoveController> moves = new List<UniMoveController>();

    public bool IsInSpawn{
		get{ return (transform.parent.position - WorldGenerator.SpawnPosition).sqrMagnitude < WorldGenerator.SpawnRadius * WorldGenerator.SpawnRadius; }
	}
    void Start(){
        moveControllerPrefab = GameObject.Find("MoveController");
        Destroy(moveControllerPrefab);
        
        int count = UniMoveController.GetNumConnected();

        //UniMoveController.PSMoveTrackerSettings settings;
        //unsafe{
        //    UniMoveController.psmove_tracker_settings_set_default(&settings);
        //}
        //settings.color_mapping_max_age = 0;
        //settings.exposure_mode = PSMoveTracker_Exposure.Exposure_LOW;
        //settings.camera_mirror = PSMove_Bool.PSMove_True;
        //unsafe{
        //    tracker = UniMoveController.psmove_tracker_new_with_settings(&settings);
        //}

        for (int i = 0; i < count; i++)
        {
            UniMoveController move = gameObject.AddComponent<UniMoveController>();  // It's a MonoBehaviour, so we can't just call a constructor

            // Remember to initialize!
            if (!move.Init(i))
            {
                Destroy(move);  // If it failed to initialize, destroy and continue on
                continue;
            }

            // This example program only uses Bluetooth-connected controllers
            PSMoveConnectionType conn = move.ConnectionType;
            if (conn == PSMoveConnectionType.Unknown || conn == PSMoveConnectionType.USB)
            {
                Destroy(move);
            }
            else
            {
                moves.Add(move);

               
                move.InitOrientation();
                move.ResetOrientation();

                //for ( ; ; ) {
                //    PSMoveTracker_Status result = UniMoveController.psmove_tracker_enable(tracker, moves[i].handle);
                //    Debug.Log("Trackeando...");
                //    if (result == PSMoveTracker_Status.Tracker_CALIBRATED){
                //        break;
                //    }
                //}

                // Start all controllers with a white LED
                move.SetLED(Color.red);

                // adding the MoveController Objects on screen
                //GameObject moveController = GameObject.Instantiate(moveControllerPrefab,
                //  Vector3.right * count * 2 + Vector3.left * i * 4, Quaternion.identity) as GameObject;
            }
        }
        
        Debris = GameObject.FindGameObjectWithTag ("Debris");
		LeftSource = GameObject.FindGameObjectWithTag ("LeftSource").GetComponent<AudioSource>();
		RightSource = GameObject.FindGameObjectWithTag ("RightSource").GetComponent<AudioSource>();
		_originalVolume = (RightSource.volume + LeftSource.volume) * .5f;
	}

	public void Lock(){
		_lock = true;
	}

	public void Unlock(){
		_lock = false;
	}

	// Update is called once per frame
	void Update () {
		LeftSource.volume = Mathf.Lerp (LeftSource.volume, _originalVolume * _leftTargetVolume, Time.deltaTime * 2f);
		RightSource.volume = Mathf.Lerp (RightSource.volume, _originalVolume * _rightTargetVolume, Time.deltaTime * 2f);

		if (LeftSource.volume < 0.05f)
			LeftSource.Stop ();

		if (RightSource.volume < 0.05f)
			RightSource.Stop ();

		if (_lock)
			return;

		_speed = Mathf.Lerp (_speed, Speed, Time.deltaTime * .25f);
		transform.parent.position += transform.forward * Time.deltaTime * 4 * _speed;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(Vector3.zero), Time.deltaTime * 2.5f);

		float zAngle = transform.localRotation.eulerAngles.z;
		float xAngle = transform.localRotation.eulerAngles.x;
		if (zAngle > 45 && zAngle < 135 || zAngle > 225 && zAngle < 315 || xAngle > 45 && xAngle < 90 || xAngle > 270 && xAngle < 315) {
			StartTrail (ref LeftTrail, LeftPosition);
			LeftSource.transform.position = LeftPosition;
			LeftSource.clip = SwooshClip;
			_leftTargetVolume = 1;
			if(!LeftSource.isPlaying)
				LeftSource.Play ();
		} else {
			StopTrail (ref LeftTrail);
			_leftTargetVolume = 0;
		}
		
		if (zAngle > 45 && zAngle < 135 || zAngle > 225 && zAngle < 315 || xAngle > 45 && xAngle < 90 || xAngle > 270 && xAngle < 315) {
			StartTrail (ref RightTrail, RightPosition);
			RightSource.transform.position = RightPosition;
			RightSource.clip = SwooshClip;
			_rightTargetVolume = 1;
			if(!RightSource.isPlaying)
				RightSource.Play ();
		} else {
			StopTrail (ref RightTrail);
			_rightTargetVolume = 0;
		}
        UniMoveController.psmove_tracker_update_image(tracker);
        UniMoveController.psmove_tracker_update(tracker, IntPtr.Zero);

        float pendiente = 0;
        float rect_x = 0;
        float rect_y = 0;
        float rect_b = 0;

        float axis_move_1_x = moves[0].Acceleration.x;
        float axis_move_1_y = moves[0].Acceleration.y;
        float axis_move_1_z = moves[0].Acceleration.z;
        float axis_move_2_x = moves[1].Acceleration.x;
        float axis_move_2_y = moves[1].Acceleration.y;
        float axis_move_2_z = moves[1].Acceleration.z;

        //float aux1_x = 0, aux1_y = 0, aux1_z = 0;
        //float aux2_x = 0, aux2_y = 0, aux2_z = 0;
        //UniMoveController.psmove_tracker_get_position(tracker, moves[0].handle, ref aux1_x, ref aux1_y, ref aux1_z);
        //UniMoveController.psmove_tracker_get_position(tracker, moves[1].handle, ref aux2_x, ref aux2_y, ref aux2_z);
        //float axis_move_1_x = aux1_x;
        //float axis_move_1_y = aux1_y;
        //float axis_move_1_z = aux1_z;
        //float axis_move_2_x = aux2_x;
        //float axis_move_2_y = aux2_y;
        //float axis_move_2_z = aux2_z;

        float rotate_move_1_x = moves[0].Gyro.x;
        float rotate_move_1_y = moves[0].Gyro.y;
        float rotate_move_1_z = moves[0].Gyro.z;
        float rotate_move_2_x = moves[1].Gyro.x;
        float rotate_move_2_y = moves[1].Gyro.y;
        float rotate_move_2_z = moves[1].Gyro.z;
        //Debug.Log("x:");
        //Debug.Log(moves[0].Position.x);
        //Debug.Log("y:");
        //Debug.Log(moves[0].Position.y);
        //Debug.Log("z:");
        //Debug.Log(moves[0].Position.z);

        //Debug.Log("x:");
        //Debug.Log(moves[0].Gyro.x);
        //Debug.Log("y:");
        //Debug.Log(moves[0].Gyro.y);
        //Debug.Log("z:");
        //Debug.Log(moves[0].Gyro.z);
        //float distancia = 10;
        pendiente = (axis_move_2_y - axis_move_1_y) / (axis_move_2_x - axis_move_1_x);
        rect_y = pendiente * rect_x + rect_b;
        float hAxis = 0;
        float vAxis = 0; ;
        if(pendiente > -1 || pendiente < 1)
        {
            hAxis = 0;
            vAxis = 0;
        }
        if(pendiente > 0)
        {
            hAxis = -1;
        }
        else if(pendiente < 0)
        {
            hAxis = 1;
        }
        if(rotate_move_1_x > 0 || rotate_move_2_x > 0)
        {
            vAxis = 1;
        }
        if (rotate_move_1_x < 0 || rotate_move_2_x < 0){
            vAxis = -1;
        }
		float scale = (Time.timeScale != 1) ? (1 / Time.timeScale) * .5f : 1;
		//float hAxis = Input.GetAxisRaw("Horizontal");
		//float vAxis = Input.GetAxisRaw("Vertical");

		if(Options.Invert)
			transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles + Vector3.right * Time.deltaTime * 64f * TurnSpeed * scale * vAxis);
		else
			transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles + Vector3.right * Time.deltaTime * 64f * TurnSpeed * scale * -vAxis);


		transform.localRotation = Quaternion.Euler (transform.localRotation.eulerAngles + Vector3.forward * Time.deltaTime * 64f * TurnSpeed * scale * -hAxis);
		transform.parent.Rotate (-Vector3.up * Time.deltaTime * 64f * TurnSpeed * scale * -hAxis);

	}

	void StartTrail(ref TrailRenderer Trail, Vector3 Position){
		if (Trail != null)
			return;
		GameObject go = new GameObject ("Trail");
		go.transform.parent = this.gameObject.transform;
		Trail = go.AddComponent<TrailRenderer> ();
		Trail.widthMultiplier = .25f;
		Trail.endColor = new Color (0, 0, 0, 0);
		Trail.startColor = TrailColor;
		Trail.transform.localPosition = Position;
		Trail.material = TrailMaterial;
		Trail.time = 1.5f;
	}

	void StopTrail(ref TrailRenderer Trail){
		if (Trail == null)
			return;

		Trail.transform.parent = (Debris != null) ?  Debris.transform : null;
		Destroy (Trail.gameObject, Trail.time+1);
		Trail = null;
	}


}
