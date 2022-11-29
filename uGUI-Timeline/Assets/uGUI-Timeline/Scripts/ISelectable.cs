using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UGUITimeline
{
    public interface ISelectable : IPointerClickHandler , IPointerEnterHandler , IPointerExitHandler
    {
        public void Select();
        public void UnSelect();
    }
}
