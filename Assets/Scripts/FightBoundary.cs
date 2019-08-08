using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FightBoundary : MonoBehaviour {

    // V6.4
    // 1. Enemies walking and attacking animations are implemented
    // but no flinch and death Animation.
    // 2. MakiRoll is invulnerable while dodging.
    // 3. Goon can die now.
    // 4. Icon is enabled after consumed the right enemy for the first time.

    // *Bug: Fight1Manager does not function properly after I imported Tempura and Maki prefabs.
    // The Maki is flickering with blue color.
    // *Bug: Player dodge doesn't roll forward (happen sometimes).

    public float bossBoundary;

    private EnemyAI enemy;
    private PlayerController player;
    // Use this for initialization
    void Start()
    {
        enemy = FindObjectOfType<EnemyAI>();
        player = FindObjectOfType<PlayerController>();
    }

    void LateUpdate()
    {
        if (this.tag == "Player")
        {
            if (SceneManager.GetActiveScene().buildIndex != 5)
            {
                Vector3 pos = Camera.main.WorldToViewportPoint(player.transform.position);
                pos.x = Mathf.Clamp(pos.x, 0.02f, 0.98f);
                pos.y = Mathf.Clamp(pos.y, -0.5f, 0.98f);

                Vector3 speed = player.rb.velocity;
                if (pos.x == 0 || pos.x == 1)
                    speed.x = 0;
                if (pos.y == 0 || pos.y == 1)
                    speed.y = 0;

                player.transform.position = Camera.main.ViewportToWorldPoint(pos);
                player.rb.velocity = speed;
            }
            else if(SceneManager.GetActiveScene().buildIndex == 5)
            {
                Vector3 pos = Camera.main.WorldToViewportPoint(player.transform.position);
                pos.x = Mathf.Clamp(pos.x, 0.02f, bossBoundary);
                pos.y = Mathf.Clamp(pos.y, -0.5f, 0.98f);

                Vector3 speed = player.rb.velocity;
                if (pos.x == 0 || pos.x == 1)
                    speed.x = 0;
                if (pos.y == 0 || pos.y == 1)
                    speed.y = 0;

                player.transform.position = Camera.main.ViewportToWorldPoint(pos);
                player.rb.velocity = speed;
            }
            
        }
        else if (this.tag == "Enemy")
        {
            if(enemy != null)
            {
                Vector3 pos = Camera.main.WorldToViewportPoint(enemy.transform.position);
                pos.x = Mathf.Clamp(pos.x, 0.02f, 0.98f);
                pos.y = Mathf.Clamp(pos.y, -0.5f, 0.98f);

                Vector3 speed = enemy.rb.velocity;
                if (pos.x == 0 || pos.x == 1)
                    speed.x = 0;
                if (pos.y == 0 || pos.y == 1)
                    speed.y = 0;

                enemy.transform.position = Camera.main.ViewportToWorldPoint(pos);
                enemy.rb.velocity = speed;
            }
            else
            {
                return;
            }
        }
    }
}
