using UnityEngine;
using System.Collections;
using System;
using Random = UnityEngine.Random;

[Serializable]
public class Order
{
	public IngredientValues.CupSize cupSize;
	public IngredientValues.CoffeeRoast coffeeRoast;
	public IngredientValues.Milk milkType;
	public IngredientValues.Flavor flavor;
	public IngredientValues.Toppings[] toppings;
}
public class OrderDisplay : MonoBehaviour
{
	public static OrderDisplay orderDisplay;
	public Order currentOrder;
	public CanvasGroup[] ingredientsEntries; // will be replaced with the ingredientsEntries prefab 
	public CanvasGroup[] dottedLinePrefabs;

	[Header("Ingredients Choices Prefabs")]
	[SerializeField] CupSizeIndicator[] CupSizePrefabs;
	[SerializeField] CoffeeRoastTypeIndicator[] CoffeeChoicesPrefabs;
	[SerializeField] MilkTypeIndicator[] MilkChoicesPrefabs;
	[SerializeField] CoffeeFlavorIndicator[] flavorChoicesPrefabs;
	[SerializeField] ToppingIndicator[] toppingsPrefabs;

	[Header("Ingredients Choices Containers")]
	[SerializeField] GameObject CupSizeContainer;
	[SerializeField] GameObject CoffeeRoastContainer;
	[SerializeField] GameObject MilkTypeContainer;
	[SerializeField] GameObject flavorContainer;
	[SerializeField] GameObject ToppingsContainer;

	public float displayDuration = 2.0f;

	private void Awake()
	{
		if (orderDisplay == null)
		{
			OrderDisplay.orderDisplay = this;
		}
		else
		{
			Destroy(this);
		}
	}

	public void StartOrder()
	{
		InitializeOrder();
		StartCoroutine(DisplayOrders());
	}

	private void InitializeOrder()
	{
		currentOrder = new Order();
		currentOrder.cupSize = InstantiateRandomCupSize(CupSizePrefabs, CupSizeContainer);
		currentOrder.coffeeRoast = InstantiateRandomRoastPrefab(CoffeeChoicesPrefabs, CoffeeRoastContainer);
		currentOrder.milkType = InstantiateRandomMilkPrefab(MilkChoicesPrefabs, MilkTypeContainer);
		currentOrder.flavor = InstantiateRandomFlavorPrefab(flavorChoicesPrefabs, flavorContainer);
		currentOrder.toppings = InstantiateRandomToppingsPrefabs(toppingsPrefabs, ToppingsContainer);
		ScoringSystem.scoringSystem.SaveOrder(currentOrder);
		LogOrderDetails();
	}

	// DEBUG Method - WILL BE DELETED
	private void LogOrderDetails()
	{
		Debug.Log("Order Details:");
		Debug.Log("Cup Size: " + currentOrder.cupSize);
		Debug.Log("Coffee Roast: " + currentOrder.coffeeRoast);
		Debug.Log("Milk Type: " + currentOrder.milkType);
		Debug.Log("Flavor: " + currentOrder.flavor);

		string toppingsList = "Toppings: ";
		foreach (var topping in currentOrder.toppings)
		{
			toppingsList += topping + ", ";
		}
		Debug.Log(toppingsList.TrimEnd(',', ' '));
	}

	IEnumerator DisplayOrders()
	{
		yield return new WaitForSeconds(1f); // wait a second to account for order generation
		for (int i = 0; i < ingredientsEntries.Length; i++)
		{
			// Show the different recipe entries           
			ingredientsEntries[i].alpha = 1;
			if (i < ingredientsEntries.Length - 1)
			{
				dottedLinePrefabs[i].alpha = 1;
				yield return new WaitForSeconds(displayDuration);
			}
		}

		StationControl.stationControl.EnableAllStationButtons();
	}

	private IngredientValues.CupSize InstantiateRandomCupSize(CupSizeIndicator[] prefabs, GameObject container)
	{
		int randomIndex = Random.Range(0, prefabs.Length);
		IngredientValues.CupSize chosenSize = prefabs[randomIndex].cupSize;
		Instantiate(prefabs[randomIndex].gameObject, container.transform);
		return chosenSize;
	}
	private IngredientValues.CoffeeRoast InstantiateRandomRoastPrefab(CoffeeRoastTypeIndicator[] prefabs, GameObject container)
	{
		int randomIndex = Random.Range(0, prefabs.Length);
		IngredientValues.CoffeeRoast coffeeType = prefabs[randomIndex].coffeeType;
		Instantiate(prefabs[randomIndex].gameObject, container.transform);
		return coffeeType;
	}
	private IngredientValues.Milk InstantiateRandomMilkPrefab(MilkTypeIndicator[] prefabs, GameObject container)
	{
		int randomIndex = Random.Range(0, prefabs.Length);
		IngredientValues.Milk milkType = prefabs[randomIndex].milkType;
		Instantiate(prefabs[randomIndex].gameObject, container.transform);
		return milkType;
	}
	private IngredientValues.Flavor InstantiateRandomFlavorPrefab(CoffeeFlavorIndicator[] prefabs, GameObject container)
	{
		int randomIndex = Random.Range(0, prefabs.Length);
		IngredientValues.Flavor flavor = prefabs[randomIndex].CoffeeFlavor;
		Instantiate(prefabs[randomIndex].gameObject, container.transform);
		return flavor;
	}

	private IngredientValues.Toppings[] InstantiateRandomToppingsPrefabs(ToppingIndicator[] prefabs, GameObject container)
	{
		int randomCount = Random.Range(1,4);
		var toppings = new IngredientValues.Toppings[randomCount];
		for (int i = 0; i < randomCount; i++)
		{
			int randomIndex = Random.Range(0, prefabs.Length);
			toppings[i] = prefabs[randomIndex].topping;
			Instantiate(prefabs[randomIndex].gameObject, container.transform);
		}
		return toppings;
	}
}
