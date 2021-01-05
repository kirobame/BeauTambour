namespace BeauTambour
{
    public class EventBoundDialogue
    {
        public EventBoundDialogue(string name, string key, string[] texts)
        {
            this.key = key;
            
            dialogues = new Dialogue[texts.Length];
            for (var i = 0; i < texts.Length; i++) dialogues[i] = Dialogue.Parse(name, texts[i]);
        }
        
        public string Key => key;
        private string key;

        private Dialogue[] dialogues;

        public Dialogue GetDialogue() => dialogues[(int)GameState.UsedLanguage];
    }
}