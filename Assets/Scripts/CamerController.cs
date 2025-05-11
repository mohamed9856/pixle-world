using UnityEngine;

public class CamerController : MonoBehaviour
{
    [SerializeField] private Transform player;
    void Update()
    {
        transform.position = new Vector3(player.position.x, player.position.y + 2, transform.position.z);
    }
}
