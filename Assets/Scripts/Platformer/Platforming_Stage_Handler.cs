using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using UnityEngine;

public class Platforming_Stage_Handler : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Enemy_Handler enemyHandler;
    [SerializeField]
    private Music_Manager musicManager;

    private Transform playerTrans;
    private Vector3 respawnPlayerPos;
    private Rigidbody2D playerRB;
    private bool respawnSet;

    long invincibilityTimer;

    private CharacterController2D playerController;

    private void Awake() {
        playerTrans = player.transform;
        respawnSet = false;

        invincibilityTimer = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond + 3000;
    }

    private void Start() {
        playerController = player.GetComponent<CharacterController2D>();
        playerController.GetTriggerCollider().enabled = false;

        playerRB = player.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CheckRespawn();
        if (respawnSet) {
            CheckPlayerFall();
        }

        DoInvincibilityFrames();
    }

    private void CheckPlayerFall() {
        if (playerTrans.position.y < Utils.yLimit) {
            playerTrans.position = respawnPlayerPos;
            playerRB.velocity = new Vector3(0f, 0f, 0f);

            playerController.GetTriggerCollider().enabled = false;
            invincibilityTimer = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond + 3000;
        }
    }

    private void CheckRespawn() {
        if (playerController.GetGrounded()) {
            respawnSet = false;
        } else {
            if (!respawnSet) {
                if (playerRB.velocity.y > 0.01f) {
                    respawnPlayerPos = playerTrans.position;
                    respawnSet = true;
                } else {
                    respawnPlayerPos = playerTrans.position;

                    if (playerController.GetFacingRight()) {
                        respawnPlayerPos -= new Vector3(1f, 0f, 0f);
                    } else {
                        respawnPlayerPos += new Vector3(1f, 0f, 0f);
                    }

                    respawnSet = true;
                }
            }
        }
    }

    private void DoInvincibilityFrames() {
        if (invincibilityTimer < DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) {
            playerController.GetTriggerCollider().enabled = true;
            playerController.ResetSpriteTransparency();
        }
    }

    public void AddFirstBatchOfKana() {
        Utils.AddKanaData("あ");
        Utils.AddKanaData("い");
        Utils.AddKanaData("う");
        Utils.AddKanaData("え");
        Utils.AddKanaData("お");
        Utils.SaveKanaData();
    }

    public void AddSecondBatchOfKana() {
        Utils.AddKanaData("か");
        Utils.AddKanaData("き");
        Utils.AddKanaData("く");
        Utils.AddKanaData("け");
        Utils.AddKanaData("こ");
        Utils.SaveKanaData();
    }

    public void AddThirdBatchOfKana() {
        Utils.AddKanaData("ア");
        Utils.AddKanaData("イ");
        Utils.AddKanaData("ウ");
        Utils.AddKanaData("エ");
        Utils.AddKanaData("オ");
        Utils.SaveKanaData();
    }

    public void AddFourthBatchOfKana() {
        Utils.AddKanaData("カ");
        Utils.AddKanaData("キ");
        Utils.AddKanaData("ク");
        Utils.AddKanaData("ケ");
        Utils.AddKanaData("コ");
        Utils.SaveKanaData();
    }

    public void LowerYLevel() {
        Utils.yLimit = -70f;
        musicManager.NextTrack();

        if (Utils.backgroundUsed == 1) {
            GameObject.Find("Background Forest").SetActive(false);
            GameObject.Find("Canvas Grid").transform.Find("Background Cave").gameObject.SetActive(true);
            Utils.backgroundUsed = 2;
        }
    }
}
