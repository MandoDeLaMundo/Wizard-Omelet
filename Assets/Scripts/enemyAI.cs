using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;

    [SerializeField] int HP;
    [SerializeField] int faceTargetSpeed;

    [SerializeField] Transform shootPos;
    [SerializeField] GameObject enemyProjectile;
    [SerializeField] float shootRate;

    Color colorOrig;

    Vector3 playerDir;

    float shootTimer;

    bool playerInRange;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrig = model.material.color;
        gameManager.instance.UpdateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;

        if(playerInRange)
        {
            playerDir = (gameManager.instance.player.transform.position - transform.position);
            agent.SetDestination(gameManager.instance.player.transform.position);

            if (shootTimer >= shootRate)
                Shoot();

            if (agent.remainingDistance <= agent.stoppingDistance)
                FaceTarget();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
            playerInRange = false;
    }

    public void TakeDamage(int amount)
    {
        HP -= amount;

        agent.SetDestination(gameManager.instance.player.transform.position);

        if (HP <= 0)
        {
            //gameManager.instance.UpdateGameGoal(-1);
            Destroy(gameObject);
        }
        else
            StartCoroutine(FlashRed());
    }

    IEnumerator FlashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.05f);
        model.material.color = colorOrig;
    }

    void FaceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, transform.position.y, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, faceTargetSpeed);
    }

    void Shoot()
    {
        shootTimer = 0;
        Instantiate(enemyProjectile, shootPos.position, transform.rotation);
    }
}
