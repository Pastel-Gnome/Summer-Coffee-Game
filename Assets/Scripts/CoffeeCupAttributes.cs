using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoffeeCupAttributes : MonoBehaviour
{
    public int coffeeType = 0;
    public int milkType = 0;
    public List<int> toppingsAdded = new List<int>();
	private GameObject coffeeObj;
	private Image coffeeImg;

	private void Start()
	{
		coffeeObj = transform.GetChild(0).gameObject;
		coffeeImg = coffeeObj.GetComponent<Image>();
		coffeeObj.SetActive(false);
	}

	public void setCoffeeType(int desCoffee)
    {
		coffeeType = desCoffee;
		switch (coffeeType){
			case (int)IngredientValues.Coffee.lightRoast:
				coffeeImg.color = new Color(0.545f, 0.4f, 0.27f);
				break;
			case (int)IngredientValues.Coffee.mediumRoast:
				coffeeImg.color = new Color(0.38f, 0.208f, 0.09f);
				break;
			case (int)IngredientValues.Coffee.mediumDarkRoast:
				coffeeImg.color = new Color(0.204f, 0.098f, 0.055f);
				break;
			case (int)IngredientValues.Coffee.darkRoast:
				coffeeImg.color = new Color(0.129f, 0.063f, 0.05f);
				break;
			default:
				coffeeImg.color = new Color(0f, 1f, 1f);
				break;
		}
		coffeeObj.SetActive(true);
    }

	public void setMilkType(int desMilk)
	{
		milkType = desMilk;
		if(milkType != (int)IngredientValues.Milk.noMilk)
		{
			coffeeImg.color = Color.Lerp(Color.white, coffeeImg.color, 0.8f);
		}
	}
}
