using UnityEngine;
using UnityEngine.UI;

public class StationControl : MonoBehaviour
{
	[SerializeField] CoffeeSteps cs;
	Button[] stationButtons;

	[SerializeField] Button brewCoffeeButton;
	[SerializeField] Button chooseMilkButton;
	[SerializeField] GameObject coffeeChoices;
	[SerializeField] GameObject milkChoices;

	private void Start()
	{
		stationButtons = new Button[transform.childCount];
		for (int i = 0; i < stationButtons.Length; i++) //assuming this script is on Station Buttons gameobject, set station buttons
		{
			stationButtons[i] = transform.GetChild(i).gameObject.GetComponent<Button>();
		}

		//disable brew coffee and choose milk buttons, so choice cannot be made without a cup
		disableMachineButton(1);
		disableMachineButton(2);
	}

	public void setCurrentScreen(int desScreen)
	{
		for (int i = 0; i < cs.transform.childCount; i++) //assuming CoffeeSteps is on Stations gameobject, turn off all stations and turn on the desired station
		{
			cs.transform.GetChild(i).gameObject.SetActive(false);
		}
		cs.transform.GetChild(desScreen).gameObject.SetActive(true);

		if (desScreen >= 0)
		{
			updateStationButtons(desScreen);

			if (desScreen > 0 && desScreen < 4)
			{
				cs.setCurrentFocus(cs.cupAnchor[desScreen - 1].gameObject);

				if (desScreen == 1) //coffee brewing
				{
					if (cs.currentFocus != null) //if cup in-play
					{
						if (cs.currentFocus.coffeeType == -1) //if no coffee in cup, make sure coffee can be brewed
						{
							enableMachineButton(1);
						}
						else //if coffee in cup, make sure coffee cannot be overridden (can be deleted when multiple coffees/espresso shots allowed)
						{
							disableMachineButton(1);
						}
					}
					else //if no cup is in-play, disable coffee machine
					{
						disableMachineButton(1);
					}
				}
				else if (desScreen == 2) //milk frothing
				{
					if (cs.currentFocus != null) //if cup in-play
					{
						if (cs.currentFocus.milkType == 0) //if no milk in cup (even after choosing "no milk" option on milk machine), make sure milk can be added
						{
							enableMachineButton(2);
						}
						else // if milk in cup, make sure milk cannot be overridden (can be deleted when multiple milks allowed)
						{
							disableMachineButton(2);
						}
					}
					else //if no cup is in-play, disable milk machine
					{
						disableMachineButton(2);
					}
				}
			} else if (desScreen == 4)
			{
				cs.setCurrentFocus(cs.cupAnchor[desScreen - 1].gameObject);
				disableAllStationButtons();
			}
		}
	}

	public void updateStationButtons(int desScreen)
	{
		for (int i = 0; i < stationButtons.Length; i++) //assuming this script is on Station Buttons gameobject, turn on all station buttons and turn off the current station button (since you are already there)
		{
			stationButtons[i].interactable = true;
		}
		if(desScreen != 4) stationButtons[desScreen].interactable = false;
	}

	public void disableAllStationButtons()
	{
		for (int i = 0; i < stationButtons.Length; i++)
		{
			stationButtons[i].interactable = false;
		}
	}

	public void disableMachineButton(int station)
	{
		if (station == 1) //brewing station
		{
			brewCoffeeButton.interactable = false;
		}
		else if (station == 2) //milk station
		{
			chooseMilkButton.interactable = false;
		}
	}

	public void enableMachineButton(int station)
	{
		if (station == 1) //brewing station
		{
			brewCoffeeButton.interactable = true;
		}
		else if (station == 2) //milk station
		{
			chooseMilkButton.interactable = true;
		}
	}

	public void openMachineChoices(int station)
	{
		if (station == 1) //brewing station
		{
			coffeeChoices.SetActive(true);
		}
		else if (station == 2) //milk station
		{
			milkChoices.SetActive(true);
		}
	}

	public void closeMachineChoices(int station)
	{
		if (station == 1) //brewing station
		{
			coffeeChoices.SetActive(false);
		}
		else if (station == 2) //milk station
		{
			milkChoices.SetActive(false);
		}
	}
}
