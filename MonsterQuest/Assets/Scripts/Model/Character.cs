namespace MonsterQuest
{
    public class Character
    {
        public string myDisplayName { get; private set; }

        public Character(string aDisplayName)
        {
            myDisplayName = aDisplayName;
        }
    }
}
