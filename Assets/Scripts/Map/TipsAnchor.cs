using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TipsAnchor : MonoBehaviour
{
    [SerializeField] float innerRadius;
    [SerializeField] float outerRadius;
    public GameObject tipsObj;
    public string text;
    public TextMeshProUGUI tips;
    public Transform canvas;
    Transform player;

    private void Awake()
    {
        tips = null;
        player = null;
    }

    private void Update()
    {
        UpdateAlpha();
    }

    private void UpdateAlpha()
    {
        Color color = Color.white;
        if (tips != null)
        {
            float distance = (transform.position - player.transform.position).sqrMagnitude;
            Debug.Log(distance);
            if (distance < outerRadius * outerRadius)
            {
                color.a = Mathf.Min((outerRadius * outerRadius - distance) / (outerRadius * outerRadius - innerRadius * innerRadius), 1f);
            }
            else
                color.a = 0f;
            tips.color = color;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObject b = Instantiate(tipsObj,canvas);
            tips = b.GetComponent<TextMeshProUGUI>();
            tips.transform.position = transform.position;
            tips.text = text;
            player = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(tips != null)
            {
                Destroy(tips.gameObject);
                tips = null;
                player = null;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, outerRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, innerRadius);
    }

}
