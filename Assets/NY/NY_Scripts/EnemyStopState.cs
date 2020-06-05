﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStopState : EnemyState
{

    public override string GetStateName() { return "EnemyStopState"; }

    private bool _isPlayerDiscovery;

    private Vector3 _initPos;

    private int _routeNum = 0;                 // 現在巡回している番号
    [SerializeField]
    private bool _isSemiAlertState = true;

    private const string SONAR_TAG_NAME = "PlayerSonar";

    void Start()
    {
        // 初期位置を設定
        _initPos = this.transform.position;
    }
    // ステートが遷移してきたとき
    public override void EnterEvent()
    {
        _prop.Agent.SetDestination(_initPos);

        Debug.Log("EnemyStopState : に移行");
    }

    public override void Execute()
    {
        Vector3 playerPos = _prop.PlayerTrs.position;
        float distance = Vector3.Distance(playerPos, StateController.transform.position);
        // プレイヤーとの距離が一定範囲内なら索敵開始
        if (distance < _prop.FovLength)
            _isPlayerDiscovery = IsSearch();
        else
            _isPlayerDiscovery = false;
        

        // プレイヤーを見つけたら警戒ステートに変更
        if (_isPlayerDiscovery)
            StateController.SetState(_nextStateName);
    }

    // ステートから出ていくとき
    public override void ExitEvent() { }


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



    void OnTriggerEnter(Collider other)
    {
        // 巡回状態で波紋が自身に触れたら準警戒状態に移行
        if (other.gameObject.tag == SONAR_TAG_NAME && StateController.GetStateName() == GetStateName())
        {
            if (_isSemiAlertState)
            {
                _prop.TargetTrs = other.transform;
                StateController.SetState("EnemySemiAlertState");
            }
            else
                StateController.SetState(_nextStateName);

        }
    }
}