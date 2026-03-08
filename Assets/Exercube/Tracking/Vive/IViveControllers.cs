using System;
using Valve.VR;

namespace Sphery.ExerCube
{
    public interface IViveControllers
    {
        event Action OnTrackerIndexed;

        bool IsSearching { get; }
        int IndexedTrackers { get; }

        void StartSearch();
        void StopSearch();
        void GetTrackerList(ref SteamVR_TrackedObject[] list);
        bool HasValidData(SteamVR_TrackedObject tracker);
    }
}
