﻿// タイトルの管理スクリプト
// 2020/05/31
// 佐竹晴登

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    // タイトルロゴ
    [SerializeField] GameObject titleLogo = null;

    // ボタングループ
    [SerializeField] GameObject buttonGroup = null;

    // プレイシーンの名前
    [SerializeField] string playSceneName = "";
    // オプションシーンの名前
    [SerializeField] string optionSceneName = "";
    
    // ボタンが操作可能になるフラグ
    bool buttonActiveFlg;

    public AudioClip sound;
    AudioSource audioSource;

    // 初期化
    void Start()
    {
        // 最初はボタンを表示しない
        buttonGroup.SetActive(false);
        // AudioComponentの取得
        audioSource = GetComponent<AudioSource>();
    }

    // 更新
    void Update()
    {
        // この処理はボタングループが非アクティブ時にのみ処理される
        if(!buttonGroup.activeSelf)
        {
            // アニメーションが終わるかキーが押されたら操作可能にする。
            if (buttonActiveFlg || Input.anyKeyDown)
            {
                titleLogo.GetComponent<TItleLogoController>().LogoAnimeEnd();
                buttonGroup.SetActive(true);
            }
        }
        
    }

    // タイトルシーンを操作可能にする
    public void TitleActive()
    {
        buttonActiveFlg = true;
    }

    // スタートボタンが押された時
    public void PushStartButton()
    {
        // アニメーションが終了したら(未実装)

        // 音を鳴らす
        audioSource.PlayOneShot(sound);

        // プレイシーンに遷移
        SceneManager.LoadScene(playSceneName);
    }

    // オプションボタンが押された時
    public void PushOptionButton()
    {
        // アニメーションが終了したら(未実装)

        // 音を鳴らす
        audioSource.PlayOneShot(sound);

        // オプションシーンに遷移
        // SceneManager.LoadScene(optionSceneName);
    }

    // ゲーム終了ボタンが押された時
    public void PushExitButton()
    {
        // アニメーションが終了したら(未実装)

        // 音を鳴らす
        audioSource.PlayOneShot(sound);

        // ゲームを終了する
    }
}