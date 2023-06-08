using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class ImageSlide : MonoBehaviour
{
    public GameObject[] sprites;
    public float slideSpeed = 1f;

    private Vector2 _spriteSize;



    private void Awake()
    {
        // Get the size of the sprite
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _spriteSize = (spriteRenderer.sprite.rect.size / spriteRenderer.sprite.pixelsPerUnit) * spriteRenderer.transform.lossyScale;
    }
    private void Start()
    {
        // Align sprites
        sprites[0].transform.position = Vector3.zero;
        sprites[1].transform.position = sprites[0].transform.position - new Vector3(0, _spriteSize.y + (0.1f), 0);
    }
    private void Update()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            // Get the sprite
            GameObject sprite = sprites[i];

            // Move the sprite vertically
            sprite.transform.position += new Vector3(0, slideSpeed * Time.deltaTime, 0);

            // Reset position when reaching a treshold
            if (sprite.transform.position.y > _spriteSize.y)
            {
                sprite.transform.position = new Vector3(0, -_spriteSize.y, 0);
            }
        }   
    }
}
