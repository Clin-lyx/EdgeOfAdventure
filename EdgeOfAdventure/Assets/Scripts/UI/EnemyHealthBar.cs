using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    private Slider slider;
    [SerializeField]private Character character;
    
    private void Awake() {
        slider = GetComponent<Slider>();
    }

    private void UpdateSlider(){
        slider.value = character.HealthPercentage();
        if (slider.value == 0) slider.gameObject.SetActive(false);
        slider.transform.localScale = new Vector3(character.transform.localScale.x * 0.01f, 0.02f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSlider();
    }
}
