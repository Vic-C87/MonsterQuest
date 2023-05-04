using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Newtonsoft.Json.Linq;


namespace MonsterQuest
{
    public class SelectionManager : MonoBehaviour
    {
        
        FactoryManager myFactory;
        List<Options> myEquipmentOptions;

        [Header("Dropdowns")]
        [SerializeField]
        TMP_Dropdown myRaceSelect;
        [SerializeField]
        TMP_Text myRaceDescription;
        [SerializeField] 
        TMP_Dropdown myClassSelect;
        [SerializeField]
        TMP_Dropdown myMonsterSelect;

        [Header("Results")]
        [SerializeField]
        RaceType myRaceType;
        [SerializeField]
        ClassType myClassType;
        [SerializeField] 
        MonsterType myMonsterType;

        [Header("Dropdown Prefab")]
        [SerializeField]
        GameObject myDropdownPrefab;
        [SerializeField]
        GameObject myDropdownParent;

        [Header("TMP Text Prefab")]
        [SerializeField]
        GameObject myOptionTextPrefab;
        [SerializeField]
        GameObject myOptionTextParent;

        GameObject[] myOptionsGO;

        OptionController[] myOptionController;

        private void Awake()
        {
            myEquipmentOptions = new List<Options>();

        }

        void Start()
        {
            myFactory = new FactoryManager();
            FillDropDown(myRaceSelect, myFactory.RaceFactory.NamesList);
            FillDropDown(myClassSelect, myFactory.ClassFactory.NamesList);
            FillDropDown(myMonsterSelect, myFactory.MonsterFactory.NamesList);
        }

        void FillDropDown(TMP_Dropdown aDropDownToFill, List<string> aListOfOptions)
        {
            aDropDownToFill.ClearOptions();
            aDropDownToFill.AddOptions(aListOfOptions);
        }

        public void OnSelectRace()
        {
            myRaceType = myFactory.RaceFactory.GetRace(myRaceSelect.captionText.text);
            myRaceDescription.text = myRaceType.myDescription;
        }

        public void OnSelectClass()
        {
            myClassType = myFactory.ClassFactory.GetClass(myClassSelect.captionText.text);
            myEquipmentOptions = myFactory.ClassFactory.GetStartingOptionsList();
            SetEquipmentOptionsMenu();
        }

        public void SetEquipmentOptionsMenu()
        {
            ClearOptions();
            myOptionsGO = new GameObject[myEquipmentOptions.Count];
            myOptionController = new OptionController[myEquipmentOptions.Count];
            for (int i = 0; i < myEquipmentOptions.Count; i++)
            {
                GameObject optionPrefab = Instantiate(myOptionTextPrefab, myOptionTextParent.transform);
                optionPrefab.GetComponentInChildren<TMP_Text>().text = myEquipmentOptions[i].Description;
                myOptionController[i] = optionPrefab.GetComponent<OptionController>();
                myOptionsGO[i] = optionPrefab;
                myOptionController[i].SetOption(myEquipmentOptions[i]);
            }            
        }

        void ClearOptions()
        {
            if (myOptionsGO != null) 
            { 
                for(int i = 0; i < myOptionsGO.Length; i++)
                {
                    Destroy(myOptionsGO[i]);
                }
            }
            
        }

        public void OnSelectMonster()
        {
            JObject monsterApiData;
            myMonsterType = myFactory.MonsterFactory.GetMonsterType(myMonsterSelect.captionText.text, out monsterApiData);
            Debug.Log("Weapons:");
            myFactory.WeaponFactory.GetMonsterWeapons(myMonsterType, monsterApiData);
            Debug.Log("Armor:");
            myFactory.ArmorFactory.GetMonsterArmor(myMonsterType, monsterApiData);
        }
    }
}
