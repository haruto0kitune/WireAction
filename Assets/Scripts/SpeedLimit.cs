using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class SpeedLimit : MonoBehaviour 
{
	private Rigidbody2D rigidbody2d;
	public int speed;
	
    void Awake ()
    {
	    rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void Start () 
    {
	    this.FixedUpdateAsObservable()
		    .Where(x => rigidbody2d.velocity.y > speed)
		    .Subscribe(_ => rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, speed));
	    
	    this.FixedUpdateAsObservable()
		    .Where(x => rigidbody2d.velocity.y < -speed)
		    .Subscribe(_ => rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, -speed));
	    
	    this.FixedUpdateAsObservable()
		    .Where(x => rigidbody2d.velocity.x > speed)
		    .Subscribe(_ => rigidbody2d.velocity = new Vector2(speed, rigidbody2d.velocity.y));
	    
	    this.FixedUpdateAsObservable()
		    .Where(x => rigidbody2d.velocity.x < -speed)
		    .Subscribe(_ => rigidbody2d.velocity = new Vector2(-speed, rigidbody2d.velocity.y));
    }
}
