using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackMyAssets_API.Domain.ModelsViews
{
    public struct HomeModelView
    {
        public string Doc { get => "/swagger"; }
        public string Message { get => "Bem vindo ao TrackMyAssets_API!"; }
    }
}