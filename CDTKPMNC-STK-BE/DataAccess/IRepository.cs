namespace CDTKPMNC_STK_BE.DataAccess
{
    public interface IRepository<Model, T>
    {
        List<Model> GetAll();
        Model GetById(T id);
        void Add(Model model);
        void Update(T id, Model model);
        void Delete(T id);
        void Delete(T id, Model newModel);
    }
}
