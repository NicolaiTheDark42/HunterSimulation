using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterChase : HunterStates
{
    // Referência para o dono do estado
    Hunter hunter;
    // Referência para os tiles do mundo
    Tile[,] tiles;

    public override void Enter(Hunter hunter)
    {
        // Pega a referência para o dono atual do estado
        this.hunter = hunter;
        // Inicializa os tiles do mundo
        tiles = hunter.tiles;
        // Muda o ícone do caçador
        hunter.ChangeReaction(Reactions.Reaction.Chasing);
    }

    public override void Update()
    {
        // Se o caçador tiver um alvo
        if (hunter.currentTarget != null)
        {
            // Atacar se estiver adjacente
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    // Calcula as coordenadas do próximo tile
                    Vector2Int tileToCheck = hunter.currentTile.pos + new Vector2Int(x, y);

                    // Verifica se está dentro do vetor
                    if (tileToCheck.x < tiles.GetLength(0) && tileToCheck.x >= 0 && tileToCheck.y < tiles.GetLength(1) && tileToCheck.y >= 0)
                    {
                        // Verifica se está ocupado
                        if (tiles[tileToCheck.x, tileToCheck.y].currentOccupant == Tile.OccupiedBy.Agent)
                        {
                            RaycastHit hitInfo;

                            // Dispara um raycast para pegar a referência da caça
                            if (Physics.Raycast(tiles[tileToCheck.x, tileToCheck.y].transform.position, Vector3.up, out hitInfo))
                            {
                                if (hitInfo.collider.CompareTag("Agent"))
                                {
                                    // Se a caça estiver adjacente
                                    if (hunter.currentTarget.currentTile.pos == hitInfo.collider.GetComponent<Agent>().currentTile.pos)
                                    {
                                        // Muda o estado para atacar e executa ele
                                        hunter.ChangeState(hunter.attack);
                                        hunter.Turn();
                                    }
                                }

                            }
                        }
                    }
                }
            }



            // Descobre a direção do alvo em relação ao caçador
            Vector2 directionToTarget = hunter.currentTarget.currentTile.pos - hunter.currentTile.pos;

            // Descobre o ângulo entre eles
            directionToTarget.x = TransformDirection(directionToTarget.x);
            directionToTarget.y = TransformDirection(directionToTarget.y);

            // Pega a direção em que o caçador precisa girar
            for (int i = 0; i < hunter.lookAt.Length; i++)
            {
                if (directionToTarget == hunter.lookAt[i])
                {
                    hunter.currentDirection = (Hunter.Diretions)i;
                }
            }

            // Muda a rotação do caçador
            ChangeDirection(hunter.lookAt[(int)hunter.currentDirection]);


            // Guarda informação sobre o último tile em que o Caçador esteve
            Tile oldTile = hunter.currentTile;

            // Calcula as coordenadas do próximo tile
            Vector2Int nextTile = hunter.currentTile.pos + hunter.lookAt[(int)hunter.currentDirection];

            // Se o próximo tile estiver dentro do campo
            if (nextTile.x < hunter.tiles.GetLength(0) && nextTile.x >= 0 && nextTile.y < hunter.tiles.GetLength(0) && nextTile.y >= 0)
            {
                // E não estiver ocupado
                if (tiles[nextTile.x, nextTile.y].currentOccupant == Tile.OccupiedBy.None)
                {
                    // Atualiza em qual tile o Caçador está
                    hunter.currentTile = tiles[nextTile.x, nextTile.y];
                    // Move o Caçador visualmente para o tile atual
                    hunter.transform.position = hunter.currentTile.transform.position + Vector3.up * 1.5f;
                    // Desocupa o último tile
                    oldTile.UpdateTile(Tile.OccupiedBy.None);
                    // Ocupa o tile atual
                    hunter.currentTile.UpdateTile(Tile.OccupiedBy.Hunter);
                }
            }
        }
        else
        {
            // Muda o estado se não tiver alvo
            hunter.ChangeState(hunter.move);
        }
    }

    public override void Exit()
    {

    }

    // Rotaciona o modelo do caçador para a direção desejada
    public void ChangeDirection(Vector2Int lookAt)
    {
        float angle = Mathf.Atan2(lookAt.x, lookAt.y) * Mathf.Rad2Deg;
        hunter.transform.eulerAngles = Vector3.up * angle;
    }

    // Converte a direção da caça em relação ao caçador para o estilo que criamos no Hunter.cs
    public int TransformDirection(float pos)
    {
        if (pos > 0)
        {
            return 1;
        }
        else if (pos < 0)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }


}
