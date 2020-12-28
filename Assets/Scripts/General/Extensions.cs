using System;
using System.Linq;
using Flux;

namespace BeauTambour
{
    public static class Extensions
    {
        public static int GetEnumCount<T>() where T : Enum => Enum.GetNames(typeof(T)).Length;

        public static TChar GetCharacter<TChar>(Actor actor) where TChar : Character
        {
            var characters = Repository.GetAll<Character>(References.Characters);
            var character = characters.First(item => item.Actor == actor);

            return character as TChar;
        }
    }
}