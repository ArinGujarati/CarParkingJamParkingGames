using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public static Wall wall;
    public Animator WallAnimator;
    public bool WallAnimationOn;
    private void Awake()
    {
        wall = this;
        WallAnimator = GetComponent<Animator>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Car")
        {
            //Debug.Log(collision.gameObject.GetComponent<Car>().CarColideCar.Equals(true) + " :: " + collision.gameObject.GetComponent<Car>().IsRightSwipe + " :: " + collision.gameObject.GetComponent<Car>().IsDownSwipe);
            if (collision.gameObject.GetComponent<Car>().CarColideCar && collision.gameObject.GetComponent<Car>().IsRightSwipe || collision.gameObject.GetComponent<Car>().CarColideCar && collision.gameObject.GetComponent<Car>().IsLeftSwipe)
            {              
                WallAnimationOn = true;
                WallAnimator.SetBool("Left", true);
                WallAnimator.SetBool("Right", false);
            }
            if (collision.gameObject.GetComponent<Car>().CarColideCar && collision.gameObject.GetComponent<Car>().IsDownSwipe)
            {              
                WallAnimationOn = true;
                WallAnimator.SetBool("Left", false);
                WallAnimator.SetBool("Right", true);
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Car")
        {
            if (WallAnimationOn)
            {
                collision.gameObject.GetComponent<Car>().Rb.isKinematic = true;
                collision.gameObject.GetComponent<Car>().CarColideCar = false;
                Invoke("AfterCarAnimationOff", 0.1f);
            }
        }
    }
    void AfterCarAnimationOff()
    {
        if (WallAnimationOn)
        {
            WallAnimator.SetBool("Left", false);
            WallAnimator.SetBool("Right", false);
        }
    }
}
