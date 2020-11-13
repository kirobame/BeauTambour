using System;
using System.Collections.Generic;
using System.Linq;
using Ludiq.PeekCore;

namespace BeauTambour
{
    public static class BeauTambourExtensions 
    {
        public static IEnumerable<T> Query<T>(this IEnumerable<Note> source) where T : NoteAttribute
        {
            return source.SelectMany(note => note.Attributes.Where(attribute => attribute is T)).Cast<T>();
        }
        public static IEnumerable<T> Query<T>(this IEnumerable<Note> source, Func<T, bool> predicate) where T : NoteAttribute
        {
            var casted =  source.SelectMany(note => note.Attributes.Where(attribute => attribute is T)).Cast<T>();
            return casted.Where(predicate);
        }
    }
}