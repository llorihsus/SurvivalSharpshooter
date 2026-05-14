using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private CharacterController controller;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (anim == null || controller == null) return;

        float speed = controller.velocity.magnitude;
        anim.SetFloat("MoveSpeed", speed);
    }
}