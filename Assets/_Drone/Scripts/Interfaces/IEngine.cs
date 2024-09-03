using UnityEngine;

namespace RDC
{
    public interface IEngine
    {
        void InitEngine();
        void UpdateEngine(Rigidbody rb, InputManager inputs);
    }

}