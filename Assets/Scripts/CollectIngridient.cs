﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectIngridient : MonoBehaviour {

    public GameObject eggs,flour,sugar,salt, milk,butter;
    public Canvas canvas;
    public Transform child;

    bool isClick,i;
    int clickTimes=0;
    Data data;
    Image eggsimage, flourimage, sugarimage, saltimage, milkimage, butterimage;

    // Use this for initialization
    void Start () {
        Button btn = this.GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
        isClick = false;
        i = true;
	}
	
	// Update is called once per frame
	void Update () {
        data = SaveNLoadTxt.Load();
        if (isClick&&i)
        {
            isClick = false;
            if (data.eggQuantity < 2)
            {
                data.eggQuantity++;
                IngredientComesOut("egg");
                SaveNLoadTxt.Save(data);
                i = false;
            }
            else if (data.flourQuantity < 2)
            {
                IngredientComesOut("flour");
                data.flourQuantity++;
                SaveNLoadTxt.Save(data);
                i = false;
            }
            else if (data.sugarQuantity < 2)
            {
                IngredientComesOut("sugar");
                data.sugarQuantity++;
                SaveNLoadTxt.Save(data);
                i = false;
            }
            else if (data.saltQuantity < 1)
            {
                IngredientComesOut("salt");
                data.saltQuantity++;
                SaveNLoadTxt.Save(data);
                i = false;
            }
            else if (data.butterQuantity < 1)
            {
                IngredientComesOut("butter");
                data.butterQuantity++;
                SaveNLoadTxt.Save(data);
                i = false;
            }
            else if (data.milkQuantity < 1)
            {
                IngredientComesOut("milk");
                data.milkQuantity++;
                SaveNLoadTxt.Save(data);
                i = false;
            }
        }
        FadeOut(eggsimage);
        FadeOut(flourimage);
        FadeOut(saltimage);
        FadeOut(sugarimage);
        FadeOut(milkimage);
        FadeOut(butterimage);

    }

    void OnClick()
    {
        isClick = true;
    }

    void IngredientComesOut(string items)
    {
        switch (items)
        {
            case "egg":
                eggsimage = Instantiate(eggs).GetComponent<Image>();
                eggsimage.transform.SetParent(canvas.transform);
                eggsimage.transform.position = child.position;
                break;
            case "flour":
                flourimage = Instantiate(flour).GetComponent<Image>();
                flourimage.transform.SetParent(canvas.transform);
                flourimage.transform.position = child.position;
                break;
            case "salt":
                saltimage = Instantiate(salt).GetComponent<Image>();
                saltimage.transform.SetParent(canvas.transform);
                saltimage.transform.position = child.position;
                break;
            case "sugar":
                sugarimage = Instantiate(sugar).GetComponent<Image>();
                sugarimage.transform.SetParent(canvas.transform);
                sugarimage.transform.position = child.position;
                break;
            case "butter":
                butterimage = Instantiate(butter).GetComponent<Image>();
                butterimage.transform.SetParent(canvas.transform);
                butterimage.transform.position = child.position;
                break;
            case "milk":
                milkimage = Instantiate(milk).GetComponent<Image>();
                milkimage.transform.SetParent(canvas.transform);
                milkimage.transform.position = child.position;
                break;
            default:
                break;
        }
    }

    void FadeOut(Image image)
    {
        if (image)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - 0.05f);
            if (image.color.a <= 0)
            {
                Destroy(image.gameObject);
                i = true;
            }
        }
    }
}