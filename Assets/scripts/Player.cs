using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [Header("Ground Properties")]
    public LayerMask groundLayer;
    public float groundDistance;
    public bool isGrounded;
    public Vector3[] footOffset;

    public float speed = 5f;
    public float jumpForce = 5f;
    private Rigidbody2D rb2d;
    private Animator animator;
    private Vector2 Moviment;
    private float XVelocity;
    private bool isAttacking = false;

    private int direction = 1;
    private float OriginalXScale;

    RaycastHit2D leftCheck;
    RaycastHit2D rightCheck;


    void Awake()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        OriginalXScale = transform.localScale.x;
       
    }


    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal"); // poder controlar o personagem usando teclado configurado no unity
        Moviment = new Vector2(horizontal, 0); // Movimentação com o vetor2 e o eixo Y = 0
        PhysicsCheck();

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb2d.velocity = Vector2.zero;
            rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
    private void FixedUpdate()
    {
        XVelocity = Moviment.normalized.x * speed; //Variavel velocidade ratonando o valor = 1

        animator.SetFloat("xVelocity", Mathf.Abs(Input.GetAxisRaw("Horizontal")));
        rb2d.velocity = new Vector2(XVelocity, rb2d.velocity.y); // velocidade no eixo x e y 

        animator.SetFloat("yVelocity", rb2d.velocity.y);
        animator.SetBool("moviment", Input.GetButton("Horizontal"));// animação de movimento
        Debug.Log(rb2d.velocity.y);

        if (XVelocity * direction < 0) // troca a direção do personagem metodo flip 
        {
            flip();
        }

        if (Input.GetButtonDown("Fire1") && !isAttacking) // Pega o botão do ataque 
        {
            Attack();
        }


    }
    private void flip() // Metodo de rotação do personagem apenas o Eixo X foi alterado  
    {
        direction *= -1;
        Vector3 scale = transform.localScale;
        scale.x = OriginalXScale * direction;
        transform.localScale = scale;

    }

    private void PhysicsCheck() // verifica se ha fisica 
    {
        //isGrounded = false;
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 2f, groundLayer);
        Debug.DrawRay(transform.position, Vector2.down * 2, Color.grey);
        Debug.Log(isGrounded);
        animator.SetBool("Isgrounded", isGrounded);
        //leftCheck = Raycast(new Vector2(footOffset[0].x, footOffset[0].y), Vector2.down, groundDistance, groundLayer);
        //rightCheck = Raycast(new Vector2(footOffset[1].x, footOffset[1].y), Vector2.down, groundDistance, groundLayer);

        // if (leftCheck || rightCheck)
        // {
        //     isGrounded = true;
        // }
    }
    private RaycastHit2D Raycast(Vector3 origin, Vector2 rayDirection, float lenght, LayerMask mask) // pega a posição do personagem e verifica se esta no chao ou não 
    {
        Vector3 pos = transform.position;

        RaycastHit2D hit = Physics2D.Raycast(pos + origin, rayDirection, lenght, mask);

        Color color = hit ? Color.red : Color.green;
        Debug.DrawRay(pos + origin, rayDirection * lenght, color);

        return hit;
    }

    private void Attack()
    {

        isAttacking = true;
        animator.SetTrigger("attack"); // Aciona a animação de ataque

        // Pode adicionar lógica de ataque aqui, como detectar colisões com inimigos

        // Temporizador para finalizar o ataque após a duração da animação
        StartCoroutine(FinishAttack());
    }

    private IEnumerator FinishAttack()
    {
        Debug.Log("end");
        //yield return new WaitForSeconds(0.5f); // Ajuste o tempo para a duração da animação de ataque
        yield return new WaitForSeconds(0f); // Ajuste o tempo para a duração da animação de ataque
        isAttacking = false;
        Debug.Log("start");
    }
}
