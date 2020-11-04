using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : MonoBehaviour
{
    // Direções que o Caçador pode se virar
    public enum Diretions { TopLeft, Forward, TopRight, Right, BottomRight, Back, BottomLeft, Left  };
    public Diretions currentDirection = Diretions.Forward;

    // Conjunto de vetores que apontam a direção em que o Caçador está olhando
    public Vector2Int[] lookAt = new Vector2Int[8];

    // Lista com todos os tiles
    public Tile[,] tiles;

    // Onde o Caçador está atualmente
    public Tile currentTile;

    // Posição inicial do Caçador
    public Vector2Int initialPos = new Vector2Int(0,0);

    // Começo dos estados do caçador
    private HunterStates _state;

    Reactions reaction;

    #region States
    public HunterMove move;
    public HunterTurn turn;
    public HunterChase chase;
    public HunterAttack attack;
    #endregion

    public Agent currentTarget;

    // Start is called before the first frame update
    void Start()
    {
        // Lista com as direções
        lookAt[0] = new Vector2Int(-1, 1); lookAt[1] = new Vector2Int(0, 1); lookAt[2] = new Vector2Int(1, 1);
        lookAt[3] = new Vector2Int(1, 0); lookAt[4] = new Vector2Int(1, -1);
        lookAt[5] = new Vector2Int(0, -1); lookAt[6] = new Vector2Int(-1, -1); lookAt[7] = new Vector2Int(-1, 0);

        // Procura pela lista de tiles
        tiles = FindObjectOfType<GridMap>().tiles;

        // Inicializa o agente na posição inicial
        currentTile = tiles[initialPos.x, initialPos.y];
        transform.position = currentTile.transform.position + Vector3.up * 1.5f;
        currentTile.UpdateTile(Tile.OccupiedBy.Hunter);

        reaction = GetComponentInChildren<Reactions>();

        // Inicializa os estados
        move = new HunterMove();
        turn = new HunterTurn();
        chase = new HunterChase();
        attack = new HunterAttack();
        ChangeState(move);

    }

    public void Turn()
    {
        _state.Update();
    }

    public void ChangeState(HunterStates nextState)
    {
        if (_state != null)
            _state.Exit();

        _state = nextState;
        _state.Enter(this);
    }

    public void ChangeReaction(Reactions.Reaction reaction)
    {
        this.reaction.ChangeReaction(reaction);
    }


}
