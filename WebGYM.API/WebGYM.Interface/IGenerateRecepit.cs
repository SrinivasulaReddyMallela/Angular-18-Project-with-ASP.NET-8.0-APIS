using WebGYM.ViewModels;

namespace WebGYM.Interface
{
    public interface IGenerateRecepit
    {
        Task<GenerateRecepitViewModel> Generate(int paymentId);
    }
}