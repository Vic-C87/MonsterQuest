using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MonsterQuest
{
    public class ButtonController : MonoBehaviour
    {
        Button myButton;
        ChoiceController myChoiceController;

        private void Awake()
        {
            myButton = GetComponent<Button>();
            myChoiceController = GetComponentInParent<ChoiceController>();
        }
        // Start is called before the first frame update
        void Start()
        {
            myButton.onClick.AddListener(myChoiceController.SelectItem);
        }        
    }
}
