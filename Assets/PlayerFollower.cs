using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    public Transform arCamera;

    void Update()
    {
        if (arCamera != null)
        {
            transform.position = arCamera.position;
        }
    }
}
