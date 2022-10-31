using UnityEngine;
using Unity.Mathematics;
using Random = UnityEngine.Random;
using System.Collections.Generic;

public class UIFurnitureQuiz : UIStoreQuiz
{
    private UIFurnitureStoreController m_FurnitureStoreController;
    private float[] m_Chances;
    private int m_FurnitureCount;
    private void Start()
    {
        m_FurnitureStoreController = UIFurnitureStoreController.Instance as UIFurnitureStoreController;
        m_FurnitureCount = m_FurnitureStoreController.FurnituresData.Count;
        m_Chances = new float[5]
        {
             .5f,
             .3f,
             .15f,
             .05f,
             .01f
        };
    }

    protected override void SetReward()
    {
        //Randomize to determine which furniture type : CHAIR/TABLE/STOVE
        float rand = Random.value;
        if ( rand < .33 )
        {
            SpawnRandom( m_FurnitureStoreController.TablesData );
        }
        else if ( rand < .66 )
        {
            SpawnRandom( m_FurnitureStoreController.ChairsData );
        }
        else
        {
            //Stove
            SpawnRandom( m_FurnitureStoreController.StovesData );
        }
        //Randomize 

    }

    protected void SpawnRandom( List<FurnitureData> furnituresData )
    {
        float chance = Random.value;
        for ( int i = 0; i < furnituresData.Count; i++ )
        {
            if ( chance > m_Chances[i] )
            {
                m_FurnitureStoreController.SpawnItem( furnituresData[i] );
                return;
            }
        }
    }
}
