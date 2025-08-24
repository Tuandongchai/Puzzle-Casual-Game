using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainManager : MonoBehaviour
{
    private IMenu[] m_menuList;

    private void Awake()
    {
        m_menuList = GetComponentsInChildren<IMenu>(true);
    }

    void Start()
    {
        for (int i = 0; i < m_menuList.Length; i++)
        {
            m_menuList[i].Setup(this);
        }
    }
}
