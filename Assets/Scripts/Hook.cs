using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class Hook : MonoBehaviour 
{
	[System.NonSerialized]
	public Rigidbody2D rigidbody2d;
	[System.NonSerialized]
	public SpringJoint2D springJoint2d;
	[System.NonSerialized]
	public SliderJoint2D sliderJoint2d;
	[System.NonSerialized]
	public SpriteRenderer spriteRenderer;
	[System.NonSerialized]
	public BoxCollider2D boxCollider2d;
	[System.NonSerialized]
	public bool isStopping;
	
	
    void Awake ()
    {
	    rigidbody2d = GetComponent<Rigidbody2D>();
	    springJoint2d = GetComponent<SpringJoint2D>();
	    sliderJoint2d = GetComponent<SliderJoint2D>();
	    spriteRenderer = GetComponent<SpriteRenderer>();
	    boxCollider2d = GetComponent<BoxCollider2D>();
    }

    void Start () 
	{
		// Stop on stuck hookable object
	    this.OnTriggerEnter2DAsObservable()
		    .Where(x => x.gameObject.tag == "Hookable")
		    .Subscribe(_ => Stop());
    }
	
	void Stop()
	{
		rigidbody2d.isKinematic = false;
		rigidbody2d.velocity = Vector2.zero;
		rigidbody2d.constraints = RigidbodyConstraints2D.FreezeAll;
		springJoint2d.enabled = true;
		//sliderJoint2d.enabled = true;
		isStopping = true;
	}
}
