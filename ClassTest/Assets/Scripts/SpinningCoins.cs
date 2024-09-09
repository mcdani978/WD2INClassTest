using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningCoins : MonoBehaviour
{
    // This rotates the coins
    void Update()
    {
        transform.Rotate(0,5,0);
    }
}
