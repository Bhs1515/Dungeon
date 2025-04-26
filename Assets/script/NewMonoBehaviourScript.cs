using UnityEngine;
using static Player;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField]
    private bool a;

    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(a == true)
            rb.AddForceY(10);

        Jump();
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForceY(100);
        }
    }
}
