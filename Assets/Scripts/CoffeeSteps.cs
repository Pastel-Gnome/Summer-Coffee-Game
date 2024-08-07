using UnityEngine;
using UnityEngine.UI;

public class CoffeeSteps : MonoBehaviour
{
	public CoffeeCupAttributes currentFocus;
	public Transform[] cupAnchor;
	[SerializeField] StationControl sc;
	[SerializeField] GameObject cupPrefab;
	public GameObject currentScreen;
	public void startOrder() //activated on order screen
	{
		OrderDisplay.orderDisplay.StartOrder();
		currentScreen.GetComponentInChildren<Button>().interactable = false;
	}

	public void createNewCup()
    {
		if(currentFocus == null)
		{
			GameObject newCup = Instantiate(cupPrefab, cupAnchor[0].position, Quaternion.identity, cupAnchor[0]);
			currentFocus = newCup.GetComponent<CoffeeCupAttributes>();
			if (currentFocus != null)
			{
				sc.enableMachineButton(1);
				sc.enableMachineButton(2);
			}
		}
    }

	public void removeCupFromMachine(int destination)
	{
		// destinations (where the cup will move to): 1 = brewing station, 2 = milk station, 3 = topping station, 4 = serving "station", 5 = order completed, -99 = trash
		if (destination == -99 || destination == 5)
		{
			// send to trash or complete order (make a branch if special actions associated with trash, such as sound effects)
			if (currentFocus != null)
			{
				if(currentScreen.name == "Brewing Station Panel") sc.resetMachine(1);
				Destroy(currentFocus.gameObject);
			}
		}
		else
		{
			if (destination == 2)
			{
				// brewing station completed, send to milk station and reset brewing station
				sc.disableMachineButton(1);
				sc.resetMachine(1);
			}
			else if (destination == 3)
			{
				// milk station completed, send to topping station and reset milk station
				sc.disableMachineButton(2);
			}
			else if (destination == 4)
			{
				sc.disableMachineButton(3);
			}
			else if (destination != 4)
			{
				Debug.LogError("Removed cup from machine but no valid destination given. No action taken, returned to machine.");
				destination = 1;
			}
			//currentFocus.transform.SetParent(cupAnchor[destination-1], false); //there are currently never any cups sent to destination "0" (order station), change number values if this becomes true
			

			// If there is a child to cs.cupAnchor[index-1], move it to cs.cupAnchor[index]
			if (destination - 1 > 0 && cupAnchor[destination - 2].transform.childCount > 0)
			{
				Transform parentTransform = cupAnchor[destination - 1].transform;
				Transform childTransform = cupAnchor[destination - 2].transform;
				foreach (Transform child in childTransform)
				{
					child.SetParent(parentTransform);
					child.position = parentTransform.position;
				}
			}
		}
		
		currentFocus = null; //currently this always runs and therefore always moves to the next screen, but this should change if multiple cups are allowed

		sc.closeMachineChoices(1);
		sc.closeMachineChoices(2);

		// if there is no other coffee in progress on this screen, disable inactive screens and enable next screen
		if (currentFocus == null && destination != -99 && destination != 5)
		{
			sc.setCurrentScreen(destination);
		}
	}

	public void ChooseCupSize(int cupSize)
	{
		currentFocus.SetCupSize((IngredientValues.CupSize)cupSize);
		sc.IsCupSizeSelected = true;
	}

	public void addCoffee(int desCoffee)
	{
		currentFocus.setCoffeeType(desCoffee);
		LiquidPourEffectController.liquidPourEffectController.Begin();
		StartCoroutine(currentFocus.PourCoffee());
	}
	public void addMilk(int desMilk)
	{
		currentFocus.setMilkType(desMilk);
		if (desMilk > 0)
		{
			LiquidPourEffectController.liquidPourEffectController.Begin();
			StartCoroutine(currentFocus.PourMilk());
		}
	}

	public void addTopping()
	{
		// add code here for adding toppings to currentFocus
		currentFocus.addTopping(0);
		Debug.Log("Topping Added!");
	}

	public void setCurrentFocus(GameObject desFocus)
	{
		if (desFocus != null)
		{
			CoffeeCupAttributes nextCoffeeInfo = desFocus.GetComponentInChildren<CoffeeCupAttributes>();
			currentFocus = nextCoffeeInfo;
		}
	}
	public void stopCoffeePouring()
	{
		currentFocus.SetBlending();
		StopAllCoroutines();
		LiquidPourEffectController.liquidPourEffectController.StopPouring();
	}
	public void serveOrder()
	{
		removeCupFromMachine(4);
		scoreOrder();
	}

	public void scoreOrder()
	{
		Debug.Log("Scoring now...");
		ScoringSystem.scoringSystem.LoadCoffee(currentFocus);
		ScoringSystem.scoringSystem.CheckOrderToCoffee();
	}

	public void completeServingOrder()
	{
		removeCupFromMachine(5);
		if (currentFocus == null)
		{
			sc.setCurrentScreen(0);
			Button orderButton = currentScreen.GetComponentInChildren<Button>(true);
			orderButton.interactable = true;
			orderButton.gameObject.SetActive(true);
		}/* else
		{
			sc.setCurrentScreen(currentFocus.myStation); // either set to first station or last station containing a cup
		}*/
	}
}
