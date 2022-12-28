using UnityEngine;

namespace MonsterQuest
{
    public class CombatPresenter : MonoBehaviour
    {
        [SerializeField] private GameObject creaturePrefab;

        private Transform _creaturesTransform;

        private void Awake()
        {
            _creaturesTransform = transform.Find("Creatures");
        }

        public void InitializeParty(GameState gameState)
        {
            // Create the character views.
            for (int i = 0; i < gameState.myParty.Count(); i++)
            {
                Creature character = gameState.myParty.myCharacters[i];

                GameObject characterGameObject = Instantiate(creaturePrefab, _creaturesTransform);
                characterGameObject.name = character.myDisplayName;
                characterGameObject.transform.position = new Vector3(((gameState.myParty.Count() - 1) * -0.5f + i) * 5, character.mySpaceInFeet / 2, 0);

                CreaturePresenter creaturePresenter = characterGameObject.GetComponent<CreaturePresenter>();
                creaturePresenter.Initialize(character);
                creaturePresenter.FaceDirection(CardinalDirection.South);
            }
        }

        public void InitializeMonster(GameState gameState)
        {
            Combat combat = gameState.myCombat;

            // Create the monster view.
            GameObject monsterGameObject = Instantiate(creaturePrefab, _creaturesTransform);
            monsterGameObject.name = combat.myMonster.myDisplayName;            monsterGameObject.transform.position = new Vector3(0, -combat.myMonster.mySpaceInFeet / 2, 0);

            CreaturePresenter creaturePresenter = monsterGameObject.GetComponent<CreaturePresenter>();
            creaturePresenter.Initialize(combat.myMonster);
            creaturePresenter.FaceDirection(CardinalDirection.North);
        }
    }
}
