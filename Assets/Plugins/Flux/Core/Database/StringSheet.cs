namespace Flux
{
    public class StringSheet : RuntimeSheet<string>
    {
        protected override string ProcessValue(string item) => item;
    }
}