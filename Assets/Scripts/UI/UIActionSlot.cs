using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIActionSlot : UIItemSlot
{
    private Image m_Image;
    private void Awake()
    {
        m_SlotsController = transform.root.GetComponent<ActionSlotsController>();
        m_Image = GetComponent<Image>();    
    }

    public void ChangeSprite(Sprite sprite)
    {
        m_Image.sprite = sprite;    
    }
}



