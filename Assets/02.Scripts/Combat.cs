using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public enum Mode { None, Combat }
    public Mode mode;
    public enum Equip { None, Melee, Range }
    public Equip equip;

    public float attackSpeed;
    public bool isCombat = false;

    private int playerLayer;
    private int npcLayer;
    private int enemyLayer;
    private int layerMask;
    private float attackDist;
    private bool disArming;
    private bool attacking;

    public SphereCollider attackPoint;
    [SerializeField] GameObject target;
    Animator anim;
    Movement playerMove;
    Camera mainCam;
    Coroutine combatToNone;
    Coroutine attackRoutine;

    void Awake()
    {
        playerMove = GetComponent<Movement>();
        mainCam = Camera.main;
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        mode = Mode.None;

        playerLayer = LayerMask.NameToLayer("Player");
        npcLayer = LayerMask.NameToLayer("Npc");
        enemyLayer = LayerMask.NameToLayer("Enemy");
        layerMask = 1 << playerLayer | 1 << npcLayer | 1 << enemyLayer;
        attackPoint.enabled = false;
    }

    void Update()
    {
        KeyCtrl();
        MouseCtrl();

        if (mode == Mode.Combat && target != null && !attacking)
        {
            attackRoutine = StartCoroutine(Attack());
        }
    }

    void KeyCtrl()
    {
        if (target != null)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3))
            {
                mode = Mode.Combat;
                if (disArming)
                    StopCoroutine(combatToNone);
            }
        }
    }

    void MouseCtrl()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                target = hit.collider.gameObject;
                if (target.CompareTag("Enemy") && Input.GetMouseButtonDown(1))
                {
                    mode = Mode.Combat;
                    if (disArming)
                        StopCoroutine(combatToNone);
                }
            }
            else
            {
                target = null;
                combatToNone = StartCoroutine(CombatToNone());
            }
        }
    }

    IEnumerator Attack()
    {
        attacking = true;
        anim.SetBool("IsCombat", true);

        switch (equip)
        {
            case Equip.None:
                attackDist = 2.5f;
                break;
            case Equip.Melee:
                attackDist = 3.5f;
                break;
            case Equip.Range:
                attackDist = 10f;
                break;
        }

        if (attackDist < (target.transform.position - transform.position).magnitude || target == null)
            yield break;

        anim.SetInteger("AttackIdx", Random.Range(0, 2));
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f);
        attackPoint.enabled = true;
        yield return new WaitForSeconds(0.01f);
        attackPoint.enabled = false;
        yield return new WaitForSeconds(attackSpeed);
        attacking = false;
    }

    IEnumerator CombatToNone()
    {
        disArming = true;
        yield return new WaitForSeconds(5f);
        mode = Mode.None;
        disArming = false;
        anim.SetBool("IsCombat", false);
    }
}