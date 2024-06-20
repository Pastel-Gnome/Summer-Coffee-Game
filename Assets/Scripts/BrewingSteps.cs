using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrewingSteps : MonoBehaviour
{
	//Transform coffeeMachine;
	[SerializeField] Transform cupAnchor;
	[SerializeField] GameObject cupPrefab;
	CoffeeCupAttributes currentFocus;

	[SerializeField] Button brewCoffeeButton;
	[SerializeField] Button chooseMilkButton;
	[SerializeField] GameObject coffeeChoices;
	[SerializeField] GameObject milkChoices;

	[SerializeField] Transform topTempAnchor;
	[SerializeField] Transform topCurrentAnchor;

	private void Start()
	{
		//coffeeMachine = cupAnchor.parent;
		brewCoffeeButton.interactable = false;
		chooseMilkButton.interactable = false;
	}

	public void createNewCup()
    {
		if(currentFocus == null)
		{
			GameObject newCup = Instantiate(cupPrefab, cupAnchor.position, Quaternion.identity, transform);
			currentFocus = newCup.GetComponent<CoffeeCupAttributes>();
			if (currentFocus != null)
			{
				brewCoffeeButton.interactable = true;
				chooseMilkButton.interactable = true;
			}
		}
    }

	public void removeCupFromMachine(int destination)
	{
		// destinations: 0 = brewing station, 1 = topping station, 2 = serving "station", -99 = trash
		if (destination == 1)
		{
			if (topCurrentAnchor.childCount == 0)
			{
				currentFocus.transform.SetParent(topCurrentAnchor);
				currentFocus.transform.position = topCurrentAnchor.position;
			}
			else
			{
				currentFocus.transform.SetParent(topTempAnchor);
				currentFocus.transform.position = topTempAnchor.position;
			}
		}
		else if (destination == -99)
		{
			if(currentFocus != null)
			{
				Destroy(currentFocus.gameObject);
			}
		}
		else
		{
			Debug.LogError("Removed cup from machine but no valid destination given. Routing to topping station");
			if (topCurrentAnchor.childCount == 0)
			{
				currentFocus.transform.SetParent(topCurrentAnchor);
				currentFocus.transform.position = topCurrentAnchor.position;
			}
			else
			{
				currentFocus.transform.SetParent(topTempAnchor);
				currentFocus.transform.position = topTempAnchor.position;
			}
		}
		
		currentFocus = null;
		brewCoffeeButton.interactable = false;
		chooseMilkButton.interactable = false;

		brewCoffeeButton.gameObject.SetActive(true);
		chooseMilkButton.gameObject.SetActive(false);

		coffeeChoices.SetActive(false);
		milkChoices.SetActive(false);
	}

	public void addCoffee(int desCoffee)
	{
		currentFocus.setCoffeeType(desCoffee);
	}

	public void addMilk(int desMilk)
	{
		currentFocus.setMilkType(desMilk);
	}
}
