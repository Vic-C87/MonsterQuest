using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace MonsterQuest
{
    public class ChoiceController : MonoBehaviour
    {
        OptionController myParent;
        Option myOption;

        [SerializeField]
        TMP_Text myText;
        [SerializeField]
        TMP_Dropdown myDropdown;

        [SerializeField]
        GameObject mySinglePrefab;
        [SerializeField]
        GameObject myChoicePrefab;

        [SerializeField]
        GameObject myButtonPrefab;

        GameObject myChoiceParent;

        public void InitializeChoice(OptionController aParent, Option anOption, bool anIsFromMultiple = false, GameObject? aChoiceParent = null)
        {
            myParent = aParent;
            myOption = anOption;
            myChoiceParent = aChoiceParent;
            switch (myOption.Type)
            {
                case EEquipmentOptionType.Multiple:
                    SetMultiple();
                    break;
                case EEquipmentOptionType.Single:
                    myText.text = myOption.EquipmentString + " x" + myOption.Quantity;
                    break;
                case EEquipmentOptionType.Choice:
                    myDropdown.ClearOptions();
                    myDropdown.AddOptions(myOption.GetEquipmentNames().Keys.ToList());
                    break;
                default:
                    break;
            }

            if(!anIsFromMultiple)
            {
                Instantiate<GameObject>(myButtonPrefab, this.transform);
            }
        }

        void SetMultiple()
        {
            int length = myOption.Options.Count;

            for (int i = 0; i < length; i++)
            {
                switch (myOption.Options[i].Type)
                {
                    case EEquipmentOptionType.Single:
                        GameObject single = Instantiate<GameObject>(mySinglePrefab, this.transform);
                        single.GetComponent<ChoiceController>().InitializeChoice(myParent, myOption.Options[i], true);
                        break;
                    case EEquipmentOptionType.Choice:
                        GameObject choice = Instantiate<GameObject>(myChoicePrefab, this.transform);
                        choice.GetComponent<ChoiceController>().InitializeChoice(myParent, myOption.Options[i], true);                        
                        break;
                    default:
                        break;
                }
            }
        }

        public void SelectItem()
        {
            myParent.ResetSelection();
            switch (myOption.Type)
            {
                case EEquipmentOptionType.Multiple:
                    int length = myOption.Options.Count;
                    for (int i = 0; i < length; i++)
                    {
                        switch (myOption.Options[i].Type)
                        {
                            case EEquipmentOptionType.Single:
                                myParent.OnSelectOption(myOption.Options[i].GetEquipmentItems()[0]);
                                break;
                            case EEquipmentOptionType.Choice:
                                myParent.OnSelectOption(GetDropdownSelection());
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case EEquipmentOptionType.Single:
                    myParent.OnSelectOption(myOption.GetEquipmentItems()[0]);
                    break;
                case EEquipmentOptionType.Choice:
                    if (myOption.Quantity > 1)
                    {
                        TMP_Dropdown[] dropdowns = myChoiceParent.GetComponentsInChildren<TMP_Dropdown>();
                        foreach (TMP_Dropdown dropdown in dropdowns)
                        {
                            myParent.OnSelectOption(new CountedReference(dropdown.captionText.text, 1));
                        }
                    }
                    else
                    {
                        myParent.OnSelectOption(GetDropdownSelection());
                    }
                    break;
                default:
                    break;
            }
        }

        public CountedReference GetDropdownSelection()
        {
            TMP_Dropdown dropdown = GetComponentInChildren<TMP_Dropdown>();
            return new CountedReference(dropdown.captionText.text, 1);
        }
    }
}
