
using UnityEngine;

public class LeverInput : MonoBehaviour
{
    [SerializeField] private LeverController leverController;




    private void Start()
    {
        Debug.Log("LeverInput is active!");
    }

   
    private void OnMouseDown()
    {
        Debug.Log("UPPER LEVER CLICKED!");
        Debug.Log("LEVER CLICK DETECTED!");

        if (leverController == null)
        {
            Debug.LogError("LeverController reference is missing!");
            return;
        }

        leverController.OnUpLeverPressed();
    }
}
