using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
public class Car : MonoBehaviour
{
    public static Car car;
    public bool ISCarTouchRoad, IsTap, IsDrag;
    public NavMeshAgent navMeshAgent;
    public Transform CarOutGamePos;
    public BoxCollider boxCollider;
    public int totalCar;
    int Speed = 12;
    Vector3 StartPos, EndPos;
    public Rigidbody Rb;
    public GameObject ExitGate;
    public bool Left, Right, Up, Down;
    public bool Istraveling, TempTraveling, CarColideCar, DragLimite;
    public bool IsRightSwipe, IsLeftSwipe, IsUpSwipe, IsDownSwipe, CarAnimationOn, CarTouchFirstRoadToGoFinal;
    public Animator CarAnimator;
    Vector3 travelingDirection;
    Vector3 stratposcar;
    public const float CarTouchThing = 0.3f;
    public const float CarTouchRoadCarPos = 0.5f;
    public const float CarPuchBackColider = 0.5f;

    private void OnEnable()
    {
        ISCarTouchRoad = false;
    }
    private void OnDisable()
    {
        ISCarTouchRoad = false;
    }
    private void Start()
    {
        //stratposcar = this.gameObject.transform.position;

        car = this;
        boxCollider = GetComponent<BoxCollider>();
        Rb = GetComponent<Rigidbody>();
        Rb.isKinematic = true;
        navMeshAgent = GetComponent<NavMeshAgent>();
        CarAnimator = GetComponent<Animator>();
        navMeshAgent.enabled = false;
        DragLimite = true;
    }
    private void OnMouseDown()
    {
        if (!TempTraveling && GameManager.gameManager.IsGamePause.Equals(false))
        {

            IsRightSwipe = IsLeftSwipe = IsUpSwipe = IsDownSwipe = false;
            IsTap = true;
            StartPos = Input.mousePosition;
        }
    }
    private void OnMouseDrag()
    {
        if (!TempTraveling && DragLimite)
        {
            Rb.isKinematic = false;
            IsDrag = true;
            EndPos = Input.mousePosition - StartPos;
        }
    }
    private void OnMouseUp()
    {
        StartPos = Vector3.zero;
        EndPos = Vector3.zero;
        Istraveling = false;
        IsTap = false;
        IsDrag = false;
    }
    private void FixedUpdate()
    {
        RotateStay();
        if (Rb.velocity.magnitude < 0.9f)
        {
            TempTraveling = false;
        }
        else if (Rb.velocity.magnitude > 0.9f)
        {
            DragLimite = false;
        }
        if (ISCarTouchRoad)
        {
            //navMeshAgent.enabled = true;
            // navMeshAgent.SetDestination(CarOutGamePos.transform.position);
        }
        if (Istraveling)
            this.GetComponent<Rigidbody>().velocity = Speed * travelingDirection;

        if (EndPos.magnitude > 0 && !TempTraveling && !CarColideCar)
        {
            float x = EndPos.x;
            float y = EndPos.y;
            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                if (x > 0)
                {
                    if (Right && IsDrag.Equals(true) && ISCarTouchRoad.Equals(false) && !TempTraveling)
                    {
                        IsLeftSwipe = true;
                        SetDirection(Vector3.right);

                    }
                    //Left
                }
                else
                {
                    if (Left && IsDrag.Equals(true) && ISCarTouchRoad.Equals(false) && !TempTraveling)
                    {
                        IsRightSwipe = true;
                        SetDirection(Vector3.left);

                    }
                    //Right
                }
            }
            else
            {
                if (y < 0)
                {
                    if (Down && IsDrag.Equals(true) && ISCarTouchRoad.Equals(false) && !TempTraveling)
                    {
                        IsDownSwipe = true;
                        SetDirection(Vector3.back);

                    }

                    //Down
                }
                else
                {
                    if (Up && IsDrag.Equals(true) && ISCarTouchRoad.Equals(false) && !TempTraveling)
                    {
                        IsUpSwipe = true;
                        SetDirection(Vector3.forward);

                    }
                    //Up
                }
            }
        }

    }
    public void SetDirection(Vector3 direction)
    {
        travelingDirection = direction;
        Istraveling = true;
        TempTraveling = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Car")
        {
            Rb.isKinematic = true;
            Rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
        }
        if (collision.gameObject.tag == "Car" || collision.gameObject.tag == "Wall")
        {
            if (TempTraveling)
            {
                CarColideCar = true;
                stratposcar = this.gameObject.transform.position;
                if (CarColideCar && IsRightSwipe)
                {
                    Vibration.Vibrate(50);
                    DDOL.dOL.PlaySound("CarHorn");
                    //Debug.Log("Right");
                    stratposcar = new Vector3(stratposcar.x + CarTouchThing, stratposcar.y, stratposcar.z);
                    this.transform.DOMove(stratposcar, CarPuchBackColider);
                    GameObject temppartical = Instantiate(GameManager.gameManager.SmokePartical.gameObject);
                    Vector3 tempparticalpos = stratposcar + new Vector3(-1.2f, 0, 0);
                    temppartical.transform.position = tempparticalpos;
                    Destroy(temppartical, 1);
                    if (collision.gameObject.transform.eulerAngles == new Vector3(0, 90, 0))
                    {
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Right", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Left", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Up", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Down", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("DownSwipe", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("UpSwipe", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("RightSwipe", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("LeftSwipe", true);
                        collision.gameObject.GetComponent<Car>().CarAnimationOn = true;
                    }
                    else if (collision.gameObject.transform.eulerAngles == new Vector3(0, 0, 0))
                    {
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Right", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Left", true);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Up", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Down", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("DownSwipe", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("UpSwipe", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("RightSwipe", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("LeftSwipe", false);
                        collision.gameObject.GetComponent<Car>().CarAnimationOn = true;
                    }
                    else
                    {
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Right", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Left", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Up", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Down", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("DownSwipe", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("UpSwipe", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("RightSwipe", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("LeftSwipe", false);
                        collision.gameObject.GetComponent<Car>().CarAnimationOn = false;
                    }
                }//Right               
                else if (CarColideCar && IsLeftSwipe)
                {
                    Vibration.Vibrate(50);
                    float tempvaule = 0.3f;
                    //Debug.Log("LEft");
                    stratposcar = new Vector3(stratposcar.x - CarTouchThing, stratposcar.y, stratposcar.z);
                    this.transform.DOMove(stratposcar, CarPuchBackColider);
                    GameObject temppartical = Instantiate(GameManager.gameManager.SmokePartical.gameObject);
                    Vector3 tempparticalpos = stratposcar + new Vector3(1.2f, 0, 0);
                    temppartical.transform.position = tempparticalpos;
                    Destroy(temppartical, 1);
                    DDOL.dOL.PlaySound("CarHorn");
                    if (collision.gameObject.transform.eulerAngles == new Vector3(0, 90, 0))
                    {
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Right", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Left", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Up", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Down", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("DownSwipe", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("UpSwipe", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("RightSwipe", true);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("LeftSwipe", false);
                        collision.gameObject.GetComponent<Car>().CarAnimationOn = true;
                    }
                    else if (collision.gameObject.transform.eulerAngles == new Vector3(0, 0, 0))
                    {
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Right", true);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Left", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Up", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Down", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("DownSwipe", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("UpSwipe", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("RightSwipe", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("LeftSwipe", false);
                        collision.gameObject.GetComponent<Car>().CarAnimationOn = true;
                    }
                    else
                    {
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Right", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Left", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Up", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Down", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("DownSwipe", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("UpSwipe", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("RightSwipe", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("LeftSwipe", false);
                        collision.gameObject.GetComponent<Car>().CarAnimationOn = false;
                    }
                } // Left
                else if (IsUpSwipe && CarColideCar)
                {
                    //Debug.Log("Up");
                    Vibration.Vibrate(50);
                    DDOL.dOL.PlaySound("CarHorn");
                    stratposcar = new Vector3(stratposcar.x, stratposcar.y, stratposcar.z - CarTouchThing);
                    this.transform.DOMove(stratposcar, CarPuchBackColider);
                    GameObject temppartical = Instantiate(GameManager.gameManager.SmokePartical.gameObject);
                    Vector3 tempparticalpos = stratposcar + new Vector3(0, 0, 1.2f);
                    temppartical.transform.position = tempparticalpos;
                    Destroy(temppartical, 1);
                    if (collision.gameObject.transform.eulerAngles == new Vector3(0, 0, 0))
                    {
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Right", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Left", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Up", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Down", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("DownSwipe", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("UpSwipe", true);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("RightSwipe", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("LeftSwipe", false);
                        collision.gameObject.GetComponent<Car>().CarAnimationOn = true;
                    }
                    else if (collision.gameObject.transform.eulerAngles == new Vector3(0, 90, 0))
                    {
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Right", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Left", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Up", true);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Down", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("DownSwipe", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("UpSwipe", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("RightSwipe", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("LeftSwipe", false);
                        collision.gameObject.GetComponent<Car>().CarAnimationOn = true;
                    }
                    else
                    {
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Right", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Left", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Up", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Down", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("DownSwipe", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("UpSwipe", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("RightSwipe", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("LeftSwipe", false);
                        collision.gameObject.GetComponent<Car>().CarAnimationOn = false;
                    }
                }//Up
                else if (IsDownSwipe && CarColideCar)
                {
                    Vibration.Vibrate(50);
                    //Debug.Log("Down");
                    DDOL.dOL.PlaySound("CarHorn");
                    stratposcar = new Vector3(stratposcar.x, stratposcar.y, stratposcar.z + CarTouchThing);
                    this.transform.DOMove(stratposcar, CarPuchBackColider);
                    GameObject temppartical = Instantiate(GameManager.gameManager.SmokePartical.gameObject);
                    Vector3 tempparticalpos = stratposcar + new Vector3(0, 0, -1.2f);
                    temppartical.transform.position = tempparticalpos;
                    Destroy(temppartical, 1);
                    if (collision.gameObject.transform.eulerAngles == new Vector3(0, 0, 0))
                    {
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Right", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Left", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Up", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Down", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("DownSwipe", true);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("UpSwipe", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("RightSwipe", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("LeftSwipe", false);
                        collision.gameObject.GetComponent<Car>().CarAnimationOn = true;
                    }
                    else if (collision.gameObject.transform.eulerAngles == new Vector3(0, 90, 0))
                    {
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Right", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Left", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Up", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Down", true);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("DownSwipe", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("UpSwipe", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("RightSwipe", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("LeftSwipe", false);
                        collision.gameObject.GetComponent<Car>().CarAnimationOn = true;
                    }
                    else
                    {
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Right", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Left", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Up", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Down", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("DownSwipe", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("UpSwipe", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("RightSwipe", false);
                        collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("LeftSwipe", false);
                        collision.gameObject.GetComponent<Car>().CarAnimationOn = false;
                    }
                }// Down
                //travelingDirection = Vector3.zero;                
            }
            travelingDirection = Vector3.zero;
        }
        else if (collision.gameObject.tag == "Wall")
        {
            Rb.isKinematic = true;
            IsRightSwipe = IsLeftSwipe = IsUpSwipe = IsDownSwipe = false;
        }
        CarColideCar = false;
        TempTraveling = false;
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Car" || collision.gameObject.tag == "Wall")
        {
            CarColideCar = true;
            stratposcar = this.gameObject.transform.position;
            if (CarColideCar && IsRightSwipe)
            {
                Vibration.Vibrate(50);
                DDOL.dOL.PlaySound("CarHorn");
                //Debug.Log("Right");
                stratposcar = new Vector3(stratposcar.x + CarTouchThing, stratposcar.y, stratposcar.z);
                this.transform.DOMove(stratposcar, CarPuchBackColider);
                GameObject temppartical = Instantiate(GameManager.gameManager.SmokePartical.gameObject);
                Vector3 tempparticalpos = stratposcar + new Vector3(-1.2f, 0, 0);
                temppartical.transform.position = tempparticalpos;
                Destroy(temppartical, 1);
                if (collision.gameObject.transform.eulerAngles == new Vector3(0, 90, 0))
                {
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Right", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Left", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Up", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Down", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("DownSwipe", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("UpSwipe", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("RightSwipe", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("LeftSwipe", true);
                    collision.gameObject.GetComponent<Car>().CarAnimationOn = true;
                }
                else if (collision.gameObject.transform.eulerAngles == new Vector3(0, 0, 0))
                {
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Right", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Left", true);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Up", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Down", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("DownSwipe", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("UpSwipe", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("RightSwipe", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("LeftSwipe", false);
                    collision.gameObject.GetComponent<Car>().CarAnimationOn = true;
                }
                else
                {
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Right", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Left", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Up", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Down", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("DownSwipe", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("UpSwipe", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("RightSwipe", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("LeftSwipe", false);
                    collision.gameObject.GetComponent<Car>().CarAnimationOn = false;
                }
            }//Right               
            else if (CarColideCar && IsLeftSwipe)
            {
                Vibration.Vibrate(50);
                float tempvaule = 0.3f;
                //Debug.Log("LEft");
                stratposcar = new Vector3(stratposcar.x - CarTouchThing, stratposcar.y, stratposcar.z);
                this.transform.DOMove(stratposcar, CarPuchBackColider);
                GameObject temppartical = Instantiate(GameManager.gameManager.SmokePartical.gameObject);
                Vector3 tempparticalpos = stratposcar + new Vector3(1.2f, 0, 0);
                temppartical.transform.position = tempparticalpos;
                Destroy(temppartical, 1);
                DDOL.dOL.PlaySound("CarHorn");
                if (collision.gameObject.transform.eulerAngles == new Vector3(0, 90, 0))
                {
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Right", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Left", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Up", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Down", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("DownSwipe", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("UpSwipe", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("RightSwipe", true);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("LeftSwipe", false);
                    collision.gameObject.GetComponent<Car>().CarAnimationOn = true;
                }
                else if (collision.gameObject.transform.eulerAngles == new Vector3(0, 0, 0))
                {
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Right", true);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Left", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Up", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Down", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("DownSwipe", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("UpSwipe", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("RightSwipe", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("LeftSwipe", false);
                    collision.gameObject.GetComponent<Car>().CarAnimationOn = true;
                }
                else
                {
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Right", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Left", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Up", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Down", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("DownSwipe", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("UpSwipe", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("RightSwipe", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("LeftSwipe", false);
                    collision.gameObject.GetComponent<Car>().CarAnimationOn = false;
                }
            } // Left
            else if (IsUpSwipe && CarColideCar)
            {
                //Debug.Log("Up");
                Vibration.Vibrate(50);
                DDOL.dOL.PlaySound("CarHorn");
                stratposcar = new Vector3(stratposcar.x, stratposcar.y, stratposcar.z - CarTouchThing);
                this.transform.DOMove(stratposcar, CarPuchBackColider);
                GameObject temppartical = Instantiate(GameManager.gameManager.SmokePartical.gameObject);
                Vector3 tempparticalpos = stratposcar + new Vector3(0, 0, 1.2f);
                temppartical.transform.position = tempparticalpos;
                Destroy(temppartical, 1);
                if (collision.gameObject.transform.eulerAngles == new Vector3(0, 0, 0))
                {
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Right", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Left", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Up", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Down", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("DownSwipe", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("UpSwipe", true);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("RightSwipe", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("LeftSwipe", false);
                    collision.gameObject.GetComponent<Car>().CarAnimationOn = true;
                }
                else if (collision.gameObject.transform.eulerAngles == new Vector3(0, 90, 0))
                {
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Right", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Left", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Up", true);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Down", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("DownSwipe", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("UpSwipe", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("RightSwipe", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("LeftSwipe", false);
                    collision.gameObject.GetComponent<Car>().CarAnimationOn = true;
                }
                else
                {
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Right", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Left", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Up", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Down", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("DownSwipe", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("UpSwipe", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("RightSwipe", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("LeftSwipe", false);
                    collision.gameObject.GetComponent<Car>().CarAnimationOn = false;
                }
            }//Up
            else if (IsDownSwipe && CarColideCar)
            {
                Vibration.Vibrate(50);
                //Debug.Log("Down");
                DDOL.dOL.PlaySound("CarHorn");
                stratposcar = new Vector3(stratposcar.x, stratposcar.y, stratposcar.z + CarTouchThing);
                this.transform.DOMove(stratposcar, CarPuchBackColider);
                GameObject temppartical = Instantiate(GameManager.gameManager.SmokePartical.gameObject);
                Vector3 tempparticalpos = stratposcar + new Vector3(0, 0, -1.2f);
                temppartical.transform.position = tempparticalpos;
                Destroy(temppartical, 1);
                if (collision.gameObject.transform.eulerAngles == new Vector3(0, 0, 0))
                {
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Right", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Left", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Up", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Down", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("DownSwipe", true);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("UpSwipe", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("RightSwipe", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("LeftSwipe", false);
                    collision.gameObject.GetComponent<Car>().CarAnimationOn = true;
                }
                else if (collision.gameObject.transform.eulerAngles == new Vector3(0, 90, 0))
                {
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Right", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Left", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Up", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Down", true);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("DownSwipe", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("UpSwipe", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("RightSwipe", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("LeftSwipe", false);
                    collision.gameObject.GetComponent<Car>().CarAnimationOn = true;
                }
                else
                {
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Right", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Left", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Up", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("Down", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("DownSwipe", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("UpSwipe", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("RightSwipe", false);
                    collision.gameObject.GetComponent<Car>().CarAnimator.SetBool("LeftSwipe", false);
                    collision.gameObject.GetComponent<Car>().CarAnimationOn = false;
                }
            }// Down
             //travelingDirection = Vector3.zero;                           
            travelingDirection = Vector3.zero;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Car")
        {
            Rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            Invoke("DelayChnageCollsionDecetctionMode", 0.3f);
        }
        if (collision.gameObject.tag == "Car" || collision.gameObject.tag == "Wall")
        {
            Invoke("CarDragLimiteDelay", 0.3f);
            travelingDirection = Vector3.zero;
            IsRightSwipe = IsLeftSwipe = IsUpSwipe = IsDownSwipe = false;
            if (CarAnimationOn)
            {
                Invoke("AfterCarAnimationOff", 0.1f);
            }
        }
    }
    void DelayChnageCollsionDecetctionMode()
    {
        Debug.Log(this.gameObject.name);
    }
    void CarDragLimiteDelay()
    {
        DragLimite = true;
    }
    void AfterCarAnimationOff()
    {
        if (CarAnimationOn)
        {
            gameObject.GetComponent<Car>().CarAnimator.SetBool("Right", false);
            gameObject.GetComponent<Car>().CarAnimator.SetBool("Left", false);
            gameObject.GetComponent<Car>().CarAnimator.SetBool("Up", false);
            gameObject.GetComponent<Car>().CarAnimator.SetBool("Down", false);
            gameObject.GetComponent<Car>().CarAnimator.SetBool("DownSwipe", false);
            gameObject.GetComponent<Car>().CarAnimator.SetBool("UpSwipe", false);
            gameObject.GetComponent<Car>().CarAnimator.SetBool("RightSwipe", false);
            gameObject.GetComponent<Car>().CarAnimator.SetBool("LeftSwipe", false);
        }
    }
    public bool TempCarTouchDiifCarOnAlways, CarColiderBackAgain;
    bool DownRo, UpRo, RightRo, LeftRo;
    void RotateStay()
    {
        float RotateSpeed, CarSpeed, CarTouchTimeRightLeft, CarTouchTimeUpDown;
        RotateSpeed = .24f;        
        CarSpeed = 15 * Time.deltaTime;
        CarTouchTimeRightLeft = .01f;
        CarTouchTimeUpDown = .009f;
        if (DownRo)
        {
            //Debug.Log("DownRO");
            Vector3 tempcar = this.transform.position;
            this.transform.DORotate(new Vector3(0, 180, 0), RotateSpeed);
            //this.transform.DOLocalMove(tempcar + new Vector3(0, 0, -CarSpeed), CarTouchTimeUpDown);
            this.transform.localPosition = (tempcar + new Vector3(0, 0, -CarSpeed));
        }
        else if (UpRo)
        {
            //Debug.Log("UpRO");
            Vector3 tempcar = this.transform.position;
            this.transform.DORotate(new Vector3(0, 0, 0), RotateSpeed);
            //this.transform.DOLocalMove(tempcar + new Vector3(0, 0, CarSpeed), CarTouchTimeUpDown);
            this.transform.localPosition = (tempcar + new Vector3(0, 0, CarSpeed));
        }
        else if (RightRo)
        {
            //Debug.Log("RightRO");
            Vector3 tempcar = this.transform.position;
            this.transform.DORotate(new Vector3(0, 90, 0), RotateSpeed).OnComplete(() => { });
            //this.transform.DOLocalMove(tempcar + new Vector3(CarSpeed, 0, 0), CarTouchTimeRightLeft);
            this.transform.localPosition = (tempcar + new Vector3(CarSpeed, 0, 0));
        }
        else if (LeftRo)
        {
            //Debug.Log("LeftRO");
            Vector3 tempcar = this.transform.position;
            this.transform.DORotate(new Vector3(0, -90, 0), RotateSpeed);
            //this.transform.DOLocalMove(tempcar + new Vector3(-CarSpeed, 0, 0), CarTouchTimeRightLeft);
            this.transform.localPosition = (tempcar + new Vector3(-CarSpeed, 0, 0));
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ifRoad")
        {
            CarTouchFirstRoadToGoFinal = true;
            Debug.Log("If Road Touch");
        }
        if (ISCarTouchRoad && !TempCarTouchDiifCarOnAlways && CarTouchFirstRoadToGoFinal)
        {
            if (other.tag == "TurnColider")
            {
                Rb.isKinematic = false;
                Rb.velocity = Vector3.zero;
                DownRo = true;
                UpRo = RightRo = LeftRo = false;
                //Debug.Log("Down");
            }
            if (other.tag == "TurnColider2")
            {
                Rb.isKinematic = false;
                Rb.velocity = Vector3.zero;
                LeftRo = true;
                UpRo = RightRo = DownRo = false;
                //Debug.Log("Left");
            }
            if (other.tag == "TurnColider3")
            {
                Rb.isKinematic = false;
                Rb.velocity = Vector3.zero;
                UpRo = true;
                DownRo = RightRo = LeftRo = false;
                //Debug.Log("Up");
            }
            if (other.tag == "TrunColider4")
            {
                Rb.isKinematic = false;
                Rb.velocity = Vector3.zero;
                //Debug.Log("Right");
                RightRo = true;
                UpRo = DownRo = LeftRo = false;
            }
        }
        if (other.tag == "Road" && this.GetComponent<Car>().TempCarTouchDiifCarOnAlways.Equals(false) && this.GetComponent<Car>().ISCarTouchRoad.Equals(false) && CarTouchFirstRoadToGoFinal)
        {
            boxCollider.isTrigger = true;
            ISCarTouchRoad = true;
            //Rb.isKinematic = true;

            if (CarAnimationOn)
                gameObject.GetComponent<Animator>().enabled = false;
            if (ISCarTouchRoad)
                gameObject.GetComponent<Animator>().enabled = false;
            AfterCarAnimationOff();
            boxCollider.center = new Vector3(0.399916f, 0.9156515f, 1.772002f);
            boxCollider.size = new Vector3(3.331727f, 1.815144f, 10.11919f);
            //boxCollider.center = new Vector3(1.214882f, 0.9156515f, 1.772002f);
            //boxCollider.size = new Vector3(4.963671f, 1.815144f, 10.11919f);
            IsRightSwipe = IsLeftSwipe = IsUpSwipe = IsDownSwipe = false;
        }
        if (other.tag == "Car" && other.GetComponent<Car>().ISCarTouchRoad.Equals(true))
        {
            if (IsLeftSwipe || IsRightSwipe || IsUpSwipe || IsDownSwipe)
            {
                Debug.Log("Car Work..........................111");
                TempCarTouchDiifCarOnAlways = true;
                DDOL.dOL.PlaySound("CarHorn");
                this.gameObject.GetComponent<NavMeshAgent>().enabled = false;
                if (Up || Down)
                    this.gameObject.transform.DOMove(new Vector3(transform.position.x, transform.position.y, transform.position.z + .5f), .35f).OnComplete(() =>
                    {                        
                        TempCarTouchDiifCarOnAlways = false;
                    });
                if (Right || Left)
                    this.gameObject.transform.DOMove(new Vector3(transform.position.x - .5f, transform.position.y, transform.position.z), .35f).OnComplete(() =>
                    {
                        TempCarTouchDiifCarOnAlways = false;                       
                    });
            }
        }
        if (other.tag == "Car" && this.GetComponent<Car>().ISCarTouchRoad.Equals(true) && this.GetComponent<Car>().TempTraveling)
        {
            TempCarTouchDiifCarOnAlways = true;
            DDOL.dOL.PlaySound("CarHorn");
            this.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            if (Up || Down)
                this.gameObject.transform.DOMove(new Vector3(transform.position.x, transform.position.y, transform.position.z + 1f), .35f).OnComplete(() =>
                {
                    TempCarTouchDiifCarOnAlways = false;                    
                });
            if (Right || Left)
                this.gameObject.transform.DOMove(new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z), .35f).OnComplete(() =>
                {
                    TempCarTouchDiifCarOnAlways = false;                    
                });
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Road" && this.GetComponent<Car>().TempCarTouchDiifCarOnAlways.Equals(false) && this.GetComponent<Car>().ISCarTouchRoad.Equals(false) && CarTouchFirstRoadToGoFinal)
        {
            boxCollider.isTrigger = true;
            ISCarTouchRoad = true;
            //Rb.isKinematic = true;
            if (CarAnimationOn)
                gameObject.GetComponent<Animator>().enabled = false;
            if (ISCarTouchRoad)
                gameObject.GetComponent<Animator>().enabled = false;
            AfterCarAnimationOff();
            boxCollider.center = new Vector3(0.399916f, 0.9156515f, 1.772002f);
            boxCollider.size = new Vector3(3.331727f, 1.815144f, 10.11919f);
            //boxCollider.center = new Vector3(1.214882f, 0.9156515f, 1.772002f);
            //boxCollider.size = new Vector3(4.963671f, 1.815144f, 10.11919f);
            IsRightSwipe = IsLeftSwipe = IsUpSwipe = IsDownSwipe = false;
        }
        if (other.tag == "OpenExitGate")
        {
            ExitGate.transform.DOLocalRotate(new Vector3(0, 0, -90f), 1);
        }
        /* if (ISCarTouchRoad && !TempCarTouchDiifCarOnAlways)
         {
             if (other.tag == "TurnColider")
             {
                 Rb.isKinematic = false;
                 Rb.velocity = Vector3.zero;
                 DownRo = true;
                 UpRo = RightRo = LeftRo = false;
                 Debug.Log("Down");
             }
             if (other.tag == "TurnColider2")
             {
                 Rb.isKinematic = false;
                 Rb.velocity = Vector3.zero;
                 LeftRo = true;
                 UpRo = RightRo = DownRo = false;
                 Debug.Log("Left");
             }
             if (other.tag == "TurnColider3")
             {
                 Rb.isKinematic = false;
                 Rb.velocity = Vector3.zero;
                 UpRo = true;
                 DownRo = RightRo = LeftRo = false;
                 Debug.Log("Up");
             }
             if (other.tag == "TrunColider4")
             {
                 Rb.isKinematic = false;
                 Rb.velocity = Vector3.zero;
                 Debug.Log("Right");                
                 RightRo = true;
                 UpRo = DownRo = LeftRo = false;
             }
         }*/

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "OpenExitGate")
        {
            DDOL.dOL.PlaySound("CarHorn");
            ExitGate.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.5f);
        }

        if (other.tag == "Car" && other.GetComponent<Car>().ISCarTouchRoad.Equals(true))
        {
            if (IsLeftSwipe || IsRightSwipe || IsUpSwipe || IsDownSwipe)
            {
                TempCarTouchDiifCarOnAlways = false;
                Debug.Log("Delpay.........");
                DelayGoOnRoad();
            }
        }

    }
    void DelayGoOnRoad()
    {
        if (IsLeftSwipe)
        {
            SetDirection(Vector3.right);
        }
        else if (IsRightSwipe)
        {
            SetDirection(Vector3.left);
        }
        else if (IsUpSwipe)
        {
            SetDirection(Vector3.forward);
        }
        else if (IsDownSwipe)
        {
            SetDirection(Vector3.back);
        }
    }
    void GameOver()
    {
        Camera.main.DOFieldOfView(110, 0.6f).OnComplete(() => { Camera.main.DOFieldOfView(100, 0.6f); });
        Car.car.ISCarTouchRoad = false;
        SceneManager.LoadScene("GamePlay");
    }
}
