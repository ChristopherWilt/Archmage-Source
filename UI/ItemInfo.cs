using TMPro;
using UnityEngine;

namespace Archmage.UI {
    public class ItemInfo : MonoBehaviour {
        public TMP_Text itemName;
        public TMP_Text itemDescription;

        public void SetInfo(string name, string description) {
            itemName.text = name;
            itemDescription.text = description;
        }

    }
}