using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IContainVariable<T>
{
    T Value { get; }
}
