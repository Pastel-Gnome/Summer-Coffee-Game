using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoffeeCupAttributes : MonoBehaviour
{
	public IngredientValues.CupSize SelectedCupSize;
	public int coffeeType = -1;
    public int milkType = 0;
	public IngredientValues.Flavor SelectedFlavor;
    public List<int> toppingsAdded = new List<int>();
	public Material CoffeeMat;

	public MeshRenderer coffeeMesh;
	private GameObject coffeeObj;
	private Material _coffeeMat;
	[SerializeField] private float fillDuration = 60f; // Duration for filling coffee (currently 1 minutes).

	private float desiredBlend;

	private void Awake()
	{
		coffeeObj = transform.GetChild(0).gameObject;
		_coffeeMat = new Material(CoffeeMat);
		coffeeMesh.sharedMaterial = _coffeeMat;
		_coffeeMat.SetFloat("_FillAmount", 0f);
		desiredBlend =_coffeeMat.GetFloat("_Blend");


	}
	public void SetCupSize(IngredientValues.CupSize selectedCupSize)
	{
		SelectedCupSize = selectedCupSize;
		switch (selectedCupSize)
		{
			case IngredientValues.CupSize.Small:
				transform.localScale = new Vector3(1f, 2f, 1f);
				break;
			case IngredientValues.CupSize.Medium:
				transform.localScale = new Vector3(1f, 2.5f, 1f);
				break;
			case IngredientValues.CupSize.Large:
				transform.localScale = new Vector3(1f, 3.03436f, 1f);
				break;
			default:
				transform.localScale = new Vector3(1f, 1f, 1f);
				break;
		}
	}
	public void setCoffeeType(int desCoffee)
    {
		coffeeType = desCoffee;
		switch (coffeeType){
			case (int)IngredientValues.CoffeeRoast.lightRoast:
				_coffeeMat.SetColor("_CoffeeColor", new Color(0.545f, 0.4f, 0.27f));
				LiquidPourEffectController.liquidPourEffectController.SetMaterial(_coffeeMat.GetColor("_CoffeeColor"));
				break;
			case (int)IngredientValues.CoffeeRoast.mediumRoast:
				_coffeeMat.SetColor("_CoffeeColor", new Color(0.38f, 0.208f, 0.09f));
				LiquidPourEffectController.liquidPourEffectController.SetMaterial(_coffeeMat.GetColor("_CoffeeColor"));
				break;
			case (int)IngredientValues.CoffeeRoast.mediumDarkRoast:
				_coffeeMat.SetColor("_CoffeeColor", new Color(0.204f, 0.098f, 0.055f));
				LiquidPourEffectController.liquidPourEffectController.SetMaterial(_coffeeMat.GetColor("_CoffeeColor"));
				break;
			case (int)IngredientValues.CoffeeRoast.darkRoast:
				_coffeeMat.SetColor("_CoffeeColor", new Color(0.129f, 0.063f, 0.05f));
				LiquidPourEffectController.liquidPourEffectController.SetMaterial(_coffeeMat.GetColor("_CoffeeColor"));
				break;
			default:
				_coffeeMat.SetColor("_CoffeeColor", new Color(1f, 1f, 1f));
				LiquidPourEffectController.liquidPourEffectController.SetMaterial(_coffeeMat.GetColor("_CoffeeColor"));
				break;
		}
    }

	public void setMilkType(int desMilk)
	{
		milkType = desMilk;
		switch (desMilk)
		{
			case (int)IngredientValues.Milk.almondMilk:
				// Almond milk typically has a light beige color
				_coffeeMat.SetColor("_MilkColor", new Color(0.94f, 0.87f, 0.80f, 1.0f));
				_coffeeMat.SetFloat("_AddMilk", 1); // Blend the milk & coffee
				LiquidPourEffectController.liquidPourEffectController.SetMaterial(_coffeeMat.GetColor("_MilkColor"));
				break;
			case (int)IngredientValues.Milk.cashewMilk:
				// Cashew milk is also light beige but slightly warmer
				_coffeeMat.SetColor("_MilkColor", new Color(0.98f, 0.86f, 0.7f, 1.0f));
				_coffeeMat.SetFloat("_AddMilk", 1); // Blend the milk & coffee
				LiquidPourEffectController.liquidPourEffectController.SetMaterial(_coffeeMat.GetColor("_MilkColor"));
				break;
			case (int)IngredientValues.Milk.coconutMilk:
				// Coconut milk is usually white or slightly off-white
				_coffeeMat.SetColor("_MilkColor", new Color(0.98f, 0.98f, 0.91f, 1.0f));
				_coffeeMat.SetFloat("_AddMilk", 1); // Blend the milk & coffee
				LiquidPourEffectController.liquidPourEffectController.SetMaterial(_coffeeMat.GetColor("_MilkColor"));
				break;
			case (int)IngredientValues.Milk.riceMilk:
				// Rice milk has a light beige to pale white color
				_coffeeMat.SetColor("_MilkColor", new Color(0.97f, 0.95f, 0.90f, 1.0f));
				_coffeeMat.SetFloat("_AddMilk", 1); // Blend the milk & coffee
				LiquidPourEffectController.liquidPourEffectController.SetMaterial(_coffeeMat.GetColor("_MilkColor"));
				break;
			case (int)IngredientValues.Milk.skimMilk:
				// Skim milk is almost white but slightly translucent
				_coffeeMat.SetColor("_MilkColor", new Color(1.0f, 1.0f, 0.94f, 1.0f));
				_coffeeMat.SetFloat("_AddMilk", 1); // Blend the milk & coffee
				LiquidPourEffectController.liquidPourEffectController.SetMaterial(_coffeeMat.GetColor("_MilkColor"));
				break;
			case (int)IngredientValues.Milk.soyMilk:
				// Soy milk can vary but is typically a light creamy color
				_coffeeMat.SetColor("_MilkColor", new Color(0.98f, 0.92f, 0.86f, 1.0f));
				_coffeeMat.SetFloat("_AddMilk", 1); // Blend the milk & coffee
				LiquidPourEffectController.liquidPourEffectController.SetMaterial(_coffeeMat.GetColor("_MilkColor"));
				break;
			case (int)IngredientValues.Milk.wholeMilk:
				// Whole milk is usually a bright white color
				_coffeeMat.SetColor("_MilkColor", new Color(1.0f, 1.0f, 1.0f, 1.0f));
				_coffeeMat.SetFloat("_AddMilk", 1); // Blend the milk & coffee
				LiquidPourEffectController.liquidPourEffectController.SetMaterial(_coffeeMat.GetColor("_MilkColor"));
				break;
			case (int)IngredientValues.Milk.noMilk:
				//  no milk option (transparent)
				_coffeeMat.SetColor("_MilkColor", new Color(1.0f, 1.0f, 1.0f, 0.0f));
				_coffeeMat.SetFloat("_AddMilk", 0); //no milk indication
				break;
			default:
				// Default (transparent)
				_coffeeMat.SetColor("_MilkColor", new Color(1.0f, 1.0f, 1.0f, 0.0f));
				_coffeeMat.SetFloat("_AddMilk", 0); //no milk indication
				break;
		}
	}

	public void addTopping(int desTopping)
	{
		toppingsAdded.Add(desTopping);
		switch (desTopping)
		{
			case (int)IngredientValues.Toppings.cream:
				//added cream
				break;
			case (int)IngredientValues.Toppings.whippedCream:
				//added whippedCream
				break;
			case (int)IngredientValues.Toppings.sprinkles:
				//added sprinkles
				break;
			case (int)IngredientValues.Toppings.cocoaPowder:
				//added cocoaPowder
				break;
			case (int)IngredientValues.Toppings.cinnamonPowder:
				//added cinnamonPowder
				break;
			case (int)IngredientValues.Toppings.chocolateSyrup:
				//added chocolateSyrup
				break;
			case (int)IngredientValues.Toppings.caramelSyrup:
				//added caramelSyrup
				break;
			case (int)IngredientValues.Toppings.brownSugar:
				//added brownSugar
				break;
			case (int)IngredientValues.Toppings.cherry:
				//added cherry
				break;
			default:
				// Default (perhaps have an error object)
				break;
		}
	}

	public IEnumerator PourCoffee()
	{
		_coffeeMat.SetFloat("_FillAmount", 0f);
		float elapsedTime = 0.0f;
		while (elapsedTime < fillDuration)
		{
			elapsedTime += Time.deltaTime;
			float fillAmount = Mathf.Clamp01(elapsedTime / fillDuration); // Calculate fill amount based on elapsed time.
			_coffeeMat.SetFloat("_FillAmount", fillAmount);
			yield return null; // Wait for the next frame.
		}
		_coffeeMat.SetFloat("_FillAmount", 1f);
		LiquidPourEffectController.liquidPourEffectController.StopPouring();

	}
	public void SetBlending()
	{
		if (_coffeeMat.GetFloat("_AddMilk") == 1)
		{
			_coffeeMat.SetFloat("_Blend", desiredBlend);
		}
	}
	public IEnumerator PourMilk()
	{
		if (_coffeeMat.GetFloat("_FillAmount") < 1.0f)
		{
			float startProgress = _coffeeMat.GetFloat("_FillAmount"); // Current progress value
			_coffeeMat.SetFloat("_Blend", 0f);
			_coffeeMat.SetFloat("_AddMilk", 1f);
			float elapsedTime = 0.0f;
			while (elapsedTime < fillDuration)
			{
				elapsedTime += Time.deltaTime;
				float fillAmount = Mathf.Lerp(startProgress, 1.0f, elapsedTime / fillDuration);
				float BlendAmount = Mathf.Lerp(0.0f, desiredBlend, elapsedTime / fillDuration);
				_coffeeMat.SetFloat("_FillAmount", fillAmount);
				_coffeeMat.SetFloat("_Blend", BlendAmount);
				yield return null;
			}

			_coffeeMat.SetFloat("_FillAmount", 1f);
			_coffeeMat.SetFloat("_Blend", desiredBlend);
			LiquidPourEffectController.liquidPourEffectController.StopPouring();
		}
	}
}
