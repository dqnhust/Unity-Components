using System;
using UnityEngine;

namespace CircleController
{
    public interface ICircleController
    {
        Vector2 Direction { get; }
        event Action<Vector2> OnChanged;
        event Action OnReleased;
    }
}