using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Referência para o contador de turnos
    public Text turnCounter;
    // Referências para os gameobjects escondendo os botões e textos
    public GameObject endOfSimulation;
    public GameObject startOfSimulation;
    public GameObject closeSimulation;

    void Update()
    {
        // Atualiza o turno
        turnCounter.text = "- Turn: " + MyGameManager.instance.turns + " -";

        // Faz o texto e o botão de final de simulação aparecer
        if (MyGameManager.instance.numberOfAgents == 0)
        {
            endOfSimulation.SetActive(true);
            closeSimulation.SetActive(true);
        }
    }

    // Inicia a simulação e esconde o botão que inicia a simulação
    public void StartSim()
    {
        MyGameManager.instance.shouldStartSim = true;
        startOfSimulation.SetActive(false);
    }

    // Fecha o programa
    public void CloseSimulation()
    {
        Application.Quit();
    }

}
