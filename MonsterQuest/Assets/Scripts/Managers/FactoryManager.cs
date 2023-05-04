using Newtonsoft.Json.Linq;
using System.Collections;
namespace MonsterQuest
{
    public class FactoryManager
    {
        public RaceFactory RaceFactory => myRaceFactory;
        public ClassFactory ClassFactory => myClassFactory;
        public WeaponFactory WeaponFactory => myWeaponFactory;
        public ArmorFactory ArmorFactory => myArmorFactory;
        public MonsterFactory MonsterFactory => myMonsterFactory;

        RaceFactory myRaceFactory;
        ClassFactory myClassFactory;
        WeaponFactory myWeaponFactory;
        ArmorFactory myArmorFactory;
        MonsterFactory myMonsterFactory;

        public FactoryManager()
        {
            myRaceFactory = new RaceFactory();
            myClassFactory = new ClassFactory();
            myWeaponFactory = new WeaponFactory();
            myArmorFactory = new ArmorFactory();
            myMonsterFactory = new MonsterFactory();
        }
    }
}
