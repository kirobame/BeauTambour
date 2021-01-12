using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Febucci.UI;
using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    public class TypewriterAnalyzer : MonoBehaviour
    {
        [Serializable]
        public enum PLAYSOUNDSYLLABEPOS
        {
            OnFirstVowel,
            OnSyllabeStart,
            OnSyllabeStartAndPreviousConsonant
        }

        [SerializeField] private TextAnimatorPlayer textAnimatorPlayer;
        [SerializeField] private PLAYSOUNDSYLLABEPOS playSoundPos;
        [SerializeField] private float consonantFirstVowelTimeRatio = 0.1f;
        [SerializeField] private VowelSoundRegistry vowelSoundRegistry;


        private DialogueHandler dialogueHandler;        
        private List<string> syllabes;
        private int currentSyll;
        private int currentIndexInSyll;

        private float timeToPlayNextSound;
        private float startClock;
        private bool passNextChar = false;
        private List<string> queueToPlay;

        #region Letter types constantes
        readonly static char[] consonants = new char[] {
            'B','C','D','F','G',
            'H','J','K','L','M','N',
            'Ñ','P','Q','R','S',
            'T','V','W','X','Y','Z'};
        readonly static char[] strongVowels = new char[] {
            'A','Á','À','E','É','È',
            'Í','O','Ó','Ú'};
        readonly static char[] weakVowels = new char[] { 'I', 'U', 'Ü' };
        readonly static char[] vowels = new char[] {
            'A','Á','À','E','É','È',
            'Í','O','Ó','Ú',
            'I','U','Ü'};
        readonly static char[] letters = new char[] {
            'A','Á','À','E','É','È',
            'Í','O','Ó','Ú',
            'I','U','Ü',
            'B','C','D','F','G',
            'H','J','K','L','M','N',
            'Ñ','P','Q','R','S',
            'T','V','W','X','Y','Z'};
        readonly static char[] punctuation = new char[] {
            '.',',',';','!','?','\n','*','\r'
        };
        readonly static char[] links = new char[] {
            '-','_','\'','`','’'
        };
        #endregion

        void Start()
        {
            syllabes = new List<string>();
            queueToPlay = new List<string>();
            dialogueHandler = Repository.GetSingle<DialogueHandler>(References.DialogueHandler);
            Event.Open(GameEvents.OnCueFinished);
            Event.Register<Cue>(GameEvents.OnNextCue, OnNextCue);
        }

        private void Update()
        {
            if(queueToPlay.Count > 0 && Time.time - startClock >= timeToPlayNextSound)
            {
                PlayCorrespondingAudio(queueToPlay[0]);
                queueToPlay.RemoveAt(0);
            }
        }

        void OnEnable()
        {
            currentIndexInSyll = 0;
            currentSyll = 0;
            
            textAnimatorPlayer.onCharacterVisible.AddListener(OnCharacterVisible);
            textAnimatorPlayer.onTextShowed.AddListener(OnTextShown);

        }
        void OnDisable()
        {
            textAnimatorPlayer.onCharacterVisible.RemoveListener(OnCharacterVisible);
            textAnimatorPlayer.onTextShowed.RemoveListener(OnTextShown);
            syllabes.Clear();
        }

        void OnTextShown()
        {
            dialogueHandler.Speaker.RuntimeLink.StopTalking();
            Event.Call(GameEvents.OnCueFinished);
        }

        void OnCharacterVisible(char character)
        {
            if (punctuation.Contains(character) || links.Contains(character)) return;

            if (!passNextChar)
            {
                if (TryGetFirstVowelSound(syllabes[currentSyll], out string vowel, out int idx))
                {
                    switch (playSoundPos)
                    {
                        case PLAYSOUNDSYLLABEPOS.OnFirstVowel:
                            if (currentIndexInSyll == idx)
                            {
                                PlayCorrespondingAudio(vowel);
                            }
                            break;
                        case PLAYSOUNDSYLLABEPOS.OnSyllabeStart:
                            if (currentIndexInSyll == 0)
                            {
                                PlayCorrespondingAudio(vowel);
                            }
                            break;
                        case PLAYSOUNDSYLLABEPOS.OnSyllabeStartAndPreviousConsonant:

                            if (consonants.Contains(char.ToUpper(character)))
                            {
                                if (currentIndexInSyll == idx - 1)
                                {
                                    passNextChar = true;                                    
                                    PlayCorrespondingAudio(character.ToString());
                                    queueToPlay.Add(vowel);
                                    startClock = Time.time;
                                }                                
                            }
                            else
                            {
                                if (currentIndexInSyll == idx)
                                {
                                    PlayCorrespondingAudio(vowel);
                                }
                            }
                            break;
                    }
                }
            }
            else
            {
                passNextChar = false;
            }

            currentIndexInSyll++;
            if (currentIndexInSyll >= syllabes[currentSyll].Length)
            {
                if (currentSyll < syllabes.Count - 1)
                {
                    currentSyll++;
                    currentIndexInSyll = 0;
                }
            }
        }

        void OnNextCue(Cue cue)
        {
            currentSyll = 0;
            currentIndexInSyll = 0;
            syllabes.Clear();

            string text = cue.Text;
            text = GetOnlyText(text);
            foreach (var word in text.Split(' '))
            {
                if (string.IsNullOrEmpty(word)) continue;
                syllabes.AddRange(GetWordSyllabes(word));
            }
        }

        private void PlayCorrespondingAudio(string chars)
        {
            Debug.Log("START - " + chars);

            chars = chars.ToLower();
            if (dialogueHandler.Speaker.AudioStringMap.TryGet(chars, out var package))
            {
                AssignToAudioSource(package);
            }
            else if(vowelSoundRegistry.TryGet(chars, out string sound))
            {
                if (dialogueHandler.Speaker.AudioStringMap.TryGet(sound, out package))
                {
                    AssignToAudioSource(package);
                }
            }
            else if(dialogueHandler.Speaker.AudioStringMap.TryGet(chars[0].ToString(), out package))
            {
                AssignToAudioSource(package);
            }
        }

        private void AssignToAudioSource(AudioPackage package)
        {
            var audioPool = Repository.GetSingle<AudioPool>(References.AudioPool);
            var audio = audioPool.RequestSingle();

            package.AssignTo(audio, EventArgs.Empty);
            audio.pitch = dialogueHandler.Speaker.PitchRange;
            audio.Play();
            timeToPlayNextSound = audio.clip.length * consonantFirstVowelTimeRatio;
        }

        private string GetOnlyText(string text)
        {
            bool isInBalise = false;
            int startBalise = 0;
            for (int i = 0; i < text.Length; i++)
            {
                //Balise detection
                if (!isInBalise && text[i] == '<')
                {
                    isInBalise = true;
                    startBalise = i;
                }
                else if (isInBalise && text[i] == '>')
                {
                    isInBalise = false;
                    text = text.Remove(startBalise, i - startBalise + 1);
                    i = startBalise - 1;
                }

                //Others
                if (!isInBalise && i >= 0) {
                    if(punctuation.Contains(text[i]))
                    {
                        text = text.Remove(i, 1);
                        i--;
                    }
                    else if(links.Contains(text[i]))
                    {
                        text = text.Replace(text[i],' ');
                    }
                }
                
            }
             return text;
        }

        private List<string> GetWordSyllabes(string word)
        {
            int cursor, startSyl, lengthMinusOne, nbSyl;
            List<string> wordSyllabes = new List<string>();
            word = word.ToUpper();
            cursor = 1;
            startSyl = 0;
            lengthMinusOne = word.Length - 1;

            while (cursor < lengthMinusOne)
            {
                int hyphen = 0;
                if (consonants.Contains(word[cursor]))
                {
                    if (vowels.Contains(word[cursor + 1]))
                    {
                        if (vowels.Contains(word[cursor - 1]))
                            hyphen = 1;
                    }
                    else if (word[cursor] == 'S' && word[cursor - 1] == 'N' && (consonants.Contains(word[cursor + 1])))
                        hyphen = 2;
                    else if (consonants.Contains(word[cursor + 1]) && vowels.Contains(word[cursor - 1]))
                        if (word[cursor + 1] == 'R')
                            if (word[cursor] == 'B' || word[cursor] == 'C' || word[cursor] == 'D' || word[cursor] == 'F' || word[cursor] == 'G' ||
                                word[cursor] == 'K' || word[cursor] == 'P' || word[cursor] == 'R' || word[cursor] == 'T' || word[cursor] == 'V')
                                hyphen = 1;
                            else
                                hyphen = 2;
                        else if (word[cursor + 1] == 'L')
                            if (word[cursor] == 'B' || word[cursor] == 'C' || word[cursor] == 'D' || word[cursor] == 'F' || word[cursor] == 'G' ||
                                word[cursor] == 'K' || word[cursor] == 'L' || word[cursor] == 'P' || word[cursor] == 'T' || word[cursor] == 'V')
                                hyphen = 1;
                            else
                                hyphen = 2;
                        else if (word[cursor + 1] == 'H')
                            if (word[cursor] == 'C' || word[cursor] == 'S' || word[cursor] == 'P')
                                hyphen = 1;
                            else
                                hyphen = 2;
                        else
                            hyphen = 2;
                }
                else if (strongVowels.Contains(word[cursor]))
                {
                    if (strongVowels.Contains(word[cursor - 1]))
                        hyphen = 1;
                }
                else if (word[cursor] == '-')
                {
                    wordSyllabes.Add(word.Substring(startSyl, cursor - startSyl));
                    wordSyllabes.Add("-");
                    cursor++;
                    startSyl = cursor;
                }

                if (hyphen == 1)
                {  // Hyphenate here
                    wordSyllabes.Add(word.Substring(startSyl, cursor - startSyl));
                    startSyl = cursor;
                }
                else if (hyphen == 2)
                {  // Hyphenate after
                    cursor++;
                    wordSyllabes.Add(word.Substring(startSyl, cursor - startSyl));
                    startSyl = cursor;
                }

                cursor++;
            }

            nbSyl = wordSyllabes.Count - 1;
            if (startSyl == lengthMinusOne && nbSyl >= 0 && consonants.Contains(word[lengthMinusOne]))
                wordSyllabes[nbSyl] = wordSyllabes[nbSyl] + word[lengthMinusOne];   // Last letter
            else
                wordSyllabes.Add(word.Substring(startSyl, lengthMinusOne - startSyl + 1));

            return wordSyllabes;
        }

        private bool TryGetFirstVowelSound(string s, out string vowel, out int vowelIndex)
        {
            string result = "";
            int startIndex = 0;
            bool vowelFound = false;
            vowelIndex = -1;
            for (int i = 0; i < s.Length; i++)
            {
                if (vowelFound) continue;
                if (vowels.Contains(s[i]))
                {
                    if (i != s.Length-1 && consonants.Contains(s[i + 1]))
                    {
                        vowelIndex = startIndex;
                        result = s.Substring(startIndex,i-startIndex+1);
                        vowelFound = true;
                    }
                    else if (i == s.Length - 1)
                    {
                        vowelIndex = startIndex;
                        result = s.Substring(startIndex, i - startIndex + 1);
                        vowelFound = true;
                    }                    
                }
                else
                {
                    startIndex++;
                }
            }

            if (string.IsNullOrEmpty(result))
            {
                vowel = null;
                return false;
            }
            else
            {
                vowel = result;
                return true;
            }
        }
    }
}