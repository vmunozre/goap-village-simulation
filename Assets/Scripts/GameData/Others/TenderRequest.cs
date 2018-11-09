public class TenderRequest {
    public string index = "mec";
    public bool available = true;
    public Hunter hunter = null;

    public TenderRequest(string _index, Hunter _hunter)
    {
        index = _index;
        hunter = _hunter;
    }
}
