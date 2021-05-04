using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace MapUtil
{
    public class ToolView : MonoBehaviour
    {
        [SerializeField] CustomToggleGroup toolToggleGroup;
        [SerializeField] CustomToggleGroup cliffToggleGroup;
        [SerializeField] CustomToggleGroup brushToggleGroup;
        [SerializeField] GameObject cliffOperationRoot;
        [SerializeField] GameObject brushOperationRoot;
        void Awake()
        {
            toolToggleGroup.OnToggleFireObservable().Subscribe(toggleName =>
            {
                OnToolSelected(toggleName);
                // Debug.LogFormat("ToggleName={0}", toggleName);
            }).AddTo(gameObject);
            cliffToggleGroup.OnToggleFireObservable().Subscribe(toggleName =>
            {
                OnToolSelected(toggleName);
                // Debug.LogFormat("ToggleName={0}", toggleName);
            }).AddTo(gameObject);
            brushToggleGroup.OnToggleFireObservable().Subscribe(toggleName =>
            {
                OnToolSelected(toggleName);
                // Debug.LogFormat("ToggleName={0}", toggleName);
            }).AddTo(gameObject);
        }
        void OnToolSelected(string name)
        {
            switch (name)
            {
                case "Arrow":
                    {
                        if (cliffOperationRoot.activeSelf == true)
                            cliffOperationRoot.SetActive(false);
                        if (brushOperationRoot.activeSelf == true)
                            brushOperationRoot.SetActive(false);
                    }
                    break;
                case "Brush":
                    {
                        if (cliffOperationRoot.activeSelf == true)
                            cliffOperationRoot.SetActive(false);
                        if (brushOperationRoot.activeSelf == false)
                            brushOperationRoot.SetActive(true);
                    }
                    break;
                case "Cliff":
                    {
                        if (cliffOperationRoot.activeSelf == false)
                            cliffOperationRoot.SetActive(true);
                        if (brushOperationRoot.activeSelf == false)
                            brushOperationRoot.SetActive(true); ;
                    }
                    break;
            }
            RecursiveRefresh(gameObject.transform);
        }
        void RecursiveRefresh(Transform trans, int depth = 0)
        {
            foreach (Transform child in trans)
            {
                RecursiveRefresh(child, depth + 1);
            }
            var fitter = trans.GetComponent<ContentSizeFitter>();
            if (fitter != null)
            {
                //                Debug.LogFormat("Refreshing={0} depth={1}", GetName(trans), depth);
                LayoutRebuilder.ForceRebuildLayoutImmediate(trans.GetComponent<RectTransform>());
            }
        }
        string GetName(Transform trans)
        {
            string nameInc = trans.name;
            while (trans.parent != null)
            {
                nameInc = trans.parent.name + "/" + nameInc;
                trans = trans.parent;
            }
            return nameInc;
        }
    }
}