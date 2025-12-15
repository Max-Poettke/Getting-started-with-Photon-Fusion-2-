using UnityEngine;

public class CardVisual : MonoBehaviour
{
    public Card target;

    // Update is called once per frame
    void Update()
    {
        if(target != null);
        transform.position = Vector3.Lerp(transform.position, target.transform.position, 0.1f);

        //var angledifference = Vector3.Angle(transform.up, target.transform.position - transform.position);
    }
}
