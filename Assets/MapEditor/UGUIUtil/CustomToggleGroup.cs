using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class CustomToggleGroup : MonoBehaviour
{
    [SerializeField] Toggle[] toggles;
    Subject<string> onToggleFireSubject = new Subject<string>();
    public IObservable<string> OnToggleFireObservable() => onToggleFireSubject.AsObservable();

    void Awake()
    {
        // OnToggleFireObservable().Subscribe(selected =>
        // {
        //     Debug.LogFormat("selected={0}", selected);
        // });
        foreach (var item in toggles)
        {
            string itemName = item.name;
            // Debug.LogFormat("{0} added toggle={1}", gameObject.name, item.name);
            item.OnValueChangedAsObservable().Subscribe(value =>
            {
                //                Debug.LogFormat("{0} toggle={1} value={2}", gameObject.name, item.name, value);
                //onToggleFireSubject.OnNext(item.name);
                if (value == true)
                {
                    for (int i = 0; i < toggles.Length; i++)
                    {
                        if (itemName == toggles[i].name)
                            continue;
                        toggles[i].isOn = false;
                    }
                    onToggleFireSubject.OnNext(itemName);

                }
                else
                {
                    bool isSomethingOn = false;
                    for (int i = 0; i < toggles.Length; i++)
                    {
                        if (itemName == toggles[i].name)
                            continue;
                        if (toggles[i].isOn == true)
                            isSomethingOn = true;
                    }
                    if (isSomethingOn == false)
                    {
                        item.SetIsOnWithoutNotify(true);
                        //onToggleFireSubject.OnNext(null);
                    }
                }
            });
        }
    }
    void OnEnable()
    {
        NotifyObserverChanges();
    }
    void OnDisable()
    {
        onToggleFireSubject.OnNext(null);
    }
    void NotifyObserverChanges()
    {
        bool didFireSomething = false;
        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i].isOn == false)
                continue;
            onToggleFireSubject.OnNext(toggles[i].name);
            didFireSomething = true;
        }
        if (didFireSomething == true)
            return;
        onToggleFireSubject.OnNext(null);
    }
}
