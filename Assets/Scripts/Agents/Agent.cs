using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    // Guarda o id para ser usado mais tarde
    public int id;

    // Usado para atualizar a quantidade de caças no MyGameManager
    public static event System.Action<int> OnDeath;

    // Posição inicial do Agent
    public Vector2Int initialPos = new Vector2Int(0, 0);

    // Lista com todos os tiles
    public Tile[,] tiles;
    // Onde o personagem está atualmente
    public Tile currentTile;

    // Começo dos estados do personagem
    private State _state;

    // Guarda a refer^^encia do Hunter
    public Hunter hunter;

    #region States
    // Estado de movimentação da máquina de estados
    public Move moveState;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        // Procura pela lista de tiles
        tiles = FindObjectOfType<GridMap>().tiles;

        // Inicializa o agente na posição inicial
        currentTile = tiles[initialPos.x, initialPos.y];
        // Atualiza a posição inicial no mundo
        transform.position = currentTile.transform.position + Vector3.up;
        // Atualiza a ocupação do tile
        currentTile.UpdateTile(Tile.OccupiedBy.Agent);

        // Pega a referência do Hunter
        hunter = FindObjectOfType<Hunter>();

        // Inicializa o move
        moveState = new Move();

        // Muda o estado atual para move
        ChangeState(moveState);        
    }
    
    // Usado pela máquina de estados para mudar o estado atual
    public void ChangeState(State nextState)
    {
        // Garante que não estamos chamando uma referência null e chama o método Exit() do estado
        if (_state != null)
            _state.Exit();

        // Atualiza o estado atual
        _state = nextState;

        // Entra no estado atual
        _state.Enter(this);
    }

    // Update is called once per frame
    public void Turn()
    {
        // Atualiza o estado atual
        _state.Update();
    }

    public void Death()
    {
        // Se a action não for null, chama ela
        if (OnDeath != null)
            OnDeath(id);

        // Desocupa o tile atual
        currentTile.currentOccupant = Tile.OccupiedBy.None;

        // Destrói a caça
        Destroy(gameObject);
    }
}
