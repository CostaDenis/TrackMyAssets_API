
namespace TrackMyAssets_API.Domain.ModelsViews
{
    public record HomeModelView
    {
        public string Doc { get => "/swagger"; }
        public string Message { get => "Bem vindo ao TrackMyAssets_API!"; }
    }
}