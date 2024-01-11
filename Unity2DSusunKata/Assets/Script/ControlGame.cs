using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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

    [Header("String keterangan gambar")]    
    public string[] stringImageSoals;
    public string[] splitStringImageSoal;
    public int[] lenghtPerText;
    public int indexTextTerpanjang;


    [Header("box kata")]
    public GameObject prefabBoxKata;
    public Transform parentKata;
    public float extraSpaceBoxKata;

    [Header("Highligth target text")]
    public int indexPosisiHightlight;
    public GameObject[] cloneBoxKatas;

    [Header("kata random")]
    public int[] indexRandomPotonganKata;
    public Text textPotonganKata;
    public int coundKata;
    public RectTransform rtPotonganKata;
    public float extraspacePotonganKata;

    [Header("Move Potongan Kata")]
    public GameObject potonganKata;
    public float speedMovementPotonganKata;
    public bool isStarMovePotonganKata;
    Vector3 firstPositionPotonganKata;
    public bool[] isKataBenar;
    public GameObject coverBlockMovement;

    void Start()//3
    {        
        RandomImageSoal();

        GeneratePotonganKata();

        firstPositionPotonganKata = potonganKata.transform.position;//posisi awal

        isKataBenar = new bool[splitStringImageSoal.Length];//create slot array
    }

    void Update()
    {
        MovementPotonganKata();    
    }

    public void ButtonPotonganKata()
    {
        if (isKataBenar[indexPosisiHightlight] == false)
        {
            isStarMovePotonganKata = true;

            coverBlockMovement.SetActive(true);//aktifkan cover
        }
    }

    void MovementPotonganKata()
    {
        if(isStarMovePotonganKata == true)
        {
            var step = speedMovementPotonganKata * Time.deltaTime;
            potonganKata.transform.position = Vector3.MoveTowards(potonganKata.transform.position, cloneBoxKatas[indexPosisiHightlight].transform.position, step); //move object

            if (Vector3.Distance(potonganKata.transform.position, cloneBoxKatas[indexPosisiHightlight].transform.position) < 0.001f)
            {
                Debug.Log("text sampai ke tujuan");

                coverBlockMovement.SetActive(false);//matikan cover

                potonganKata.transform.position = firstPositionPotonganKata;//reset position

                if (textPotonganKata.text == splitStringImageSoal[indexPosisiHightlight]) //index posisi - count kata yang respon
                {
                    Debug.Log(textPotonganKata.text + " ? " + splitStringImageSoal[indexPosisiHightlight]);
                    Debug.Log("Jawaban Benar");

                    isKataBenar[indexPosisiHightlight] = true;//merubah bool atau kondisi box text

                    cloneBoxKatas[indexPosisiHightlight].transform.GetChild(0).GetComponent<Text>().text = textPotonganKata.text;//change text dengan jawaban

                    if (coundKata<splitStringImageSoal.Length - 1)
                    {
                        coundKata += 1;//increase karena benar
                        textPotonganKata.text = splitStringImageSoal[indexRandomPotonganKata[coundKata]];//update text kata
                        rtPotonganKata.sizeDelta = new Vector2(textPotonganKata.preferredWidth + extraspacePotonganKata, rtPotonganKata.sizeDelta.y);//resize box
                    }
                    else
                    {
                        Debug.Log("Generate Level Beikutnya");
                    }                   
                }
                else
                {
                    Debug.Log("Jawaban Salah");
                }
                isStarMovePotonganKata = false;
            }
        }

        
    }

    void GeneratePotonganKata()
    {
        indexRandomPotonganKata = new int[splitStringImageSoal.Length];//create slot

        for (int i = 0; i < indexRandomPotonganKata.Length; i++)
        {
            indexRandomPotonganKata[i] = i; //fill aray
        }

        RandomValue(indexRandomPotonganKata);//random index

        textPotonganKata.text = splitStringImageSoal[indexRandomPotonganKata[coundKata]];// update ui text

        rtPotonganKata.sizeDelta = new Vector2(textPotonganKata.preferredWidth + extraspacePotonganKata,rtPotonganKata.sizeDelta.y);//resize box
    }

    public void ButtonLeftRightHighlight()
    {
        if(EventSystem.current.currentSelectedGameObject.name == "left")
        {
            indexPosisiHightlight -= 1;

            if(indexPosisiHightlight < 0)
            {
                indexPosisiHightlight = cloneBoxKatas.Length - 1;//last element
            }

        }
        else//right
        {
            indexPosisiHightlight += 1;

            if (indexPosisiHightlight > cloneBoxKatas.Length - 1)
            {
                indexPosisiHightlight = 0;//first element
            }
        }

        for (int i = 0; i < cloneBoxKatas.Length; i++)
        {
            if(i != indexPosisiHightlight)
            {
                cloneBoxKatas[i].transform.GetChild(1).gameObject.SetActive(false);//matikan semua
            }
            else
            {
                cloneBoxKatas[i].transform.GetChild(1).gameObject.SetActive(true);//nyalakan
            }
        }

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

        //Implementasi keterangan gambar
        splitStringImageSoal = stringImageSoals[indexRandomSprites[gameRound]].Split(' ');//dipotong dengan acuan spasi

        lenghtPerText = new int[splitStringImageSoal.Length];

        for (int i = 0; i <lenghtPerText.Length; i++)
        {
            lenghtPerText[i] = splitStringImageSoal[i].Length; //di isi dari lenght text
        }

        for (int i = 0; i < lenghtPerText.Length; i++)
        {
            if (lenghtPerText[i] == Mathf.Max(lenghtPerText))
            {
                indexTextTerpanjang = i; //take index terpanjang
            }
        }

        cloneBoxKatas = new GameObject[splitStringImageSoal.Length];//create slot array

        //respon box
        for(int i=0; i<splitStringImageSoal.Length; i++)
        {
            GameObject cloneBoxKata = Instantiate(prefabBoxKata);//respawn
            cloneBoxKata.transform.SetParent(parentKata);//set parent

            cloneBoxKatas[i] = cloneBoxKata;//fill array

            if(i == 0) //for change size x
            {
                Text textTerpanjang = cloneBoxKata.transform.GetChild(0).GetComponent<Text>();

                textTerpanjang.text = splitStringImageSoal[indexTextTerpanjang];//get text

                parentKata.GetComponent<GridLayoutGroup>().cellSize = new Vector2(textTerpanjang.preferredWidth + extraSpaceBoxKata,parentKata.GetComponent<GridLayoutGroup>().cellSize.y);

                cloneBoxKata.transform.GetChild(1).gameObject.SetActive(true); //Mengaktifkan highligh pertama
            }

            Text textCloneBoxKata = cloneBoxKata.transform.GetChild(0).GetComponent<Text>();//set text

            textCloneBoxKata.text = "";

            for (int j = 0; j <splitStringImageSoal[i].Length; j++)
            {
                textCloneBoxKata.text += "_";//change text dengan _
            }
        }
        
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

    void RandomValueString(string[] indexRandoms)
    {
        for (int i = 0; i < indexRandoms.Length; i++)
        {
            string a = indexRandoms[i];
            int b = Random.Range(0, indexRandoms.Length);
            indexRandoms[i] = indexRandoms[b];
            indexRandoms[b] = a;
        }
    }
}
