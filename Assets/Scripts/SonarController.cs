﻿// 波紋を好きな時に出すオブジェクト
// 2020/05/17
// 佐竹晴登
// 山口寛雅

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonarController : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] const float activetime = 3.0f;
    [SerializeField] GameObject rippleSphere;
    float count = 0;
    bool flg;

    void Start()
    {
        flg = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 波紋を出すインターバル
        if (flg)
        {
            if (count >= activetime)
            {
                flg = false;
            }
                
            count += Time.deltaTime;
        }
        else
        {
            // スペースキーで自分からソナーが出る。
            if(Input.GetKeyDown(KeyCode.Space))
            {
                flg = true;
                count = 0.0f;
                // 2020/06/02追加分-2020/06/03編集---------------------------
                CameraManager.Get().sonarFx.Pulse(this.transform.position);
                // ----------------------------------------------------------

            }
        }
        //CameraManager.Get().sonarFxSwitcher.SetFlag(flg);
    }
}