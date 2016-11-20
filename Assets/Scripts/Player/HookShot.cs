using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class HookShot : MonoBehaviour
{
	LineRenderer lineRenderer;
	[SerializeField]
	GameObject player;
	Rigidbody2D rigidbody2d;
	ObservableStateMachineTrigger observableStateMachineTrigger;
	[SerializeField]
	GameObject hook;
	Hook _hook;
	Vector3 directionVectorStore;
	[SerializeField]
	GameObject hookShotDisableRange;
	GameObject returnPoint;
	public int speed;
	public float pullSpeed;
	Vector2 vectorStore;
	
    void Awake ()
	{	
		lineRenderer = GetComponent<LineRenderer>();
	    rigidbody2d = player.GetComponent<Rigidbody2D>();
	    observableStateMachineTrigger = player.GetComponent<Animator>().GetBehaviour<ObservableStateMachineTrigger>();
	    _hook = hook.GetComponent<Hook>();
    }

    void Start () 
	{
		//debug
		this.FixedUpdateAsObservable()
			.Subscribe(_ => rigidbody2d.AddForce(new Vector2(0, 10f)));
		
		// player_shot -> player_shot_move_upper
		observableStateMachineTrigger
			.OnStateUpdateAsObservable()
			.Where(x => x.StateInfo.IsName("player_shot"))
			.Where(x => _hook.isStopping)
			.Subscribe(_ => 
			{
				_.Animator.SetBool("isShooting", false);
				_.Animator.SetBool("isShotMoveUpper", true);
			});
		
		this.UpdateAsObservable()
			.Subscribe(_ => 
			{
				lineRenderer.SetPosition(0, player.transform.position);
				lineRenderer.SetPosition(1, _hook.transform.position);
			});
		
		// Tap or Press Button shots a hook				
		var hookShot = this.FixedUpdateAsObservable()
			.Select(x => Input.touchCount > 0)
			.Publish()
			.RefCount();		
		
		// Shot on tapped
		hookShot
			.Where(x => x && Input.GetTouch(0).phase == TouchPhase.Began)
			.ThrottleFirstFrame(3) // Prevent continuous shot
			.Subscribe(_ => 
			{
				if(!_hook.isStopping && !_hook.spriteRenderer.enabled)
				{
					Shot(Input.GetTouch(0).position);
					CreateReturnPoint(Input.GetTouch(0).position);
				}
				else if(_hook.isStopping && _hook.spriteRenderer.enabled)
				{
					DisableHook();
					DestroyReturnPoint();
				}
			});
		
		// Pull player on stopping hook
		//this.FixedUpdateAsObservable()
		//	.Where(x => _hook.isStopping)
		//	.Subscribe(_ => PullPlayer());
		
		// Disable hook
		_hook.OnCollisionEnter2DAsObservable()
			.Where(x => x.gameObject.tag != "Hookable")
			.Subscribe(_ => 
			{
				DisableHook();
				DestroyReturnPoint();
			});
	}
	
	void Shot(Vector2 touchPosition)
	{
		// Get tap position
		var _touchPosition = Camera.main.ScreenToWorldPoint(touchPosition);
		
		// Get direction vector
		directionVectorStore = _touchPosition - _hook.transform.position;
		directionVectorStore = new Vector2(directionVectorStore.x / Mathf.Sqrt(Mathf.Pow(directionVectorStore.x, 2) + Mathf.Pow(directionVectorStore.y, 2)), directionVectorStore.y / Mathf.Sqrt(Mathf.Pow(directionVectorStore.x, 2) + Mathf.Pow(directionVectorStore.y, 2)));
		
		// Initialilze hook
		_hook.rigidbody2d.constraints = RigidbodyConstraints2D.FreezeRotation;
		_hook.spriteRenderer.enabled = true;
		_hook.springJoint2d.enabled = false;
		_hook.boxCollider2d.enabled = true;
		hookShotDisableRange.GetComponent<CircleCollider2D>().enabled = false;
		
		// Shot a hook
		_hook.rigidbody2d.velocity = directionVectorStore * speed;
		
		// Draw Wire
		lineRenderer.enabled = true;
	}
		
	void DisableHook()
	{
		_hook.rigidbody2d.constraints = RigidbodyConstraints2D.FreezeRotation;
		_hook.springJoint2d.enabled = false;
		_hook.sliderJoint2d.enabled = false;
		_hook.rigidbody2d.velocity = Vector2.zero;
		_hook.transform.localPosition = Vector2.zero;
		_hook.spriteRenderer.enabled = false;
		_hook.boxCollider2d.enabled = false;
		_hook.isStopping = false;
		lineRenderer.enabled = false;
		hookShotDisableRange.GetComponent<CircleCollider2D>().enabled = false;
		vectorStore = Vector2.zero;
	}
	
	void CreateReturnPoint(Vector2 touchPosition)
	{
		// Get tap position
		var _touchPosition = Camera.main.ScreenToWorldPoint(touchPosition);
		
		// Create Return Point
		returnPoint = new GameObject();
		returnPoint.transform.position = _touchPosition;
		returnPoint.AddComponent<CircleCollider2D>();
		returnPoint.GetComponent<CircleCollider2D>().radius = 0.01f;
		returnPoint.GetComponent<CircleCollider2D>().isTrigger = true;
		
		// Disable hook and Destroy return point on enter return point trigger
		returnPoint.OnTriggerEnter2DAsObservable()
			.Where(x => x.gameObject.tag == "Hook")
			.Subscribe(_ => 
			{
				DisableHook();
				DestroyReturnPoint();
			})
			.AddTo(returnPoint);
	}
	
	void DestroyReturnPoint()
	{
		Destroy(returnPoint);
	}
	
	void PullPlayer()
	{
		Debug.Log("hook localPosition" + _hook.transform.localPosition);
		// player's position left up
		if( _hook.transform.position.x > player.transform.position.x && _hook.transform.position.y < player.transform.position.y)
		{
			rigidbody2d.AddForce(new Vector2(_hook.transform.localPosition.x * pullSpeed, _hook.transform.localPosition.y * pullSpeed));
		}
		
		// player's position left down
		if(_hook.transform.position.x > player.transform.position.x && _hook.transform.position.y > player.transform.position.y)
		{
			rigidbody2d.AddForce(new Vector2(_hook.transform.localPosition.x * pullSpeed, _hook.transform.localPosition.y * pullSpeed));
		}
		
		// player's position right up
		if(_hook.transform.position.x < player.transform.position.x && _hook.transform.position.y < player.transform.position.y)
		{
			rigidbody2d.AddForce(new Vector2(_hook.transform.localPosition.x * pullSpeed, _hook.transform.localPosition.y * pullSpeed));
		}
		
		// player's position right down
		if(_hook.transform.position.x < player.transform.position.x && _hook.transform.position.y > player.transform.position.y)
		{
			rigidbody2d.AddForce(new Vector2(_hook.transform.localPosition.x * pullSpeed, _hook.transform.localPosition.y * pullSpeed));
		}
	}
}