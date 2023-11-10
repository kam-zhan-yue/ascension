/*
 * Author: Alexander Kam
 * Date: 30-5-20
 * Licence: Unity Personal Editor Licence
 */
using UnityEngine;
using TMPro;

public class TextPopUp : MonoBehaviour
{
    public TextMeshPro textMesh;
    private float disappearTimer = 1f;
    private float moveYSpeed = 0.5f;
    private Color textColor;

    //===============PROCEDURE===============//
    public void PointsPopUp(int number)
    //Purpose:          Adds a pop up for points with a gold colour
    {
        textColor = new Color32(255, 187, 50, 255);
        textMesh.color = textColor;
        textMesh.text = "+" + number.ToString();
    }

    //===============PROCEDURE===============//
    public void DamagePopUp(int number)
    //Purpose:          Adds a pop up for damage with a red colour
    {
        textColor = new Color32(178, 10, 10, 255);
        textMesh.color = textColor;
        moveYSpeed = 1f;
        disappearTimer = 0.5f;
        textMesh.text = number.ToString();
    }

    private void Update()
    {
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;
        disappearTimer -= Time.deltaTime;
        if(disappearTimer<0)
        {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0)
                Destroy(gameObject);
        }
    }
}
