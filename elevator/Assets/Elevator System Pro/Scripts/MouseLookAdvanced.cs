using UnityEngine;

[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLookAdvanced : MonoBehaviour
{
	public float sensitivityX = 5F;
	public float sensitivityY = 5F;

	public float minimumX = -360F;
	public float maximumX = 360F;

	public float minimumY = -90F;
	public float maximumY = 90F;

	public float smoothSpeed = 20F;

	float verticalAcceleration = 0f;

	float rotationX = 0F;
	float smoothRotationX = 0F;
	float rotationY = 0F;
	float smoothRotationY = 0F;
	Vector3 vMousePos;
	public float Speed = 100f;
	//bool bActive = false;

	void Start()
	{
		rotationY = -transform.localEulerAngles.x;
		rotationX = transform.localEulerAngles.y;
		smoothRotationX = transform.localEulerAngles.y;
		smoothRotationY = -transform.localEulerAngles.x;
	}

	bool IsCursorLock
	{
		get { return Cursor.lockState == CursorLockMode.Locked; }
		set
		{
			Cursor.lockState = value ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	Vector3 sp, ep;
	float dist;
	bool capt = false, captRot = false;
	void FixedUpdate()
	{
		if (Input.touchCount > 0)
		{
			if (Input.touchCount == 1 && !captRot)
			{
				sp = Input.touches[0].position;
				captRot = true;
			}
			if (Input.touchCount == 1 && captRot)
			{
				Vector2 dir = (Vector2)sp - Input.touches[0].position;
				transform.Rotate(dir.normalized * 5);
			}
			if (captRot && (Input.touchCount == 0 || Input.touches[0].phase == TouchPhase.Ended))
			{
				captRot = false;
			}
			if (Input.touchCount > 1 && !capt)
			{
				sp = Input.touches[0].position;
				ep = Input.touches[1].position;
				dist = Vector3.Distance(sp, ep);
				capt = true;
			}
			if (capt)
			{
				if (Input.touchCount == 0)
				{
					capt = false;
					return;
				}
				if (Input.touches[0].phase == TouchPhase.Ended)
				{
					capt = false;
					return;
				}
				float dist2 = Vector3.Distance(Input.touches[0].position, Input.touches[1].position);
				if (dist2 > dist)
				{
					Vector3 inputM = new Vector3(0, 0, dist2 - dist);
					Vector3 inputMove = transform.rotation * inputM;
					transform.position += inputMove * Time.smoothDeltaTime;
				}
				if (dist2 < dist)
				{
					Vector3 inputM = new Vector3(0, 0, (dist2 - dist));
					Vector3 inputMove = transform.rotation * inputM;
					transform.position += inputMove * Time.smoothDeltaTime;
				}
			}
			return;
		}
		verticalAcceleration = 0.0f;

		if (Input.GetMouseButtonDown(1))
		{
			IsCursorLock = !IsCursorLock;
		}

		if (Input.GetKey(KeyCode.Space)) { verticalAcceleration = 1.0f; }
		if (Input.GetKey(KeyCode.LeftShift)) { verticalAcceleration = -1.0f; }


		if (!IsCursorLock)
			return;

		rotationX += Input.GetAxis("Mouse X") * sensitivityX;
		rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
		rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

		// smooth mouse look
		smoothRotationX += (rotationX - smoothRotationX) * smoothSpeed * Time.smoothDeltaTime;
		smoothRotationY += (rotationY - smoothRotationY) * smoothSpeed * Time.smoothDeltaTime;

		// transform camera to new direction
		transform.localEulerAngles = new Vector3(-smoothRotationY, smoothRotationX, 0);

		// handle camera movement via controller
		Vector3 inputMag = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		Vector3 inputMoveDirection = transform.rotation * inputMag;
		transform.position += inputMoveDirection * Speed * Time.smoothDeltaTime;
		transform.position += new Vector3(0f, (Speed / 2f) * verticalAcceleration * Time.smoothDeltaTime, 0);

		//transform.position += Vector3.up * (Input.GetAxis("VerticalOffset") * 10.0f * Time.smoothDeltaTime);
		transform.position += (transform.rotation * Vector3.forward) * Input.GetAxis("Mouse ScrollWheel") * 200.0f;
	}
}
