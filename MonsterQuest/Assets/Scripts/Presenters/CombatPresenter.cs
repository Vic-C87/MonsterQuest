using System.Collections;
using System.Linq;
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

        public IEnumerator InitializeParty(GameState gameState)
        {
            Character[] characters = gameState.myParty.myCharacters.ToArray();

            // Create the character views.
            for (int i = 0; i < characters.Length; i++)
            {
                Creature character = characters[i];

                GameObject characterGameObject = Instantiate(creaturePrefab, _creaturesTransform);
                characterGameObject.name = character.myDisplayName;
                characterGameObject.transform.position = new Vector3(((characters.Length - 1) * -0.5f + i) * 5, character.mySpaceInFeet / 2, 0);

                CreaturePresenter creaturePresenter = characterGameObject.GetComponent<CreaturePresenter>();
                creaturePresenter.Initialize(character);

                yield return creaturePresenter.FaceDirection(CardinalDirection.South, true);
            }
        }

        public IEnumerator InitializeMonster(GameState gameState)
        {
            Combat combat = gameState.myCombat;

            // Create the monster view.
            GameObject monsterGameObject = Instantiate(creaturePrefab, _creaturesTransform);
            monsterGameObject.name = combat.myMonster.myDisplayName;
            monsterGameObject.transform.position = new Vector3(0, -combat.myMonster.mySpaceInFeet / 2, 0);

            CreaturePresenter creaturePresenter = monsterGameObject.GetComponent<CreaturePresenter>();
            creaturePresenter.Initialize(combat.myMonster);

            yield return creaturePresenter.FaceDirection(CardinalDirection.North, true);
        }
    }
}
