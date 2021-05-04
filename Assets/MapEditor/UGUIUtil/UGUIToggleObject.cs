using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UGUIUtil
{
    [RequireComponent(typeof(Toggle))]
    public class UGUIToggleObject : MonoBehaviour
    {
        public GameObject target;
        Toggle currentToggle;
        // Start is called before the first frame update
        void Start()
        {
            currentToggle = GetComponent<Toggle>();
            ValidateGroup();
        }
        void Update()
        {
            ValidateGroup();
        }
        void ValidateGroup()
        {
            if (target.activeSelf == currentToggle.isOn)
                return;
            target.SetActive(currentToggle.isOn);
            LayoutRebuilder.ForceRebuildLayoutImmediate(target.GetComponent<RectTransform>());
        }
    }
}