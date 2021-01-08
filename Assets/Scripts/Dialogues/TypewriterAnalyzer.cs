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
        [SerializeField] private TextAnimatorPlayer textAnimatorPlayer;
        [SerializeField] private int gap;

        private DialogueHandler dialogueHandler;        
        private List<string> syllabes;
        private int currentSyll;
        private int counter;

        #region Letter types constantes
        readonly static char[] consonants = new char[] {
            'B','C','D','F','G',
            'H','J','K','L','M','N',
            'Ñ','P','Q','R','S',
            'T','V','W','X','Y','Z'};
        readonly static char[] strongVowels = new char[] {
            'A','Á','E','É',
            'Í','O','Ó','Ú'};
        readonly static char[] weakVowels = new char[] { 'I', 'U', 'Ü' };
        readonly static char[] vowels = new char[] {
            'A','Á','À','E','É',
            'Í','O','Ó','Ú',
            'I','U','Ü'};
        readonly static char[] letters = new char[] {
            'A','Á','À','E','É',
            'Í','O','Ó','Ú',
            'I','U','Ü',
            'B','C','D','F','G',
            'H','J','K','L','M','N',
            'Ñ','P','Q','R','S',
            'T','V','W','X','Y','Z'};
        readonly static char[] punctuation = new char[] {
            '.','-','_',',',';','!','?','\n',
        };
        #endregion

        void Start()
        {
            syllabes = new List<string>();
            dialogueHandler = Repository.GetSingle<DialogueHandler>(References.DialogueHandler);
            Event.Open(GameEvents.OnCueFinished);
            Event.Register<Cue>(GameEvents.OnNextCue, OnNextCue);
        }

        void OnEnable()
        {
            counter = 0;
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
            dialogueHandler.Speaker.StopTalking();
            Event.Call(GameEvents.OnCueFinished);
        }

        void OnCharacterVisible(char character)
        {
            if (counter <= 0)
            {
                character = char.ToLower(character);
                if (dialogueHandler.Speaker.AudioCharMap.TryGet(character, out var package))
                {
                    var audioPool = Repository.GetSingle<AudioPool>(References.AudioPool);
                    var audio = audioPool.RequestSingle();
                    
                    package.AssignTo(audio, EventArgs.Empty);
                    audio.Play();

                    counter = syllabes[currentSyll].Length - 1;
                    Debug.Log($"{syllabes[currentSyll].Replace(' ','*')}");
                    currentSyll++;
                    if(currentSyll > syllabes.Count)
                    {
                        counter = 2;
                    }
                }
            }
            else counter--;
        }

        void OnNextCue(Cue cue)
        {
            currentSyll = 0;
            syllabes.Clear();

            string text = cue.Text;
            text = GetOnlyText(text);
            foreach (var word in text.Split(' '))
            {
                if (string.IsNullOrEmpty(word)) continue;
                syllabes.AddRange(GetWordSyllabes(word));
            }
        }

        private string GetOnlyText(string text)
        {
            bool isInBalise = false;
            int startBalise = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if(!isInBalise && text[i] == '<')
                {
                    isInBalise = true;
                    startBalise = i;
                }
                else if(isInBalise && text[i] == '>')
                {
                    isInBalise = false;
                    text = text.Remove(startBalise,i-startBalise+1);
                    i = startBalise;
                }

                if (!isInBalise && punctuation.Contains(text[i]))
                {                    
                    text =  text.Remove(i,1);
                    i--;    
                }
            }
            return text;
        }

        public List<string> GetWordSyllabes(string word)
        {
            int cursor, startSyl, lengthMinusOne, nbSyl;
            List<string> wordSyllabes = new List<string>();
            //bool space = false;
            //bool isInBalise = false;
            word = word.ToUpper();
            cursor = 1;
            startSyl = 0;
            lengthMinusOne = word.Length - 1;

            while (cursor < lengthMinusOne)
            {
                int hyphen = 0;
                /*if (s[cursor - 1] == '<' || s[cursor - 1] == '>')
                {
                    if (!isInBalise)
                    {
                        isInBalise = true;
                    }
                    else
                    {
                        isInBalise = false;
                        startSyl = cursor;
                        cursor++;
                    }
                }

                if (s[cursor] == ' ' || s[cursor] == ',' || s[cursor] == ';' || s[cursor] == '-')
                {
                    if (!isInBalise)
                    {
                        hyphen = 1;
                    }                    
                    space = true;
                }
                if (isInBalise) {
                    cursor++;
                    continue;
                }
                if (space) { 
                    //Nothing
                }
                else*/
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
                /*if (space) { 
                    startSyl++;
                    cursor++;
                    space = false;
                }*/
                cursor++;
            }

            nbSyl = wordSyllabes.Count - 1;
            if (startSyl == lengthMinusOne && nbSyl >= 0 && consonants.Contains(word[lengthMinusOne]))
                wordSyllabes[nbSyl] = wordSyllabes[nbSyl] + word[lengthMinusOne];   // Last letter
            else
                wordSyllabes.Add(word.Substring(startSyl, lengthMinusOne - startSyl + 1));

            return wordSyllabes;
        }
    }
}