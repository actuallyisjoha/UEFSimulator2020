using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

namespace UEFSimulator
{
    public class ControllerScript : MonoBehaviour
    {
        public int TukiKuukaudet, Nopat, Rahat, VapaaAika, Olut, Nalka, Kofeiini, Motivaatio, Psykoosi;
        public bool RakkausElama, ZynZyn;
        public Text TukiKuukaudetText, NopatText, RahatText, VapaaAikaText, OlutText, NalkaText, KofeiiniText, MotivaatioText, PsykoosiText, RakkausElamaText;
        public GameObject PopupImage;
        public GameObject RigidBodyFPSController;

        private float secondTime = 0.05f;
        private float secondTimer = 0.0f;
        private float dayTime = 2.0f;
        private float dayTimer = 0.0f;
        private float monthTime = 5.0f;
        private float monthTimer = 0.0f;

        public int MotivationPenalty = 1;
        public int MonthlyAllowance;

        private bool popupActive = false;
        private float timerBeforeNextPopupAllowed = 0.0f;
        private bool increasePopupTimer;

        public static bool GameOver = false;

        private int popupMonth = 0;

        // Start is called before the first frame update
        void Start()
        {
            PopupImage.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if(GameOver) return;
            updateUIText();

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debug.Log("pressed key");
                PopupImage.SetActive(false);
                popupActive = false;
            }

            dayTimer += Time.deltaTime;
            if (dayTimer > dayTime)
            {
                IncreaseHunger();
                dayTimer = 0;
            }

            secondTimer += Time.deltaTime;
            if (secondTimer > secondTime)
            {
                DecreaseMoney(2);
                secondTimer = 0;
            }

            monthTimer += Time.deltaTime;
            if (monthTimer > monthTime)
            {
                Rahat += MonthlyAllowance;
                DecreaseMonths();
                monthTimer = 0;
            }


            if (increasePopupTimer) timerBeforeNextPopupAllowed += Time.deltaTime;
        }


        void updateUIText()
        {
            TukiKuukaudetText.text = "Tukikuukausia jäljellä: " + TukiKuukaudet + " kk";
            NopatText.text = "Nopat: " + Nopat + " op";
            RahatText.text = "Rahat: " + Rahat + "€";
            VapaaAikaText.text = "Vapaa-aika: " + VapaaAika + " min";
            OlutText.text = "Olut: " + Olut + " l";
            NalkaText.text = "Nälkä: " + Nalka;
            KofeiiniText.text = "Kofeiini: " + Kofeiini + " mg";
            MotivaatioText.text = "Motivaatio: " + Motivaatio + "%";
            PsykoosiText.text = "Psykoosi: " + Psykoosi + "%";
            if (RakkausElama) RakkausElamaText.text = "Rakkauselämä: on";
            else RakkausElamaText.text = "Rakkauselämä: ei";
        }

        #region PlayerActions
        public void Study()
        {
            DecreaseMotivation();
            DecreaseFreeTime();
            DecreaseCaffeine();

            IncreaseDice();
            IncreaseHunger();
            IncreasePsychosis();
        }

        public void Eat()
        {
            if (Rahat >= 4)
            {
                DecreaseMoney(4);
                DecreaseHunger();

                int randNumber = Random.Range(1, 25);
                // Random events from money running out
                if (randNumber == 13)
                {
                    ShowPopup("Sait vatsataudin syötyäsi Kemerissä!\n1) OK");
                }

                popupMonth = TukiKuukaudet;
            }
        }

        public void Game()
        {
            IncreaseMotivation();
            IncreaseHunger();
        }
        #endregion

        private void ShowPopup(string text)
        {
            if (popupMonth == TukiKuukaudet) return;
            PopupImage.SetActive(true);
            PopupImage.GetComponentInChildren<Text>().text = text;
            
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
                ShowPopup("Oho! Tukikuukaudet loppuivat. Kela ei enää sponsoroi sinua.");
            }
        }

        private void DecreaseMotivation()
        {
            Motivaatio -= 1 * MotivationPenalty;
            if (Motivaatio <= 0) LoseGame();
        }

        private void DecreaseFreeTime()
        {
            VapaaAika--;
            // Modify MotivationPenalty in here
            //if (VapaaAika <= 0) GameOver();
        }

        private void DecreaseCaffeine()
        {
            Kofeiini--;
        }

        private void DecreaseMoney(int value)
        {
            Rahat -= value;
            if (Rahat <= 0)
            {
                Rahat = 0;
                if (popupActive) return;
                int randNumber = Random.Range(1, 10);
                popupActive = true;
                // Random events from money running out
                if (randNumber == 7)
                {
                    ShowPopup("Rahat loppuivat!\nHaluatko myydä munuaisen ja saada 5000€?\n1) Kyllä 2) Ei");
                }
                else
                {
                    ShowPopup("Hups! Rahat loppuivat. Loppukuu nuudeleita!\n1) OK");
                }
                popupMonth = TukiKuukaudet;
            }
        }

        private void IncreaseHunger()
        {
            Nalka += 3;
            if (Nalka >= 100)
            {
                ShowPopup("Kuolit nälkään. Olisit käynyt Kemerissä!");
                LoseGame();
            }
        }

        private void DecreaseHunger()
        {
            Nalka -= 15;
            if (Nalka < 0) Nalka = 0;
        }

        private void IncreasePsychosis()
        {
            Psykoosi++;
            if (Psykoosi >= 100) LoseGame();
        }

        private void IncreaseDice()
        {
            Nopat++;
            if (Nopat >= 300) Debug.Log("Kutittaa. Voitit pelin.");
        }

        private void IncreaseMotivation()
        {
            Motivaatio++;
            if (Motivaatio > 100) Motivaatio = 100;
        }

        #endregion
        private void LoseGame()
        {
            GameOver = true;
            RigidBodyFPSController.GetComponent<RigidbodyFirstPersonController>().enabled = false;
        }
    }
}