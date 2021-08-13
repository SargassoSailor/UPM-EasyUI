using UnityEngine;

public class playAnimOnEnable : MonoBehaviour
{
    public string animTriggerName;
    Animator anim;
    // Start is called before the first frame update

    private void OnEnable()
    {
        anim = GetComponent<Animator>();
        anim.SetTrigger(animTriggerName);
    }

}
