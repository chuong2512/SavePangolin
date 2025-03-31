using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class CarController : MonoBehaviour,IListener
{
	public ParkingColor                         parkingColor;
	public LineRenderer                         lineRenderer;
	public List<Vector3>                        listOfPoints;
	public float                                distance2points=0.05f;
	bool                                        isDrawningLine =false;
	Vector2                                     lastPoint;
	[Header("--- CAR ENGINE ---")] public float speed         =7.5f;
	public                                float accPower      =2f;
	public                                float breakPower    =3;
	public                                float steeringPower =2.5f;
	public                                float steeringSmooth=3;
	float                                       steeringAmount;
	public float                                stopDistance=1;

	int     currentPos=1;
	Vector2 posTarget;
	float   currentSpeed=0;

	bool                        isMoving               =false;
	[ReadOnly] public bool      isReadyToGo            =false;
	[ReadOnly] public bool      isReachToTheDestination=false;
	[ReadOnly] public bool      isParkedCarCorrectly   =false;
	public            Transform renderTrans;

	void Start()
	{
		lineRenderer              =GetComponent<LineRenderer>();
		listOfPoints              =new List<Vector3>();
		lineRenderer.positionCount=0;

		GameManager.Instance.RegisterCar(this);
	}

	void Update()
	{
		renderTrans.rotation=Quaternion.identity;
		if(isMoving)
		{
			if(isReachToTheDestination)
			{
				speed=0;
			}
			else
			{
				Vector3 targetDir=listOfPoints[Mathf.Min(currentPos+3,listOfPoints.Count-1)]-transform.position;
				float   angle    =Vector2.SignedAngle(targetDir,transform.up);
				steeringAmount=-Mathf.Sign(angle);

				if(Mathf.Abs(angle)<5)
					transform.up=targetDir;
				else
				{
					transform.Rotate(Vector3.forward,steeringAmount*steeringPower*transform.up.magnitude);
				}
			}

			currentSpeed=Mathf.Lerp(currentSpeed,speed,(isReachToTheDestination ? breakPower : accPower)*Time.deltaTime);
			transform.Translate(Vector2.up*currentSpeed*Time.deltaTime,Space.Self);
			return;
		}

		if(!isReadyToGo)
		{
			if(Input.GetMouseButtonDown(0))
			{
				RaycastHit2D[] hits=Physics2D.CircleCastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition),0.01f,Vector2.zero);
				if(hits.Length>0)
				{
					foreach(var hit in hits)
					{
						if(hit.collider.gameObject==gameObject)
						{
							isDrawningLine=true;
							if(HandTutorial.Instance)
								HandTutorial.Instance.StopAll();

							listOfPoints.Clear();

							lastPoint=transform.position;
							listOfPoints.Add(lastPoint);
						}
					}
				}
			}
			else if(Input.GetMouseButtonUp(0))
			{
				if(isDrawningLine)
				{
					isDrawningLine=false;
					//check if release at the target or not
					var hitTarget=Physics2D.CircleCast(Camera.main.ScreenToWorldPoint(Input.mousePosition),0.1f,Vector2.zero);
					if(hitTarget.collider!=null && hitTarget.collider.gameObject.CompareTag("Target"))
					{
						StartCoroutine(Action());
						//isMoving = true;
						isReadyToGo=true;
						GameManager.Instance.CarLineConnected();
					}
					else
					{
						lineRenderer.positionCount=0;
					}
				}
			}

			if(isDrawningLine)
			{
				var mouseWorldPos=Camera.main.ScreenToWorldPoint(Input.mousePosition);
				if(Vector2.Distance(mouseWorldPos,lastPoint)>distance2points)
				{
					lastPoint=mouseWorldPos;
					listOfPoints.Add(lastPoint);
				}

				lineRenderer.positionCount=listOfPoints.Count;
				lineRenderer.SetPositions(listOfPoints.ToArray());
			}
		}
	}

	public IEnumerator Action()
	{
		while (!isMoving)
			yield return null;

		List<Vector3> _drawPoints=new List<Vector3>(listOfPoints);

		for(int i=0;i<listOfPoints.Count;i++)
		{
			if(Vector2.Distance(transform.position,listOfPoints[i])<2)
			{
				_drawPoints.RemoveAt(0);
				UpdateLine(_drawPoints);
				currentPos++;
			}
			else
				break;
		}

		while (currentPos<listOfPoints.Count)
		{
			posTarget=listOfPoints[currentPos];
			while ((Vector2.Distance(transform.position,posTarget)>stopDistance)
			       && (Vector2.Distance(transform.position,posTarget)<
			           Vector2.Distance(transform.position,listOfPoints[currentPos+1]))) //wait until the car reach to the destination
			{
				yield return null;
			}

			_drawPoints.RemoveAt(0);
			UpdateLine(_drawPoints);
			currentPos++;
			yield return null;
		}

		ReachedTheTarget();
	}

	void ReachedTheTarget()
	{
		lineRenderer.positionCount=0;
		isReachToTheDestination   =true;
		isParkedCarCorrectly      =isParkCarCorrectly();
		GameManager.Instance.CheckCarFinish();
	}

	void UpdateLine(List<Vector3> list)
	{
		lineRenderer.positionCount=list.Count;
		lineRenderer.SetPositions(list.ToArray());
	}

	ParkingTarget parkingTarget;
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(canStopNow())
		{
			parkingTarget=collision.gameObject.GetComponent<ParkingTarget>();
			if(parkingTarget!=null)
			{
				StopAllCoroutines();
				ReachedTheTarget();
			}
		}
	}

	bool canStopNow()
	{
		for(int i=currentPos;i<listOfPoints.Count;i++)
		{
			if(Vector2.Distance(transform.position,listOfPoints[i])>(3.5f)) //if all the rest of points in the
				return false;
		}

		return true;
	}

	//check if the car parked same direction and same color or not
	bool isParkCarCorrectly()
	{
		if(parkingTarget==null)
			return false; //if somehow it doesn't stay in the target, false

		if(parkingTarget!=null && parkingTarget.parkingColor!=parkingColor)
			return false; //if different color, false

		/*float angle=Vector2.SignedAngle(parkingTarget.transform.up,transform.up);
		if(Mathf.Abs(angle)>15) //if the angle of the car and parking larger than this value => false
			return false;*/

		return true; // car parking correctly!
	}

	public void IPlay()
	{
		if(isReadyToGo)
			isMoving=true;
	}

	public void ISuccess() { }

	public void IPause() { }

	public void IUnPause() { }

	public void IGameOver() { }

	public void IOnRespawn() { }

	public void IOnStopMovingOn() { }

	public void IOnStopMovingOff() { }

	public GameObject destroyFx;
	public AudioClip  soundDestroy;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(destroyFx)
			Instantiate(destroyFx,transform.position,destroyFx.transform.rotation);

		SoundManager.PlaySfx(soundDestroy);

		GameManager.Instance.GameOver();

		gameObject.SetActive(false);
	}
}
