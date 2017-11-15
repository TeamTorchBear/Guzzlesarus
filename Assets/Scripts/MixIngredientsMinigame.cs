﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public struct IngredientNeeded {
    public string ingredient;
    public int amount;
}
[CreateAssetMenu(menuName = "Guzzlesaurus/Minigames/MixIngredients")]
public class MixIngredientsMinigame : Minigame {

    public int SCORE = 0;

    public List<IngredientNeeded> ingredientsNeeded;
    private PromptControl promptControl;
    private ShelfControl shelfControl;
    private Dictionary<string, Sprite> ingredients;
    private Dictionary<string, int> currentIngredients;
    private int totalIngredientsAmount;
    private int currentIngredientsAmount;
    private int currentIngredient;
    private MinigameManager manager;



    public override void StartMinigame() {
        base.StartMinigame();
        promptControl = FindObjectOfType<PromptControl>();
        shelfControl = FindObjectOfType<ShelfControl>();
        ingredients = new Dictionary<string, Sprite>();
        currentIngredients = new Dictionary<string, int>();

        manager = FindObjectOfType<MinigameManager>();

        totalIngredientsAmount = 0;
        currentIngredientsAmount = 0;
        currentIngredient = 0;

        foreach (IngredientNeeded i in ingredientsNeeded) {
            totalIngredientsAmount += i.amount;
            currentIngredients[i.ingredient] = 0;
        }

        Ingredient[] ings = FindObjectsOfType<Ingredient>();
        foreach (Ingredient i in ings) {
            if (!ingredients.ContainsKey(i.ingredientName)) {
                ingredients.Add(i.ingredientName, i.ingredientSprite);
            }
            i.Init();
        }
        //shelfControl.PlaceIngredients();
        AskForIngredient(ingredientsNeeded[currentIngredient].ingredient, ingredientsNeeded[currentIngredient].amount);
    }

    private void AskForIngredient(string name, int amount) {
        // Debug.Log("Ask: " + name);
        promptControl.SetIngredient(ingredients[name], amount, name);
        
        promptControl.ShowPromptAfter(timeToPromt, promptTime);
    }

    private void NextIngredient() {
        currentIngredient++;
        currentIngredientsAmount = 0;
        if (currentIngredient == ingredientsNeeded.Count) {
            if(CheckIngredients()){
				EndMinigame();
            }
            return;
        }
        AskForIngredient(ingredientsNeeded[currentIngredient].ingredient, ingredientsNeeded[currentIngredient].amount);
    }

    public void AddIngredient(Ingredient i) {
        // Debug.Log("Added ingredient: " + i.ingredientName);
        if (!currentIngredients.ContainsKey(i.ingredientName)) {
            currentIngredients[i.ingredientName] = 0;
        }
        if (i.ingredientName == ingredientsNeeded[currentIngredient].ingredient) {
            SCORE += 10;
        }
        currentIngredients[i.ingredientName]++;
        currentIngredientsAmount++;
        if (IsStepFinished()) {
            NextIngredient();
        }
    }

    public bool IsStepFinished() {
        //Debug.Log(currentIngredientsAmount + "/" + totalIngredientsAmount);
        return currentIngredientsAmount == ingredientsNeeded[currentIngredient].amount;
    }

    private bool CheckIngredients() {
        foreach (IngredientNeeded i in ingredientsNeeded) {
            if (currentIngredients[i.ingredient] < i.amount) {
                return false;
            }
        }
        return true;
    }

    private void EndMinigame() {
        // TODO: save score somewhere
        Debug.Log("SCORE: " + SCORE);
        manager.ScreenFadeOut("MixingWetIngredients");
    }

    

}