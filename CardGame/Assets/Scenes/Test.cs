using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    

    private int Hp = 100;
    
    void Start()
    {
        TestAwait().Forget();
    }
    
    private void FixedUpdate()
    {
        Hp = Hp - 10;
        Debug.LogError($"现在HP：{Hp}");
    }

    // private IEnumerable TestAwait()
    // {
    //     while (true)
    //     {
    //         Debug.LogError("循环中");
    //         if (Hp == 0)
    //         {
    //             Debug.LogError("跳出循环");
    //             return;
    //         }
    //     }
    // }

    private async UniTask TestAwait()
    {
        while (true)
        {
            Debug.LogError("循环中");
            if (Hp == 0)
            {
                Debug.LogError("跳出循环");
                return;
            }

            await UniTask.Delay(1000);
        }
    }
}
