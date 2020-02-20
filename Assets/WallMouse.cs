using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WallMouse : MonoBehaviour
{
    public GameObject tooltip; //<--Assign in inspector & deactivate.
    public TextMeshProUGUI text;

    private void OnMouseUp()
    {
        var mask = LayerMask.GetMask("Cubes");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
        {
            var o = hit.collider.gameObject;
            tooltip.transform.position = hit.point + Camera.main.transform.forward * -1;

            text.text = o.name;
            StartCoroutine(ShowTooltip());

        }
    }

    IEnumerator ShowTooltip()
    {
        tooltip.SetActive(true);
        yield return new WaitForSeconds(5);
        tooltip.SetActive(false);
    }
}
