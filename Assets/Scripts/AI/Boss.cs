using System;
using System.Collections;
using AI.BossPattern;
using MyBox;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AI
{
    public class Boss : Enemy
    {
        public static Boss Instance;

        public static Action<PlayerController> OnTriggerAttack;
        [ReadOnly] public Pattern currentPattern;
        public new BossData data;

        [SerializeField] private string[] voicelines;
        [SerializeField] private string[] voicelinesDead;
        [SerializeField] private GameObject FXShield;
        [SerializeField] private BossBT bossBt;

        private int shieldHealth = 0;

        private void Awake()
        {
            Instance = this;
        }


        private void OnEnable()
        {
            GameManager.OnLaunchingBoss += BeginAttack;
        }

        private void OnDisable()
        {
            GameManager.OnLaunchingBoss -= BeginAttack;
        }

        private void BeginAttack()
        {
            maxHp = data.maxHealth; // possible to change max health value
            healthEnemy.Init(maxHp);
            ResetAttackBossDefaultValue();
            Print_Argh();
            AddShield();
            bossBt.enabled = true;
        }


        public void LaunchPattern(IEnumerator coroutine)
        {
            StartCoroutine(coroutine);
        }

        private void ShieldTakeDamage(int damage, PlayerController origin)
        {
            if (shieldColor != PlayerColor.None && shieldColor != origin.Color.PColor) return;
            shieldHealth -= damage;
            if (shieldHealth <= 0)
            {
                ResetAttackBossDefaultValue();
                FXShield.SetActive(false);
            }
        }

        private void TakeBossDamage(int _damage, PlayerController origin)
        {
            BossDamage(_damage);
        }

        private void BossDamage(int damage)
        {
            float ratio = healthEnemy.GetRatio();
            healthEnemy.LoseHealth(damage);
            if (ratio > 0.5f && healthEnemy.GetRatio() <= 0.5f)
            {
                AddShield();
            }
        }

        private void ResetAttackBossDefaultValue()
        {
            IsWasAttacked = TakeBossDamage;
        }

        private void LootSystem()
        {
            for (int i = 0; i < PlayerManager.Players.Count; i++)
            {
                int indexGun = Random.Range(0, data.lootGun.Length);
                int indexGunAmmo = Random.Range(0, data.lootGunAmmo.Length);

                Instantiate(data.lootGun[Random.Range(0, data.lootGun.Length)],
                        transform.position + Vector3.back * 10 + Vector3.right * (i - 2) + Vector3.right * i * 2 -
                        Vector3.up * 4.5f, Quaternion.identity)
                    .GetComponent<Renderer>().material.color = PlayerManager.Players[i].Color.PColor == PlayerColor.Blue
                    ? UnityEngine.Color.blue
                    : UnityEngine.Color.red;
                Instantiate(data.lootGunAmmo[Random.Range(0, data.lootGunAmmo.Length)],
                        transform.position + Vector3.back * 10 + Vector3.right * (i - 1) + Vector3.right * i * 2 -
                        Vector3.up * 4.5f, Quaternion.identity)
                    .GetComponent<Renderer>().material.color = PlayerManager.Players[i].Color.PColor == PlayerColor.Blue
                    ? UnityEngine.Color.blue
                    : UnityEngine.Color.red;
            }
        }

        protected override void OnDie()
        {
            LootSystem();
            Print_DieVoicelines();
            GameManager.OnEndFightBoss?.Invoke();
            gameObject.SetActive(false);
        }

        private void AddShield()
        {
            shieldColor = (PlayerColor)Random.Range(0, 2);
            shieldHealth = data.maxHealthShield;
            FXShield.GetComponent<Renderer>().material.color = (shieldColor == PlayerColor.Blue)
                ? new Color(0, 0, 1, 0.5f)
                : new Color(1, 0, 0, 0.5f);
            FXShield.SetActive(true);
            IsWasAttacked = ShieldTakeDamage;
        }

        private void Print_Argh()
        {
            UIManager.Instance.SetVoicelineText(voicelines[Random.Range(0, voicelines.Length)]);
        }

        private void Print_DieVoicelines()
        {
            UIManager.Instance.SetVoicelineText(voicelinesDead[Random.Range(0, voicelinesDead.Length)]);
        }
    }
}