using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StationControl : MonoBehaviour
{
	public static StationControl stationControl;

	[SerializeField] CoffeeSteps cs;
	Button[] stationButtons;
	[SerializeField]  string[] stationScenesNames;

	[SerializeField] Button brewCoffeeButton;
	[SerializeField] Button chooseMilkButton;
	[SerializeField] GameObject CupSizeChoices;
	[SerializeField] GameObject coffeeChoices;
	[SerializeField] GameObject milkChoices;

	private bool isCupSizeSelected = false;

	public bool IsCupSizeSelected
	{
		get { return isCupSizeSelected; }
		set { isCupSizeSelected = value; }
	}
	private void Awake()
	{
		if (stationControl == null)
		{
			StationControl.stationControl = this;
		}
		else
		{
			Destroy(this);
		}
	}
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
		setCurrentScreen(0);

	}

	public void setCurrentScreen(int desScreen)
	{
		for (int i = 0; i < cs.transform.childCount; i++) //assuming CoffeeSteps is on Stations gameobject, turn off all stations and turn on the desired station
		{
			cs.transform.GetChild(i).gameObject.SetActive(false);
		}
		cs.transform.GetChild(desScreen).gameObject.SetActive(true);
		UnloadScenes();
		SceneManager.LoadScene(stationScenesNames[desScreen], LoadSceneMode.Additive);

		if (desScreen >= 0)
		{
			updateStationButtons(desScreen);

			if (desScreen > 0 && desScreen < 4)
			{
				cs.setCurrentFocus(cs.cupAnchor[desScreen - 1].gameObject);
				// WOULD NEED TO UPDATE ONCE THE UI IS UPDATED
				switch (desScreen)
				{
					case 1: // coffee brewing
						if (cs.currentFocus != null) // if cup in-play
						{
							if (cs.currentFocus.coffeeType == -1) // if no coffee in cup
							{
								enableMachineButton(1);
							}
							else // if coffee in cup
							{
								disableMachineButton(1);
							}
							enableAnchor(0);
						}
						else // if no cup is in-play
						{
							disableMachineButton(1);
						}
						break;

					case 2: // milk frothing
						if (cs.currentFocus != null) // if cup in-play
						{
							if (cs.currentFocus.milkType == 0) // if no milk in cup
							{
								enableMachineButton(2);
							}
							else // if milk in cup
							{
								disableMachineButton(2);
							}
							enableAnchor(1);
						}
						else // if no cup is in-play
						{
							disableMachineButton(2);
						}
						break;
					case 4: // will be the toppings; temporary the serving state

						// Set current focus to cs.cupAnchor[desScreen - 1].gameObject
						cs.setCurrentFocus(cs.cupAnchor[desScreen - 1].gameObject);
						// Disable all station buttons
						disableAllStationButtons();
						break;
					default:
						break;
				}
			}
				
		}
	}

	private void UnloadScenes()
	{
		if (SceneManager.sceneCount > 1)
		{
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				Scene scene = SceneManager.GetSceneAt(i);
				if (scene.name != "Game")
				{
					SceneManager.UnloadSceneAsync(scene);
				}
			}
		}
	}

	private void enableAnchor(int index)
	{
		// Disable all cup anchors
		foreach (var anchor in cs.cupAnchor)
		{
			anchor.gameObject.SetActive(false);
		}

		// Enable the specific cup anchor at the given index
		cs.cupAnchor[index].gameObject.SetActive(true);

		// If there is a child to cs.cupAnchor[index-1], move it to cs.cupAnchor[index]
		if (index > 0 && cs.cupAnchor[index - 1].transform.childCount > 0)
		{
			Transform parentTransform = cs.cupAnchor[index].transform;
			Transform childTransform = cs.cupAnchor[index - 1].transform;
			foreach (Transform child in childTransform)
			{
				child.SetParent(parentTransform);
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

	public void EnableAllStationButtons()
	{
		for (int i = 0; i < stationButtons.Length; i++)
		{
			stationButtons[i].interactable = true;
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
			(isCupSizeSelected ? coffeeChoices : CupSizeChoices).SetActive(true);
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
