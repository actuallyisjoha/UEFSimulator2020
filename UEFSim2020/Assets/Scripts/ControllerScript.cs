using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

namespace UEFSimulator
{
    public class ControllerScript : MonoBehaviour
    {
        public static bool GameOver = false;
        public int TukiKuukaudet, Nopat, Rahat, VapaaAika, Olut, Nalka, Kofeiini, Motivaatio, Psykoosi, MotivationPenalty, MonthlyAllowance;
        public bool RakkausElama, ZynZyn;
        public Text TukiKuukaudetText, NopatText, RahatText, VapaaAikaText, OlutText, NalkaText, KofeiiniText, MotivaatioText, PsykoosiText, RakkausElamaText;
        public GameObject PopupImage, RigidBodyFPSController;
        public AudioClip ShotgunSound, EatSound, DiceSound, GameSound, StomachSound, VomitSound;

        private AudioSource audioSource;

        private int popupMonth = 0;
        private float secondTime = 0.05f;
        private float secondTimer = 0.0f;
        private float dayTime = 2.0f;
        private float dayTimer = 0.0f;
        private float monthTime = 5.0f;
        private float monthTimer = 0.0f;
        private float timerBeforeNextPopupAllowed = 0.0f;

        private bool popupActive = false;
        private bool increasePopupTimer;

        // Start is called before the first frame update
        void Start()
        {
            PopupImage.SetActive(false);
            audioSource = GetComponent<AudioSource>();
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
            PlaySound(DiceSound);
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
                PlaySound(EatSound);
                DecreaseMoney(4);
                DecreaseHunger();

                int randNumber = Random.Range(1, 25);
                // Random events from money running out
                if (randNumber == 13)
                {
                    PlaySound(VomitSound);
                    ShowPopup("Sait Kemeristä vatsataudin, joka on tappava. Game over!");
                    LoseGame();
                }

                popupMonth = TukiKuukaudet;
            }
        }

        public void Game()
        {
            PlaySound(GameSound);
            IncreaseMotivation();
            IncreaseHunger();
        }
        #endregion

        private void ShowPopup(string text)
        {
            //if (popupMonth == TukiKuukaudet) return;
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
            if (Motivaatio <= 0)
            {
                Motivaatio = 0;
                PlaySound(ShotgunSound);
                ShowPopup("Motivaatiosi opiskella loppui. Hävisit pelin!");
                LoseGame();
            }
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
            if(Rahat == 0) return;
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
                PlaySound(StomachSound);
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

        private void PlaySound(AudioClip clip)
        {
            if (audioSource.isPlaying) audioSource.Stop();
            audioSource.PlayOneShot(clip);
        }
    }
}