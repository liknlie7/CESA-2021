﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySemiAlertState : EnemyState
{    
    // ステートの名前を取得
    public override string GetStateName() { return "EnemySemiAlertState"; }
    [SerializeField]
    private GameObject _questionObj;

    private float _attentionTime = 0.0f;
    private float _crawlAttentionTime = 8.0f;

    // ステートが遷移してきたとき
    public override void EnterEvent()
    {
        _questionObj.SetActive(true);
        _prop.Agent.SetDestination(_prop.TargetTrs.position);
        Debug.Log("SemiAlertState : に移行");

        _attentionTime = _crawlAttentionTime;
    }


    public override void Execute()
    {  
        // プレイヤーを見つけていたら警戒状態に移行
        if (IsSearch())
            StateController.SetState("EnemyAlertState");

        // 目的地付近に着いたら、周りを見渡す
        if (_prop.Agent.remainingDistance <= 0.1f)
        {
            Vector3 rotate = new Vector3(0.0f, Mathf.Cos(Time.time) * 0.5f, 0.0f);
            this.transform.Rotate(rotate);
            _attentionTime -= Time.deltaTime;

            // 警戒時間が経過すると巡回ステートへ
            if (_attentionTime <= 0.0f)
                StateController.SetState("EnemyCrawlState");
        }
    }

    // ステートから出ていくとき
    public override void ExitEvent()
    {
        _questionObj.SetActive(false);
    }


    bool IsSearch()
    {
        Vector3 playerPos = _prop.PlayerTrs.position;
        Vector3 dir = StateController.transform.forward;

        // プレイヤーが視野範囲内に入ったら
        if (Vector3.Angle((playerPos - StateController.transform.position).normalized, dir) <= _prop.FovAngle / 2)
            return IsPlayerSee();

        return false;
    }

    // プレイヤーを障害物なしに視認できたか
    bool IsPlayerSee()
    {
        // プレイヤーを障害物なしに視認できたか
        Vector3 playerPos = _prop.PlayerTrs.position;
        Vector3 searchDire = playerPos - StateController.transform.position;
        Ray ray = new Ray(StateController.transform.position, searchDire.normalized);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, _prop.FovLength))
        {
            if (hit.collider.gameObject.tag == "Player")
                return true;
        }
        return false;
    }
}