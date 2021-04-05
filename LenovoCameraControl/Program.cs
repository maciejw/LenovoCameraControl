using Lenovo.Multimedia.Contracts.Camera.CameraTypes;
using Lenovo.Multimedia.Core;
using Lenovo.Multimedia.Core.CoreCamera;
using Lenovo.Multimedia.Native.Dispatch;

using System;
using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LenovoCameraControl
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var rootCommand = new RootCommand("Lenovo Camera Control");

            rootCommand.AddCommand(new Command("toggle", "Toggles front camera")
            {
                Handler = CommandHandler.Create(Toggle)
            });
            rootCommand.AddCommand(new Command("state", "Gets front camera state")
            {
                Handler = CommandHandler.Create(State)
            });
            return await rootCommand.InvokeAsync(args);
        }
        private static void Toggle()
        {
            CameraContext cameraContext = GetCameraContext();
            if (cameraContext != null)
            {
                string friendlyName = cameraContext.FriendlyName;
                var nativeCamera = new NativeCamera();
                int currentState = 0;

                nativeCamera.GetPrivacy(friendlyName, ref currentState);

                var newState = ToggleState(currentState);

                Console.WriteLine($"Toggling {friendlyName} from {CoreRuntimeDefs.StateOnOff[currentState]} to {newState}");

                nativeCamera.SetPrivacy(friendlyName, CoreRuntimeDefs.State01[newState]);
            }
        }
        private static string ToggleState(int currentState)
        {
            return currentState == 0 ? CoreRuntimeDefs.StateOnOff[1] : CoreRuntimeDefs.StateOnOff[0];
        }
        private static void State()
        {
            CameraContext cameraContext = GetCameraContext();
            if (cameraContext != null)
            {
                string friendlyName = cameraContext.FriendlyName;
                var nativeCamera = new NativeCamera();
                int currentState = 0;

                nativeCamera.GetPrivacy(friendlyName, ref currentState);

                Console.WriteLine($"Current state of {friendlyName} is {CoreRuntimeDefs.StateOnOff[currentState]}");
            }

        }
        private static CameraContext GetCameraContext(string location = "front")
        {
            return new CameraRT().GetCameraContext(CameraRuntimeDefs.Location[location]);
        }
    }
}