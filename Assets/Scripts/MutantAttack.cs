using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MutantAttack : Attack
{
    protected Rigidbody2D rigidBody;

    public GameObject CurrentTarget { get; protected set; }

    protected override void Awake()
    {
        base.Awake();
        rigidBody = GetComponent<Rigidbody2D>();
        FindTarget();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (CurrentTarget == null)
        {
            FindTarget();
        }
        if (AttackIsPossible)
        {
            SoundPlayer.PlayAttackSound();
            ATTACK_StartAnimation();
        }
        base.Update();
    }

    void FindTarget()
    {
        // get root objects in scene
        List<GameObject> rootObjects = new List<GameObject>();
        Scene scene = SceneManager.GetActiveScene();
        scene.GetRootGameObjects(rootObjects);

        Vector2 myPos = rigidBody.position;

        float minDistance = float.MaxValue;
        GameObject nearestHuman = null;
        for (int i = 0; i < rootObjects.Count; ++i)
        {
            GameObject curGameObject = rootObjects[i];
            if (curGameObject.name == "Letov")
            {
                Vector2 humanPos = curGameObject.GetComponent<Rigidbody2D>().position;
                Vector2 dif = humanPos - myPos;
                float distance = dif.magnitude;
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestHuman = curGameObject;
                }
            }
        }
        if (nearestHuman != null)
            CurrentTarget = nearestHuman;
    }

    bool AttackIsPossible
    {
        get
        {
            if (animationInProgress)
                return false;
            Vector2 myPos = rigidBody.position;
            Vector2 enemyPos = CurrentTarget.transform.position;
            float distance = (enemyPos - myPos).magnitude;
            return distance < AttackDistance;
        }
    }
}
