﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

namespace UEFSimulator
{
    public class ControllerScript : MonoBehaviour
    {
        public int TukiKuukaudet, Nopat, Rahat, VapaaAika, Nalka, Kofeiini, Motivaatio, Psykoosi, MotivationPenalty, MonthlyAllowance, Kolikot;
        public float Promillet = 0.0f;
        public bool RakkausElama, GameOver, PopupActive;
        public GameObject PopupImage, RigidBodyFPSController, GameOverImage;
        public Image OluttaImage;
        public Text TukiKuukaudetText, NopatText, RahatText, VapaaAikaText, OlutText, NalkaText, KofeiiniText, MotivaatioText, PsykoosiText, RakkausElamaText;

        private int popupMonth = 0;
        private float secondTime = 0.1f;
        private float secondTimer = 0.0f;
        private float dayTime = 2.5f;
        private float dayTimer = 0.0f;
        private float monthTime = 6.0f;
        private float monthTimer = 0.0f;
        private bool virgin = true;
        private bool ekaVuosiPopUp, tokaVuosiPopup, kolmasVuosiPopup, neljasVuosiPopup, joleneVisited;
        
        private RigidbodyFirstPersonController fpsController;
        private AudioScript audioScript;

        void Start()
        {
            PopupActive = false;
            if(PopupImage != null) PopupImage.SetActive(false);

            fpsController = RigidBodyFPSController.GetComponent<RigidbodyFirstPersonController>();
            audioScript = GetComponent<AudioScript>();

            ShowPopup("Hei fuksipallero! Valitettavasti siivoojat nukkuivat tänään pommiin, joten tietokoneluokassa on hieman sotkuista.\n\nPaina Enter esittääksesi välittävän siistiydestä.");
        }

        void Update()
        {
            if (GameOver) {
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
                {
                    GameOver = false;
                    SceneManager.LoadScene("MainMenuScene", LoadSceneMode.Single);
                }
            }
            else
            {
                updateUIText();

                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
                {
                    if (PopupImage != null) PopupImage.SetActive(false);
                    PopupActive = false;
                }

                secondTimer += Time.deltaTime;
                if (secondTimer > secondTime)
                {
                    DecreaseMoney(3);
                    secondTimer = 0;
                    
                }

                dayTimer += Time.deltaTime;
                if (dayTimer > dayTime)
                {
                    IncreaseHunger();
                    dayTimer = 0;
                    Promillet -= 3.5f;
                    if (Promillet < 0) Promillet = 0;
                }

                monthTimer += Time.deltaTime;
                if (monthTimer > monthTime)
                {
                    Rahat += MonthlyAllowance;
                    DecreaseMonths();
                    monthTimer = 0;
                    if (Random.Range(1, 10) == 1 && audioScript != null && !audioScript.AmbientSoundPlaying())
                        audioScript.PlayAmbientSound(AmbientSounds.WorkReminder);
                }

                if(fpsController != null)
                {
                    if (Promillet < 10) fpsController.movementSettings.ForwardSpeed = 8.0f;
                    if (Promillet > 10) fpsController.movementSettings.ForwardSpeed = 7.25f;
                    if (Promillet > 15) fpsController.movementSettings.ForwardSpeed = 6.5f;
                    if (Promillet > 20) fpsController.movementSettings.ForwardSpeed = 5.75f;
                    if (Promillet > 25) fpsController.movementSettings.ForwardSpeed = 5.0f;
                    if (Promillet > 30) fpsController.movementSettings.ForwardSpeed = 4.25f;
                    if (Promillet > 35) fpsController.movementSettings.ForwardSpeed = 3.5f;
                    if (Promillet > 40) fpsController.movementSettings.ForwardSpeed = 2.75f;
                }

                try {
                    byte bytePromillet = System.Convert.ToByte(Promillet * 5);
                    if (bytePromillet > 210) bytePromillet = 220;
                    OluttaImage.color = new Color32(255, 255, 255, bytePromillet);
                }
                catch
                {
                    OluttaImage.color = new Color32(255, 255, 255, 220);
                }
                
            }
        }

        void updateUIText()
        {
            try
            {
                TukiKuukaudetText.text = "Tukikuukausia jäljellä: " + TukiKuukaudet + " kk";
                NopatText.text = "Nopat: " + Nopat + " op";
                RahatText.text = "Rahat: " + Rahat + "€";
                VapaaAikaText.text = "Vapaa-aika: " + VapaaAika + " min";
                OlutText.text = "Promillet: " + System.Math.Round(Promillet / 30, 2);

                NalkaText.text = "Nälkä: " + Nalka;
                KofeiiniText.text = "Kofeiini: " + Kofeiini + " mg";
                MotivaatioText.text = "Motivaatio: " + Motivaatio + "%";
                PsykoosiText.text = "Psykoosi: " + Psykoosi + "%";
                if (RakkausElama) RakkausElamaText.text = "Rakkauselämä: olutta";
                else if (!virgin) RakkausElamaText.text = "Rakkauselämä: ollutta";
                else RakkausElamaText.text = "Rakkauselämä: ei";
            }
            catch
            {
                // Do you really to expect things to happen in here?
            }

        }

