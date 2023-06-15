using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public enum Mode { None, Combat }
    public Mode mode;

    public bool isCombat = false;

    private int playerLayer;
    private int npcLayer;
    private int enemyLayer;
    private int layerMask;

    [SerializeField] GameObject target;
    Animator anim;
    Movement playerMove;
    Camera mainCam;

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
        MouseCtrl();
    }

    void KeyCtrl()
    {

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
                if (target.CompareTag("Enemy"))
                {
                    mode = Mode.Combat;
                }
            }
            else
            {
                target = null;
                StartCoroutine(CombatToNone());
            }
        }
    }

    IEnumerator CombatToNone()
    {
        yield return new WaitForSeconds(5f);
        mode = Mode.None;
    }
}