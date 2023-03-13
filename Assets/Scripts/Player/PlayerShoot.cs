using System.Collections;
using Player;
using UnityEngine;
using Utils;

    public class PlayerShoot : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private WeaponData weaponData;
        [SerializeField] private Transform firePoint;

        private bool canShoot = true;

        public void Shoot()
        {
            if (!canShoot) return;
            playerController.Animator.SetTrigger("Attack");
            Bullet bullet = Pooler.Instance.Pop(Key.Bullet).GetComponent<Bullet>();
            bullet.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);
            bullet.Init(playerController, weaponData);
            StartCoroutine(ShootCooldown());
        }

        private IEnumerator ShootCooldown()
        {
            canShoot = false;
            yield return new WaitForSeconds(1 );
            canShoot = true;
        }
    }