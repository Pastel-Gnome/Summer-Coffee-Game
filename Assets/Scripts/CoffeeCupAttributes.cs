using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoffeeCupAttributes : MonoBehaviour
{
	public IngredientValues.CupSize SelectedCupSize;
	public int coffeeType = -1;
    public int milkType = 0;
    public List<int> toppingsAdded = new List<int>();
	public Material CoffeeMat;

	public MeshRenderer coffeeMesh;
	private GameObject coffeeObj;
	private Image coffeeImg;
	private Material _coffeeMat;
	[SerializeField] private float fillDuration = 60f; // Duration for filling coffee (currently 1 minutes).

	private void Awake()
	{
		coffeeObj = transform.GetChild(0).gameObject;
		coffeeImg = coffeeObj.GetComponent<Image>();
		_coffeeMat = new Material(CoffeeMat);
		coffeeMesh.sharedMaterial = _coffeeMat;
		_coffeeMat.SetFloat("Progress", 0f);

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
			case (int)IngredientValues.Coffee.lightRoast:
				_coffeeMat.SetColor("_WaterColor", new Color(0.545f, 0.4f, 0.27f));
				LiquidPourEffectController.liquidPourEffectController.SetMaterial(_coffeeMat.GetColor("_WaterColor"));
				break;
			case (int)IngredientValues.Coffee.mediumRoast:
				_coffeeMat.SetColor("_WaterColor", new Color(0.38f, 0.208f, 0.09f));
				LiquidPourEffectController.liquidPourEffectController.SetMaterial(_coffeeMat.GetColor("_WaterColor"));
				break;
			case (int)IngredientValues.Coffee.mediumDarkRoast:
				_coffeeMat.SetColor("_WaterColor", new Color(0.204f, 0.098f, 0.055f));
				LiquidPourEffectController.liquidPourEffectController.SetMaterial(_coffeeMat.GetColor("_WaterColor"));
				break;
			case (int)IngredientValues.Coffee.darkRoast:
				_coffeeMat.SetColor("_WaterColor", new Color(0.129f, 0.063f, 0.05f));
				LiquidPourEffectController.liquidPourEffectController.SetMaterial(_coffeeMat.GetColor("_WaterColor"));
				break;
			default:
				_coffeeMat.SetColor("_WaterColor", new Color(1f, 1f, 1f));
				LiquidPourEffectController.liquidPourEffectController.SetMaterial(_coffeeMat.GetColor("_WaterColor"));
				break;
		}
    }


	public void setMilkType(int desMilk)
	{
		milkType = desMilk;
		if(milkType != (int)IngredientValues.Milk.noMilk)
		{
			coffeeImg.color = Color.Lerp(Color.white, coffeeImg.color, 0.8f);
		}
	}

	public IEnumerator FillCup()
	{
		_coffeeMat.SetFloat("_Progress", 0f);
		float elapsedTime = 0.0f;
		while (elapsedTime < fillDuration)
		{
			elapsedTime += Time.deltaTime;
			float fillAmount = Mathf.Clamp01(elapsedTime / fillDuration); // Calculate fill amount based on elapsed time.
			_coffeeMat.SetFloat("_Progress", fillAmount);

			yield return null; // Wait for the next frame.
		}
		_coffeeMat.SetFloat("_Progress", 1f);
		LiquidPourEffectController.liquidPourEffectController.stopCoffeePouring();

	}
}
