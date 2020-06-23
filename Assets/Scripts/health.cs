using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class health : MonoBehaviour
{
    public System.Action respawnCallBack;
    public int maxHealth = 10;

    public int actualHealth = 10;

    public bool destoryWhenDead = true;

    private Vector3 respawnVector3 = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        actualHealth = maxHealth;
        respawnVector3 = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (actualHealth <= 0)
        {
            if (destoryWhenDead)
            {
                Destroy(gameObject);
            }
            else
            {
                actualHealth = maxHealth;
                transform.position = respawnVector3;
                if (respawnCallBack != null)
                {
                    respawnCallBack();
                }
            }
        }
    }
}
