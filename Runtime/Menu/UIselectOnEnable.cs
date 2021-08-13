using UnityEngine;
using UnityEngine.UI;

public class UIselectOnEnable : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<Button>().Select();
    }
}
