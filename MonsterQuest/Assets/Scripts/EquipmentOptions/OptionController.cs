using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace MonsterQuest
{
    public class OptionController : MonoBehaviour
    {
        Options myOptions;

        [SerializeField]
        TMP_Text myChosenItem;


        [SerializeField]
        GameObject mySinglePrefab;
        [SerializeField]
        GameObject myMultiplePrefab;
        [SerializeField]
        GameObject myChoicePrefab;
        [SerializeField]
        GameObject myOptionParent;
        [SerializeField]
        Transform myOptionParentTransform;

        List<CountedReference> mySelections;

        private void Awake()
        {
            mySelections = new List<CountedReference>();
        }

        public void SetOption(Options someOptions)
        {
            myOptions = someOptions;
            SortOptions();
        }

        void SetSingle(Option anOption)
        {
            GameObject single = Instantiate<GameObject>(mySinglePrefab, myOptionParent.transform);
            single.GetComponent<ChoiceController>().InitializeChoice(this, anOption);
        }

        void SetMultiple(Option anOption)
        {
            GameObject multiple = Instantiate<GameObject>(myMultiplePrefab, myOptionParent.transform);
            multiple.GetComponent<ChoiceController>().InitializeChoice(this, anOption);
        }

        void SetChoice(Option anOption) 
        {
            if (anOption.Quantity > 1) 
            { 
                GameObject multiple = Instantiate<GameObject>(myMultiplePrefab, myOptionParent.transform);
                for (int i = 0; i < anOption.Quantity; i++) 
                {
                    GameObject choice = Instantiate<GameObject>(myChoicePrefab, multiple.transform);
                    choice.GetComponent<ChoiceController>().InitializeChoice(this, anOption, !(i == (anOption.Quantity -1)), multiple);
                }
            }
            else
            {
                GameObject choice = Instantiate<GameObject>(myChoicePrefab, myOptionParent.transform);
                choice.GetComponent<ChoiceController>().InitializeChoice(this, anOption);
            }
        }

        void SortOptions()
        {
            myOptionParent = Instantiate<GameObject>(myOptionParent, myOptionParentTransform);
            int lenght = myOptions.MyOptions.Count;
            for (int i = 0; i < lenght; i++) 
            {
                switch (myOptions.MyOptions[i].Type)
                {
                    case EEquipmentOptionType.Single:
                        SetSingle(myOptions.MyOptions[i]);
                        break;
                    case EEquipmentOptionType.Multiple:
                        SetMultiple(myOptions.MyOptions[i]);
                        break;
                    case EEquipmentOptionType.Choice:
                        SetChoice(myOptions.MyOptions[i]);
                        break;
                    default:
                        break;
                }
            }
        }

        public void OnSelectOption(CountedReference aSelection)
        {
            mySelections.Add(aSelection);
            myChosenItem.text += aSelection.EquipmentString + " x" + aSelection.Quantity + "\n";
        }

        public void ResetSelection()
        {
            mySelections = new List<CountedReference>();
            myChosenItem.text = "";
        }
    }
}
