using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

public class EasyToggleGroup : ToggleGroup
{
    Subject<string> onToggleFireSubject = new Subject<string>();
    public IObservable<string> OnToggleFireObservable() => onToggleFireSubject.AsObservable();
    int beforeToggleCnt = -1;
    List<IDisposable> toggleListenerList = new List<IDisposable>();
    void Update()
    {
        if (m_Toggles.Count == beforeToggleCnt)
            return;
        beforeToggleCnt = m_Toggles.Count;
        for (int i = 0; i < toggleListenerList.Count; i++)
        {
            toggleListenerList[i].Dispose();
        }
        toggleListenerList.Clear();

        foreach (var item in m_Toggles)
        {
            //            Debug.LogFormat("{0} added toggle={1}", gameObject.name, item.name);
            var listener = item.OnValueChangedAsObservable().Where(value => value == true).Select(_ => Unit.Default).Subscribe(_ =>
            {
                onToggleFireSubject.OnNext(item.name);
            });
            toggleListenerList.Add(listener);
        }
    }
}
