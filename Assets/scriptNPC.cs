using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class scriptNPC : MonoBehaviour
{
    private NavMeshAgent agente;
    public GameObject pc;
    public bool enabledPowerPC = false;
    public GameObject [] waypoints = new GameObject[4];
    private int index = 0;
    public float maxDist = 2;
    private bool perseguicao = false;
    public float maxDistAlvo = 10;
    
    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "PC" && pc.GetComponent<scriptPC>().powerPC)
        {
            enabledPowerPC = true;
            gameObject.SetActive(false);
            Invoke("RespawnNPC", 10f);
            pc.GetComponent<scriptPC>().pontos += 20;
		}
		else
		{
            enabledPowerPC = false;
        }
    }

    private void RespawnNPC()
    {      
        transform.position = new Vector3(0, 0, 0);
        gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        Proximo();
    }

    private void Proximo()
	{
        agente.SetDestination(waypoints[index++].transform.position);
        if (index >= waypoints.Length)
            index = 0;
	}
    // Update is called once per frame
    void Update()
    {
        if(perseguicao || Vector3.Distance(transform.position,pc.transform.position) <= maxDistAlvo)
		{
            perseguicao = true;
            agente.SetDestination(pc.transform.position);
		}
        else if (Vector3.Distance(transform.position, agente.destination) < maxDist)
            Proximo();

    }
}
