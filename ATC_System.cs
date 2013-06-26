using System.Collections.Generic;
using System.Web.UI;

namespace ATC_System
{

    public class AtcSystem : IATC_AtcSystem
    {

        private List<IATC_SystemTrack> _tracks = new List<IATC_SystemTrack>();
        private IATC_SystemEventDetector _EventDetector;
        private IATC_SystemRender _Render;
        private IATC_SystemLog _log;

        public int GetTrackCount { get { return _tracks.Count; } }

        public AtcSystem(IATC_SystemEventDetector eventDetector, IATC_SystemRender render, IATC_SystemLog log)
        {
            _EventDetector = eventDetector;
            _Render = render;
            _log = log;
        }

        public void Tick(int td)
        {
            foreach (IATC_SystemTrack t in _tracks)
            {
                t.update(td);
            }
            
            List<IEvent> events = _EventDetector.detectEvents(_tracks);

            foreach (IEvent eEvent in events)
            {
                _log.Output(eEvent);

                if (eEvent.RemoveTrack == true)
                {
                    Pair tracks = eEvent.Tracks();
                    
                    if (tracks.First == tracks.Second)
                    {
                        removeTrack((IATC_SystemTrack) tracks.First);
                    }
                    else
                    {
                        removeTrack((IATC_SystemTrack)tracks.First);
                        removeTrack((IATC_SystemTrack)tracks.Second);
                    }
                    
                }
            }

            foreach (IATC_SystemTrack t in _tracks)
            {
                _Render.RenderTracks(t);
            }
            
        }

        public void removeTrack(IATC_SystemTrack track)
        {
            _tracks.Remove(track);
        }

        public void handleIncomingTrack(IATC_SystemTrack track)
        {
            _tracks.Add(track);
        }

    }
}
