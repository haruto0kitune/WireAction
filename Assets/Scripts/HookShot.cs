using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class HookShot : MonoBehaviour
{
	[SerializeField]
	GameObject player;
	Rigidbody2D rigidbody2d;
	[SerializeField]
	GameObject hook;
	Hook _hook;
	Vector3 directionVectorStore;
	[SerializeField]
	GameObject hookShotDisableRange;
	GameObject returnPoint;
	[SerializeField]
	int speed;
	
    void Awake ()
    {	
	    rigidbody2d = player.GetComponent<Rigidbody2D>();
	    _hook = hook.GetComponent<Hook>();
    }

    void Start () 
	{
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
	}
		
	void DisableHook()
	{
		_hook.rigidbody2d.constraints = RigidbodyConstraints2D.FreezeRotation;
		_hook.springJoint2d.enabled = false;
		_hook.rigidbody2d.velocity = Vector2.zero;
		_hook.transform.localPosition = Vector2.zero;
		_hook.spriteRenderer.enabled = false;
		_hook.boxCollider2d.enabled = false;
		_hook.isStopping = false;
		hookShotDisableRange.GetComponent<CircleCollider2D>().enabled = false;
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
			.Do(x => Debug.Log("Enter returnPoint"))
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
}