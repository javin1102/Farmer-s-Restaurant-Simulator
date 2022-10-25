using UnityEngine;
[CreateAssetMenu( fileName = "Item", menuName = "Custom Data/New Ingredient" )]
public class IngredientData : ItemData
{
    [TextArea]
    public string deskripsiKandungan;

    [TextArea]
    public string deskripsiNutrisi;

    [TextArea(10,70)]
    public string deskripsiManfaat;
}
