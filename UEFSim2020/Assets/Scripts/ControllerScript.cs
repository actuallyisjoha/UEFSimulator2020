using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerScript : MonoBehaviour
{
    public int Nopat, Rahat, VapaaAika, Olut, Nalka, Kofeiini, Motivaatio, Psykoosi;
    public bool RakkausElama, ZynZyn;
    public Text NopatText, RahatText, VapaaAikaText, OlutText, NalkaText, KofeiiniText, MotivaatioText, PsykoosiText, RakkausElamaText;

    private float timer = 0.0f;
    private float waitTime = 2.0f;

    public int MotivationPenalty = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        updateUIText();
        timer += Time.deltaTime;

        // Check if we have reached beyond 2 seconds.
        // Subtracting two is more accurate over time than resetting to zero.
        if (timer > waitTime)
        {
            FixedTimeUpdate();
            timer = 0;
        }
    }

    void FixedTimeUpdate()
    {
        DecreaseMoney(10);
        IncreaseHunger();
    }

    void updateUIText()
    {
        NopatText.text = "Nopat: " + Nopat + " op";
        RahatText.text = "Rahat: " + Rahat + "€";
        VapaaAikaText.text = "Vapaa-aika: " + VapaaAika + " min";
        OlutText.text = "Olut: " + Olut + " l";
        NalkaText.text = "Nälkä: " + Nalka;
        KofeiiniText.text = "Kofeiini: " + Kofeiini + " mg";
        MotivaatioText.text = "Motivaatio: " + Motivaatio + "%";
        PsykoosiText.text = "Psykoosi: " + Psykoosi + "%";
        if(RakkausElama) RakkausElamaText.text = "Rakkauselämä: on";
        else RakkausElamaText.text = "Rakkauselämä: ei";
    }

    public void Study()
    {
        IncreaseDice();
        DecreaseMotivation();
        DecreaseFreeTime();
        DecreaseCaffeine();
        IncreaseHunger();
        IncreasePsychosis();
    }

    public void Eat()
    {
        DecreaseMoney(4);
        DecreaseHunger();
    }

    public void DecreaseMotivation()
    {
        Motivaatio -= 1 * MotivationPenalty ;
        if (Motivaatio <= 0) GameOver();
    }

    public void DecreaseFreeTime()
    {
        VapaaAika--;
        // Modify MotivationPenalty in here
        //if (VapaaAika <= 0) GameOver();
    }

    public void DecreaseCaffeine()
    {
        Kofeiini--;
    }

    public void DecreaseMoney(int value)
    {
        Rahat -= value;
    }

    public void IncreaseHunger()
    {
        Nalka++;
        if (Nalka >= 100) GameOver();
    }

    public void DecreaseHunger()
    {
        Nalka -= 15;
        if (Nalka < 0) Nalka = 0;
    }

    public void IncreasePsychosis()
    {
        Psykoosi++;
        if (Psykoosi >= 100) GameOver();
    }

    public void IncreaseDice()
    {
        Nopat++;
        if (Nopat >= 300) Debug.Log("Kutittaa. Voitit pelin.");
    }

    public void GameOver()
    {

    }
}
