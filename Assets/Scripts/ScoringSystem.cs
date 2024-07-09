using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoringSystem : MonoBehaviour
{
	public static ScoringSystem scoringSystem;
	CoffeeCupAttributes currentCoffee;
	List<Order> orders = new List<Order>();
	int orderIndex = 0;

	private void Awake()
	{
		if (scoringSystem == null)
		{
			ScoringSystem.scoringSystem = this;
		}
		else
		{
			Destroy(this);
		}
	}

	public void SaveOrder(Order tempOrder)
    {
        orders.Add(tempOrder);
		ChangeActiveOrder(orders.Count - 1);
	}

	public void LoadCoffee(CoffeeCupAttributes tempCoffee)
	{
		currentCoffee = tempCoffee;
		Debug.Log(currentCoffee != null);
	}

	public int CheckOrdersAmount()
	{
		return orders.Count;
	}

	public void ChangeActiveOrder(int desOrder)
	{
		orderIndex = desOrder;
	}

	public void CheckOrderToCoffee()
	{
		Debug.Log(orders[orderIndex] != null);
		float scoreValue = 0;
		float COFFEE_SCORE = 40f; // cup size & coffee type
		float MILK_FLAVOR_SCORE = 40f; // milk type and flavor syrup
		float TOPPING_SCORE = 20f; // toppings

		if (currentCoffee.SelectedCupSize == orders[orderIndex].cupSize) scoreValue += COFFEE_SCORE / 2;
		if (currentCoffee.coffeeType == ((int)orders[orderIndex].coffeeRoast)) scoreValue += COFFEE_SCORE / 2;

		if (currentCoffee.milkType == ((int)orders[orderIndex].milkType)) scoreValue += MILK_FLAVOR_SCORE / 2;
		if (currentCoffee.SelectedFlavor == orders[orderIndex].flavor) scoreValue += MILK_FLAVOR_SCORE / 2;

		scoreValue += Mathf.Max(TOPPING_SCORE - Mathf.Abs((currentCoffee.toppingsAdded.Count - orders[orderIndex].toppings.Length)), 0);
		Debug.Log("Final Score: " + scoreValue);
		orders.RemoveAt(orderIndex);
	}
}
