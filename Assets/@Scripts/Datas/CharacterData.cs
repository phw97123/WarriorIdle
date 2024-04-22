[System.Serializable]
public class CharacterData
{
    private float speed = 15.0f;
    private int hp = 100;
    private int maxHp = 100; 

    public float Speed { get { return speed; } protected set { speed = value; } }
    public int HP { get { return hp; } protected set { hp = value; } }
    public int MaxHp { get {  return maxHp; } protected set {  maxHp = value; } }
}
