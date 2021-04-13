using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class scriptPC : MonoBehaviour
{
    private Rigidbody rbd;

    public Text txtPlacar;
    public Text txtPowerUp;
    public int pontos = 0;
    public Image imgVida1, imgVida2, imgVida3;
    private int vida = 3;

    private AudioSource som;

    public GameObject points;
    private bool totalStars = false, totalPowerItens = false;
    public bool powerPC = false;
    private int numStars = 0, numPowerItens = 0;

    private bool isPortal = false,  respawnPC = false;

    public float velocidade = 5;
    public float velRot = 10;
    private Quaternion rotOriginal;
    private float rotMouseX = 0;
    private float rotTecladoX = 0;

    // Start is called before the first frame update
    void Start()
    {
        som = GetComponent<AudioSource>();

        txtPlacar.enabled = true;
        imgVida1.enabled = true;
        imgVida2.enabled = true;
        imgVida3.enabled = true;
        txtPowerUp.enabled = false;

        rbd = GetComponent<Rigidbody>();
        rotOriginal = transform.localRotation;

        transform.position = new Vector3(38.3f, 0.5f, 38.5f);
    }

    // Update is called once per frame
    void Update()
    {
        txtPlacar.text = "Pontuação: " + pontos.ToString();
        
        //Portal
        if (isPortal)
        {
            float posAuxPortal = 0;

            if (transform.position.x > 0) posAuxPortal = transform.position.x - 0.6f;
            else if (transform.position.x < 0) posAuxPortal = transform.position.x + 0.6f;

            transform.position = new Vector3(-posAuxPortal, transform.position.y, transform.position.z);

            isPortal = false;
        }

        //RespawnPC
        if (respawnPC)
        {
            transform.position = new Vector3(40f, 0.5f, 40f);
            respawnPC = false;
        }

        RestartGame();

        //MOVIMENTO
        float moveFrente = Input.GetAxis("Vertical");
        float moveLado = Input.GetAxis("Horizontal");

        Vector3 vel = transform.TransformDirection( new Vector3(moveLado * velocidade, rbd.velocity.y, moveFrente * velocidade));
        rbd.velocity = vel;

        //ROTAÇÃO MOUSE
        rotMouseX += Input.GetAxis("Mouse X");

        if (Input.GetKey(KeyCode.Q))
            rotTecladoX -= 1;
        else if (Input.GetKey(KeyCode.E))
            rotTecladoX += 1;

        Quaternion lado = Quaternion.AngleAxis(rotMouseX + rotTecladoX, Vector3.up);
        transform.localRotation = rotOriginal * lado;
    }


    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "star")
        {
            pontos++;
            numStars++;
            Destroy(col.gameObject);

            if (numStars == 100)
            {
                totalStars = true;
                numStars = 0;
            }
        }
        else if (col.tag == "powerPC")
        {
            pontos += 10;
            numPowerItens++;
            col.gameObject.SetActive(false);
            powerPC = true;
            som.Play();
            txtPowerUp.enabled = true;
            Invoke("TimePowerPC", 10f);

            if (numPowerItens == 4)
            {
                totalPowerItens = true;
                numPowerItens = 0;
            }
        }
        else if (col.CompareTag("portal"))
        {
            isPortal = true;
        }

        else if (col.CompareTag("NPC") && !col.gameObject.GetComponent<scriptNPC>().enabledPowerPC)
        {
            vida--;

            if (vida == 2)
            {
                imgVida3.enabled = false;
                respawnPC = true;
            }

            else if (vida == 1)
            {
                imgVida2.enabled = false;
                respawnPC = true;
            }

            else if (vida == 0)
            {
                imgVida1.enabled = false;
                SceneManager.LoadScene(2);
            }
        }
    }
    private void TimePowerPC()
    {
        som.Pause();
        powerPC = false;
        txtPowerUp.enabled = false;
    }
    private void RestartGame()
    {
        if (totalStars && totalPowerItens)
        {
            GameObject auxPoints = GameObject.FindWithTag("pointsPrefab");
            Destroy(auxPoints.gameObject);
            Instantiate(points, new Vector3(0, 0, 0), Quaternion.identity);
            totalPowerItens = false;
            totalStars = false;
        }
    }
}