        #region PlayerActions
        public void ChangeRadio()
        {
            if(audioScript != null) audioScript.ChangeRadio();
        }

        public void Study()
        {
            if (audioScript != null) audioScript.PlaySound(Sounds.Study);

            int rand = Random.Range(1, 6);
            if (rand != 4)
                IncreaseDice();
            else
            {
                IncreasePsychosis();

                int randS = Random.Range(1, 8);
                if (randS == 1 && joleneVisited && Promillet == 0) ShowPopup("Et päässyt kurssia läpi, koska olit liian darrassa tentissä!\n\nPaina Enter oksentaaksesi tenttipaperiin.");
                else if (randS == 2) ShowPopup("Et päässyt kurssia läpi, koska olet liian tyhmä!\n\nPaina Enter vittuuntuaksesi.");
                else if (randS == 3) ShowPopup("Et päässyt kurssia läpi, koska et ole professorin suosikkilistalla.\n\nPaina Enter lähettääksesi Facebook-viestejä suosion toivossa.");
                else if (randS == 4 && joleneVisited) ShowPopup("Et päässyt kurssia läpi, koska vietit liikaa aikaa Jolenessa.\n\nPaina Enter alkoholisoituaksesi.");
                else if (randS == 5) ShowPopup("Et päässyt kurssia läpi, koska x-tehtävät menivät tunteisiin.\n\nPaina Enter hajottaaksesi näppäimistön.");
                else if (randS == 6) ShowPopup("Et päässyt kurssia läpi, koska ICT-opintopolun opetus ei vastaa tentissä vaadittua osaamista.\n\nPaina Enter lähettääksesi vihaista kurssipalautetta.");
                else if (randS == 7 && RakkausElama) ShowPopup("Et päässyt kurssia läpi, koska olit liikaa tunnellissa.\n\nPaina Enter mennäksesi tunneliin vitutuksesta.");
                else ShowPopup("Et päässyt kurssia läpi, koska professorin kissat söivät läksysi.\n\nPaina Enter opetellaksesi tekemään varmuuskopioita.");
            }

            DecreaseMotivation();
            DecreaseFreeTime();
            DecreaseCaffeine();

            IncreaseHunger();
            IncreasePsychosis();

            RakkausElama = false;
        }

        public void Bottle()
        {
            if (audioScript != null) audioScript.PlaySound(Sounds.Bottle);
            Kolikot++;
            ShowPopup("Palautit pullon kauppaan!\n\nPaina Enter saadaksesi rahaa.");
        }

        public void Eat()
        {
            if (Rahat >= 4)
            {
                if (audioScript != null) audioScript.PlaySound(Sounds.Eat);
                DecreaseMoney(4);
                DecreaseHunger();

                int randNumber = Random.Range(1, 35);

                // Random events from money running out
                if (randNumber == 13)
                {
                    if (audioScript != null) audioScript.PlaySound(Sounds.FoodPoisoning);
                    ShowPopup("Sait Kemeristä vatsataudin, joka on tappava. Game over!\n\nPaina Enter uudelleensyntyäksesi fuksipallerona.");
                    LoseGame();
                }

                popupMonth = TukiKuukaudet;
                RakkausElama = false;
            }
        }

        public void Drink()
        {
            if (Rahat >= 45)
            {
                if (audioScript != null) audioScript.PlaySound(Sounds.Drink);
                DecreaseMoney(50);
                DecreasePsychosis();
                IncreaseBeer();
                IncreaseHunger();
                if (Promillet > 30)
                {
                    int rand = Random.Range(1, 5);
                    if (rand == 1)
                    {
                        int rand2 = Random.Range(1, 4);
                        if(rand2 == 1) ShowPopup("Kompastuit astuessasi pois karaokelavalta ja lensit hurmaavan henkilön päälle!\n\nPaina Enter saadaksesi klamydian.");
                        else if (rand2 == 2) ShowPopup("Kompastuit astuessasi pois karaokelavalta ja lensit hurmaavan henkilön päälle!\n\nPaina Enter saadaksesi tippurin.");
                        else if (rand2 == 3) ShowPopup("Kompastuit astuessasi pois karaokelavalta ja lensit hurmaavan henkilön päälle!\n\nPaina Enter saadaksesi HIV:n.");
                        else ShowPopup("Kompastuit astuessasi pois karaokelavalta ja lensit hurmaavan henkilön päälle!\n\nPaina Enter saadaksesi kondylooman.");
                        RakkausElama = true;
                        virgin = false;
                    }
                    else if (rand == 2)
                    {
                        ShowPopup("Törmäsit suosikkiprofessoriisi ollessasi Jolenessa.\n\nPaina Enter saadaksesi ylimääräisiä noppia vapaa-ajan aktiivisuudesta.");
                        IncreaseDice();
                        IncreaseDice();
                    }
                }
                else RakkausElama = false;
                joleneVisited = true;
            }
        }

