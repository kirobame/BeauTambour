using System;
using System.Collections;
using System.Linq;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    public static class Extensions
    {
        public static int GetEnumCount<T>() where T : Enum => Enum.GetNames(typeof(T)).Length;
        
        public static TChar GetCharacter<TChar>(Actor actor) where TChar : Character
        {
            var characters = Repository.GetAll<Character>(References.Characters);
            var character = characters.FirstOrDefault(item => item.Actor == actor && item is TChar);

            return character as TChar;
        }

        public static void Delay(this MonoBehaviour hook, Action call, int frameCount) => hook.StartCoroutine(DelayRoutine(call, frameCount));
        private static IEnumerator DelayRoutine(Action call, int frameCount)
        {
            for (var i = 0; i < frameCount; i++) yield return new WaitForEndOfFrame();
            call();
        }

        public static string FirstToUpper(this string value)
        {
            var charArray = value.ToCharArray();
            charArray[0] = Char.ToUpper(charArray[0]);
            
            return new string(charArray);
        }
    }
}