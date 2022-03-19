using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    // Definiciones
    public Camera cam;
    public GameObject player;
    public GameObject mouse;
    Rigidbody2D rb;
    public int controlTipo; /// 0 = Control Directo; 1 = Control Estratégico.
    public float velocidadMov;

    // Inicio
    void Start()
    {
        cam = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player"); /// Temporal, hasta que se haga el Control Estratégico
        mouse = GameObject.Find("Mouse");
        rb = this.GetComponent<Rigidbody2D>();
        velocidadMov = 3f; /// Temporal, hasta que se desarrolle más.
    }

    // Actualización
    private void Update()
    {
        /// Controles generales
        if (Input.GetKeyUp(KeyCode.Tab)) /// Cambiar de control directo a estratégico o viceversa.
        {
            if (controlTipo == 0) { controlTipo = 1; player.GetComponent<Rigidbody2D>().velocity = Vector3.zero; }
            else if (controlTipo == 1) { controlTipo = 0; }
            else { Debug.Log("La variable 'controlTipo' no es ni 0 ni 1. Error."); }
        }
    }

    void FixedUpdate()
    {
        /// Seguimiento de cámara
        Vector3 offset = new Vector3(0, 0, -5f);
        cam.transform.position = Vector3.MoveTowards(cam.transform.position, this.transform.position + offset, Time.fixedDeltaTime * 9f);

        /// Control Directo
        if (controlTipo == 0)
        {
            /// Arreglos
            this.gameObject.transform.position = player.gameObject.transform.position; /// El GameManager se desplaza hacia el PlayerModel
            Vector2 direccion = cam.ScreenToWorldPoint(Input.mousePosition) - player.transform.position;
            float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
            Quaternion rotacion = Quaternion.AngleAxis(angulo, Vector3.forward);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, rotacion, velocidadMov * Time.fixedDeltaTime);

            /// Controles de movimiento CD
            float mH = Input.GetAxis("Horizontal");
            float mV = Input.GetAxis("Vertical");
            Rigidbody2D playerRB = player.GetComponent<Rigidbody2D>();
            playerRB.velocity = new Vector2(mH * velocidadMov, mV * velocidadMov);
        }

        /// Control Estratégico
        if (controlTipo == 1)
        {
            /// Controles de movimiento CE
            float mH = Input.GetAxis("Horizontal");
            float mV = Input.GetAxis("Vertical");
            rb.velocity = new Vector2(mH * velocidadMov * 2f, mV * velocidadMov * 2f);
        }
    }
}
