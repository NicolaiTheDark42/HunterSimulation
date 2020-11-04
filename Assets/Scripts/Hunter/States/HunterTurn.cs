using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterTurn : HunterStates
{
    // Referência para o dono do estado
    Hunter hunter;

    public override void Enter(Hunter hunter)
    {
        // Pega a referência para o dono atual do estado
        this.hunter = hunter;
    }

    public override void Update()
    {
        // Pega o número de direções possíveis
        int numberOfDirections = Enum.GetNames(typeof(Hunter.Diretions)).Length;
        int currentDirection = 0;
        
        // Pega a direção atual
        currentDirection = (int)hunter.currentDirection;
        // Escolhe a próxima da lista
        currentDirection++;

        // Corrige se valor estiver fora do vetor
        if (currentDirection >= numberOfDirections)
        {
            currentDirection = 0;
        }

        // Atualiza a direção atual para o Move state
        hunter.currentDirection = (Hunter.Diretions)currentDirection;

        // Altera a direção do modelo
        ChangeDirection(hunter.lookAt[(int)hunter.currentDirection]);

        // Muda o estado
        hunter.ChangeState(hunter.move);
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


}
