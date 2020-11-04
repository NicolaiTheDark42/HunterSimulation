using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : State
{
    // Referência para o dono do estado
    Agent agent;
    // Referência para os tiles do mundo
    Tile[,] tiles;
    

    public override void Enter(Agent agent)
    {
        // Inicializa o dono do estado e os tiles
        this.agent = agent;
        // Inicializa os tiles do mundo
        tiles = agent.tiles;
    }

    public override void Update()
    {
        // Pega a referência para o estado em que a caça está atualmente
        Tile oldTile = agent.currentTile;
        
        // Pega os tiles disponíveis ao redor da caça
        List<Tile> availableTiles = AvailableTiles(agent.currentTile);

        // Se tiver pelo menos um tile disponível ao redor da caça
        if (availableTiles.Count > 0)
        {
            // Escolhe aleatóriamente um dos tiles mais distantes do caçador
            Tile chosenTile = ChooseTile(availableTiles);           

            agent.transform.position = chosenTile.transform.position + Vector3.up;

            oldTile.UpdateTile(Tile.OccupiedBy.None);
            chosenTile.UpdateTile(Tile.OccupiedBy.Agent);

            agent.currentTile = chosenTile;
        }
      
    }

    public override void Exit()
    {
        return;
    }

    // Verifica todos os tiles disponíveis para a movimentação
    List<Tile> AvailableTiles(Tile currentTile)
    {
        // Cria uma lista de todos os tiles disponíveis ao redor da caça
        List<Tile> tempList = new List<Tile>();

        // Procura todos os tiles disponíveis nas redondezas
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                // Verifica a posição do próximo tile
                int neighbourX = currentTile.pos.x + x;
                int neighbourY = currentTile.pos.y + y;

                // Verifica se está dentro da matriz
                if ((neighbourX < tiles.GetLength(0) && neighbourX >= 0) && (neighbourY < tiles.GetLength(1) && neighbourY >= 0))
                {
                    // Verifica se está livre
                    if (agent.tiles[neighbourX, neighbourY].currentOccupant == Tile.OccupiedBy.None)
                    {
                        // Adiciona a lista de tiles livres
                        tempList.Add(agent.tiles[neighbourX, neighbourY]);
                    }
                }
            }
        }

        // Retorna a lista
        return tempList;
    }

    // Escolher de uma lista de tiles mais distantes
    Tile ChooseTile(List<Tile> availableTiles)
    {
        // Guarda todas as distâncias dos tiles vizinhos em relação a posição do caçador
        float[] distances = new float[availableTiles.Count];
        // Média das distâncias
        float average = 0;

        // Calcula a distância dos tiles vizinhos  em relação a posição do caçador
        for (int i = 0; i < availableTiles.Count; i++)
        {
            distances[i] = Vector2.Distance(availableTiles[i].pos, agent.hunter.currentTile.pos);
            average += distances[i];
        }

        // Pega a média
        average /= availableTiles.Count;
        List<Tile> mostDistant = new List<Tile>();

        // Seleciona os tiles mais distantes baseado na média
        for (int i = 0; i < availableTiles.Count; i++)
        {
            if (distances[i] >= average)
            {
                mostDistant.Add(availableTiles[i]);
            }
        }

        // Retorna um dos tiles mais distantes aleatoriamente.
        return mostDistant[UnityEngine.Random.Range(0, mostDistant.Count)];
    }

}
