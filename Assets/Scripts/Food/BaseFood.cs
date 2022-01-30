using UnityEngine;

namespace Food
{
    public abstract class BaseFood : MonoBehaviour
    {
        
        public int TimeUntilEaten => _timeUntilEaten;
        
        [SerializeField] protected int _nutrition;
        [SerializeField] protected int _timeUntilEaten;


        public virtual bool CanBeEaten()
        {
            return true;
        }
        
        public virtual int Eat()
        {
            return _nutrition;
        }
    }
}