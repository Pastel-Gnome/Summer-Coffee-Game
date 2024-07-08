using UnityEngine;

public class IngredientValues : MonoBehaviour
{
    public enum CupSize
    {
        Small,
        Medium,
        Large,
    }
    public enum CoffeeRoast
    {
        lightRoast, //tastes light, fruity, and floral. has a higher amount of caffeine.
        mediumRoast, //sweeter than a light roast with a more balanced level of flavor and acidity
        mediumDarkRoast, //darker and richer than medium roasts with a heavier body
        darkRoast //a dark brown to black appearance with a bitter, smoky taste
    }

    public enum Milk
    {
		noMilk,
		wholeMilk,
        skimMilk,
        coconutMilk, //mildly nutty flavor with a unique aftertaste
        almondMilk, //light in body and flavor, but has a distinct nutty taste
        riceMilk, //most similar to skim milk with a thin body
        soyMilk, //thicker and sweeter than almond milk
        cashewMilk //neutral taste that lets the coffee speak for itself
    }

    public enum Toppings
    {
        cream,
        whippedCream,
        sprinkles,
        cocoaPowder,
        cinnamonPowder,
        chocolateSyrup,
        caramelSyrup,
        brownSugar,
        cherry,
    }

    public enum Flavor
    {
        FrenchVanilla,
        Caramel,
        Hazelnut,
        PumpkinSpice,
        ButterPecan,
        Chocolate,
        WhiteChocolate,
        Mint
    }
}