        public void Work()
        {
            if (Nopat > 180)
            {
                if (audioScript != null) audioScript.PlaySound(Sounds.Work);
                Rahat += 600;
                IncreaseHunger();
                IncreasePsychosis();
                DecreaseMotivation();
                IncreasePsychosis();
                DecreaseMotivation();
            }
            else
            {
                int rand = Random.Range(1, 7);
                if(rand == 1) ShowPopup("Sinulla ei ole tarpeeksi opintopisteitä, joten kukaan ei halua palkata sinua.\n\nPaina Enter pysyäksesi työttömänä.");
                else if (rand == 2) ShowPopup("Sinulla ei ole tarpeeksi työkokemusta, joten kukaan ei halua palkata sinua.\n\nPaina Enter pysyäksesi työttömänä.");
                else if (rand == 3) ShowPopup("Olet liian ruma, joten kukaan ei halua palkata sinua.\n\nPaina Enter pysyäksesi työttömänä.");
                else if (rand == 4) ShowPopup("Oksensit haastattelijan kengille, joten sinua ei palkattu.\n\nPaina Enter pysyäksesi työttömänä.");
                else if (rand == 5) ShowPopup("Työhaastattelijan koira söi työhakemuksesi.\n\nPaina Enter pysyäksesi työttömänä.");
                else if (rand == 6) ShowPopup("Insert jokin tekosyy miksi et pääse töihin.\n\nPaina Enter pysyäksesi työttömänä.");
                else if (rand == 7) ShowPopup("Et sovi työyhteisön sosiaalisiin normeihin.\n\nPaina Enter pysyäksesi työttömänä.");
                if (audioScript != null) audioScript.PlayAmbientSound(AmbientSounds.WorkReminder);
            }
        }

        public void IncreaseBeer()
        {
            Promillet += 7;
            if(Promillet > 50)
            {

            }
        }

        public void Game()
        {
            if (audioScript != null) audioScript.PlaySound(Sounds.Game);
            IncreaseMotivation();
            IncreaseHunger();
            RakkausElama = false;
        }

        public void Vending()
        {
            if (Kolikot > 0)
            {
                if (audioScript != null) audioScript.PlaySound(Sounds.Vending);
                Kolikot--;
                Kofeiini += 10;
                ShowPopup("Slurp. Ostit energiajuomaa.\n\nPaina Enter päristäksesi.");
            }
            else
            {
                ShowPopup("Sinulla ei ole kolikoita mukanasi, joten et voi ostaa energiajuomaa.\n\nPaina Enter kärsiäksesi vieroitusoireista.");
            }
        }
        #endregion

        private void ShowPopup(string text)
        {
            try
            {
                PopupImage.SetActive(true);
                PopupActive = true;
                PopupImage.GetComponentInChildren<Text>().text = text;
            }
            catch { }
        }

        #region Modify stats
        private void DecreaseMonths()
        {
            if (TukiKuukaudet > 0)
            {
                TukiKuukaudet--;
            }
            else
            {
                MonthlyAllowance = 0;
                if (Random.Range(1, 3) == 1)
                {
                    ShowPopup("Oho! Tukikuukaudet loppuivat. Kela ei enää sponsoroi sinua.");
                    if (audioScript != null) audioScript.PlayAmbientSound(AmbientSounds.WorkReminder);
                }
            }
        }

        private void DecreaseMotivation()
        {
            Motivaatio -= 1 + (1* MotivationPenalty);
            if (Motivaatio > 0 && Motivaatio < 50 && Random.Range(1, 5) == 1)
            {
                if (audioScript != null) audioScript.PlayAmbientSound(AmbientSounds.LowMotivation);
            }
            if (Motivaatio <= 0)
            {
                Motivaatio = 0;
                if (audioScript != null) audioScript.PlaySound(Sounds.MotivationDeath);
                ShowPopup("Motivaatiosi opiskella loppui. Hävisit pelin!\n\nPaina Enter uudelleensyntyäksesi fuksipallerona.");
                LoseGame();
            }
        }

        private void DecreaseFreeTime()
        {
            VapaaAika--;
            if(VapaaAika <= 0)
            {
                VapaaAika = 0;
                ShowPopup("Sinulla ei ole yhtään vapaa-aikaa!\n\nPaina Enter todetaksesi olevasi lammas. Bää bää. Babylon kutsuu.");
            }
        }

