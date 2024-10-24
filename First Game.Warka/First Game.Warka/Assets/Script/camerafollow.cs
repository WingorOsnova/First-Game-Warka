using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerafollow : MonoBehaviour
{
    [SerializeField] float minX, maxX, minY, maxY;
    [SerializeField] Transform target;
    [SerializeField] float followspeed;
    Animator anim;

    
    public static camerafollow instance;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    void FixedUpdate()
    {
        if (!target) return;
        
       
        
        
        transform.position = Vector3.Lerp(transform.position, new Vector3(Mathf.Clamp(target.position.x, minX, maxX),
            Mathf.Clamp(target.position.y, minY, maxY), -10), followspeed * Time.fixedDeltaTime);


            /* new Vector3(
            Mathf.Clamp(target.position.x, minX, maxX),
            Mathf.Clamp(target.position.y, minY, maxY), - 10); */   
    }

    public void CamShake()
    {
        anim.Play("Shake_Camera");
    }
}
