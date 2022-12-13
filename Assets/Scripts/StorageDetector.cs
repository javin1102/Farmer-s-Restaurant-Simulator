using UnityEngine;

public class StorageDetector : MonoBehaviour
{
    private FoodsController m_FoodsController;
    void Start()
    {
        m_FoodsController = FoodsController.Instance;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Ingredients ingredient))
        {
            IngredientData ingredientData = ingredient.Data as IngredientData;
            m_FoodsController.StoreIngredient(ingredientData, ingredient.DropQuantity);
            Destroy(other.gameObject);
        }
    }
}