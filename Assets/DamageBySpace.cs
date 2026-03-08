using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Sphery.ExerCube
{

    public class DamageBySpace : MonoBehaviour
    {
        // UnityEvent, das im Inspector sichtbar ist
        public UnityEvent onSpacePressed;

        void Update()
        {
            // Prüfen, ob die Leertaste gedrückt wurde
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Event auslösen, falls etwas zugewiesen ist
                onSpacePressed?.Invoke();
            }
        }
    }
}
