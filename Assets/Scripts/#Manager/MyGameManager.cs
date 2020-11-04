using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGameManager : MonoBehaviour
{
    // Aqui nós criamos um Singleton que vai cuidar dos turnos e do fim da simulação
    static MyGameManager _instance;
    public static MyGameManager instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("Game Manager is Null!");

            return _instance;
        }
    }

    void Start()
    {
        // Colocamos a referência do Singleton como a instance atual
        _instance = this;

        // Increvemos o método OnAgentDeath na action dos agentes.
        Agent.OnDeath += OnAgentDeath;
        // Posicionamos os agentes
        SetAgentsPositions();
        // Posicionamos o caçador
        SetHunterPosition();

        // Iniciamos o marcador de tempo
        marker = Time.time + waitForX;
    }

    // Serve para marcar quanto tempo deve passar antes do próximo turno
    float marker;
    // Define quanto tempo o sistema deve esperar antes de executar o próximo turno
    float waitForX = 0.1f;

    // Quantidade de turnos
    public int turns = 0;
    // Se deve ou não iniciar a simulação
    public bool shouldStartSim;


    void Update()
    {
        // Se tiver caças e tiver permissão para iniciar a simulação, comece
        if (numberOfAgents > 0 && shouldStartSim)
        {
            // Verifica se o tempo já chegou na marca que queremos
            if (Time.time > marker)
            { 
                // atualiza os turnos
                turns++;

                // Executa o turno do caçador
                hunter.Turn();

                // Executa o turno das caças
                for (int i = 0; i < agents.Count; i++)
                {
                    agents[i].Turn();
                }

                // Atualiza a marca de tempo
                marker = Time.time + waitForX;
            }
        }
        
    }

    // Pega a referência para o prefab da caça
    public GameObject agentPrefab;
    // Define quantas caças teremos
    public int numberOfAgents = 10;
    // Lista com as caças criadas
    List<Agent> agents = new List<Agent>();

    // Referência do mapa
    public GridMap map;

    // Lista com os tiles disponíveis (não ocupados)
    List<Vector2Int> availableTiles = new List<Vector2Int>();

    void SetAgentsPositions()
    {
        // Pega os tiles do mapa
        for (int x = 0; x < map.tiles.GetLength(0); x++)
        {
            for (int y = 0; y < map.tiles.GetLength(1); y++)
            {
                availableTiles.Add(map.tiles[x, y].pos);
            }
        }

        // Cria as caças
        for (int i = 0; i < numberOfAgents; i++)
        {
            // Cria uma nova caça
            GameObject newAgent = Instantiate(agentPrefab, Vector3.zero, Quaternion.identity);
            // Adiciona na lista
            agents.Add(newAgent.GetComponent<Agent>());

            // Seleciona a posição inicial da lista de possíveis tiles
            int initialPos = Random.Range(0, availableTiles.Count);

            // Atualiza a posição inicial
            newAgent.GetComponent<Agent>().initialPos = availableTiles[initialPos];
            // Atualiza o id
            newAgent.GetComponent<Agent>().id = i;

            // Remove o tile usado da lista
            availableTiles.RemoveAt(initialPos);
        }
    }

    // Pega a referência para o prefab do caçador
    public GameObject hunterPrefab;
    // Guarda a referência do caçador criado
    Hunter hunter;

    void SetHunterPosition()
    {
        // Cria um novo caçador
        GameObject newHunter = Instantiate(hunterPrefab, Vector3.zero, Quaternion.identity);
        // Pega a referência dele
        hunter = newHunter.GetComponent<Hunter>();

        // Seleciona a posição inicial da lista de possíveis tiles
        int initialPos = Random.Range(0, availableTiles.Count);

        // Atualiza a posição inicial
        hunter.initialPos = availableTiles[initialPos];

        // Remove o tile usado da lista
        availableTiles.RemoveAt(initialPos);
    }

    // Atualiza a quantidade de caças vivas e remove a referência da lista
    void OnAgentDeath(int id)
    {
        numberOfAgents--;

        for (int i = 0; i < agents.Count; i++)
        {
            if (agents[i].id == id)
            {
                agents.RemoveAt(i);
            }
        }
       
    }



}
