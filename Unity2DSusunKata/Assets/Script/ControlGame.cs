using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlGame : MonoBehaviour
{
    public int gameRound;

    [Header("Image Question")]
    
    public Image ImageSoal;
    
    public Sprite[] spriteSoal;

    public int[] indexRandomSprites;

    [Tooltip("Jika ingin random tekan ini")]

    public bool isRandomSprite;

    void Start()//3
    {
        RandomImageSoal();    
    }

    void RandomImageSoal()//2
    {
        indexRandomSprites = new int[spriteSoal.Length];//membuat slot secara otomatis sesuai prite yang digunakan
        for(int i=0; i<indexRandomSprites.Length; i++)
        {
            indexRandomSprites[i] = i;//fill element array
        }
        if(isRandomSprite == true)
        {
            RandomValue(indexRandomSprites); //acak index
        }

        ImageSoal.sprite = spriteSoal[indexRandomSprites[gameRound]];//implementasi sprite stelah di acak
    }

    void RandomValue(int[] indexRandoms)//1
    {
        for(int i=0; i<indexRandoms.Length; i++)
        {
            int a = indexRandoms[i];
            int b = Random.Range(0, indexRandoms.Length);
            indexRandoms[i] = indexRandoms[b];
            indexRandoms[b] = a;
        }
    }
}
