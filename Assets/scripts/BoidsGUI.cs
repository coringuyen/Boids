using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BoidsGUI : MonoBehaviour {

    public Slider Cohension;
    public Slider Alignment;
    public Slider Seperation;
    public Button Exit;
    public Button ResetSliders;

    float cohension, alignment, seperation;

    void Start()
    {
        Cohension.GetComponent<RectTransform>().position = new Vector3(Screen.width * 0.5f, Screen.height * 0.1f, 0);
        Alignment.GetComponent<RectTransform>().position = new Vector3(Screen.width * 0.2f, Screen.height * 0.1f, 0);
        Seperation.GetComponent<RectTransform>().position = new Vector3(Screen.width * 0.8f, Screen.height * 0.1f, 0);

        Exit.GetComponent<RectTransform>().position = new Vector3(Screen.width * 0.8f, Screen.height * 0.95f, 0);
        ResetSliders.GetComponent<RectTransform>().position = new Vector3(Screen.width * 0.2f, Screen.height * 0.95f, 0);

        cohension = Cohension.value;
        alignment = Alignment.value;
        seperation = Seperation.value;

        Cohension.gameObject.SetActive(true);
        Alignment.gameObject.SetActive(true);
        Seperation.gameObject.SetActive(true);
        Exit.gameObject.SetActive(true);
        ResetSliders.gameObject.SetActive(true);
    }

    public void ResetSlider()
    {
        Cohension.value = cohension;
        Alignment.value = alignment;
        Seperation.value = seperation;
    }
}
