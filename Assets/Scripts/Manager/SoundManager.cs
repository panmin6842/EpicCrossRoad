using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{  
    //사운드 변수들

    //AudioSource
    public AudioSource moveAudioSource;
    public AudioSource jumpAudioSource;
    public AudioSource playerAttackSource;
    public AudioSource enemySource;
    public AudioSource bgmAudioSource;
    public AudioSource uiAudioSource;
    public AudioSource playerSkill;

    public AudioSource spadeEnemyAudioSource;
    public AudioSource healEnemyAudioSource;
    public AudioSource diamondEnemyAudioSource;
    public AudioSource cloverEnemyAudioSource;

    public AudioSource bossAudioSource;
    public AudioSource bossHitAudioSource;

    //UISFX
    public AudioClip buttonClickClip;

    //BGM
    public AudioClip forestBattleClip;
    public AudioClip forestNormalClip;
    public AudioClip villageNormalClip;
    public AudioClip villageBattleClip;
    public AudioClip castleEntClip;
    public AudioClip castleRoadClip;
    public AudioClip bossStageClip;

    //SFX
    public AudioClip dirtWalkAudioClip; //흙 걸을 때 걷는 소리
    public AudioClip dirtRunAudioClip; //흙 뛸 때 걷는 소리
    public AudioClip dirtJumpDownAudioClip;

    public AudioClip stoneWalkAudioClip; //돌 걸을 때 걷는 소리
    public AudioClip stoneRunAudioClip; //돌 뛸 때 걷는 소리
    public AudioClip stoneJumpDownAudioClip;

    public AudioClip marbleWalkAudioClip; //대리석 걸을 때 걷는 소리
    public AudioClip marbleRunAudioClip; //대리석 뛸 때 걷는 소리
    public AudioClip marbleJumpDownAudioClip;

    public AudioClip playerDieClip;

    public AudioClip gauntletAttack1;
    public AudioClip gauntletAttack2;

    public AudioClip swordAttack;

    public AudioClip stampAttack1;
    public AudioClip stampAttack2;

    public AudioClip spearAttack;

    public AudioClip enemyHit;
    public AudioClip enemyDie;
    public AudioClip spadeEnemyAttack;
    public AudioClip healEnemyAttack;

    public AudioClip bossUpslashReady; //베어올리기 대기
    public AudioClip bossUpslashHit; //베이올리기 공격
    public AudioClip bossJumpAttackReady; //내려찍기 대기
    public AudioClip bossJumpAttackHit; //내려찍기 공격
    public AudioClip bossHeavySlashReady; //참격 준비
    public AudioClip bossHeavySlashHit; //참격 공격
    public AudioClip bossHeavySlashFail; //참격 실패
    public AudioClip bossDash; //보스 돌격
    public AudioClip bossHit; //보스 피격

    public AudioClip itemGet;

    //정방향 효과음
    public AudioClip spadeSkill;
    public AudioClip cloverSkill;
    public AudioClip heartSkill;
    public AudioClip diamondSkill;

    public bool sfxStop; //효과음이 나와야할지 안 나와야할지

    static public SoundManager instance;

    private void Awake()
    {
        instance = this;
        sfxStop = false;
    }

    public void SetMusicVolume(float volume)
    {
        bgmAudioSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        moveAudioSource.volume = volume;
        jumpAudioSource.volume = volume;
        playerAttackSource.volume = volume;
        enemySource.volume = volume;
        uiAudioSource.volume = volume;
        playerSkill.volume = volume;

        spadeEnemyAudioSource.volume = volume;
        healEnemyAudioSource.volume = volume;
        diamondEnemyAudioSource.volume = volume;
        cloverEnemyAudioSource.volume = volume;

        bossAudioSource.volume = volume;
        bossHitAudioSource.volume = volume;
    }
}
