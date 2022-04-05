using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerCtr : MonoBehaviour
{
    public Animator m_Animator;
    public CharacterController m_CharacterController;
    public delegate void Move(Vector3 wasd_v, Vector3 mouse_v, bool wasd_b, bool jump_b);
    public Move m_move;

    public Vector3 lastMousePosition;

    public float moveSpeed;
    public float rotSpeed;
    public float gravity;
    private float gravitySpeed;
    public float jumpSpeed;
    // Start is called before the first frame update
    void Start()
    {
        m_move = PeopleController;
    }

    // Update is called once per frame
    void Update()
    {
        KMController();
    }

    void KMController()
    {
        float wa = Input.GetAxis("Vertical");
        float sd = Input.GetAxis("Horizontal");
        bool wasd_b = false;
        if (Input.GetButton("Vertical")|| Input.GetButton("Horizontal"))
        {
            wasd_b = true;
        }
        Vector3 wasd = new Vector3(sd, 0, wa);
        float tdt = Time.deltaTime > 0 ? Time.deltaTime : 0.00390625f;
        Vector3 mouse = (Input.mousePosition - lastMousePosition) / tdt;
        bool jump_b = Input.GetButton("Jump");
        m_move(wasd, mouse, wasd_b, jump_b);
        lastMousePosition = Input.mousePosition;
    }

    void PeopleController(Vector3 wasd_v, Vector3 mouse_v, bool wasd_b, bool jump_b)
    {
        if (wasd_b)
        {
            transform.forward =Vector3.Lerp(transform.forward, wasd_v.normalized, Time.deltaTime*rotSpeed) ;
            m_CharacterController.Move(transform.forward * moveSpeed * Time.deltaTime);
            m_Animator.SetFloat("Speed", moveSpeed);
        }
        else
        {
            m_Animator.SetFloat("Speed", 0);
        }
       
        if (jump_b && m_CharacterController.isGrounded)
        {
            gravitySpeed = jumpSpeed;
        }
        PeopleControllerGravity();

       
    }

    void PeopleControllerGravity()
    {
        gravitySpeed -= gravity * Time.deltaTime;
        m_CharacterController.Move(Vector3.up * gravitySpeed);
        if (m_CharacterController.isGrounded)
        {
            gravitySpeed = 0;
        }
    }

}
