using System.Collections;
using System.Collections.Generic;
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
            yield return InitializeCreatures(gameState.myParty.myCharacters, 0, CardinalDirection.South);
        }

        public IEnumerator InitializeMonster(GameState gameState)
        {
            yield return InitializeCreatures(new Creature[]{gameState.myCombat.myMonster}, 0, CardinalDirection.North);
        }

        private IEnumerator InitializeCreatures(IEnumerable<Creature> creatures, float y, CardinalDirection direction)
        {
            Creature[] creaturesArray = creatures.ToArray();

            float totalWidth = creaturesArray.Sum(creature => creature.mySpaceInFeet);
            float currentX = -totalWidth / 2;
            Vector3 facingDirection = CardinalDirectionHelper.cardinalDirectionVectors[direction];

            foreach (Creature creature in creaturesArray)
            {
                currentX += creature.mySpaceInFeet;

                if (creature.myLifeStatus == ELifeStatus.Dead) continue;

                float spaceRadius = creature.mySpaceInFeet / 2;

                GameObject characterGameObject = Instantiate(creaturePrefab, _creaturesTransform);
                characterGameObject.name = creature.myDisplayName;

                Vector3 position = new Vector3(currentX - spaceRadius, y, 0) - facingDirection * spaceRadius;
                position.z = position.y * 0.01f;
                characterGameObject.transform.position = position;

                CreaturePresenter creaturePresenter = characterGameObject.GetComponent<CreaturePresenter>();
                creaturePresenter.Initialize(creature);

                yield return creaturePresenter.FaceDirection(direction, true);
            }
        }
    }
}
