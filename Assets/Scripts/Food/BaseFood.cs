using UnityEngine;

namespace Food
{
    public abstract class BaseFood : MonoBehaviour
    {
        
        public int TimeUntilEaten => _timeUntilEaten;
        
        [SerializeField] protected int _nutrition;
        [SerializeField] protected int _timeUntilEaten;


        public BaseCreature Eater;
        public bool InUse;
        
        public virtual bool CanBeEaten()
        {
            return true;
        }

        public void StartEating(BaseCreature eater)
        {
            if (Eater == null)
            {
                Eater = eater;
                InUse = true;
            }
        }
        
        public void StopEating(BaseCreature stopper)
        {
            if (stopper == Eater)
            {
                InUse = false;
                Eater = null;
            }
            else
            {
                Debug.Log($"stopper != Eater");
            }
        }
        
        public virtual int Eat()
        {
            InUse = false;
            Eater = null;
            return _nutrition;
        }
    }
}