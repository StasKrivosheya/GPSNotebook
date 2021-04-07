using GPSNotebook.Models;
using GPSNotebook.ViewModels;
using Xamarin.Forms.GoogleMaps;

namespace GPSNotebook.Extensions
{
    public static class PinExtensions
    {
        public static PinModel ToPinModel(this PinViewModel pinViewModel)
        {
            PinModel pinModel = null;

            if (pinViewModel != null)
            {
                pinModel = new PinModel
                {
                    Id = pinViewModel.Id,
                    UserId = pinViewModel.UserId,
                    Name = pinViewModel.Name,
                    Description = pinViewModel.Description,
                    IsFavorite = pinViewModel.IsFavorite,
                    Latitude = pinViewModel.Latitude,
                    Longitude = pinViewModel.Longitude,
                    PinImagePath = pinViewModel.PinImagePath
                };
            }

            return pinModel;
        }

        public static PinViewModel ToPinViewModel(this PinModel pinModel)
        {
            PinViewModel pinViewModel = null;

            if (pinModel != null)
            {
                pinViewModel = new PinViewModel
                {
                    Id = pinModel.Id,
                    UserId = pinModel.UserId,
                    Name = pinModel.Name,
                    Description = pinModel.Description,
                    IsFavorite = pinModel.IsFavorite,
                    PinImagePath = pinModel.PinImagePath,
                    Position = new Position(
                        double.Parse(pinModel.Latitude), double.Parse(pinModel.Longitude))
                };
            }

            return pinViewModel;
        }

        public static Pin ToPin(this PinViewModel pinViewModel)
        {
            Pin pin = null;

            if (pinViewModel != null)
            {
                pin = new Pin
                {
                    Address = pinViewModel.Description,
                    IsVisible = pinViewModel.IsFavorite,
                    Label = pinViewModel.Name,
                    Position = pinViewModel.Position
                };
            }

            return pin;
        }
    }
}
