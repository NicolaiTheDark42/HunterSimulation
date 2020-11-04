using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    // Guarda os ocupantes possíveis
    public enum OccupiedBy { None, Agent, Hunter};
    // Guarda quem está ocupando o tile
    public OccupiedBy currentOccupant = OccupiedBy.None;

    // Guarda a posição do tile na matriz
    public Vector2Int pos = new Vector2Int();

    // Guarda os materiais que vamos usar
    #region Materials
    Color originalColor = Color.green * 0.8f;
    float emissionColorOriginal = 0.1f;

    Color agentColor = Color.white;   
    float emissionColorAgent = 2;

    Color hunterColor = Color.red;
    float emissionColorHunter = 2;

    Material mat;
    #endregion

    void Awake()
    {
        // Pega a referência para o material do tile
        mat = GetComponent<Renderer>().material;
        // Pega a cor original do tile
        mat.color = originalColor;
        // Muda a cor do tile
        mat.SetColor("_EmissionColor", originalColor * emissionColorOriginal);
    }

    // Muda a cor do tile dependendo se está ocupado ou não
    public void UpdateTile(OccupiedBy occupant)
    {
        // Atualiza o ocupante
        currentOccupant = occupant;

        // Muda a cor do tile de acordo com quem está ocupando ele
        if (currentOccupant == OccupiedBy.None)
        {
            mat.color = originalColor;
            mat.SetColor("_EmissionColor", originalColor * emissionColorOriginal);
        }
        else if (currentOccupant == OccupiedBy.Agent)
        {
            mat.color = agentColor;
            mat.SetColor("_EmissionColor", agentColor * emissionColorAgent);
        }
        else if (currentOccupant == OccupiedBy.Hunter)
        {
            mat.color = hunterColor;
            mat.SetColor("_EmissionColor", hunterColor * emissionColorHunter);
        }
    }
}
