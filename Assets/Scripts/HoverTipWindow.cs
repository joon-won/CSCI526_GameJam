using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CSCI526GameJam
{
    public class HoverTipWindow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public GameObject TipWindow;
        public void OnPointerEnter(PointerEventData eventData)
        {
            TipWindow.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TipWindow.SetActive(false);
        }
    }
}
