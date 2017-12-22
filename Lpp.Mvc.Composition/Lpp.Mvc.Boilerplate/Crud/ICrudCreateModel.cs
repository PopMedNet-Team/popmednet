namespace Lpp.Mvc
{
    public interface ICrudCreateModel
    {
        string ReturnTo { get; set; }
    }

    public class CrudCreateModel : ICrudCreateModel
    {
        public string ReturnTo { get; set; }
    }
}