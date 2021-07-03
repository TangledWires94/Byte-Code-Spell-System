﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfTarget : ISpellType
{
    Element.ElementType element = Element.ElementType.None;
    GameObject newForm = default;
    Vector3 newPlayerPosition = Vector3.zero;
    float range = 0, duration = 0;

    TargettingPointer targetpointer;

    public void Cast()
    {
        Debug.LogFormat("Spell Type = Self-Targetting, Element = {0}, New Position = {1}, Teleport Range = {2}, Duration = {3}", element, newPlayerPosition, range, duration);
    }

    public void SetDuration(float duration)
    {
        this.duration = duration;
    }

    public void SetElement(Element.ElementType element)
    {
        this.element = element;
    }

    public void SetGameObject(GameObject go)
    {
        newForm = go;
    }

    public void SetPosition(Vector3 position)
    {
        newPlayerPosition = position;
    }

    public void SetRange(float range)
    {
        //Not used
    }

    public void SetRotation(Quaternion rotation)
    {
        //Not used
    }

    public void StartTargetting(TargettingPointer targetpointer)
    {
        this.targetpointer = targetpointer;
    }

    public bool Target()
    {
        //Self targetting so no position as it will effect player stats
        SetPosition(Vector3.zero);
        targetpointer.DisableTargetting();
        return true;
    }

    public override string ToString()
    {
        return "Self";
    }
}
