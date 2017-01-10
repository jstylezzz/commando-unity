//  
// Copyright (c) Jari Senhorst. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

using UnityEngine;
using System.Collections;

public class CannonScript : MonoBehaviour 
{

    // Shooting cooldown time
    [SerializeField]
    private float m_shootCooldown;


	/// <summary>
	/// Script entry point
	/// </summary>
	void Start() 
    {
        ShootBullet();
	}
	
    /// <summary>
    /// Shoot a bullet
    /// </summary>
    private void ShootBullet()
    {
        GameObject bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet/cannonBullet");
        GameObject theBullet = Instantiate(bulletPrefab);
        theBullet.GetComponent<BulletController>().SetOwner(m_entityTypes.TYPE_ENEMY);
        theBullet.transform.position = transform.position;
        theBullet.GetComponent<BulletController>().SetDirection(m_moveDirections.DownRight);
        StartCoroutine(ShotCooldown());
    }

    /// <summary>
    /// Wait a little while before shooting again
    /// </summary>
    private IEnumerator ShotCooldown()
    {
        yield return new WaitForSeconds(m_shootCooldown);
        ShootBullet();
    }
}
