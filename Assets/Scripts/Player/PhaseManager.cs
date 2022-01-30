using PathFinding.AStar;
using UnityEngine;

namespace CycleManager
{
    public class PhaseManager : MonoBehaviour
    {

        [SerializeField] private BaseCreatureData _fluff;
        [SerializeField] private BaseCreatureData _fluffAggressive;
        
        private void Start()
        {
            PathRequestManager.ClearRequests();
            CycleManager.Instance.OnCycleShift += OnPhaseChanged;
        }

        private void OnPhaseChanged(Phases phase)
        {
            var fluffs = FindAllFluffs();

            foreach (var fluff in fluffs)
            {
                fluff.ChangeCreatureData(phase == Phases.Day ? _fluff : _fluffAggressive);
            }
        }


        private FluffCreature[] FindAllFluffs()
        {
            return FindObjectsOfType<FluffCreature>();
        }
    }
}