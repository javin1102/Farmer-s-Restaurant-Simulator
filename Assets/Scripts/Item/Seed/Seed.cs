using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Seed : Item
{
    public SeedData seedData{ get => m_seedData; }
    [SerializeField] protected SeedData m_seedData;
    
}
