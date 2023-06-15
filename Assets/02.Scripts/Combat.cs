using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public enum Mode { None, Combat }
    public Mode mode;
    public enum Equip { None, Melee, Range }
    public Equip equip;

    public bool isCombat = false;

    private int playerLayer;
    private int npcLayer;
    private int enemyLayer;
    private int layerMask;
    private bool disArming;

    [SerializeField] GameObject target;
    Animator anim;
    Movement playerMove;
    Camera mainCam;
    Coroutine combatToNone;

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
    }

    void Update()
    {
        KeyCtrl();
        MouseCtrl();
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

                Attack();
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

                    Attack();
                }
            }
            else
            {
                target = null;
                combatToNone = StartCoroutine(CombatToNone());
            }
        }
    }

    void Attack()
    {

    }

    IEnumerator CombatToNone()
    {
        disArming = true;
        yield return new WaitForSeconds(5f);
        mode = Mode.None;
        disArming = false;
    }
}