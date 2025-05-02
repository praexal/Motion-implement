using UnityEngine;
using TMPro;
public class Vision : MonoBehaviour
{
    public GameObject player;
    public GameObject playerParent;
    public bool InRange, InFOV, notHidden;
    float angle, distance;
    RaycastHit hit;

    public TMP_Text range;
    public TMP_Text fov;
    public TMP_Text hidden;
    private void Awake()
    {

    }
    private void Update()
    {
        Vector3 direction = player.transform.position - transform.position;
        angle = Vector3.Angle(transform.forward, direction);
        
        if(angle > 30f)
        {
            InFOV = false;
            fov.color = Color.green;
        }
        else
        {
            InFOV = true;
            fov.color = Color.red;
        }

        distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance > 10f)
        {
            InRange = false;
            range.color = Color.green;
        }
        else
        {
            InRange = true;
            range.color = Color.red;
        }
        Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity);
        Debug.Log(hit.collider);
        if(hit.transform != playerParent.transform)
        {
            notHidden = false;
            hidden.color = Color.green;
            hidden.text = "Hidden";
        }
        else
        {
            notHidden = true;
            hidden.color = Color.red;
            hidden.text = "Not Hidden";
        }
    }
    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, (player.transform.position - transform.position), Color.red);
    }
}
