using UnityEngine;

public class Collector : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        IItems item = collision.GetComponent<IItems>();
        if(item != null)
        {
            item.Collect();
        }
    }
}
