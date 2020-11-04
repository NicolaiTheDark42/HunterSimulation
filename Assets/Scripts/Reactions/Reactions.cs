using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reactions : MonoBehaviour
{
    // Guarda as reações possíveis
    public enum Reaction { Moving, Chasing };
    // Guarda as imagens das reações
    public Sprite[] reactionsArray;
    // Guarda a referência para o spriterenderer(unity)
    public SpriteRenderer spriteRenderer;
    
    void Update()
    {
        // Faz com que o spriterenderer sempre esteja virado para quem estpa observando
        spriteRenderer.transform.LookAt(Camera.main.transform.position);
    }

    // Muda a reação atual
    public void ChangeReaction(Reaction reaction)
    {
        spriteRenderer.sprite = reactionsArray[(int)reaction];
    }

}
