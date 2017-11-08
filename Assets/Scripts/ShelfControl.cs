﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ShelfIngredient {
    public string name;
    public GameObject prefab;
}

public class ShelfControl : MonoBehaviour {

    [Header("Parameters")]
    public float verticalSeparation = 2.4f;

    [Header("External references")]
    public Animator animatorLeft;
    public Animator animatorRight;
    public ShelfIngredient[] ingredients;
    public Transform anchorLeft;
    public Transform anchorRight;

    public void PlaceIngredients() {
        // Destroy previous ingredients
        foreach (Transform t in anchorLeft) {
            Destroy(t.gameObject);
        }
        foreach (Transform t in anchorRight) {
            Destroy(t.gameObject);
        }

        Shuffle();
        int i;
        for (i = 0; i < ingredients.Length / 2; i++) {
            float pos = i * verticalSeparation;
            GameObject ing = Instantiate(ingredients[i].prefab);
            ing.transform.parent = anchorLeft;
            ing.transform.localPosition = new Vector2(0f, pos);
        }
        int j = 0;
        for (; i < ingredients.Length; i++) {
            float pos = j * verticalSeparation;
            j++;
            GameObject ing = Instantiate(ingredients[i].prefab);
            ing.transform.parent = anchorRight;
            ing.transform.localPosition = new Vector2(0f, pos);
        }

        Ingredient[] ings = FindObjectsOfType<Ingredient>();
        foreach (Ingredient ing in ings) {         
            ing.Init();
        }

    }

    public void OpenShelf() {
        animatorLeft.Play("PanelLeftOpen");
        animatorRight.Play("PanelRightOpen");
    }
    public void CloseShelf() {
        animatorLeft.Play("PanelLeftClose");
        animatorRight.Play("PanelRightClose");
    }


    private void Shuffle() {
        int n = ingredients.Length;
        while (n > 1) {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            ShelfIngredient value = ingredients[k];
            ingredients[k] = ingredients[n];
            ingredients[n] = value;
        }
    }
}
