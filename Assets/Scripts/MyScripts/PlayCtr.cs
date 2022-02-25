using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayCtr : NetworkBehaviour
{
    public Vector3 mouseMoves_DS;
    public Vector3 mouseMovesLast_DS;
    public CharacterController m_CharacterController;
    public delegate void MoveDelegate(Vector3 getAxis, Vector3 getmouseMoves, bool jump);
    public MoveDelegate moveMode;
    public bool groundedPlayer;
    public Vector3 playerVelocity;
    public float speedTranslation;
    public float speedRotating;
    public float gravityValue = 9.81f;
    public float jumpPow = 1.0f;

    public GameObject mc_camera;

    public MeshRenderer m_MeshRenderer;
    [SyncVar]
    public Color m_Color;
    [SyncVar]
    public float hp;

    public GameObject summon;
    // Start is called before the first frame update
    void Start()
    {
        if (!isLocalPlayer)
        {
            mc_camera.SetActive(false);
        }
        else
        {
            mc_camera.SetActive(true);
        }
        moveMode = CharacterControllerMove;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {

        }
        else
        {
            MouseMovementComputing();
            Vector3 input_DL = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveMode(input_DL, mouseMoves_DS, Input.GetButton("Jump"));
            if (Input.GetKeyDown(KeyCode.E))
            {
                Summon();
               
                
            }
        }
        m_MeshRenderer.material.SetColor("_Color", m_Color) ;
    }

    [Command]
    void Summon()
    {
        GameObject newSummon = Instantiate(summon);
        newSummon.transform.position = transform.position + transform.forward;
        NetworkServer.Spawn(newSummon);
    }

    void MouseMovementComputing()
    {
        mouseMoves_DS = Input.mousePosition - mouseMovesLast_DS;
        mouseMovesLast_DS = Input.mousePosition;
    }

    void CharacterControllerMove(Vector3 getAxis, Vector3 getmouseMoves, bool jump)
    {
        StateDetection();
        MYCharacterControllerKey(getAxis, getmouseMoves, jump);
        MYCharacterControllerGravity();

    }

    void StateDetection()
    {
        groundedPlayer = m_CharacterController.isGrounded;
    }


    void MYCharacterControllerKey(Vector3 axis, Vector3 getmouseMoves, bool jump)
    {
        transform.localRotation *= Quaternion.AngleAxis(getmouseMoves.x* speedRotating, Vector3.up);
        Vector3 move = new Vector3(axis.x, 0, axis.z);
        //transform.forward

        Vector3 m_moveDirTick = transform.TransformDirection(move * speedTranslation * Time.deltaTime);
        m_CharacterController.Move(m_moveDirTick);
        if (jump && groundedPlayer)
        {
            playerVelocity.y = jumpPow;
            groundedPlayer = false;
        }
    }
    void MYCharacterControllerGravity()
    {
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        playerVelocity.y -= gravityValue * Time.deltaTime;
        m_CharacterController.Move(playerVelocity * Time.deltaTime);
    }



}
