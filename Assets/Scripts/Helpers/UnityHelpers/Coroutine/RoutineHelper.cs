using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace Helpers.Timing
{
    public class RoutineHelper : MonoBehaviour
    {

        public static MonoBehaviour mono {
            get { return ActionHelper.mono; }
        }

        private static Hashtable enqueuedRoutines = new Hashtable();

        public static void Enqueue(UnityEngine.GameObject obj, IEnumerator routine)
        {
            if (!enqueuedRoutines.ContainsKey(obj))
                enqueuedRoutines.Add(obj, new Queue<IEnumerator>());

            Queue<IEnumerator> objRoutines = enqueuedRoutines[obj] as Queue<IEnumerator>;
            objRoutines.Enqueue(routine);

            if (objRoutines.Count == 1)
                ExecuteNext(obj);
        }

        private static void ExecuteNext(UnityEngine.GameObject obj)
        {
            if (enqueuedRoutines.ContainsKey(obj))
            {
                Queue<IEnumerator> objRoutines = enqueuedRoutines[obj] as Queue<IEnumerator>;

                if (objRoutines.Count > 0)
                {
                    IEnumerator nextRoutine = objRoutines.Peek();
                    UnityAction continuityAction = () => { Dequeue(obj); ExecuteNext(obj); };
                    mono.StartCoroutine(QueueRoutine(nextRoutine, continuityAction));
                }
            }
        }

        public static IEnumerator Dequeue(UnityEngine.GameObject obj)
        {
            if (!enqueuedRoutines.ContainsKey(obj))
                return null;
            else
            {
                Queue<IEnumerator> objRoutines = enqueuedRoutines[obj] as Queue<IEnumerator>;
                IEnumerator routine = objRoutines.Dequeue();

                if (objRoutines.Count == 0)
                    enqueuedRoutines.Remove(obj);

                return routine;
            }
        }

        private static IEnumerator QueueRoutine(IEnumerator routine, UnityAction continuityAction)
        {
            yield return mono.StartCoroutine(routine);
            continuityAction.Invoke();
        }
    }
}