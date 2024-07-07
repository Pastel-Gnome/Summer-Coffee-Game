using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class OrderDisplay : MonoBehaviour
{
    public static OrderDisplay orderDisplay;
    public CanvasGroup[] ingredientsEntries; // will be replaced with the ingredientsEntries prefab 
    public CanvasGroup[]dottedLinePrefabs;

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
        StartCoroutine(DisplayOrders());
    }

    IEnumerator DisplayOrders()
    {
        for (int i = 0; i < ingredientsEntries.Length; i++)
        {
            //Show the different recipe entries           
            ingredientsEntries[i].alpha = 1;
            //In the future will change the different recipe elements here

            if (i < ingredientsEntries.Length - 1)
            {
                dottedLinePrefabs[i].alpha = 1;
                yield return new WaitForSeconds(displayDuration);
            }
        }
    }


}
