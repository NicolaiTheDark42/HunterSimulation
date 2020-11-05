using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterMove : HunterStates
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
        hunter.ChangeReaction(Reactions.Reaction.Moving);
    }

    public override void Update()
    {
        // Verifica se existe algum alvo próximo
        CheckForTargets();

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
        else
        {
            // Muda o estado atual
            hunter.ChangeState(hunter.turn);
            // Executa o estado atual
            hunter.Turn();
        }
    }

    public override void Exit()
    {

    }


    void CheckForTargets()
    {
        // Gera uma lista de tiles ocupados nas redondezas
        List<Tile> occupiedTiles = new List<Tile>();

        // Verifica todos os tiles em uma range
        for (int x = -5; x <= 5; x++)
        {
            for (int y = -5; y <= 5; y++)
            {
                // Calcula as coordenadas do próximo tile
                Vector2Int tileToCheck = hunter.currentTile.pos + new Vector2Int(x, y);

                // Verifica se está dentro do vetor
                if (tileToCheck.x < tiles.GetLength(0) && tileToCheck.x >= 0 && tileToCheck.y < tiles.GetLength(1) && tileToCheck.y >= 0)
                {
                    // Verifica se está ocupado
                    if (tiles[tileToCheck.x, tileToCheck.y].currentOccupant == Tile.OccupiedBy.Agent)
                    {
                        // Adiciona a lista
                        occupiedTiles.Add(tiles[tileToCheck.x, tileToCheck.y]);
                    }
                }
            }
        }

        // Se tiver algum tile ocupado nas redondezas
        if (occupiedTiles.Count > 0)
        {
            Tile target = hunter.currentTile;
            float closestDistance = Vector2.Distance(occupiedTiles[0].pos, hunter.currentTile.pos);

            // Verifica todos os tiles
            for (int i = 0; i < occupiedTiles.Count; i++)
            {
                // Calcula a distância do Caçador ao alvo
                float distance = Vector2.Distance(occupiedTiles[i].pos, hunter.currentTile.pos);

                // Decide se é o Agente mais próximo
                if (distance <= closestDistance)
                {
                    closestDistance = distance;
                    target = occupiedTiles[i];
                }
            }


            RaycastHit hitInfo;
           
            // Procura pela referência do Agente mais próximo
            if (Physics.Raycast(target.transform.position, Vector3.up, out hitInfo))
            {
                if (hitInfo.collider.CompareTag("Agent"))
                {
                    // Pega a referência do Agente e coloca como alvo atual
                    hunter.currentTarget = hitInfo.collider.GetComponent<Agent>();
                    // Muda o state para caçar o agente
                    hunter.ChangeState(hunter.chase);
                    hunter.Turn();
                }

            }


        }

    }


}
