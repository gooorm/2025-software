using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hp_system : MonoBehaviour
{

    [SerializeField] float HP_max = 100f;
    [SerializeField] float HP = 100f;

    [SerializeField] Image HP_bar;
    [SerializeField] TextMeshProUGUI HP_text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateUI()
    {
        HP_text.text = HP.ToString();
        HP_bar.fillAmount = HP / HP_max;
    }

    public void UpdateHP(int value) { 

        HP = HP + value;

        if(HP < 0) HP = 0;
        else if(HP > HP_max) HP = HP_max;

        UpdateUI();
    }
}
