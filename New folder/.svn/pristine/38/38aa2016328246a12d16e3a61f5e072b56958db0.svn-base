using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Shooting : MonoBehaviour
{
	public int remainingAmmo;
	[SerializeField]
	float defaultReloadTime;

	//float weaponTimer;
	float shotTimer;
	
	public Weapon currentWeapon;
	[SerializeField]
	Weapon defaultWeapon;

	bool reloading;

	[SerializeField]
	Transform bulletSpawnPos;

	[SerializeField]
	AudioClip gunShot;
	[SerializeField]
	AudioClip shotGun;
	[SerializeField]
	AudioSource source;

	PlayerMotor motor;
	PlayerController controller;

	UIManager uiManager;

	Rewired.Player player;

	[SerializeField]
	ReloadBar reloadBar;

	void Start()
	{
		motor = GetComponent<PlayerMotor>();
		controller = GetComponent<PlayerController>();
		EquipDefaultWeapon();
		player = ReInput.players.GetPlayer(controller.playerID);
		shotTimer = currentWeapon.shotCooldown;
		uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
		reloadBar = GetComponentInChildren<ReloadBar>();
		InitialiseAmmoUI();
	}

	// Update is called once per frame
	void Update()
	{
		shotTimer += Time.deltaTime;

		if (currentWeapon.weaponType == Weapon.WeaponType.Gun)
		{

			if (currentWeapon.fireRate == Weapon.FireRate.Automatic)
			{
				//shoot every x seconds
				if (player.GetButtonDown("Fire"))
				{
					InvokeRepeating("AutomaticFire", 0f, currentWeapon.shotCooldown);
				}
				else if (player.GetButtonUp("Fire"))
				{
					CancelInvoke();
				}
			}
			else
			{
				if (player.GetButtonDown("Fire"))
				{
					Fire();
				}
			}

			if (player.GetButtonDown("Reload"))
			{
				if (currentWeapon.defaultWeapon && remainingAmmo < currentWeapon.maxAmmo)
				{
					remainingAmmo = 0;
				}
			}

			if (reloading)
			{
				reloadBar.ReloadBarFill(defaultReloadTime);
			}

		} else if (currentWeapon.weaponType == Weapon.WeaponType.Projectile)
		{
			if (player.GetButtonDown("Fire"))
			{
				LaunchProjectile();
			}
		}

		//if (!currentWeapon.defaultWeapon)
		//{
		//	weaponTimer -= Time.deltaTime;

		//	if (weaponTimer <= 0f)
		//	{
		//		EquipDefaultWeapon();
		//	}
		//}

		CheckAmmo();
	}

	public void UpdateWeapon(Weapon _weapon)
	{
		//adjust player weapon stats to that of new equipped weapon
		currentWeapon = _weapon;
		remainingAmmo = currentWeapon.maxAmmo;
		//weaponTimer = currentWeapon.timeActive;
		shotTimer = currentWeapon.shotCooldown;
		CancelInvoke();
	}

	public void EquipDefaultWeapon()
	{
		//adjust player weapon stats to stats of default weapon
		currentWeapon = defaultWeapon;
		UpdateWeapon(currentWeapon);
	}

	void Fire()
	{
		//check to see if shot cooldown has passed
		if (shotTimer >= currentWeapon.shotCooldown && remainingAmmo > 0)
		{
			SpawnBullet();
			controller.ControllerVibrate(currentWeapon.vibrateIntensity, currentWeapon.vibrateDuration);
			motor.ApplyKnockback(currentWeapon.knockBackForce);
			shotTimer = 0f;
			PlayGunShot();
			remainingAmmo -= 1;
			uiManager.UpdateAmmo(controller.playerID);
		}
	}

	void AutomaticFire()
	{
		if (remainingAmmo > 0)
		{
			SpawnBullet();
			PlayGunShot();
			controller.ControllerVibrate(currentWeapon.vibrateIntensity, currentWeapon.vibrateDuration);
			motor.ApplyKnockback(currentWeapon.knockBackForce);
			remainingAmmo -= 1;
			uiManager.UpdateAmmo(controller.playerID);
		}
	}

	void LaunchProjectile()
	{
		remainingAmmo -= 1;
		uiManager.UpdateAmmo(controller.playerID);

		SpawnProjectile();
	}

	void SpawnBullet()
	{
		GameObject bulletIns = Instantiate(currentWeapon.bulletPrefab, bulletSpawnPos.position, bulletSpawnPos.transform.rotation);

		if (currentWeapon.fireRate != Weapon.FireRate.Spread)
		{
			bulletIns.GetComponent<Rigidbody>().velocity = bulletIns.transform.forward * currentWeapon.bulletVelocity;
			bulletIns.GetComponent<Bullet>().damage = currentWeapon.damage;
			bulletIns.GetComponent<Bullet>().playerID = controller.playerID;
			bulletIns.GetComponent<Bullet>().forceAmount = currentWeapon.hitImpactForce;
			bulletIns.GetComponent<Bullet>().maxCollisions = currentWeapon.maxCollisions;
		}
		else if (currentWeapon.fireRate == Weapon.FireRate.Spread)
		{
			Bullet[] bulletScripts = bulletIns.GetComponentsInChildren<Bullet>();
			Rigidbody[] rbs = bulletIns.GetComponentsInChildren<Rigidbody>();

			foreach (Rigidbody rb in rbs)
			{
				rb.velocity = rb.gameObject.transform.forward * currentWeapon.bulletVelocity;
			}

			foreach (Bullet bullet in bulletScripts)
			{
				bullet.damage = currentWeapon.damage;
				bullet.playerID = controller.playerID;
				bullet.forceAmount = currentWeapon.hitImpactForce;
			}
		}
	}

	void SpawnProjectile()
	{
		GameObject projectileIns = Instantiate(currentWeapon.projectilePrefab, bulletSpawnPos.position, bulletSpawnPos.transform.rotation);

		projectileIns.GetComponent<Rigidbody>().velocity = projectileIns.transform.forward * currentWeapon.projectileVelocity;
		projectileIns.GetComponent<Projectile>().damage = currentWeapon.damage;
		projectileIns.GetComponent<Projectile>().damageDropoff = currentWeapon.damageDropoff;
		projectileIns.GetComponent<Projectile>().areaOfEffect = currentWeapon.areaOfEffect;
		projectileIns.GetComponent<Projectile>().forceAmount = currentWeapon.explosionForce;
		projectileIns.GetComponent<Projectile>().explosionPrefab = currentWeapon.explosionPrefab;
		projectileIns.GetComponent<Projectile>().playerID = controller.playerID;
	}

	void CheckAmmo()
	{
		if (remainingAmmo <= 0f)
		{
			if (currentWeapon.defaultWeapon)
			{
				if (!reloading)
				{
					StartCoroutine("Reload");
				} else
				{
					return;
				}
			}
			else
			{
				EquipDefaultWeapon();
				uiManager.UpdateAmmo(controller.playerID);
			}
		}
	}

	IEnumerator Reload()
	{
		reloading = true;
		reloadBar.ReloadBarEmpty();
		yield return new WaitForSeconds(defaultReloadTime);
		remainingAmmo = currentWeapon.maxAmmo;
		uiManager.UpdateAmmo(controller.playerID);
		reloading = false;
	}

	void InitialiseAmmoUI()
	{
		uiManager.UpdateAmmo(0);
		uiManager.UpdateAmmo(1);
		uiManager.UpdateAmmo(2);
		uiManager.UpdateAmmo(3);
	}

	void PlayGunShot()
	{
		if (currentWeapon.fireRate == Weapon.FireRate.Spread)
		{
			source.clip = shotGun;
			source.Play();
		}
		else
		{
			source.clip = gunShot;
			source.Play();
		}
	}
}
