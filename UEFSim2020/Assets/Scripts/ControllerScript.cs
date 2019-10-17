using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerScript : MonoBehaviour
{
    public int Nopat, Rahat, VapaaAika, Olut, Nalka, Kofeiini, Motivaatio, Psykoosi, RakkausElama;
    public Text NopatText, RahatText, VapaaAikaText, OlutText, NalkaText, KofeiiniText, MotivaatioText, PsykoosiText, RakkausElamaText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Nopat++;
        Rahat++;
        VapaaAika++;
        Olut++;
        Nalka++;
        Kofeiini++;
        Motivaatio++;
        Psykoosi++;
        RakkausElama++;
        updateUIText();
    }

    void updateUIText()
    {
        NopatText.text = "Nopat: " + Nopat;
        RahatText.text = "Rahat: " + Rahat;
        VapaaAikaText.text = "Vapaa-aika: " + VapaaAika;
        OlutText.text = "Olut: " + Olut;
        NalkaText.text = "Nälkä: " + Nalka;
        KofeiiniText.text = "Kofeiini: " + Kofeiini;
        MotivaatioText.text = "Motivaatio: " + Motivaatio;
        PsykoosiText.text = "Psykoosi: " + Psykoosi;
        RakkausElamaText.text = "Rakkauselämä: " + RakkausElama;
    }
}
