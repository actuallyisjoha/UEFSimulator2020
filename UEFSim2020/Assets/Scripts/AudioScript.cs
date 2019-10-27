using UnityEngine;

namespace UEFSimulator
{
    public enum Sounds
    {
        FoodPoisoning,
        MotivationDeath,
        Victory,
        Bottle,
        Drink,
        Eat,
        Game, 
        Study,
        Vending,
        Work,
        PsychosisDeath,
    }
    public enum AmbientSounds
    {
        WorkReminder,
        WantingBeer,
        LowMotivation,
        NoMoney,
        Hunger,
    }

    public enum Music
    {
        ValueForLifeMusic,
        TimeToBeSmartMusic,
        ZynZynMusic
    }
    public class AudioScript : MonoBehaviour
    {
        public GameObject Radio;
        public AudioClip FoodPoisoningSound, MotivationDeathSound, VictorySound, BottleSound, DrinkSound, EatSound, GameSound, StudySound, VendingSound, WorkSound,
            HungerSound, WorkReminderSound, ValueForLifeMusic, UefMusic, ZynMusic, NoMoneySound, PsychosisDeathSound, WantingBeerSound, LowMotivationSound;

        /// <summary>
        /// Audio Source for main player interactions
        /// </summary>
        private AudioSource audioSource;

        /// <summary>
        /// Audio Source for secondary backgroud noises
        /// </summary>
        private AudioSource ambientAudioSource;

        // Start is called before the first frame update
        void Start()
        {
            audioSource = GetComponents<AudioSource>()[0];
            ambientAudioSource = GetComponents<AudioSource>()[1];

            // Randomize radio station
            for (int i = 0; i < Random.Range(0, 3); i++)
            {
                ChangeRadio();
            }
        }

        public void PlaySound(Sounds sound)
        {
            AudioClip clip = null;
            switch (sound)
            {
                case Sounds.FoodPoisoning:
                    clip = FoodPoisoningSound;
                    break;
                case Sounds.MotivationDeath:
                    clip = MotivationDeathSound;
                    break;
                case Sounds.Victory:
                    audioSource.Stop();
                    clip = VictorySound;
                    break;
                case Sounds.Bottle:
                    clip = BottleSound;
                    break;
                case Sounds.Drink:
                    clip = DrinkSound;
                    break;
                case Sounds.Eat:
                    clip = EatSound;
                    break;
                case Sounds.Game:
                    clip = GameSound;
                    break;
                case Sounds.Study:
                    clip = StudySound;
                    break;
                case Sounds.Vending:
                    clip = VendingSound;
                    break;
                case Sounds.Work:
                    clip = WorkSound;
                    break;
                case Sounds.PsychosisDeath:
                    clip = PsychosisDeathSound;
                    break;
            }

            if (clip != null)
            {
                if (audioSource.isPlaying) audioSource.Stop();
                audioSource.PlayOneShot(clip);
            }
        }

        public void PlayAmbientSound(AmbientSounds sound)
        {
            AudioClip clip = null;
            switch (sound)
            {
                case AmbientSounds.WorkReminder:
                    clip = WorkReminderSound;
                    break;
                case AmbientSounds.WantingBeer:
                    clip = WantingBeerSound;
                    break;
                case AmbientSounds.LowMotivation:
                    clip = LowMotivationSound;
                    break;
                case AmbientSounds.NoMoney:
                    clip = NoMoneySound;
                    break;
                case AmbientSounds.Hunger:
                    clip = HungerSound;
                    break;
            }
            if (clip != null)
            {
                if (!ambientAudioSource.isPlaying)
                    ambientAudioSource.PlayOneShot(clip);
            }
            
        }

        public bool SoundPlaying()
        {
            return audioSource.isPlaying;
        }

        public bool AmbientSoundPlaying()
        {
            return ambientAudioSource.isPlaying;
        }
        public void StopSounds()
        {
            ambientAudioSource.Stop();
            audioSource.Stop();
            Radio.GetComponent<AudioSource>().Stop();
        }

        public void ChangeRadio()
        {
            Radio.GetComponent<AudioSource>().Stop();

            if (Radio.GetComponent<AudioSource>().clip == ZynMusic)
            {
                Radio.GetComponent<AudioSource>().clip = UefMusic;
            }
            else if (Radio.GetComponent<AudioSource>().clip == UefMusic)
            {
                Radio.GetComponent<AudioSource>().clip = ValueForLifeMusic;
            }
            else
            {
                Radio.GetComponent<AudioSource>().clip = ZynMusic;
            }

            Radio.GetComponent<AudioSource>().Play();
        }
    }
}