        private void DecreaseCaffeine()
        {
            Kofeiini--;
            if(Kofeiini <= 0)
            {
                Kofeiini = 0;
                ShowPopup("Vieroitusoireet iskee!\n\nPaina Enter harkitaksesi ES tölkin ostamista lähimmästä Siwasta.");
            }
        }

        private void DecreaseMoney(int value)
        {
            if(Rahat == 0) return;
            Rahat -= value;
            if (Rahat <= 0)
            {
                Rahat = 0;
                int randNumber = Random.Range(1, 20);
                PopupActive = true;
                
                // Random events from money running out
                if (randNumber == 7)
                {
                    ShowPopup("Rahat loppuivat!\n\nPaina Enter myydäksesi munuaisen.");
                }
                else
                {
                    ShowPopup("Hups! Rahat loppuivat.\n\nPaina Enter syödäksesi loppukuun nuudeleita ilman tonnikalaa.");
                }
                popupMonth = TukiKuukaudet;
                if (audioScript != null) audioScript.PlayAmbientSound(AmbientSounds.NoMoney);
            }
        }

        private void IncreaseHunger()
        {
            Nalka += 3;
            if(Nalka > 60 && Nalka < 70)
                if (audioScript != null) audioScript.PlayAmbientSound(AmbientSounds.Hunger);
            if (Nalka >= 100)
            {
                if (audioScript != null) audioScript.PlayAmbientSound(AmbientSounds.Hunger);
                ShowPopup("Kuolit nälkään. Olisit käynyt Kemerissä!\n\nPaina Enter uudelleensyntyäksesi fuksipallerona.");
                LoseGame();
            }
        }

        private void DecreaseHunger()
        {
            Nalka -= 40;
            if (Nalka < 0) Nalka = 0;
        }

        private void DecreasePsychosis()
        {
            Psykoosi -= 10;
            if (Psykoosi < 0)
            {
                Psykoosi = 0;
            }
        }

        private void IncreasePsychosis()
        {
            Psykoosi += 2;
            if (Psykoosi > 50 && Random.Range(1, 5) == 1)
            {
                if (audioScript != null) audioScript.PlayAmbientSound(AmbientSounds.WantingBeer);
            }
            if (Psykoosi >= 100)
            {
                ShowPopup("Liian kovat psykoosit tulilla! Hävisit pelin.\n\nPaina Enter uudelleensyntyäksesi fuksipallerona.");
                if (audioScript != null) audioScript.PlaySound(Sounds.PsychosisDeath);
                LoseGame();
            }
        }

        private void IncreaseDice()
        {
            Nopat += 4 * Kofeiini / 100;
            if (Nopat >= 60 && !ekaVuosiPopUp)
            {
                ShowPopup("Pääsit fuksivuoden opinnoista onnistuneesti läpi.\n\nPaina Enter hajotaksesi tuleviin x-tehtäviin.");
                ekaVuosiPopUp = true;
            }
            else if (Nopat >= 120 && !tokaVuosiPopup)
            {
                ShowPopup("Pääsit kakkosvuoden opinnoista onnistuneesti läpi.\n\nPaina Enter hajotaksesi tulevaan kandin kirjoittamiseen.");
                tokaVuosiPopup = true;
            }
            else if (Nopat >= 180 && !kolmasVuosiPopup)
            {
                ShowPopup("Suoritit luonnontieteiden kandidaatin tutkinnon onnistuneesti.\n\nPaina Enter hajotaksesi tulevaan kissojen älykkyyden kurssiin.");
                kolmasVuosiPopup = true;
            }
            else if (Nopat >= 240 && !neljasVuosiPopup)
            {
                ShowPopup("Pääsit neljännen vuoden opinnoista onnistuneesti läpi.\n\nPaina Enter hajotaksesi tulevaan gradun kirjoittamiseen.");
                neljasVuosiPopup = true;
            }
            else if (Nopat >= 300)
            {
                ShowPopup("Onnittelut! Toisin kuin suurin osa opiskelijoista, sinä valmistuit ajoissa. Amanuenssi on tyytyväinen.\n\nPaina Enter voittaaksesi pelin.");
                if (audioScript != null) audioScript.PlaySound(Sounds.Victory);
                LoseGame();
            }
        }

        private void IncreaseMotivation()
        {
            Motivaatio += 4;
            if (Motivaatio > 100) Motivaatio = 100;
        }

        #endregion
        private void LoseGame()
        {
            GameOver = true;
            try
            {
                GameOverImage.SetActive(true);
                PopupImage.SetActive(true);
                RigidBodyFPSController.GetComponent<RigidbodyFirstPersonController>().enabled = false;
            }
            catch { }
        }
    }
}